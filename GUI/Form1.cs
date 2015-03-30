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
        LPoint center = new LPoint(0, 0);
        LWindow w;
        Point mouseLocation;
        LVectorLayer vl = new LVectorLayer();
        bool mouseDragging;
        public Form1()
        {
            InitializeComponent();
            w = new LWindow(pictureBox1, center, 50.0);
            LPolyline pl;
            LPolygon pg;
            vl.Add(new LPoint(2, 2));
            vl.Add(pl=new LPolyline());
            vl.Add(pg=new LPolygon());
            pl.Add(new LPoint(1, 1));
            pl.Add(new LPoint(1, 3));
            pl.Add(new LPoint(3, 2));
            pg.Add(new LPoint(1, 1));
            pg.Add(new LPoint(1, 3));
            pg.Add(new LPoint(3, 2));
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            w.Draw(e.Graphics, vl);
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {

        }

        private void btnZoom_Click(object sender, EventArgs e)
        {
            w.Scale = w.Scale * 1.25;
            pictureBox1.Invalidate();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            w.Scale = w.Scale * .8;
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDragging = true;
            mouseLocation = e.Location;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDragging = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDragging)
            {
                w.AlterCenter(e.Location.X - mouseLocation.X, e.Location.Y - mouseLocation.Y);
                mouseLocation = e.Location;
                pictureBox1.Invalidate();
            }
            else
                return;
        }

        private void btnZoomToLayer_Click(object sender, EventArgs e)
        {
            w.ZoomToLayer(vl);
        }
    }
}
