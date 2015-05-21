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
