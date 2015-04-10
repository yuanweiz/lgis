using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Lgis
{
    public partial class LLayerView : UserControl
    {
        #region Properties
        public LLayerGroup Layers = new LLayerGroup();
        #endregion
        public LLayerView()
        {
            InitializeComponent();
        }

        private void ShowLayers(TreeNode treeNode,LLayerGroup layers)
        {
            int Count = layers.Count;
            treeNode.Name = layers.Name;
            TreeNode tn;
            for (int i = 0; i < Count; ++i)
            {
                tn = new TreeNode(layers[i].Name);
                tn.Tag = layers[i];
                treeNode.Nodes.Add(tn);
                if (layers[i].ObjectType == ObjectType.Layer)
                {
                    tn.Checked = ((LLayer)tn.Tag).Visible;
                    continue;
                }
                else
                {
                    tn.Checked = ((LLayerGroup)tn.Tag).Visible;
                    ShowLayers(tn, ((LLayerGroup)layers[i]));
                }
            }
        }

        private void LLayerView_Load(object sender, EventArgs e)
        {
        }

        private void trvLayers_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void LLayerView_Paint(object sender, PaintEventArgs e)
        {
            trvLayers.Nodes.Clear();
            trvLayers.Nodes.Add(Layers.Name);
            trvLayers.Nodes[0].Checked = Layers.Visible;
            trvLayers.BeginUpdate();
            ShowLayers(trvLayers.Nodes[0],Layers);
            trvLayers.EndUpdate();
        }
    }
}
