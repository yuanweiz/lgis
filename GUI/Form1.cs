#define DEBUG
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Lgis;
namespace GUI
{
    public partial class Form1 : Form
    {
        Point mouseLocation = new Point(0,0);
        LUnitTest ut = new LUnitTest();
        //bool mouseDragging;
        public Form1()
        {
            InitializeComponent();
            btnStopEditing.Enabled = false;
            //lWindow1.Layers=ut.TestLayerView();
            
            LShapefileReader sr = new LShapefileReader(@"C:\Program Files\ESRI\MapObjects2\Samples\Data\USA\STATES.SHP");
            lWindow1.Layers.Add(sr.Layer);
            sr = new LShapefileReader(@"C:\Program Files\ESRI\MapObjects2\Samples\Data\USA\USHIGH.SHP");
            lWindow1.Layers.Add(sr.Layer);
            sr = new LShapefileReader(@"C:\Program Files\ESRI\MapObjects2\Samples\Data\USA\CAPITALS.SHP");
            lWindow1.Layers.Add(sr.Layer);
            lLayerView1.Layers = lWindow1.Layers;
            lLayerView1.Refresh();
            lLayerComboBox1.Layers = lWindow1.Layers;
            LDataTable table = sr.Layer.DataTable;
            table.Print();
            lLayerComboBox1.Refresh();
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            lWindow1.ZoomIn();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            lWindow1.ZoomOut();
        }

        private void btnZoomToLayer_Click(object sender, EventArgs e)
        {
            lWindow1.ZoomToLayer();
            lWindow1.Refresh();
        }

        private void lblScale_Click(object sender, EventArgs e)
        {

        }

        private void btnStartEditting_Click(object sender, EventArgs e)
        {
            btnStopEditing.Enabled = true;
            btnStartEditing.Enabled = false;
            lWindow1.StartEditing();
        }

        private void btnStopEditing_Click(object sender, EventArgs e)
        {
            btnStopEditing.Enabled = false;
            btnStartEditing.Enabled = true;
            lWindow1.StopEditing();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void lWindow1_Load(object sender, EventArgs e)
        {

        }

        private void lblCoordinate_Click(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //lWindow1.Layers = new LLayerGroup();
            //vl = new LVectorLayer();
            lWindow1.Refresh();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            FrmSetting frmSetting = new FrmSetting();
            if (frmSetting.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
            }
            frmSetting.Dispose();
        }

        private void lLayerView1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void btnRotate_Click(object sender, EventArgs e)
        {
            Matrix3D rot = LMapTools.GetRotateMatrix(30.0, new LPoint());
            LMapTools.LinearTransform(lWindow1.editingLayer, rot);
            lWindow1.Refresh();
        }

        private void lLayerComboBox1_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            Console.WriteLine(e.Layer.Name);
            //FIXME:Still some compatity problems
            if (e.Layer.LayerType == LayerType.Vector)
                lWindow1.editingLayer = (LVectorLayer)e.Layer;
        }

        private void lLayerView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            lWindow1.Refresh();
        }

    }
}
