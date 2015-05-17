using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Lgis
{

    /// <summary>
    /// 矢量要素类的基类
    /// </summary>
    public class LVectorObject : LMapObject 
    {
        public GeometryType GeometryType { get; protected set; }
        public LVectorObject() : base(ObjectType.Vector) { GeometryType = Lgis.GeometryType.Unknown; }
        public LVectorObject(GeometryType t) : base(ObjectType.Vector) { GeometryType = t; }
        public virtual IEnumerable<LPoint> Vertices { get {  yield break; } }
        public virtual IEnumerable<LLineseg> Edges { get {  yield break; } }

        public virtual int BlobSize
        {
            get { return -1; }
        }
        internal virtual byte[] AsBlob()
        {
            byte[] bytes=new byte[0];
            return bytes;
        }
    }

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
                //return new LEnvelope(X, X, Y, Y);
                return new LEnvelope(X, Y, X, Y);
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
        public override string Info()
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

        internal override byte[] AsBlob()
        {
            byte[] bytes = new byte[BlobSize];
            unsafe
            {
                fixed (byte* pbyte = bytes)
                {

                }
            }
            return bytes;
        }
        public override int BlobSize
        {
            get
            {
                return 2 * sizeof(double);
            }
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
        public override string Info()
        {
            string s = "Polypolyline:\n";
            foreach (LPolyline pl in Polylines)
            {
                s = s + pl.Info() + "\n";
            }
            return s;
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
        public override string Info()
        {
            string s = "Lineseg:\n" + A.Info() + " " + B.Info();
            return s;
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
                foreach (LPoint p in this.Vertices)
                {
                    l += p.Envelope;
                }
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

        public override string Info()
        {
            string s = "PolyPoint:\n";
            foreach (LPoint p in this.Vertices)
            {
                s = s + p.Info() + "\n";
            }
            return s;
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

        public override string Info()
        {
            string s = "Polyline:\n";
            foreach (LPoint p in this.Vertices)
                s = s + p.Info() + "\n";
            return s;
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
        public override string Info()
        {
            string s="Polygon:\n";
            foreach (LPoint p in this.Vertices){
                s = s + p.Info() + "\n";
            }
            return s;
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
        public override string Info()
        {
            string s = "Polypolygon:\n";
            foreach (LPolygon plg in Polygons){
                s = s + plg.Info() + "\n";
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
