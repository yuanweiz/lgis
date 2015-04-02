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
        public LPoint(double x, double y) : base(FeatureType.Point)
        {
            X = x;
            Y = y;
            Owner.RefreshEnvelope();
        }
        public LPoint(LPoint p):base(FeatureType.Point)
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

    public class LPolyPoint : LVectorObject
    {
        public LPolyPoint() : base(FeatureType.Polypoint) { }
        public LPolyPoint(int capacity) : base(FeatureType.Polypoint) { 
            Points = new List<LPoint>(capacity);
        }
        public LPolyPoint(List<LPoint> points)
            : base(FeatureType.Polypoint)
        {
            Points = points;
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
            FeatureType = FeatureType.Polyline;
        }
        public LPolyline(List<LPoint> lp) : base(lp)
        {
            FeatureType = FeatureType.Polyline;
        }
        public LPolyline(int capacity)
            : base(capacity)
        {
            FeatureType = FeatureType.Polyline;
        }
        public LPolyline Copy()
        {
            return base.Copy().ToLPolyline();
        }
    }
    public class LPolygon : LPolyPoint {
        public LPolygon():base()
        {
            FeatureType = FeatureType.Polygon;
        }
        public LPolygon(List<LPoint> lp)
            : base(lp)
        {
            FeatureType = FeatureType.Polygon;
        }
        public LPolygon(int capacity)
            : base(capacity)
        {
            FeatureType = FeatureType.Polygon;
        }
        public new LPolygon Copy()
        {
            return base.Copy().ToLPolygon();
        }
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
        public LRectangle(double _xmin, double _ymin, double _xmax, double _ymax) : base(FeatureType.Rectangle)
        {
        }
        public LRectangle(LRectangle r) :base(FeatureType.Rectangle)
        {
        }
    }

}
