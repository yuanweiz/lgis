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
        BinaryReader shpReader,shxReader;
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
                string Basename = BaseName(fileInfo.FullName);
                if (!File.Exists(Basename + ".shx") || !File.Exists(Basename + ".dbf"))
                    throw new LIOException("No .shx or .dbf found");
                
                Console.WriteLine(Basename);
                length = (int)fileInfo.Length;
                shpReader = new BinaryReader(
                    new FileStream(filename, FileMode.Open,FileAccess.Read));
                shxReader = new BinaryReader(
                    new FileStream(Basename+".shx",FileMode.Open,FileAccess.Read));
                fileInfo = new FileInfo(Basename + ".shx");
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
                Console.WriteLine(ShiftEndian(code));
                Console.WriteLine(nrecord);
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
/*
public List<GayData.GayGeometryObject.GayPoint> readpoint(BinaryReader br)
{
	GayData.GayGeometryObject.GayPoint p = new GayData.GayGeometryObject.GayPoint();
	uint n;
	uint l;
	int c;
	List<GayData.GayGeometryObject.GayPoint> points = new List<GayData.GayGeometryObject.GayPoint>();
	try
	{
		while (true)
		{
			n = BigtoLittle32(br.ReadUInt32());
			l = BigtoLittle32(br.ReadUInt32());
			c = br.ReadInt32();
			p.X = br.ReadDouble();
			p.Y = br.ReadDouble();
			if(c==11)
			{
				br.ReadDouble();
				br.ReadDouble();
			}
			else if(c==21)
			{
				br.ReadDouble();
			}
			points.Add(p);
			Console.WriteLine(n.ToString() + "   " + l.ToString() + "   " + c.ToString() + "   " + p.x.ToString() + "   " + p.y.ToString() + "   ");
		}
	}
	catch (Exception)
	{
		Console.WriteLine("读取结束！");
	}
	return points;
}
public List<GayData.GayGeometryObject.GayMultiPoint> readmultiPoint(BinaryReader br)
{
	uint n;
	uint l;
	int c;
	double Xmin, Ymin, Xmax, Ymax;
	int  np;
	GayData.GayGeometryObject.GayPoint p = new GayData.GayGeometryObject.GayPoint();
	List<GayData.GayGeometryObject.GayPoint> points = new List<GayData.GayGeometryObject.GayPoint>();
	List<GayData.GayGeometryObject.GayMultiPoint> multipoints=new  List<GayData.GayGeometryObject.GayMultiPoint>();

	try
	{
		while (true)
		{
			n = BigtoLittle32(br.ReadUInt32());
			l = BigtoLittle32(br.ReadUInt32());
			c = br.ReadInt32();
			Xmin = br.ReadDouble();
			Ymin = br.ReadDouble();
			Xmax = br.ReadDouble();
			Ymax = br.ReadDouble();
			np = br.ReadInt32();

			for (int i = 0; i < np; i++)
			{
				p.X = br.ReadDouble();
				p.Y = br.ReadDouble();
				points.Add(p);
				if (c == 18)
				{
					br.ReadDouble();
					br.ReadDouble();
				}
				else if (c == 28)
				{
					br.ReadDouble();
				}
			}

			multipoints.Add(new GayData.GayGeometryObject.GayMultiPoint(points));

			Console.Write(n.ToString() + "   " + l.ToString() + "   " + c.ToString() + "   " + Xmin.ToString() + "   " + Ymin.ToString() + "   " + Xmax.ToString() + "   " + Ymax.ToString() + "   " + Xmax.ToString() + "   ");



			for (int i = 0; i < np; i++)
			{
				Console.Write(points[i].x.ToString() + "   " + points[i].y.ToString() + "   ");
			}
			Console.Write("\n");
		}
	}
	catch (Exception)
	{
		Console.WriteLine("\n读取结束！");
	}
	return multipoints;

}
public List<GayData.GayGeometryObject.GayPolyline> readpolyline(BinaryReader br)
{
	uint n;
	uint l;
	int c;
	double Xmin, Ymin, Xmax, Ymax;
	int nl, np;
	GayData.GayGeometryObject.GayPoint p = new GayData.GayGeometryObject.GayPoint();
	List<GayData.GayGeometryObject.GayPoint> points = new List<GayData.GayGeometryObject.GayPoint>();
	List<int> parts = new List<int>();

	List<GayData.GayGeometryObject.GayPolyline> polylines = new List<GayData.GayGeometryObject.GayPolyline>();
	try
	{
		while (true)
		{
			n = BigtoLittle32(br.ReadUInt32());
			l = BigtoLittle32(br.ReadUInt32());
			c = br.ReadInt32();
			Xmin = br.ReadDouble();
			Ymin = br.ReadDouble();
			Xmax = br.ReadDouble();
			Ymax = br.ReadDouble();
			nl = br.ReadInt32();
			np = br.ReadInt32();
			for (int i = 0; i < nl; i++)
			{
				parts.Add(br.ReadInt32());
			}
			for (int i = 0; i < np; i++)
			{
				p.X = br.ReadDouble();
				p.Y = br.ReadDouble();
				points.Add(p);
				if (c == 13)
				{
					br.ReadDouble();
					br.ReadDouble();
				}
				else if (c == 23)
				{
					br.ReadDouble();
				}
			}

			polylines.Add(new GayData.GayGeometryObject.GayPolyline(points,parts));

			Console.Write(n.ToString() + "   " + l.ToString() + "   " + c.ToString() + "   " + Xmin.ToString() + "   " + Ymin.ToString() + "   " + Xmax.ToString() + "   " + Ymax.ToString() + "   " + Xmax.ToString() + "   ");
			for (int i = 0; i < nl; i++)
			{
				Console.Write(parts[i].ToString() + "   ");
			}


			for (int i = 0; i < np; i++)
			{
				Console.Write(points[i].x.ToString() + "   " + points[i].y.ToString() + "   ");
			}
			Console.Write("\n");
		}
	}
	catch (Exception)
	{
		Console.WriteLine("\n读取结束！");
	}
	return polylines;

}
public List<GayData.GayGeometryObject.GayPolygon> readpolygon(BinaryReader br)
{
	uint n;
	uint l;
	int c;
	double Xmin, Ymin, Xmax, Ymax;
	int nl, np;
	GayData.GayGeometryObject.GayPoint p = new GayData.GayGeometryObject.GayPoint();
	List<GayData.GayGeometryObject.GayPoint> points = new List<GayData.GayGeometryObject.GayPoint>();
	List<int> parts = new List<int>();

	List<GayData.GayGeometryObject.GayPolygon> polygons = new List<GayData.GayGeometryObject.GayPolygon>();
	try
	{
		while (true)
		{
			n = BigtoLittle32(br.ReadUInt32());
			l = BigtoLittle32(br.ReadUInt32());
			c = br.ReadInt32();
			Xmin = br.ReadDouble();
			Ymin = br.ReadDouble();
			Xmax = br.ReadDouble();
			Ymax = br.ReadDouble();
			nl = br.ReadInt32();
			np = br.ReadInt32();
			for (int i = 0; i < nl; i++)
			{
				parts.Add(br.ReadInt32());
			}
			for (int i = 0; i < np; i++)
			{
				p.X = br.ReadDouble();
				p.Y = br.ReadDouble();
				points.Add(p);
				if (c == 15)
				{
					br.ReadDouble();
					br.ReadDouble();
				}
				else if (c == 25)
				{
					br.ReadDouble();
				}
			}

			polygons.Add(new GayData.GayGeometryObject.GayPolygon(points, parts));

			Console.Write(n.ToString() + "   " + l.ToString() + "   " + c.ToString() + "   " + Xmin.ToString() + "   " + Ymin.ToString() + "   " + Xmax.ToString() + "   " + Ymax.ToString() + "   " + Xmax.ToString() + "   ");
			for (int i = 0; i < nl; i++)
			{
				Console.Write(parts[i].ToString() + "   ");
			}


			for (int i = 0; i < np; i++)
			{
				Console.Write(points[i].x.ToString() + "   " + points[i].y.ToString() + "   ");
			}
			Console.Write("\n");
		}
	}
	catch (Exception)
	{
		Console.WriteLine("\n读取结束！");
	}
	return polygons;

}
public GayData.GayMapObject.GayFeatureClass readfile(string filename)
{

	GayData.GayMapObject.GayFeatureClass featureclass = new GayData.GayMapObject.GayFeatureClass();

	FileStream fs = new FileStream(filename, FileMode.OpenOrCreate);
	BinaryReader br = new BinaryReader(fs);
	uint[] ftb = new uint[7];
	int[] ftl = new int[2];
	double[] B = new double[8];
	for (int i = 0; i < 7; i++)
	{
		ftb[i] = BigtoLittle32(br.ReadUInt32());
		Console.WriteLine(ftb[i]);
	}
	ftl[0] = br.ReadInt32();
	ftl[1] = br.ReadInt32();
	Console.WriteLine(ftl[0]);
	Console.WriteLine(ftl[1]);
	for (int i = 0; i < 8; i++)
	{
		B[i] = br.ReadDouble();
		Console.WriteLine(B[i]);
	}

	switch(ftl[1])
	{
		case 1:
			featureclass.Type = GayData.GayMapObject.GayFeatureType.GayPoint;

			break;
		case 3:
			featureclass.Type = GayData.GayMapObject.GayFeatureType.GayPolyline;

			break;
		case 5:
			featureclass.Type = GayData.GayMapObject.GayFeatureType.GayPolygon;

			break;
		case 8:
			featureclass.Type = GayData.GayMapObject.GayFeatureType.GayMultiPoint;

			break;
		case 11:
			featureclass.Type = GayData.GayMapObject.GayFeatureType.GayPoint;

			break;
		case 13:
			featureclass.Type = GayData.GayMapObject.GayFeatureType.GayPolyline;

			break;
		case 15:
			featureclass.Type = GayData.GayMapObject.GayFeatureType.GayPolygon;

			break;
		case 18:
			featureclass.Type = GayData.GayMapObject.GayFeatureType.GayMultiPoint;

			break;
		case 21:
			featureclass.Type = GayData.GayMapObject.GayFeatureType.GayPoint;

			break;
		case 23:
			featureclass.Type = GayData.GayMapObject.GayFeatureType.GayPolyline;

			break;
		case 25:
			featureclass.Type = GayData.GayMapObject.GayFeatureType.GayPolygon;

			break;
		case 28:
			featureclass.Type = GayData.GayMapObject.GayFeatureType.GayMultiPoint;

			break;
	}

}
 * */
