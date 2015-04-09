using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lgis
{

    public enum LayerType{ Vector,Raster}
    /// <summary>
    /// 图层数据结构，提供矢量与栅格对象的容器
    /// </summary>
    public class LLayer: LMapObject
    {
        public readonly LayerType LayerType;
        public bool Visible{get;set;}
        protected LLayer (LayerType ft):base(ObjectType.Layer){
            LayerType= ft;
        }

        /* depricated implicit conversion : dangerous and confusing
        public static implicit operator LLayerGroup (LLayer l){
            LLayerGroup lg = new LLayerGroup();
            lg.Add(l);
            return lg;
        }
        */
    }

    /// <summary>
    /// 矢量图层，是LVectorObject的容器
    /// </summary>
    public class LVectorLayer : LLayer
    {
        #region 私有字段
        Lgis.FeatureType FeatureType{get;set;}
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
            RefreshEnvelope();
        }
        public void RemoveAt(int idx)
        {
            VectorObjects[idx].Owner = LMapObject.Null;
            VectorObjects.RemoveAt(idx);
            RefreshEnvelope();
        }
        internal override void RefreshEnvelope()
        {
            Envelope = LEnvelope.Null;
            foreach (LVectorObject vo in VectorObjects)
            {
                Envelope += vo.Envelope;
            }
            Owner.RefreshEnvelope();
        }
        #endregion

    }

    
}
