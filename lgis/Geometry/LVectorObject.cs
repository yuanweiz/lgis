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
                Envelope = new LEnvelope(_X, _Y, _X, _Y);
                Owner.RefreshEnvelope();
            }
        }
        public double Y
        {
            get { return _Y;}
            set { 
                _Y = value;
                Envelope = new LEnvelope(_X, _Y, _X, _Y);
                Owner.RefreshEnvelope();
            }
        }
        double _X, _Y;
        public LPoint(double x = 0, double y = 0) : base(GeometryType.Point)
        {
            X = x;
            Y = y;
            Owner.RefreshEnvelope();
        }
        public LPoint(LPoint p):base(GeometryType.Point)
        {
            X = p.X;
            Y = p.Y;
            Owner.RefreshEnvelope();
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
        public LPoint Copy()
        {
            return new LPoint(this);
        }
    }
    public class LPolyPolyline : LVectorObject
    {
        public LPolyPolyline()
            : base(GeometryType.PolyPolyline)
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
            pl.Owner = this;
            RefreshEnvelope();
        }
        public void RemoveAt(int idx)
        {
            Lines[idx].Owner = LMapObject.Null;
            Lines.RemoveAt(idx);
            RefreshEnvelope();
        }
        internal override void RefreshEnvelope()
        {
            Envelope = LEnvelope.Null;
            foreach (LPolyline pl in Lines)
            {
                Envelope += pl.Envelope;
            }
            Owner.RefreshEnvelope();
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

    public class LVector : LVectorObject
    {
        public double X, Y;
        public LVector ( double x,double y){X=x;Y=y;}
        public static LVector operator + (LVector a,LVector b)
        {
            return new LVector (a.X+b.X,a.Y+b.Y);
        }
        public static LVector operator - (LVector a,LVector b)
        {return new LVector ( a.X-b.X,a.Y-b.Y);}

        public static double operator *(LVector a, LVector b)
        {
            return a.X * b.X + a.Y * b.Y;
        }
    }

    public class LLineseg : LVectorObject
    {
        public LPoint A, B;
        internal override void RefreshEnvelope()
        {
            Envelope = A.Envelope + B.Envelope;
            Owner.RefreshEnvelope();
        }
    }
    public class LPolyPoint : LVectorObject
    {
        public LPolyPoint() : base(GeometryType.Polypoint) { }
        public LPolyPoint(int capacity) : base(GeometryType.Polypoint) { 
            Points = new List<LPoint>(capacity);
        }
        public LPolyPoint(List<LPoint> points)
            : base(GeometryType.Polypoint)
        {
            Points = points;
            RefreshEnvelope();
        }
        public List<LPoint> Points = new List<LPoint>();
        public int Count { get { return Points.Count; } }
        public LPoint this[int idx]
        {
            get { return Points[idx]; }
            set { Points[idx] = value; }
        }
        public void Add(LPoint p)
        {
            p.Owner = this;
            Points.Add(p);
            RefreshEnvelope();
        }
        public void RemoveAt(int idx)
        {
            Points[idx].Owner = LMapObject.Null;
            Points.RemoveAt(idx);
            RefreshEnvelope();
        }
        public LPolyPoint Copy(){
            LPolyPoint newpp = new LPolyPoint(Count);
            foreach (LPoint p in Points ){
                newpp.Add(p.Copy());
            }
            return newpp;
        }

        public LPolygon ToLPolygon(){
            return new LPolygon(Points);
        }
        public LPolyline ToLPolyline()
        {
            return new LPolyline(Points);
        }

        internal override void RefreshEnvelope()
        {
            Envelope = LEnvelope.Null;
            foreach (LPoint p in Points)
            {
                Envelope += p.Envelope;
            }
            Owner.RefreshEnvelope();
        }
    }

    public class LPolyline : LPolyPoint
    {
        public LPolyline(): base()
        {
            GeometryType = GeometryType.Polyline;
        }
        public LPolyline(List<LPoint> lp) : base(lp)
        {
            GeometryType = GeometryType.Polyline;
        }
        public LPolyline(int capacity)
            : base(capacity)
        {
            GeometryType = GeometryType.Polyline;
        }
        public new LPolyline Copy()
        {
            return base.Copy().ToLPolyline();
        }
    }
    public class LPolygon : LPolyPoint {
        public LPolygon():base()
        {
            GeometryType = GeometryType.Polygon;
        }
        public LPolygon(List<LPoint> lp)
            : base(lp)
        {
            GeometryType = GeometryType.Polygon;
        }
        public LPolygon(int capacity)
            : base(capacity)
        {
            GeometryType = GeometryType.Polygon;
        }
        public new LPolygon Copy()
        {
            return base.Copy().ToLPolygon();
        }
    }

    public class LPolyPolygon : LVectorObject
    {
        public LPolyPolygon() : base(GeometryType.PolyPolygon) { }
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
            p.Owner = this;
            Polygons.Add(p);
            RefreshEnvelope();
        }
        public void RemoveAt(int idx)
        {
            Polygons[idx].Owner = LMapObject.Null;
            Polygons.RemoveAt(idx);
            RefreshEnvelope();
        }
        internal override void RefreshEnvelope()
        {
            Envelope = LEnvelope.Null;
            foreach (LPolygon p in Polygons)
            {
                Envelope += p.Envelope;
            }
            Owner.RefreshEnvelope();
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
        public LRectangle(double _xmin, double _ymin, double _xmax, double _ymax) : base(GeometryType.Rectangle)
        {
        }
        public LRectangle(LRectangle r) :base(GeometryType.Rectangle)
        {
        }
    }

}
