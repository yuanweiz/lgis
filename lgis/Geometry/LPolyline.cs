using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lgis
{

    public class LPolyPolyline : LVectorObject, IEnumerable<LPolyline>
    {
        #region Enumerable Properties
        
        public IEnumerator<LPolyline> GetEnumerator()
        {
            for (int i = 0; i < Count; ++i)
                yield return this[i];
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

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
}
