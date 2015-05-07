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
    public partial class LLayerComboBox : UserControl
    {
        LLayerGroup _Layers = new LLayerGroup();
        public LLayerGroup Layers { get { return _Layers; }
            set 
            {
                _Layers = value;
                RefreshComboBoxItem();
            }
        }
        public LLayerComboBox()
        {
            InitializeComponent();
        }

        public static string Encode(LLayerGroup lg)
        {
            string layerstr = "";
            dfs(ref layerstr , lg, "");
            if (layerstr.Length > 0)
                layerstr = layerstr.Substring(0, layerstr.Length - 1);
            return layerstr;
        }

        private static void dfs(ref string layerstr,LLayerGroup lg, string prefix)
        {
            if (lg == null)
                return;
            for (int i = 0; i < lg.Count; ++i)
            {
                if (lg[i].ObjectType == ObjectType.Layer)
                {
                    layerstr += (prefix + "/"+lg[i].Name + ";");
                }
                else
                    dfs(ref layerstr , (LLayerGroup)lg[i], prefix +"/"+ lg[i].Name);
            }
        }

        private void LLayerComboBox_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
        private void LLayerComboBox_Paint(object sender, PaintEventArgs e)
        {
        }

        public void RefreshComboBoxItem()
        {
            comboBox1.BeginUpdate();
            comboBox1.Items.Clear();
            foreach (string layername in Encode(Layers).Split(';'))
                comboBox1.Items.Add(layername);
            comboBox1.EndUpdate();
        }
    }
}