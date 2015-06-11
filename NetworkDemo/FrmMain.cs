using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Lgis;

namespace NetworkDemo
{
    public partial class FrmMain : Form
    {
        LLineLayer lineLayer = new LLineLayer();
        LPointLayer crossPointLayer = new LPointLayer();
        LNetwork network;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            network = new LNetwork(lineLayer);
            lWindow1.Layers.Add(lineLayer);
            lWindow1.EditingLayer = lineLayer;
            lWindow1.Layers.Add(crossPointLayer);
            lWindow1.OpType = LWindow.OperationType.Edit;
            LPointSymbol symbol = crossPointLayer.Renderer.Symbol;
            symbol.LinearUnit = LinearUnit.Meter;
            symbol.OutLineColor = Color.Blue;
            symbol.Diameter = 0.002;
            symbol.OutLineWidth = 0.002;
            LPolyline line = new LPolyline();
            /*
            line.Add(new LPoint(-100,-100));
            line.Add(new LPoint(100,100));
            lineLayer.Add(line);
            line = new LPolyline();
            line.Add(new LPoint(-100,100));
            line.Add(new LPoint(100,-100));
            lineLayer.Add(line);
            */
            
        }

        private void lWindow1_Load(object sender, EventArgs e)
        {
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            network = new LNetwork(lineLayer);
            network.BuildGraph();
            crossPointLayer.Clear();
            foreach (LPoint p in network.CrossPoints)
                crossPointLayer.Add(p.Copy());
            lWindow1.ForceRedraw();
            foreach (LPoint p in crossPointLayer)
                Console.WriteLine(p.Info());
            //network.PrintInfo();
            RefreshItem(cmbEndPoint);
            RefreshItem(cmbStartPoint);
        }

        private void btnOpenShp_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Shapefile(.shp)|*.shp";
            if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                LShapefileReader reader = new LShapefileReader(dialog.FileName);
                if (reader.Layer.GetType() != typeof(LLineLayer))
                    return;
                lineLayer = reader.Layer as LLineLayer;
                lWindow1.Layers.Add(lineLayer);
                lWindow1.ForceRedraw();
            }
            RefreshItem(cmbEndPoint);
            RefreshItem(cmbStartPoint);
        }

        private void btnFullExtent_Click(object sender, EventArgs e)
        {
            lWindow1.ZoomToLayer();
        }

        private void lWindow1_MouseMove(object sender, MouseEventArgs e)
        {
            LPoint p = lWindow1.ToGeographicCoordinate(e.Location);
            lblCoordinate.Text = "(" + p.X.ToString() + "," + p.Y.ToString() + ")";
        }

        private void btnSetFont_Click(object sender, EventArgs e)
        {
            crossPointLayer.Renderer.Symbol.SetFont();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        void RefreshItem(ComboBox box)
        {
            int cnt = network.CrossPoints.Count();
            box.BeginUpdate();
            box.Items.Clear();
            for (int i = 0; i < cnt; ++i)
                box.Items.Add(i);
            box.EndUpdate();
        }
        private void btnDijkstra_Click(object sender, EventArgs e)
        {
            int start, end;
            start = cmbStartPoint.SelectedIndex;
            end = cmbEndPoint.SelectedIndex;
            int[] track = network.Dijkstra(start,end);
            for (int i = 0; i < track.Length; ++i)
                txtNearestPath.Text+=(track[i].ToString() + " ");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lineLayer.Clear();
            crossPointLayer.Clear();
            lWindow1.ForceRedraw();
        }
    }
}
