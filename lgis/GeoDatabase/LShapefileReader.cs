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
                int dbfRecordLen = dbfReader.ReadInt16();
                Console.WriteLine(dbfRecordLen);
                dbfReader.BaseStream.Seek(32L, SeekOrigin.Begin);
                //Read FieldDescribe
                char fieldType;
                LDataTable table = Layer.DataTable;
                Dictionary<DataColumn, int> fieldLengths = new Dictionary<DataColumn,int>();
                while (dbfReader.PeekChar()!=0x0d)
                {
                    byte[] fieldDesc = dbfReader.ReadBytes(32);
                    string columnName;
                    byte fieldLen;
                    unsafe
                    {
                        fixed (byte* pchar = fieldDesc)
                        {
                            columnName = "";
                            for (int i = 0; i < 10; ++i)
                            {
                                if (pchar[i] != 0x00)
                                    columnName += (char)pchar[i];
                            }
                            fieldType = (char)pchar[11];
                            fieldLen = pchar[16];
                            
                        }
                    }
                    DataColumn col ;
                    switch (fieldType)
                    {
                        case 'C':
                            col = new DataColumn(columnName,typeof(string));
                            break;
                        case 'N':
                            col = new DataColumn(columnName, typeof(double));
                            //col = new DataColumn(columnName, typeof(object));
                            //FIXME: dbf numeric field really sucks!
                            break;
                        case 'D':
                        case 'F':
                            col = new DataColumn(columnName,typeof(double));
                            break;
                        case 'I':
                        case 'L':
                            col = new DataColumn(columnName,typeof(int));
                            break;
                        default:
                            throw new LNotImplementedException("Unrecognized dbf field");
                    }
                    Console.WriteLine(fieldType);
                    col.ColumnName = columnName;
                    table.Columns.Add(col);
                    fieldLengths[col] = (int)fieldLen;
                }
                //Ready to read record header
                shxReader.BaseStream.Seek(100L, SeekOrigin.Begin);
                shpReader.BaseStream.Seek(100L, SeekOrigin.Begin);
                dbfReader.BaseStream.Seek(1L, SeekOrigin.Current); //Skip the 0x0D end flag

                for (int i = 0; i < nrecord; ++i)
                {
                    int offset = ShiftEndian(shxReader.ReadInt32());
                    int shpRecordLen = ShiftEndian(shxReader.ReadInt32()) * 2; // ESRI shp uses 16-bit short as size unit
                    int uid = ShiftEndian(shpReader.ReadInt32());
                    shpReader.BaseStream.Seek(4L, SeekOrigin.Current);// length, unused
                    //getblob
                    byte[] shpblob = shpReader.ReadBytes(shpRecordLen);
                    byte[] dbfblob = dbfReader.ReadBytes(dbfRecordLen);//TAG
                    //Console.WriteLine(BlobToString(dbfblob));
                    LVectorObject vo;
                    DataRow row = table.NewRow();
                    vo = ToLVectorObject(shpblob);
                    //Layer.Add(vo); // Should use new api Layers.Rows.Add
                    int start = 1; //The first is space ' ' or star '*'
                    for (int j = 2; j < table.Columns.Count; ++j)
                    {
                        DataColumn col = table.Columns[j];
                        int fieldLen = fieldLengths[col];
                        unsafe{
                            fixed (byte * pchar = dbfblob){
                                if (col.DataType == typeof(string))
                                {
                                    string s = System.Text.Encoding.ASCII.GetString(dbfblob, start, fieldLen);
                                    row[col] = s;
                                }
                                else if (col.DataType == typeof(double))
                                {
                                    string s = System.Text.Encoding.ASCII.GetString(dbfblob, start, fieldLen);
                                    row[col] = System.Convert.ToDouble( decimal.Parse(s));
                                }
                                else if (col.DataType == typeof(int))
                                {
                                    throw new LNotImplementedException("Int Type not implemented");
                                }
                            }
                        }
                        start += fieldLen;
                    }
                    row["Geometry"] = vo;
                    table.Rows.Add(row);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (dbfReader != null)
                    dbfReader.Close();
                if (shpReader != null)
                    shpReader.Close();
                if (shxReader != null)
                    shxReader.Close();
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

        public static string BlobToString(byte[] blob)
        {
            string s="";
            byte b;
            for (int i = 0; i < blob.Length; ++i)
            {
                b = blob[i];
                if (b >= 0x20 && b <= 126)
                    s += (char)b;
                else
                    s += "0x??";
            }
            return s;
        }

    }
}
