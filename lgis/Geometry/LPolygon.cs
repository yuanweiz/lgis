using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lgis
{

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

    public class LPolyPolygon : LVectorObject, IEnumerable<LPolygon>
    {
        #region enumerable properties
        public IEnumerator<LPolygon> GetEnumerator()
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
        public override LEnvelope Envelope
        {
            get
            {
                LEnvelope envelope = LEnvelope.Null;
                foreach (LPoint p in this.Vertices)
                    envelope += p.Envelope;
                return envelope;
            }
        }
    }

}
