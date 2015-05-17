using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lgis
{
    public class LShapefileReader
    {
        public static UInt32 ShiftEndian(UInt32 i){
            return ((i << 24) & 0xff000000) |
                ((i << 8) & 0x00ff0000) |
                ((i >> 8) & 0x0000ff00) |
                ((i >> 24) & 0x000000ff);
        }
    }
}
/*
 *public uint BigtoLittle32(uint A)
{
	return ((((uint)(A) & 0xff000000) >> 24) | (((uint)(A) & 0x00ff0000) >> 8) | (((uint)(A) & 0x0000ff00) << 8) | (((uint)(A) & 0x000000ff) << 24));
}

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
长弓牧影  15:44:49

	长弓牧影  15:44:53
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
