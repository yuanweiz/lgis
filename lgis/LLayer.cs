using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lgis
{
    public enum LayerType{ Vector,Raster}
    public class LLayer: LMapObject
    {
        public readonly LayerType LayerType;
        public bool Visible{get;set;}
        protected LLayer (LayerType ft){
            LayerType= ft;
        }
    }
    public class LVectorLayer : LLayer 
    {
        Lgis.FeatureType FeatureType{get;set;}
        List<LVectorObject> VectorObjects = new List<LVectorObject>();
        public int Count { get { return VectorObjects.Count; } }
        public LVectorObject this[int idx]
        {
            get { return VectorObjects[idx]; }
            set { VectorObjects[idx] = value; }
        }
        public LVectorLayer()
            : base(LayerType.Vector)
        {
        }
        public void Add(LVectorObject vo)
        {
            vo._Owner = this;
            VectorObjects.Add(vo);
            RefreshEnvelope();
        }
        public void RemoveAt(int idx)
        {
            VectorObjects[idx]._Owner = LMapObject.Null;
            VectorObjects.RemoveAt(idx);
            RefreshEnvelope();
        }
        internal override void RefreshEnvelope()
        {
            _Envelope = LEnvelope.Null;
            foreach (LVectorObject vo in VectorObjects)
            {
                _Envelope += vo.Envelope;
            }
            Owner.RefreshEnvelope();
        }
    }
}
