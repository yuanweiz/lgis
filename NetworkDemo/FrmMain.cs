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

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
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
            LNetwork network = new LNetwork(lineLayer);
            network.BuildGraph();
            crossPointLayer.Clear();
            foreach (LPoint p in network.CrossPoints)
                crossPointLayer.Add(p.Copy());
            lWindow1.ForceRedraw();
            foreach (LPoint p in crossPointLayer)
                Console.WriteLine(p.Info());
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
        }

        private void btnFullExtent_Click(object sender, EventArgs e)
        {
            lWindow1.ZoomToLayer();
        }
    }
}
