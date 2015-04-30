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

        public override IEnumerable<LPoint> Vertices
        {
            get
            {
                foreach (LPolyline pl in Polylines)
                    foreach (LPoint p in pl.Vertices)
                        yield return p;
            }
        }

        public override IEnumerable<LLineseg> Edges
        {
            get
            {
                foreach (LPolyline pl in Polylines)
                    foreach (LLineseg ls in pl.Edges)
                        yield return ls;
            }
        }

        #endregion
        public LPolyPolyline()
            : base(GeometryType.PolyPolyline)
        {
        }

        List<LPolyline> Polylines = new List<LPolyline>();

        public int Count { get { return Polylines.Count; } }

        public override LEnvelope Envelope
        {
            get
            {
                LEnvelope l = LEnvelope.Null;
                for ( int i =0;i< Count;++i)
                    l += Polylines[i].Envelope;
                return l;
            }
        }

        public LPolyline this[int idx] {
            get
            {
                return Polylines[idx];
            }
            set
            {
                Polylines[idx] = value;
            }
        }

        public void Add (LPolyline pl){
            Polylines.Add(pl);
            pl.Owner = this;
        }
        public void RemoveAt(int idx)
        {
            Polylines[idx].Owner = LMapObject.Null;
            Polylines.RemoveAt(idx);
        }

        public override string ToString()
        {
            string s = "in the polypolyline:\n";
            foreach (LPolyline l in Polylines ){
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
        public override IEnumerable<LPoint> Vertices
        {
            get
            {
                yield return A;
                yield return B;
            }
        }

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
        #region Enumerable interfaces

        public override IEnumerable<LPoint> Vertices
        {
            get
            {
                for (int i = 0; i < Count; ++i)
                    yield return Points[i];
            }
        }

        #endregion
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

        protected List<LPoint> Points = new List<LPoint>();
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

        #region enumerable properties

        public override IEnumerable<LLineseg> Edges
        {
            get
            {
                for (int i = 0; i < Count - 1; ++i)
                    yield return new LLineseg(this[i], this[i + 1]);
            }
        }

        #endregion
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

    public class LPolygon : LPolyPoint
    {
        #region enumerable properties

        public override IEnumerable<LLineseg> Edges
        {
            get
            {
                if (Count < 2)
                    yield break;
                LPoint A;
                A = Points[Count - 1];
                for (int i = 0; i < Count -1; ++i)
                {
                    yield return new LLineseg(this[i], this[i+1]);
                }
                yield return new LLineseg(this[0], A);
            }
        }

        #endregion
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
        #region enumerable properties

        public override IEnumerable<LPoint> Vertices
        {
            get
            {
                foreach (LPolygon plg in Polygons)
                {
                    foreach (LPoint p in plg.Vertices)
                        yield return p;
                }
            }
        }

        public override IEnumerable<LLineseg> Edges
        {
            get {
                foreach (LPolygon plg in Polygons)
                {
                    foreach (LLineseg ls in plg.Edges)
                        yield return ls;
                }
            }
        }

        #endregion

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
