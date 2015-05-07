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
        public LLayerGroup Layers = new LLayerGroup();
        public LLayerComboBox()
        {
            InitializeComponent();
        }
        public static string Encode(LLayerGroup lg)
        {
            layerstr = "";
            dfs(lg, "");
            layerstr = layerstr.Substring(0, layerstr.Length - 1);
            return layerstr;
        }
        static string layerstr;
        private static void dfs(LLayerGroup lg, string prefix)
        {
            for (int i = 0; i < lg.Count; ++i)
            {
                if (lg[i].ObjectType == ObjectType.Layer)
                {
                    layerstr += (prefix + "/"+lg[i].Name + ";");
                }
                else
                    dfs((LLayerGroup)lg[i], prefix +"/"+ lg[i].Name);
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
            Encode(Layers);
            comboBox1.BeginUpdate();
            comboBox1.Items.Clear();
            foreach (string layername in layerstr.Split(';'))
                comboBox1.Items.Add(layername);
            comboBox1.EndUpdate();
        }
    }
}