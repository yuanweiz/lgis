using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Lgis
{
    public class LPoint:LVectorObject
    {
        public double X
        {
            get { return _X;}
            set { 
                _X = value;
                _Envelope = new LEnvelope(_X, _Y, _X, _Y);
            }
        }
        public double Y
        {
            get { return _Y;}
            set { 
                _Y = value;
                _Envelope = new LEnvelope(_X, _Y, _X, _Y);
            }
        }
        double _X, _Y;
        public LPoint(double x, double y) : base(FeatureType.Point)
        {
            X = x;
            Y = y;
            _Owner.RefreshEnvelope();
        }
        public LPoint(LPoint p):base(FeatureType.Point)
        {
            X = p.X;
            Y = p.Y;
            _Owner.RefreshEnvelope();
        }
        public override string ToString()
        {
            string s="";
            s += "(" + _X.ToString() + "," + _Y.ToString() + ")";
            return s;
        }
        public static implicit operator Point(LPoint p){
            return new Point((int)p.X, (int)p.Y);
        }
    }
    public class LPolyline : LVectorObject
    {
        List<LPoint> Points = new List<LPoint>();
        public int Count { get { return Points.Count; } }
        public static explicit operator List<LPoint>(LPolyline pl)
        {
            return pl.Points;
        }
        public LPoint this[int index]
        {
            get{ return Points[index];}
            set { Points[index] = value; }
        }
        public LPolyline(List<LPoint> lp):base(FeatureType.Polyline)
        {
            Points = lp;
        }
        public LPolyline():base(FeatureType.Polyline) { }
        public void Add (LPoint p){
            p._Owner = this;
            Points.Add(p);
            RefreshEnvelope();
        }
        public void RemoveAt(int idx)
        {
            Points[idx]._Owner = LMapObject.Null;
            Points.RemoveAt(idx);
            RefreshEnvelope();
        }
        public static explicit operator LPoint[](LPolyline p)
        {
            return p.Points.ToArray();
        }
        public static explicit operator Point[](LPolyline p)
        {
            Point[] pts = new Point[p.Count];
            for (int i = 0; i < p.Count; ++i)
            {
                pts[i] = new Point((int)p[i].X, (int)p[i].Y);
            }
            return pts;
        }
        internal override void RefreshEnvelope()
        {
            _Envelope = LEnvelope.Null;
            foreach (LPoint p in Points)
            {
                _Envelope += p.Envelope;
            }
            _Owner.RefreshEnvelope();
        }
        #region debug
        public override string ToString()
        {
            string s = "";
            foreach (LPoint p in Points)
            {
                s += p.ToString();
            }
            return s;
        }
        #endregion
    }
    public class LPolyPolyline : LVectorObject
    {
        public LPolyPolyline()
            : base(FeatureType.PolyPolyline)
        {
        }
        List<LPolyline> Lines = new List<LPolyline>();
        public int Count { get { return Lines.Count; } }
        public LPolyline this[int idx] {
            get
            {
                return Lines[idx];
            }
            set
            {
                Lines[idx] = value;
            }
        }
        public void Add (LPolyline pl){
            Lines.Add(pl);
            pl._Owner = this;
            RefreshEnvelope();
        }
        public void RemoveAt(int idx)
        {
            Lines[idx]._Owner = LMapObject.Null;
            Lines.RemoveAt(idx);
            RefreshEnvelope();
        }
        internal override void RefreshEnvelope()
        {
            _Envelope = LEnvelope.Null;
            foreach (LPolyline pl in Lines)
            {
                _Envelope += pl.Envelope;
            }
            _Owner.RefreshEnvelope();
        }
        public override string ToString()
        {
            string s = "in the polypolyline:\n";
            foreach (LPolyline l in Lines ){
                s += l.ToString();
                s += "\n";
            }
            return s;
        }
    }
    public class LPolyPoint : LVectorObject
    {
        public LPolyPoint() : base(FeatureType.Polypoint) { }
        public List<LPoint> Points = new List<LPoint>();
        public int Count { get { return Points.Count; } }
        public LPoint this[int idx]
        {
            get { return Points[idx]; }
            set { Points[idx] = value; }
        }
        public void Add(LPoint p)
        {
            p._Owner = this;
            Points.Add(p);
            RefreshEnvelope();
        }
        public void RemoveAt(int idx)
        {
            Points[idx]._Owner = LMapObject.Null;
            Points.RemoveAt(idx);
            RefreshEnvelope();
        }
        internal override void RefreshEnvelope()
        {
            _Envelope = LEnvelope.Null;
            foreach (LPoint p in Points)
            {
                _Envelope += p.Envelope;
            }
            _Owner.RefreshEnvelope();
        }
    }

    public class LPolygon : LVectorObject
    {
        List<LPoint> Points = new List<LPoint>();
        public int Count { get { return Points.Count; } }
        public static explicit operator List<LPoint>(LPolygon pl)
        {
            return pl.Points;
        }
        public LPoint this[int index]
        {
            get{ return Points[index];}
            set { Points[index] = value; }
        }
        public LPolygon(List<LPoint> lp):base (FeatureType.Polygon)
        {
            Points = lp;
        }
        public LPolygon():base(FeatureType.Polygon) { }
        public void Add(LPoint p)
        {
            p._Owner = this;
            Points.Add(p);
            RefreshEnvelope();
        }
        public void RemoveAt(int idx)
        {
            Points[idx]._Owner = LMapObject.Null;
            Points.RemoveAt(idx);
            RefreshEnvelope();
        }
        internal override void RefreshEnvelope()
        {
            _Envelope = LEnvelope.Null;
            foreach (LPoint p in Points)
            {
                _Envelope += p.Envelope;
            }
            _Owner.RefreshEnvelope();
        }
        #region debug
        public override string ToString()
        {
            string s = "";
            foreach (LPoint p in Points)
            {
                s += ("(" + p.X.ToString() + "," + p.Y.ToString() + ")");
            }
            return s;
        }
        #endregion
    }
    public class LPolyPolygon : LVectorObject
    {
        public LPolyPolygon() : base(FeatureType.PolyPolygon) { }
        List<LPolygon> Polygons = new List<LPolygon>();
        public int Count { get { return Polygons.Count; } }
        public LPolygon this[int idx] {
            get
            {
                return Polygons[idx];
            }
            set
            {
                Polygons[idx] = value;
            }
        }
        public void Add (LPolygon p){
            p._Owner = this;
            Polygons.Add(p);
            RefreshEnvelope();
        }
        public void RemoveAt(int idx)
        {
            Polygons[idx]._Owner = LMapObject.Null;
            Polygons.RemoveAt(idx);
            RefreshEnvelope();
        }
        internal override void RefreshEnvelope()
        {
            _Envelope = LEnvelope.Null;
            foreach (LPolygon p in Polygons)
            {
                _Envelope += p.Envelope;
            }
            _Owner.RefreshEnvelope();
        }
        public override string ToString()
        {
            string s = "in the polypolygon:\n";
            foreach (LPolygon p in Polygons ){
                s += p.ToString();
                s += "\n";
            }
            return s;
        }
    }
    public class LRectangle : LVectorObject
    {
        public LRectangle(double _xmin, double _ymin, double _xmax, double _ymax) : base(FeatureType.Rectangle)
        {
        }
        public LRectangle(LRectangle r) :base(FeatureType.Rectangle)
        {
        }
    }

}
