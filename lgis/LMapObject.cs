using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lgis
{
    public enum ObjectType { Vector, Raster, LayerGroup, Unknown };
    public enum FeatureType { Point, Polygon, Polyline, PolyPolyline, PolyPolygon, Polypoint,Rectangle,Unknown };
    public class LMapObject
    {
        public static LMapObject Null = new LMapObject();

        public readonly ObjectType ObjectType=ObjectType.Unknown;
        public LMapObject Owner {get{return _Owner;}}
        internal LMapObject _Owner = LMapObject.Null;
        public LEnvelope Envelope{ get{return _Envelope;}}
        internal LEnvelope _Envelope = LEnvelope.Null;
        public double XMin {get{return _Envelope.XMin ;}}
        public double XMax {get{return _Envelope.XMax ;}}
        public double YMin {get{return _Envelope.YMin ;}}
        public double YMax {get{return _Envelope.YMax ;}}
        public double Width { get { return XMax - XMin; } }
        public double Height { get { return YMax - YMin; } }
        public LMapObject () { }
        public LMapObject (ObjectType t){ObjectType = t;}
        internal virtual void RefreshEnvelope()
        {
            if (this != LMapObject.Null)
                throw new NotImplementedException("RefreshEnvelope() not implemented");
        }
    }
    public class LVectorObject : LMapObject
    {
        public readonly FeatureType FeatureType ;
        public LVectorObject() : base(ObjectType.Vector) { FeatureType = Lgis.FeatureType.Unknown; }
        public LVectorObject(FeatureType t) : base(ObjectType.Vector) { FeatureType = t; }
    }
    public class LRasterObject : LMapObject
    {
        public readonly FeatureType FeatureType ;
        public LRasterObject():base (ObjectType.Raster){}
    }
    public class LEnvelope
    {
        public static LEnvelope Null = new LEnvelope();
        public double XMax { get { return xmax; } }
        public double XMin { get { return xmin; } }
        public double YMax { get { return ymax; } }
        public double YMin { get { return ymin; } }
        double
            xmax = double.NaN,
            xmin = double.NaN,
            ymax = double.NaN,
            ymin = double.NaN;
        public LEnvelope() { }
        public LEnvelope(double _xmin, double _ymin, double _xmax, double _ymax)
        {
            xmin = _xmin;
            xmax = _xmax;
            ymin = _ymin;
            ymax = _ymax;
        }
        public static LEnvelope operator +(LEnvelope a, LEnvelope b)
        {
            if (!a.IsNull() && !b.IsNull())
            {
                return new LEnvelope(Math.Min(a.xmin, b.xmin),
                    Math.Min(a.ymin, b.ymin),
                    Math.Max(a.xmax, b.xmax),
                    Math.Max(a.ymax, b.ymax));
            }
            else if (a.IsNull())
                return b;
            else
                return a;
        }
        public bool IsNull()
        {
            return (double.IsNaN(xmin) || double.IsNaN(ymin) || double.IsNaN(xmax) || double.IsNaN(ymax));
        }
    }
}
