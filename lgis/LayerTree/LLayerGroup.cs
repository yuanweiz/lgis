using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Lgis
{
    /// <summary>
    /// 图层组树结构
    /// </summary>
    public class LLayerGroup : LMapObject ,IEnumerable<LLayer>
    {
        #region Enumerable properties

        //XXX: the hard way, leading to a waste of memory and time
        static void dfs(ref List<LLayer> list,LLayerGroup lg)
        {
            for (int i = 0; i < lg.Count;++i )
            {
                LMapObject mo = lg[i];
                switch (mo.ObjectType)
                {
                    case ObjectType.Layer:
                        list.Add((LLayer)mo);
                        break;
                    default:
                        dfs(ref list,(LLayerGroup)mo);
                        break;
                }
            }
        }
        
        public IEnumerator<LLayer> GetEnumerator()
        {
            List<LLayer> list = new List<LLayer>();
            dfs(ref list, this);
            foreach (LLayer l in list)
                yield return l;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

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
        }

        /// <summary>
        /// 删除序号为idx的元素
        /// </summary>
        /// <param name="idx"></param>
        public void RemoveAt(int idx)
        {
            Children[idx].Owner = LMapObject.Null;
            Children.RemoveAt(idx);
        }
        public override LEnvelope Envelope
        {
            get
            {
                LEnvelope l = LEnvelope.Null;
                foreach (LLayer layer in this)
                    l += layer.Envelope;
                return l;
            }
        }
        public override string Info()
        {
            string s = "LayerGroup "+this.Name+":\n";
            foreach (LLayer l in this)
                s = s + l.Info() +"";
            return s;
        }

    }
}
