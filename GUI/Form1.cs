using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Lgis;
namespace GUI
{
    public partial class Form1 : Form
    {
        Point mouseLocation = new Point(0,0);
        LVectorLayer vl = new LVectorLayer();
        List<Point> trackingPolygon = new List<Point>();
        //bool mouseDragging;
        enum OperationType { None,Edit,Pan} ;
        OperationType opType = OperationType.None;
        public Form1()
        {
            InitializeComponent();
            btnStopEditing.Enabled = false;
            lWindow1.Layers.Add(vl) ;
            lWindow1.Invalidate();
        }

        private void btnZoom_Click(object sender, EventArgs e)
        {
            lWindow1.Scale *= 1.25;
            lWindow1.Refresh();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            lWindow1.Scale *= .8;
            lWindow1.Refresh();
        }

        private void btnZoomToLayer_Click(object sender, EventArgs e)
        {
            lWindow1.ZoomToLayer(vl);
            lblScale.Refresh();
            lWindow1.Refresh();
        }

        private void lblScale_Click(object sender, EventArgs e)
        {

        }

        private void lblScale_Paint(object sender, PaintEventArgs e)
        {
            lblScale.Text = "Scale:" + lWindow1.Scale.ToString();
        }

        private void lblCoordinate_Paint(object sender, PaintEventArgs e)
        {
            lblCoordinate.Text = "(X,Y)=" + lWindow1.ToGeographicCoordinate(mouseLocation).ToString();
        }

        private void btnStartEditting_Click(object sender, EventArgs e)
        {
            opType = OperationType.Edit;
            btnStopEditing.Enabled = true;
            btnStartEditing.Enabled = false;
        }

        private void btnStopEditing_Click(object sender, EventArgs e)
        {
            opType = OperationType.None;
            btnStopEditing.Enabled = false;
            btnStartEditing.Enabled = true;
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

        private void lWindow1_Paint(object sender, PaintEventArgs e)
        {
            if (trackingPolygon.Count == 0)
                return;
            Graphics g = e.Graphics;
            if (trackingPolygon.Count > 1)
            {
                Point[] pts = trackingPolygon.ToArray();
                g.DrawLines(Pens.Black, pts);
            }
            g.DrawLine(Pens.Black, trackingPolygon[0], mouseLocation);
            g.DrawLine(Pens.Black, trackingPolygon[trackingPolygon.Count-1], mouseLocation);
        }

        private void lWindow1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                switch (opType)
                {
                    case OperationType.Pan:
                        if (e.Button != MouseButtons.Left)
                            return;
                        lWindow1.AlterCenter(e.Location.X - mouseLocation.X, e.Location.Y - mouseLocation.Y);
                        mouseLocation = e.Location;
                        lblCoordinate.Invalidate();
                        lWindow1.Invalidate();
                        break;
                    case OperationType.Edit:
                        mouseLocation = e.Location;
                        lWindow1.Refresh();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void lWindow1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = e.Location;
            switch (opType)
            {
                case OperationType.None:
                    opType = OperationType.Pan;
                    break;
                case OperationType.Edit:
                    trackingPolygon.Add(mouseLocation);
                    break;
                default:
                    break;
            }
        }

        private void lWindow1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            LPolygon plg = new LPolygon();
            foreach (Point p in trackingPolygon)
            {
                plg.Add(lWindow1.ToGeographicCoordinate(p));
            }
            vl.Add(plg);
            trackingPolygon.Clear();
        }
    }
}
