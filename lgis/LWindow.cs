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
    public partial class LWindow : UserControl
    {
        [ToolboxItem(true)]
        enum StatusType { Lock, Edit, Pan, None ,ZoomIn,ZoomOut}
        enum EditingType { Polygon, PolyLine, Point }
        EditingType editingType = EditingType.Polygon;

        #region Public Properties and fields

        public LPoint Center
        {
            get { return _Center; }
        }
        public new double Scale
        {
            get { return _Scale; }
            set { 
                _Scale = value;
            }
        }
        public LVectorLayer editingLayer = null;
        public LLayerGroup Layers
        {
            get { return _Layers; }
            set { _Layers = value; }
        }

        LLayerGroup _Layers = new LLayerGroup();
        LPoint _Center = new LPoint(0,0);
        double _Scale = 1.0;

        #endregion

        #region private fields

        StatusType status = StatusType.Pan;

        //Editing Target

        //List<Point> editingPolygon = new List<Point>();
        //List<Point> editingPolyline = new List<Point>();
        //Point editingPoint = new Point();
        LPolygon editingPolygon = new LPolygon();
        List<Point> editingPolyline = new List<Point>();
        Point editingPoint = new Point();

        //Symbol Settings
        Color fillingColor = Color.FromArgb(239, 228, 176);
        Color boundaryColor = Color.Black;
        Color trackingColor = Color.DarkGreen;

        float boundaryWidth = 1.0f;
        float trackingWidth = 1.0f;

        Pen boundaryPen
        {
            get { return new Pen(boundaryColor, boundaryWidth); }
        }
        Pen trackingPen
        {
            get { return new Pen(trackingColor, trackingWidth); }
        }
        SolidBrush fillingBrush
        {
            get { return new SolidBrush(fillingColor); }
        }


        //Cursors
        Cursor csrCross = new Cursor(typeof(LWindow), "Resources.Cross.ico");
        Cursor csrZoomIn = new Cursor(typeof(LWindow), "Resources.ZoomIn.ico");
        Cursor csrZoomOut = new Cursor(typeof(LWindow), "Resources.ZoomOut.ico");
        Cursor csrPanUp = new Cursor(typeof(LWindow), "Resources.PanUp.ico");

        //mouse
        Point mouseLocation;

        #endregion

        #region Methods

        #region public Methods
        /// <summary>
        /// Defalut Constructor, inherited from UserControl
        /// </summary>
        public LWindow()
        {
            InitializeComponent();
            Cursor = csrPanUp;
        }

        //Zoom
        public void ZoomIn()
        {
            Scale *= 1.25;
            Cursor = csrZoomIn;
            Refresh();
        }
        public void ZoomOut()
        {
            Scale *= .8;
            Cursor = csrZoomOut;
            Refresh();
        }

        //Pan
        public void Pan()
        {
            status = StatusType.Pan;
            Cursor = csrPanUp;
        }

        /// <summary>
        /// Set status to enable editting
        /// </summary>
        public void StartEditing()
        {
            status = StatusType.Edit;
            Cursor = csrCross;
        }
        public void StopEditing()
        {
            switch (editingType)
            {
                case EditingType.Polygon:
                    editingPolygon = new LPolygon();
                    break;
            }
            Refresh();
            Pan();
        }

        #endregion

        #region Drawing Functions

        public void AlterCenter(int screendx, int screendy)
        {
            Center.X -= (double)screendx / Scale;
            Center.Y += (double)screendy / Scale;
        }

        public void DrawLayer (LLayerGroup lg){
            Layers = lg;
        }

        public void ZoomToLayer()
        {
            double ch = Height;
            double cw = Width;
            double lh = Layers.Height;
            double lw = Layers.Width;
            if (double.IsNaN(lh) || double.IsNaN(lw) || Width == 0 || lw < 1.0)
                return;
            Center.X = (Layers.XMax + Layers.XMin) / 2;
            Center.Y = (Layers.YMax + Layers.YMin) / 2;
            if (ch / cw > lh / lw)
                Scale = cw / lw;
            else
                Scale = ch / lh;

            // some alignment around the layer
            Scale *= .95;

        }
        public void ZoomToLayer( LLayerGroup l)
        {
            double ch = Height;
            double cw = Width;
            double lh = l.Height;
            double lw = l.Width;
            if (double.IsNaN(lh) || double.IsNaN(lw) || Width == 0 || lw < 1.0)
                return;
            Center.X = (l.XMax + l.XMin) / 2;
            Center.Y = (l.YMax + l.YMin) / 2;
            if (ch / cw > lh / lw)
                Scale = cw / lw;
            else
                Scale = ch / lh;

            // some alignment around the layer
            Scale *= .95;
        }

        public LPoint ToGeographicCoordinate(Point p) 
        {
            double X = p.X, Y = p.Y;
            double newX, newY;
            newX = (X - Width / 2) / Scale + Center.X;
            newY = (Height / 2 - Y) / Scale + Center.Y;
            return new LPoint(newX, newY);
        }

        void Draw(Graphics g, LLayerGroup lg)
        {
            LMapObject o ;
            for(int i =0;i< lg.Count; ++i)
            {
                o=lg[i];
                switch (o.ObjectType)
                {
                    case ObjectType.LayerGroup:
                        Draw(g, (LLayerGroup)o);
                        break;
                    case ObjectType.Layer:
                        Draw(g, (LLayer)o);
                        break;
                    default:
                        throw new Exception("ObjectType can't be Drawn: ObjectType:"+o.ObjectType.ToString());
                }
            }
        }

        void DrawEditingPolygon(Graphics g)
        {
            int Count = editingPolygon.Count;
            if (Count == 0)
                return;
            Point[] pts = new Point[Count];
            for (int i = 0; i < Count; ++i)
                pts[i] = ToScreenCoordinate(editingPolygon[i]); 
            if (editingPolygon.Count > 1)
            {
                //Point[] pts = editingPolygon.ToArray();
                g.DrawLines(trackingPen, pts);
            }
            g.DrawLine(trackingPen, pts[0], mouseLocation);
            g.DrawLine(trackingPen, pts[Count-1], mouseLocation);
        }

        void Draw(Graphics g, LLayer l)
        {
            switch (l.LayerType)
            {
                case LayerType.Vector:
                    LVectorLayer vl = (LVectorLayer)l;
                    for (int j = 0; j < vl.Count; ++j)
                    {
                        LVectorObject vo = vl[j];
                        switch (vo.FeatureType)
                        {
                            case FeatureType.Point:
                                DrawPoint(g, (LPoint)vo);
                                break;
                            case FeatureType.Polyline:
                                DrawLines(g, (LPolyline)vo);
                                break;
                            case FeatureType.Polygon:
                                DrawPolygon(g, (LPolygon)vo);
                                break;
                            case FeatureType.Polypoint:
                                for (int i =0;i< ((LPolyPoint)vo).Count;++i)
                                    DrawPoint(g, ((LPolyPoint)vo)[i]);
                                break;
                            case FeatureType.PolyPolyline:
                                for (int i =0;i< ((LPolyPolyline)vo).Count;++i)
                                    DrawLines(g, ((LPolyPolyline)vo)[i]);
                                break;
                            case FeatureType.PolyPolygon:
                                for (int i =0;i< ((LPolyPolygon)vo).Count;++i)
                                    DrawPolygon(g, ((LPolyPolygon)vo)[i]);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case LayerType.Raster:
                    break;
            }
        }

        Point ToScreenCoordinate(LPoint p)
        {
            double X = p.X, Y = p.Y;
            double newX, newY;
            newX = (X - Center.X) * Scale + Width / 2;
            newY = (-Y + Center.Y) * Scale + Height / 2;
            return new Point((int)newX, (int)newY);
        }

        void DrawPoint(Graphics g, LPoint _p)
        {
            Point p = ToScreenCoordinate(_p);
            g.FillEllipse(Brushes.Black, p.X, p.Y, 2, 2);
        }

        void DrawLines(Graphics g, LPolyline pl)
        {
            Point[] pts = new Point[pl.Count];
            for (int i = 0; i < pl.Count; ++i)
            {
                pts[i] = ToScreenCoordinate(pl[i]);
            }
            g.DrawLines(boundaryPen, pts);
        }

        void DrawPolygon(Graphics g, LPolygon pg)
        {
            Point[] pts = new Point[pg.Count];
            for (int i = 0; i < pg.Count; ++i)
            {
                pts[i] = ToScreenCoordinate(pg[i]);
            }
            g.FillPolygon(fillingBrush, pts);
            g.DrawPolygon(boundaryPen, pts);
        }


        #endregion

        #region Winform-related events

        private void LWindow_Load(object sender, EventArgs e)
        {

        }

        private void LWindow_Paint(object sender, PaintEventArgs e)
        {
                Draw(e.Graphics, Layers);
                switch (status)
                {
                    case StatusType.Edit:
                        DrawEditingPolygon(e.Graphics);
                        break;
                    default: 
                        break;
                }
        }
        private void LWindow_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = e.Location;
            if (e.Clicks > 1)
                return;
            switch (status)
            {
                case StatusType.ZoomIn:
                    ZoomIn();
                    break;
                case StatusType.ZoomOut:
                    ZoomOut();
                    break;
                case StatusType.None:
                    status = StatusType.Pan;
                    break;
                case StatusType.Edit:
                    editingPolygon.Add(ToGeographicCoordinate(mouseLocation));
                    break;
                default:
                    break;
            }

        }

        private void LWindow_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            switch ( editingType ){
                case EditingType.Polygon:
                    if (editingPolygon.Count > 2)
                    {
                        editingLayer.Add(editingPolygon.Copy());
                        editingPolygon = new LPolygon();
                    }
                    else
                    {
                        editingPolygon = new LPolygon();
                        Refresh();
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        private void LWindow_MouseMove(object sender, MouseEventArgs e)
        {
            switch (status)
            {
                case StatusType.Pan:
                    if (e.Button != MouseButtons.Left)
                        return;
                    AlterCenter(e.Location.X - mouseLocation.X, e.Location.Y - mouseLocation.Y);
                    mouseLocation = e.Location;
                    Invalidate();
                    break;
                case StatusType.Edit:
                    mouseLocation = e.Location;
                    Refresh();
                    break;
                default:
                    break;
            }
        }

#endregion
    }
}
