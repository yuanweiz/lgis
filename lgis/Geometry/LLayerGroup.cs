using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lgis
{
    /// <summary>
    /// 图层组树结构
    /// </summary>
    public class LLayerGroup : LMapObject
    {
        List<LMapObject> Children = new List<LMapObject>();
        public bool Visible = true;
        public LLayerGroup()
            : base(ObjectType.LayerGroup)
        {
        }

        public int Count
        {
            get { return Children.Count; }
        }

        /// <summary>
        /// 返回第idx个元素，可能是LLayerGroup或LLayer
        /// </summary>
        /// <param name="idx">元素在图层组中的序号</param>
        /// <returns></returns>
        public LMapObject this[int idx]
        {
            get { return Children[idx]; }
        }

        /// <summary>
        /// 添加一个元素，只允许是LLayer和LLayerGroup类，否则抛出异常
        /// </summary>
        /// <param name="o"></param>
        public void Add(LMapObject o){
            if (o.ObjectType != ObjectType.Layer &&
                o.ObjectType != ObjectType.LayerGroup)
                throw new Exception("ObjectType not match layergroup");
            o.Owner = this;
            Children.Add(o);
            RefreshEnvelope();
        }

        /// <summary>
        /// 删除序号为idx的元素
        /// </summary>
        /// <param name="idx"></param>
        public void RemoveAt(int idx)
        {
            Children[idx].Owner = LMapObject.Null;
            Children.RemoveAt(idx);
            RefreshEnvelope();
        }

        internal override void RefreshEnvelope()
        {
            Envelope = LEnvelope.Null;
            foreach (LMapObject o in Children)
            {
                Envelope += o.Envelope;
            }
            Owner.RefreshEnvelope();
        }
    }
}
