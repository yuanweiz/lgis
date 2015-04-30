using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Lgis
{

    public enum LayerType{ Vector,Raster}
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

        /* depricated implicit conversion : dangerous and confusing
        public static implicit operator LLayerGroup (LLayer l){
            LLayerGroup lg = new LLayerGroup();
            lg.Add(l);
            return lg;
        }
        */
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
        Lgis.GeometryType FeatureType{get;set;}
        List<LVectorObject> VectorObjects = new List<LVectorObject>();
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
        public LVectorLayer()
            : base(LayerType.Vector)
        {
        }
        public void Add(LVectorObject vo)
        {
            vo.Owner = this;
            VectorObjects.Add(vo);
        }
        public void RemoveAt(int idx)
        {
            VectorObjects[idx].Owner = LMapObject.Null;
            VectorObjects.RemoveAt(idx);
        }
        #endregion

    }

    
}
