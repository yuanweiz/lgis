using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lgis
{
    public class LLayerGroup : LMapObject
    {
        List<LMapObject> Children = new List<LMapObject>();
        public LLayerGroup()
            : base(ObjectType.LayerGroup)
        {
        }
        public int Count
        {
            get { return Children.Count; }
        }
        public LMapObject this[int idx]
        {
            get { return Children[idx]; }
        }
        public void Add(LMapObject o){
            o._Owner = this;
            Children.Add(o);
            RefreshEnvelope();
        }
        public void RemoveAt(int idx)
        {
            Children[idx]._Owner = LMapObject.Null;
            Children.RemoveAt(idx);
            RefreshEnvelope();
        }
        internal override void RefreshEnvelope()
        {
            _Envelope = LEnvelope.Null;
            foreach (LMapObject o in Children)
            {
                _Envelope += o.Envelope;
            }
            Owner.RefreshEnvelope();
        }
    }


    #region Old
    /*

    public class LLayerTree : LObject
    {
        List<LLayerTreeNode> Children = new List<LLayerTreeNode>();
        public LLayerTreeNode this[int idx]
        {
            get { return Children[idx]; }
        }
        public void Add(LLayerTreeNode n)
        {
            Children.Add(n);
        }
        public LLayerTree()
            : base(ObjectType.Layer)
        {
        }
    }
    public abstract class LLayerTreeNode :LObject
    {
        public enum NodeType { Group, Layer, Unknown }
        public readonly NodeType nodeType;
        protected LLayerTreeNode(NodeType nt){
            nodeType = nt;
        }
    }
    public class LLayerTreeLayer : LLayerTreeNode
    {
        public LLayer Layer=null;
        public  LLayerTreeLayer () :base (NodeType.Layer){
        }
        public LLayerTreeLayer(LLayer l):base(NodeType.Layer)
        {

        }
    }
    public class LLayerTreeGroup : LLayerTreeNode
    {
        public List<LLayerTreeNode> children = null;
        public  LLayerTreeGroup ():base (NodeType.Group){
        }
    }
    */
#endregion
}
