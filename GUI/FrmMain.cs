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
    public partial class FrmMain : Form
    {
        Point mouseLocation = new Point(0,0);
        public IEnumerable<DataRow> RecordSet;
        LUnitTest ut = new LUnitTest();
        LWindow.OperationType OpType
        {
            get { return lWindow1.OpType; }
            set { lWindow1.OpType = value; }
        }
        LLayerGroup Layers
        {
            get { return lWindow1.Layers; }
            set { lWindow1.Layers = value; }
        }
        LVectorLayer EditingLayer
        {
            get { return lWindow1.EditingLayer; }
            set
            {
                lWindow1.EditingLayer = value as LVectorLayer;
            }
        }
        //bool mouseDragging;
        public FrmMain()
        {
            InitializeComponent();
            btnStopEditing.Enabled = false;
            lLayerComboBox1.Layers = lWindow1.Layers;
            lLayerView1.Layers = lWindow1.Layers;
            //lWindow1.Layers=ut.TestLayerView();
            /*
            LShapefileReader sr = new LShapefileReader(@"C:\Program Files\ESRI\MapObjects2\Samples\Data\USA\STATES.SHP");
            lWindow1.Layers.Add(sr.Layer);
            sr = new LShapefileReader(@"C:\Program Files\ESRI\MapObjects2\Samples\Data\USA\USHIGH.SHP");
            lWindow1.Layers.Add(sr.Layer);
            sr = new LShapefileReader(@"C:\Program Files\ESRI\MapObjects2\Samples\Data\USA\CAPITALS.SHP");
            lWindow1.Layers.Add(sr.Layer);
            lLayerView1.Refresh();
            LDataTable table = sr.Layer.DataTable;
            table.Print();
            lLayerComboBox1.Refresh();
            */
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            OpType = LWindow.OperationType.ZoomIn;
        }
        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            OpType = LWindow.OperationType.ZoomOut;
        }
        private void btnPan_Click(object sender, EventArgs e)
        {
            OpType = LWindow.OperationType.Pan;
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
            OpType = LWindow.OperationType.Edit;
        }

        private void btnStopEditing_Click(object sender, EventArgs e)
        {
            btnStopEditing.Enabled = false;
            btnStartEditing.Enabled = true;
            OpType = LWindow.OperationType.None;
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
            lWindow1.ForceRedraw();
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
            LMapTools.LinearTransform(lWindow1.EditingLayer, rot);
            lWindow1.ForceRedraw();
        }

        private void lLayerComboBox1_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            Console.WriteLine(e.Layer.Name);
            EditingLayer = e.Layer as LVectorLayer;
            //FIXME:Still some compatity problems
            if (e.Layer.LayerType == LayerType.Vector)
                lWindow1.EditingLayer = (LVectorLayer)e.Layer;
        }

        private void lLayerView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            //lWindow1.Refresh();
            lWindow1.ForceRedraw();
        }

        private void btnGridView_Click(object sender, EventArgs e)
        {
            FrmGridView frmGridView = new FrmGridView();
            if (EditingLayer == null)
                return;
            frmGridView.DataTable = EditingLayer.DataTable;
            frmGridView.Show(this);
        }

        private void openShapefileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofDialog = new OpenFileDialog();
            ofDialog.Filter = "Shapefile(*.shp)|*.shp";
            if (ofDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK){
                LShapefileReader reader = new LShapefileReader(ofDialog.FileName);
                lWindow1.Layers.Add(reader.Layer);
                lLayerView1.RefreshItems();
                lLayerComboBox1.RefreshComboBoxItem();
                lWindow1.ForceRedraw();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpType = LWindow.OperationType.Select;
        }

        private void lWindow1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lWindow1_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void btnUniqueValue_Click(object sender, EventArgs e)
        {

        }

        private void btnSingleValue_Click(object sender, EventArgs e)
        {
            if (EditingLayer == null)
                return;
            ColorDialog dialog = new ColorDialog();
            Color color;
            if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                color = dialog.Color;
            }
            else return;
            
            switch (EditingLayer.FeatureType)
            {
                case FeatureType.Polygon:
                    LPolygonLayer plgLayer = EditingLayer as LPolygonLayer;
                    LSimplePolygonRenderer renderer = new LSimplePolygonRenderer();
                    plgLayer.Renderer = renderer;
                    renderer.Symbol.FillColor = color;
                    renderer.Symbol.OutlineStyle = SymbolStyle.SolidLine;
                    renderer.Symbol.Style = SymbolStyle.SolidColorFill;
                    break;
                case FeatureType.Line:
                    LLineLayer lLayer = EditingLayer as LLineLayer;
                    lLayer.Renderer.Symbol.Color = color;
                    break;
                case FeatureType.Point:
                    LPointLayer pLayer = EditingLayer as LPointLayer;
                    pLayer.Renderer.Symbol.FillColor = color;
                    break;
            }
            lWindow1.ForceRedraw();
        }

        private void btnShowLabel_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Layers.Count; ++i)
            {
                if (((LVectorLayer)Layers[i]).FeatureType == FeatureType.Point)
                {
                    bool show = (Layers[i] as LPointLayer).Renderer.Symbol.ShowLabel;
                    (Layers[i] as LPointLayer).Renderer.Symbol.ShowLabel = !show;
                }
            }
            lWindow1.ForceRedraw();
        }
    }
}