using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Lgis
{
    public class LShapefileReader
    {
        
        string filename;
        public LVectorLayer Layer = new LVectorLayer();
        BinaryReader shpReader,shxReader,dbfReader;
        public readonly GeometryType GeometryType;
        int length;
        delegate string strmap(string s);
        strmap BaseName = s => s.Substring(0, s.LastIndexOf('.'));
        public LShapefileReader(string filename)
        {
            this.filename = filename;
            try
            {
                FileInfo fileInfo = new FileInfo(filename);
                int nrecord;
                string basename = BaseName(fileInfo.FullName);
                if (!File.Exists(basename + ".shx") || !File.Exists(basename + ".dbf"))
                    throw new LIOException("No .shx or .dbf found");
                
                Console.WriteLine(basename);
                length = (int)fileInfo.Length;
                shpReader = new BinaryReader(
                    new FileStream(filename, FileMode.Open,FileAccess.Read));
                shxReader = new BinaryReader(
                    new FileStream(basename+".shx",FileMode.Open,FileAccess.Read));
                dbfReader = new BinaryReader(
                    new FileStream(basename+".dbf",FileMode.Open,FileAccess.Read));
                fileInfo = new FileInfo(basename + ".shx");
                nrecord = ((int)fileInfo.Length - 100) / 8;
                shxReader.BaseStream.Seek(24L, SeekOrigin.Begin);
                int code = shpReader.ReadInt32();
                shpReader.BaseStream.Seek(32L, SeekOrigin.Begin);
                int type = shpReader.ReadInt32();
                if (Enum.IsDefined(typeof(GeometryType), type))
                    GeometryType = (GeometryType)type;
                else
                {
                    GeometryType = GeometryType.Unknown;
                    throw new LIOException("Unknown Shapefile format");
                }
                switch (GeometryType)
                {
                    case Lgis.GeometryType.Point:
                    case Lgis.GeometryType.Polypoint:
                        Layer = new LPointLayer();
                        break;
                    case Lgis.GeometryType.Polyline:
                    case Lgis.GeometryType.PolyPolyline:
                        Layer = new LLineLayer();
                        break;
                    case Lgis.GeometryType.Polygon:
                    case Lgis.GeometryType.PolyPolygon:
                        Layer = new LPolygonLayer();
                        break;
                    default: break;
                }
                Layer.Name = BaseName(fileInfo.Name);
                byte version = dbfReader.ReadByte();
                Console.Write("Version:");
                Console.WriteLine((int)version);
                dbfReader.BaseStream.Seek(10L , SeekOrigin.Begin);
                Int16 dbfRecordLen = dbfReader.ReadInt16();
                Console.WriteLine(dbfRecordLen);
                dbfReader.BaseStream.Seek(32L, SeekOrigin.Begin);
                //Read FieldDescribe
                char fieldType;
                while (dbfReader.PeekChar()!=0x0d)
                {
                    byte[] fieldDesc = dbfReader.ReadBytes(32);
                    string fieldName;
                    LDataTable table = Layer.DataTable;
                    unsafe
                    {
                        fixed (byte* pchar = fieldDesc)
                        {
                            fieldName = "";
                            for (int i = 0; i < 10; ++i)
                            {
                                if (pchar[i] != 0x00)
                                    fieldName += (char)pchar[i];
                            }
                            fieldType = (char)pchar[11];
                            Console.WriteLine("type=" + fieldType.ToString());
                            Console.WriteLine(fieldName);
                        }
                    }
                    switch (fieldType)
                    {
                        case 'C':
                            table.Columns.Add(new DataColumn(fieldName,typeof(string)));
                            break;
                        case 'N':
                            table.Columns.Add(new DataColumn(fieldName,typeof(double)));
                            break;
                        default:
                            throw new LNotImplementedException("Unrecognized dbf field");
                    }
                }
                //Ready to read record header
                shxReader.BaseStream.Seek(100L, SeekOrigin.Begin);
                shpReader.BaseStream.Seek(100L, SeekOrigin.Begin);

                for (int i = 0; i < nrecord; ++i)
                {
                    int offset = ShiftEndian(shxReader.ReadInt32());
                    int recordLength = ShiftEndian(shxReader.ReadInt32()) * 2; // ESRI shp uses 16-bit DWORD as size unit
                    int uid = ShiftEndian(shpReader.ReadInt32());
                    shpReader.BaseStream.Seek(4L, SeekOrigin.Current);// length, unused
                    //getblob
                    byte[] blob = shpReader.ReadBytes(recordLength);
                    LVectorObject vo;
                    vo = ToLVectorObject(blob);
                    //FIXME:unnecesary?
                    Layer.Add(vo);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        LVectorObject ToLVectorObject(byte[] blob)
        {
            BinaryReader blobReader = new BinaryReader(new MemoryStream(blob));
            switch (GeometryType)
            {
                case  Lgis.GeometryType.Polygon:
                case Lgis.GeometryType.PolyPolygon:
                    LPolyPolygon polypolygon = new LPolyPolygon();
                    LPolygon polygon;
                    LPoint point;
                    blobReader.BaseStream.Seek(36L, SeekOrigin.Begin);
                    int nParts = blobReader.ReadInt32();
                    int nPoints = blobReader.ReadInt32();
                    int[] parts = new int[nParts + 1];
                    for (int i = 0; i < nParts; ++i)
                        parts[i] = blobReader.ReadInt32();
                    parts[nParts] = nPoints;
                    for (int i = 0; i < nParts; ++i)
                    {
                        polygon = new LPolygon();
                        for (int j = parts[i]; j < parts[i+1]; ++j)
                        {
                            byte[] pointBlob;
                            pointBlob = blobReader.ReadBytes(2 * sizeof(double));
                            point = new LPoint(pointBlob);
                            polygon.Add(point);
                        }
                        polypolygon.Add(polygon);
                    }
                    return polypolygon;
                case Lgis.GeometryType.Polyline:
                case Lgis.GeometryType.PolyPolyline:
                    LPolyPolyline polypolyline = new LPolyPolyline();
                    LPolyline polyline;
                    //LPoint point;
                    blobReader.BaseStream.Seek(36L, SeekOrigin.Begin);
                     nParts = blobReader.ReadInt32();
                     nPoints = blobReader.ReadInt32();
                     parts = new int[nParts + 1];
                    for (int i = 0; i < nParts; ++i)
                        parts[i] = blobReader.ReadInt32();
                    parts[nParts] = nPoints;
                    for (int i = 0; i < nParts; ++i)
                    {
                        polyline = new LPolyline();
                        for (int j = parts[i]; j < parts[i+1]; ++j)
                        {
                            byte[] pointBlob;
                            pointBlob = blobReader.ReadBytes(2 * sizeof(double));
                            point = new LPoint(pointBlob);
                            polyline.Add(point);
                        }
                        polypolyline.Add(polyline);
                    }
                    return polypolyline;
                case Lgis.GeometryType.Polypoint:
                    LPolyPoint polypoint = new LPolyPoint();
                    blobReader.BaseStream.Seek(36L, SeekOrigin.Begin);
                    nPoints = blobReader.ReadInt32();
                    for (int i = 0; i < nPoints; ++i)
                    {
                        polypoint.Add( new LPoint(blobReader.ReadBytes(sizeof(double) * 2)));
                    }
                    return polypoint;
                case Lgis.GeometryType.Point:
                    blobReader.BaseStream.Seek(4L, SeekOrigin.Begin);
                    return new LPoint(blobReader.ReadBytes(2*sizeof(double)));
                default:
                    throw new Exception("NotImplemented");
            }
        }

        public static UInt32 ShiftEndian(UInt32 i)
        {
            return ((i << 24) & 0xff000000) |
                ((i << 8) & 0x00ff0000) |
                ((i >> 8) & 0x0000ff00) |
                ((i >> 24) & 0x000000ff);
        }
        public static Int32 ShiftEndian(Int32 i)
        {
            UInt32 code = ShiftEndian((UInt32)i);
            return (Int32)code;
        }

    }
}
