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

        //renderer related

        StatusType status = StatusType.Pan;

        //Tracking Geometry Objects
        //List<Point> editingPolygon = new List<Point>();
        //List<Point> editingPolyline = new List<Point>();
        //Point editingPoint = new Point();
        LPolygon trackingPolygon = new LPolygon();
        LPolyline trackingPolyline = new LPolyline();
        LPoint trackingPoint = new LPoint();

        //Symbol Settings
        Color fillColor = Color.FromArgb(239, 228, 176);
        Color boundaryColor = Color.Black;
        Color trackingColor = Color.DarkGreen;

        float boundaryWidth = 1.0f;
        float trackingWidth = 1.2f;
        float vertexSize = 7.0f;

        float zoomInRatio = 1.25f;
        float zoomOutRatio = .8f;
        

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
            get { return new SolidBrush(fillColor); }
        }
        SolidBrush trackingBrush
        {
            get { return new SolidBrush(trackingColor); }
        }
        SolidBrush vertexBrush
        {
            get { return new SolidBrush(trackingColor); }
        }


        //Cursors
        //TODO: fatal error with resources' code generating
//        Cursor csrCross = new Cursor(typeof(LWindow), "Resources.Cross.ico");
//        Cursor csrZoomIn = new Cursor(typeof(LWindow), "Resources.ZoomIn.ico");
//        Cursor csrZoomOut = new Cursor(typeof(LWindow), "Resources.ZoomOut.ico");
//        Cursor csrPanUp = new Cursor(typeof(LWindow), "Resources.PanUp.ico");
        Cursor csrCross = Cursors.Default;
        Cursor csrZoomIn = Cursors.Default;
        Cursor csrZoomOut = Cursors.Default;
        Cursor csrPanUp = Cursors.Default;

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
                    trackingPolygon = new LPolygon();
                    break;
            }
            Refresh();
            Pan();
        }

        #endregion

        #region Mapviewing Related Methods

        public void AlterCenter(int screendx, int screendy)
        {
            Center.X -= (double)screendx / Scale;
            Center.Y += (double)screendy / Scale;
        }

        public void DrawLayer (LLayerGroup lg){
            Layers = lg;
        }

        //Zoom to layer/layergroup , overrided

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

        public void ZoomToLayer(LLayer l)
        {
            LLayerGroup lg = new LLayerGroup();
            lg.Add(l);
            ZoomToLayer(lg);
        }

        public void ZoomByCenter(Point p)
        {
            LPoint lp = ToGeographicCoordinate(p);
            double dx, dy;
            dx = lp.X - Center.X;
            dy = lp.Y - Center.Y;
            if (status == StatusType.ZoomIn)
            {
                dx *= zoomOutRatio;
                Scale /= zoomOutRatio;
            }
            else
            {
                dx *= zoomInRatio;
                Scale /= zoomInRatio;
            }
            Center.X = lp.X - dx;
            Center.Y = lp.Y - dy;

        }

        //State Machine Related
        public void ZoomIn()
        {
            //Scale *= zoomInRatio;
            status = StatusType.ZoomIn;
            Cursor = csrZoomIn;
            Refresh();
        }

        public void ZoomOut()
        {
            //Scale *= zoomOutRatio;
            status = StatusType.ZoomOut;
            Cursor = csrZoomOut;
            Refresh();
        }

        // Coordinate Convertion methods
        public LPoint ToGeographicCoordinate(Point p) 
        {
            double X = p.X, Y = p.Y;
            double newX, newY;
            newX = (X - Width / 2) / Scale + Center.X;
            newY = (Height / 2 - Y) / Scale + Center.Y;
            return new LPoint(newX, newY);
        }

        Point ToScreenCoordinate(LPoint p)
        {
            double X = p.X, Y = p.Y;
            double newX, newY;
            newX = (X - Center.X) * Scale + Width / 2;
            newY = (-Y + Center.Y) * Scale + Height / 2;
            return new Point((int)newX, (int)newY);
        }

        // Drawing methods
        void Draw(Graphics g, LLayerGroup lg)
        {
            LMapObject o ;
            for(int i =0;i< lg.Count; ++i)
            {
                o=lg[i];
                switch (o.ObjectType)
                {
                    case ObjectType.LayerGroup:
                        if (((LLayerGroup)o).Visible == false)
                            continue;
                        Draw(g, (LLayerGroup)o);
                        break;
                    case ObjectType.Layer:
                        if (((LLayer)o).Visible == false)
                            continue;
                        Draw(g, (LLayer)o);
                        break;
                    default:
                        throw new Exception("ObjectType can't be Drawn: ObjectType:"+o.ObjectType.ToString());
                }
            }
        }


        void Draw(Graphics g, LLayer l)
        {
            switch (l.LayerType)
            {
                case LayerType.Vector:
                    LVectorLayer vl = (LVectorLayer)l;
                    switch (vl.FeatureType)
                    {
                        case FeatureType.Point:
                            LPointRenderer pointRenderer = ((LPointLayer)vl).Renderer;
                            foreach (LPoint p in (LPointLayer)vl)
                                pointRenderer.Render(g,ToScreenCoordinate(p));
                            break;
                        case FeatureType.Line:
                            LLineRenderer lineRenderer = ((LLineLayer)vl).Renderer;
                            foreach (LPolyline pll in ((LLineLayer)vl).Lines)
                            {
                                PointF[] points = new PointF[pll.Count];
                                for (int i=0;i< pll.Count;++i){
                                    points[i] = ToScreenCoordinate(pll[i]);
                                }
                                lineRenderer.Render(g,points);
                            }
                            break;
                        case FeatureType.Polygon:
                            LPolygonRenderer polygonRenderer = ((LPolygonLayer)vl).Renderer;
                            //polygonRenderer.Render(g, );
                            foreach (LPolygon plg in ((LPolygonLayer)vl).Polygons)
                            {
                                PointF[] points = new PointF[plg.Count];
                                for (int i=0;i< plg.Count;++i){
                                    points[i] = ToScreenCoordinate(plg[i]);
                                }
                                polygonRenderer.Render(g,points);
                            }
                            break;
                        default:
                            break;
                    }
                    
                    break;
                default:
                    break;
            }
        }

        // Tracking Geometry Object related
        void DrawTrackingPolygon(Graphics g)
        {
            int Count = trackingPolygon.Count;
            if (Count == 0)
                return;
            Point[] pts = new Point[Count];
            for (int i = 0; i < Count; ++i)
                pts[i] = ToScreenCoordinate(trackingPolygon[i]); 
            //Draw Vertex
            foreach (Point i in pts){
                RectangleF rect = new RectangleF(i.X - vertexSize / 2, i.Y - vertexSize / 2,
                    vertexSize, vertexSize);
                g.FillRectangle(vertexBrush, rect);
            }

            //Draw Edges
            if (trackingPolygon.Count > 1)
            {
                g.DrawLines(trackingPen, pts);
            }
            g.DrawLine(trackingPen, pts[0], mouseLocation);
            g.DrawLine(trackingPen, pts[Count-1], mouseLocation);
        }

        void DrawTrackingPolyline(Graphics g)
        {
            int Count = trackingPolyline.Count;
            if (Count == 0)
                return;
            Point[] pts = new Point[Count];
            for (int i = 0; i < Count; ++i)
                pts[i] = ToScreenCoordinate(trackingPolyline[i]); 
            //Draw Vertex
            foreach (Point i in pts){
                RectangleF rect = new RectangleF(i.X - vertexSize / 2, i.Y - vertexSize / 2,
                    vertexSize, vertexSize);
                g.FillRectangle(vertexBrush, rect);
            }

            //Draw Edges
            if (trackingPolygon.Count > 1)
            {
                g.DrawLines(trackingPen, pts);
            }
            g.DrawLine(trackingPen, pts[Count-1], mouseLocation);
        }
        #endregion


        #region Winform-related events

        private void LWindow_Load(object sender, EventArgs e)
        {

        }

        private void LWindow_Paint(object sender, PaintEventArgs e)
        {
            if (Layers == null)
                return;
            Draw(e.Graphics, Layers);
            switch (status)
            {
                case StatusType.Edit:
                    DrawTrackingPolygon(e.Graphics);
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
                case StatusType.ZoomOut:
                    ZoomByCenter(e.Location);
                    Refresh();
                    break;
                case StatusType.None:
                    status = StatusType.Pan;
                    break;
                case StatusType.Edit:
                    trackingPolygon.Add(ToGeographicCoordinate(mouseLocation));
                    break;
                default:
                    break;
            }

        }

        private void LWindow_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            switch ( editingType ){
                case EditingType.Polygon:
                    if (editingLayer == null)
                        return;
                    if (trackingPolygon.Count > 2)
                    {
                        editingLayer.Add(trackingPolygon.Copy());
                        trackingPolygon = new LPolygon();
                    }
                    else
                    {
                        trackingPolygon = new LPolygon();
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
