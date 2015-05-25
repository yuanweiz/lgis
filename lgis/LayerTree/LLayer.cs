using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Lgis
{

    /// <summary>
    /// 图层数据结构，提供矢量与栅格对象的容器
    /// </summary>
    public class LLayer: LMapObject 
    {
        public readonly LayerType LayerType;
        public LDataTable DataTable { get; protected set; }
        public bool Visible = true;

        protected LLayer (LayerType ft):base(ObjectType.Layer){
            LayerType= ft;
        }

        public LLayerGroup AsLayerGroup()
        {
            LLayerGroup g = new LLayerGroup();
            g.Add(this);
            return g;
        }

        public static explicit operator LLayerGroup (LLayer l){
            LLayerGroup lg = new LLayerGroup();
            lg.Add(l);
            return lg;
        }
    }

    /// <summary>
    /// 矢量图层，是LVectorObject的容器
    /// </summary>
    public class LVectorLayer : LLayer, IEnumerable<LVectorObject>
    {
        public IEnumerator<LVectorObject> GetEnumerator()
        {
            for (int i = 0; i < Count; ++i)
                yield return this[i];
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region 私有字段

        //protected List<LVectorObject> VectorObjects = new List<LVectorObject>();
        protected IEnumerable<LVectorObject> VectorObjects
        {
            get
            {
                return from row in DataTable.AsEnumerable()
                       select (LVectorObject)row["Geometry"];
            }
        }
        public readonly FeatureType FeatureType;
        
        #endregion

        #region 属性
        /// <summary>
        /// 访问图层中的第idx个VectorObject对象
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public LVectorObject this[int idx]
        {
            get
            {
                return (LVectorObject)DataTable.Rows[idx]["Geometry"];
            }
        }
        public int Count { get { return DataTable.Rows.Count; } }

        //public LDataTable DataTable { get; private set; }

        public override LEnvelope Envelope
        {
            get
            {
                LEnvelope l = LEnvelope.Null;
                foreach (LVectorObject vo in VectorObjects)
                    l += vo.Envelope;
                return l;
            }
        }
        
        #endregion

        #region 方法
        public LVectorLayer(FeatureType featureType = Lgis.FeatureType.Unknown)
            : base(LayerType.Vector)
        {
            FeatureType = featureType;
            DataTable = new LDataTable(this);
        }
        public virtual void Add(LVectorObject vo)
        {
            vo.Owner = this;
            DataRow row = DataTable.NewRow();
            row["Geometry"] = vo;
            DataTable.Rows.Add(row);
        }
        public void RemoveAt(int idx)
        {
            DataTable.Rows.RemoveAt(idx);
        }

        public override string Info()
        {
            string s = "LVectorLayer "+this.Name+":\n";
            s += "Visible:" + Visible + "\n";
            foreach (LVectorObject vo in this)
            {
                s = s + vo.Info() + "\n";
            }
            return s;
        }
        #endregion
    }

    public class LPointLayer : LVectorLayer
    {
        public LPointRenderer Renderer = new LPointRenderer();

        public IEnumerable<LPoint> Points
        {
            get {
                List<LPoint> _points = new List<LPoint>();
                _points.Clear();
                foreach (LVectorObject vo in this)
                {
                    if (vo.GeometryType == GeometryType.Point)
                        _points.Add((LPoint)vo);
                    else
                        foreach (LPoint p in ((LPolyPoint)vo).Vertices)
                            _points.Add(p);
                }
                foreach (LPoint p in _points)
                    yield return p;
            }
        }

        public LPointLayer():base (FeatureType.Point)
        {
            //default is 1mm
            LPointSymbol Symbol = new LPointSymbol();
            Renderer.Symbol = Symbol;
            Symbol.LinearUnit = LinearUnit.Meter;
            Symbol.Diameter = 0.003;
            Symbol.OutLineWidth = 0.001;
            Symbol.Style = SymbolStyle.CircleMarker;
        }
        public override void Add(LVectorObject vo)
        {
            switch (vo.GeometryType)
            {
                case GeometryType.Point:
                case GeometryType.Polypoint:
                    vo.Owner = this;
                    base.Add(vo);
                    break;
                default:
                    //FIXME:Exception handle here?
                    throw new LTypeMismatchException("Only Points are allowed");
            }
        }
    }

    public class LLineLayer : LVectorLayer 
    {
        //LLineSymbol Symbol = new LLineSymbol();
        public IEnumerable<LPolyline> Lines
        {
            get {
                List<LPolyline> lst = new List<LPolyline>();
                foreach (LVectorObject line in this)
                {
                    switch (line.GeometryType)
                    {
                        case GeometryType.Polyline:
                            lst.Add((LPolyline)line);
                            break;
                        default:
                            foreach (LPolyline pll in (LPolyPolyline)line)
                                lst.Add(pll);
                            break;
                    }
                }
                foreach (LPolyline line in lst)
                {
                    yield return line;
                }

            }
        }
        
        public LLineRenderer Renderer = new LLineRenderer ();
        public LLineLayer ():base (FeatureType.Line){}
        public override void Add(LVectorObject vo)
        {
            switch (vo.GeometryType)
            {
                case GeometryType.Polyline:
                case GeometryType.PolyPolyline:
                    base.Add(vo);
                    break;
                default:
                    throw new LTypeMismatchException("Only lines are allowed");
            }
        }
    }

    public class LPolygonLayer : LVectorLayer
    {
        //Default is simple renderer
        public IEnumerable<LPolygon> Polygons
        {
            get
            {
                List<LVectorObject> lst= new List<LVectorObject>();
                foreach (LVectorObject vo in this)
                {
                    if (vo.GeometryType == GeometryType.Polygon)
                        lst.Add(vo);
                    else
                        foreach (LPolygon plg in (LPolyPolygon)vo)
                        {
                            lst.Add(plg);
                        }
                }
                foreach (LVectorObject vo in lst)
                    yield return (LPolygon)vo;
            }
        }
        public LPolygonRenderer Renderer= new LSimplePolygonRenderer();
        public LPolygonLayer() : base(FeatureType.Polygon) { }
        public override void Add(LVectorObject vo)
        {
            switch (vo.GeometryType)
            {
                case GeometryType.Polygon:
                case GeometryType.PolyPolygon:
                    base.Add(vo);
                    break;
                default:
                    throw new LTypeMismatchException("Only polygons are allowed");
            }
        }
    }
}
