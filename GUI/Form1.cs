﻿using System;
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
        bool editing;
        Point mouseLocation = new Point(0,0);
        LVectorLayer vl = new LVectorLayer();
        bool mouseDragging;
        public Form1()
        {
            InitializeComponent();
            btnStopEditing.Enabled = false;
            //lblScale.Text = "Scale:" + w.Scale.ToString();
            LPolyline pl;
            LPolygon pg;
            vl.Add(new LPoint(2, 2));
            vl.Add(pl=new LPolyline());
            vl.Add(pg=new LPolygon());
            pl.Add(new LPoint(1, 1));
            pl.Add(new LPoint(1, 3));
            pl.Add(new LPoint(3, 2));
            pg.Add(new LPoint(2, 1));
            pg.Add(new LPoint(2, 3));
            pg.Add(new LPoint(4, 2));
            lWindow1.Layers.Add(vl) ;
            lWindow1.Invalidate();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //w.Draw(e.Graphics, vl);
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {

        }

        private void btnZoom_Click(object sender, EventArgs e)
        {
            lWindow1.Scale *= 1.25;
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            lWindow1.Scale *= .8;
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
                lWindow1.AlterCenter(e.Location.X - mouseLocation.X, e.Location.Y - mouseLocation.Y);
                mouseLocation = e.Location;
            }
            else
            {
                mouseLocation = e.Location;
                lblCoordinate.Invalidate();
            }
        }

        private void btnZoomToLayer_Click(object sender, EventArgs e)
        {
            lWindow1.ZoomToLayer(vl);
            lblScale.Invalidate();
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
            editing = true;
            btnStopEditing.Enabled = true;
            btnStartEditing.Enabled = false;
        }

        private void btnStopEditing_Click(object sender, EventArgs e)
        {
            editing = false;
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

        }

        private void lWindow1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDragging = false;
        }

        private void lWindow1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDragging)
            {
                lWindow1.AlterCenter(e.Location.X - mouseLocation.X, e.Location.Y - mouseLocation.Y);
                mouseLocation = e.Location;
            }
            else
            {
                mouseLocation = e.Location;
                lblCoordinate.Invalidate();
            }
        }

        private void lWindow1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDragging = true;
            mouseLocation = e.Location;
        }
    }
}
