using System;
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

        protected List<LVectorObject> VectorObjects = new List<LVectorObject>();
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
            get { return VectorObjects[idx]; }
            set { VectorObjects[idx] = value; }
        }
        public int Count { get { return VectorObjects.Count; } }

        public LRenderer Renderer = new LSimpleRenderer();

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
        }
        public virtual void Add(LVectorObject vo)
        {
            vo.Owner = this;
            VectorObjects.Add(vo);
        }
        public void RemoveAt(int idx)
        {
            VectorObjects[idx].Owner = LMapObject.Null;
            VectorObjects.RemoveAt(idx);
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
        public LPointLayer():base (FeatureType.Point)
        {
        }
        public override void Add(LVectorObject vo)
        {
            switch (vo.GeometryType)
            {
                case GeometryType.Point:
                case GeometryType.Polypoint:
                    vo.Owner = this;
                    VectorObjects.Add(vo);
                    break;
                default:
                    //FIXME:Exception handle here?
                    throw new LTypeMismatchException("Only Points are allowed");
            }
        }
    }

    public class LLineLayer : LVectorLayer
    {
        public LLineLayer ():base (FeatureType.Line){}
        public override void Add(LVectorObject vo)
        {
            switch (vo.GeometryType)
            {
                case GeometryType.Polyline:
                case GeometryType.PolyPolyline:
                    vo.Owner = this;
                    VectorObjects.Add(vo);
                    break;
                default:
                    throw new LTypeMismatchException("Only lines are allowed");
            }
        }
    }

    public class LPolygonLayer : LVectorLayer
    {
        public LPolygonLayer() : base(FeatureType.Polygon) { }
        public override void Add(LVectorObject vo)
        {
            switch (vo.GeometryType)
            {
                case GeometryType.Polygon:
                case GeometryType.PolyPolygon:
                    vo.Owner = this;
                    VectorObjects.Add(vo);
                    break;
                default:
                    throw new LTypeMismatchException("Only polygons are allowed");
            }
        }
    }
}
