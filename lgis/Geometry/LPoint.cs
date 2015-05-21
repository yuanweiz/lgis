using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

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
                //return new LEnvelope(X, X, Y, Y);
                return new LEnvelope(X, Y, X, Y);
            }
        }
        public LPoint(byte[] blob):base (GeometryType.Point)
        {
            unsafe
            {
                fixed (byte* p = blob)
                {
                    X = *((double*)p);
                    Y = *((double*)(p+sizeof(double)));
                }
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

}
