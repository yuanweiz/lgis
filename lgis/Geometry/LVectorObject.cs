using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Lgis
{
    public class LPoint:LVectorObject
    {
        public double X
        {
            get;
            set;
        }
        public double Y
        {
            get;
            set;
        }
        public override LEnvelope Envelope
        {
            get
            {
                return new LEnvelope(X, X, Y, Y);
            }
        }
        public LPoint(double x = 0, double y = 0) : base(GeometryType.Point)
        {
            X = x;
            Y = y;
        }
        public LPoint(LPoint p):base(GeometryType.Point)
        {
            X = p.X;
            Y = p.Y;
        }
        public override string ToString()
        {
            string s="";
            s += "(" + X.ToString() + "," + Y.ToString() + ")";
            return s;
        }
        public static implicit operator Point(LPoint p){
            return new Point((int)p.X, (int)p.Y);
        }
        public static LLineseg operator +(LPoint A, LPoint B)
        {
            return new LLineseg(A, B);
        }
        public static LVector operator -(LPoint A, LPoint B)
        {
            return new LVector(B.X - A.X, B.Y - A.Y);
        }
        public LPoint Copy()
        {
            return new LPoint(this);
        }
    }

    public class LPolyPolyline : LVectorObject
    {
        #region Enumerable Properties

        public LPolyline[] Vertices {
            get { return Lines.ToArray(); }
        }

        #endregion
        public LPolyPolyline()
            : base(GeometryType.PolyPolyline)
        {
        }
        List<LPolyline> Lines = new List<LPolyline>();
        public int Count { get { return Lines.Count; } }
        public override LEnvelope Envelope
        {
            get
            {
                LEnvelope l = LEnvelope.Null;
                for ( int i =0;i< Count;++i)
                    l += Lines[i].Envelope;
                return l;
            }
        }
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
        }
        public void RemoveAt(int idx)
        {
            Lines[idx].Owner = LMapObject.Null;
            Lines.RemoveAt(idx);
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
        public LVector ( double x=0,double y=0){X=x;Y=y;}
        public static LVector operator + (LVector a,LVector b)
        {
            return new LVector (a.X+b.X,a.Y+b.Y);
        }
        public static LVector operator - (LVector a,LVector b)
        {return new LVector ( a.X-b.X,a.Y-b.Y);}
        public static LVector operator * (LVector a , double k)
        { 
            return new LVector(a.X * k, a.Y * k); 
        }
        public static double operator *(LVector a, LVector b)
        {
            return a.X * b.X + a.Y * b.Y;
        }
        public double Norm()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

    }

    public class LLineseg : LVectorObject
    {
        public LLineseg() { }
        public LLineseg(LPoint A, LPoint B)
        {
            this.A = A;
            this.B = B;
        }
        public LPoint A, B;
        public override LEnvelope Envelope
        {
            get
            {
                return A.Envelope + B.Envelope;
            }
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
        }
        public override LEnvelope Envelope
        {
            get
            {
                LEnvelope l = LEnvelope.Null;
                for (int i = 0; i < Count; ++i)
                    l += Points[i].Envelope;
                return l;
            }
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
        }
        public void RemoveAt(int idx)
        {
            Points[idx].Owner = LMapObject.Null;
            Points.RemoveAt(idx);
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
        }
        public void RemoveAt(int idx)
        {
            Polygons[idx].Owner = LMapObject.Null;
            Polygons.RemoveAt(idx);
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
