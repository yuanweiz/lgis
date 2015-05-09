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
        #region event
        public delegate void SelectedItemChangedHandler(object sender, SelectedItemChangedEventArgs e);
        public event SelectedItemChangedHandler SelectedItemChanged;
        private void OnSelectedItemChanged (object sender , 
            SelectedItemChangedEventArgs e)
        {
            if (SelectedItemChanged != null)
                SelectedItemChanged(sender, e);
        }
        #endregion

        public string LabelText { get { return label1.Text; } set { label1.Text = value; } }
        class Item {
            public LLayer Layer = null; 
            public string tag ;
            public Item ( LLayer l, string tag){
                this.Layer = l; this.tag=tag;
            }
            public override string ToString()
            {
                return tag;
            }
        }
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

        static List<Item> Encode(LLayerGroup lg)
        {
            string layerstr = "";
            List<Item> items = new List<Item>();
            dfs(ref items , lg, "");
            if (layerstr.Length > 0)
                layerstr = layerstr.Substring(0, layerstr.Length - 1);
            return items;
        }

        private static void dfs(ref List<Item> items ,LLayerGroup lg, string prefix)
        {
            if (lg == null)
                return;
            for (int i = 0; i < lg.Count; ++i)
            {
                if (lg[i].ObjectType == ObjectType.Layer)
                {
                    items.Add(new Item((LLayer)lg[i],prefix + "/"+lg[i].Name));
                }
                else
                    dfs(ref items , (LLayerGroup)lg[i], prefix +"/"+ lg[i].Name);
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
            foreach (Item item in Encode(Layers))
                comboBox1.Items.Add(item); //FIXME
            comboBox1.EndUpdate();
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SelectedItemChangedEventArgs args=new SelectedItemChangedEventArgs();
            args.Layer = ((Item)comboBox1.SelectedItem).Layer;
            
            OnSelectedItemChanged(sender, args);
        }

        private void label1_Click(object sender, EventArgs e)
        {
        
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
    public class SelectedItemChangedEventArgs : EventArgs
    {
        public LLayer Layer;
    }
}