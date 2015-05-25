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
    
    /*******************************************************
     * Note the origin point of this control, it
     * takes time to understand
     * 
     * O----------------------------->x of System.Drawing.Graphics
     * |              ^y of LWindow  region
     * |              |
     * |              |
     * |              O--------> x of LWindow region
     * |            Center( Not always (0,0)  )
     * |
     * |
     * V y of System.Drawing.Graphics
     * 
     * **************************************************/
    public partial class LWindow : UserControl
    {
        [ToolboxItem(true)]

        #region Public Properties and fields
        public enum OperationType { Lock, Edit, Pan, None ,ZoomIn,ZoomOut}
        public OperationType OpType;
        public LPoint Center
        {
            get { return _Center; }
            set
            {
                _Center = value;
                //RaiseCenterAlteredEvent(this,new LCenterAlteredEventArgs(value));
            }
        }

        //linearunit per pixel
        public new double Scale
        {
            get { return _Scale; }
            set { 
                _Scale = value;
                //RaiseScaleChangedEvent(this, new LScaleChangedEventArgs(value));
            }
        }
        public LVectorLayer editingLayer = null;

        public LLayerGroup Layers
        {
            get { return _Layers; }
            set { _Layers = value; }
        }

        public LEnvelope Extent
        {
            get
            {
                LPoint pmin = ToGeographicCoordinate(new Point( 0, Height));
                LPoint pmax = ToGeographicCoordinate(new Point( Width, 0 ));
                return new LEnvelope(pmin.X, pmin.Y, pmax.X, pmax.Y);
            }
        }

        LLayerGroup _Layers = new LLayerGroup();
        LPoint _Center = new LPoint(0,0);
        double _Scale = 1.0;

        #endregion

        #region private fields

        //renderer related

        //double buffer supported
        Bitmap bmpCache; // 
        LPoint bmpCenter;
        LEnvelope bmpExtent
        {
            get
            {
                LVector diff = bmpCenter - Center;
                LPoint pmin = ToGeographicCoordinate(new Point(-Width , 2*Height));
                LPoint pmax = ToGeographicCoordinate(new Point(2 * Width, -Height));
                return new LEnvelope(pmin.X + diff.X,
                    pmin.Y + diff.Y,
                    pmax.X + diff.X,
                    pmax.Y + diff.Y);
            }
        }

        float zoomInRatio = 1.25f;
        float zoomOutRatio = .8f;

        Point zoomStartLoc ;
        Point zoomEndLoc;
        
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
        }

        #endregion

        #region Mapviewing Related Methods

        public void AlterCenter(int screendx, int screendy)
        {
            Center.X -= (double)screendx / Scale;
            Center.Y += (double)screendy / Scale;
        }


        //Zoom to layer/layergroup , overrided

        public void ZoomToLayer()
        {
            ZoomToExtent(Layers.Envelope);
        }

        public void ZoomToExtent(LEnvelope e)
        {
            double ch = Height;
            double cw = Width;
            double lh = e.YMax-e.YMin;
            //double lw = Layers.Width;
            double lw = e.XMax - e.XMin;
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
            ForceRedraw();
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
            if (OpType == OperationType.ZoomIn)
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

        #region Winform-related events

        private void LWindow_Load(object sender, EventArgs e)
        {
            //Event 
            //Warning: event subscribe-publish mechanism causes low efficiency
            //Now aborted
            //
            //CenterAltered += new CenterAlteredEventHandler(LWindow_CenterAltered);
            //ScaleChanged += new ScaleChangedEventHandler(LWindow_ScaleChanged);

            bmpCache = new Bitmap(3 * Width, 3 * Height);
            //Force the ExtentWithInBitmap() to return false
            bmpCenter = new LPoint(double.MaxValue, double.MaxValue);
        }

        /**********************************************
         * when redraw bmp cache, an important thing 
         * is to handle the differences between different
         * coorinate origin point
         * 
         * O-------------------->bmpX
         * |               | Height
         * |         O------->Lwindow X
         * |         |
         * |<-width->|
         * |         |
         * |         V  LWindowY
         * V bmpY
        */
        private void LWindow_Paint(object sender, PaintEventArgs e)
        {
            Redraw(sender, e, false);
        }

        void Redraw( object sender,PaintEventArgs e ,bool forceRedraw)
        {
            if (Layers == null)
                return;
            //if out of cache range, redraw it
            //if (false == ExtentWithinBitmap())
            if (forceRedraw || (!ExtentWithinBitmap()))
                RedrawBmpBuffer();

            //notice the centers may differ 
            LVector diff = bmpCenter - Center;
            float dx = (float)(-Width + -diff.X * Scale);
            float dy = (float)(-Height + diff.Y * Scale);
            e.Graphics.DrawImage(bmpCache, dx, dy);
            //TODO : DrawTrackingLayers()
        }


        void RedrawBmpBuffer()
        {
            bmpCenter = Center.Copy();
            Graphics grLayers = Graphics.FromImage(bmpCache);
            //Clear the bitmap cache
            grLayers.FillRectangle(new SolidBrush(Color.White), 0, 0, 3 * Width, 3 * Height);
            grLayers.TranslateTransform(Width, Height);
            Draw(grLayers, Layers);
            Console.WriteLine("Trigger redraw buffer");
        }

        private void LWindow_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = e.Location;
            if (e.Clicks > 1)
                return;
            switch (OpType)
            {
                case OperationType.ZoomIn:
                case OperationType.ZoomOut:
                    zoomEndLoc = zoomStartLoc = e.Location;
                    break;
                case OperationType.Edit:
                    break;
                default:
                    break;
            }
        }
        private void LWindow_MouseUp(object sender, MouseEventArgs e)
        {
            mouseLocation = e.Location;
            switch (OpType)
            {
                case OperationType.ZoomIn:
                case OperationType.ZoomOut:
                    Refresh();
                    Graphics g = CreateGraphics();
                    zoomEndLoc = e.Location;
                    int dx = zoomEndLoc.X - zoomStartLoc.X;
                    int dy = zoomEndLoc.Y - zoomStartLoc.Y;
                    //If the zoom tracking rectangle is too small
                    if (Math.Abs(dx) <5 && Math.Abs(dy) < 5)
                    {
                        ZoomByCenter(e.Location);
                    }
                    else if (OpType == OperationType.ZoomIn)
                    {
                        LPoint start = ToGeographicCoordinate(zoomStartLoc);
                        LPoint end = ToGeographicCoordinate(zoomEndLoc);
                        LEnvelope en = new LEnvelope(
                            Math.Min(start.X, end.X),
                            Math.Min(start.Y, end.Y),
                            Math.Max(start.X, end.X),
                            Math.Max(start.Y, end.Y)
                            );
                        ZoomToExtent( en);
                    }
                    break;
                default:
                    break;
            }

        }

        private void LWindow_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private bool ExtentWithinBitmap()
        {
            if (Extent.IsNull() || bmpExtent.IsNull())
                return false;
            return (Extent.XMax <= bmpExtent.XMax &&
                Extent.YMax <= bmpExtent.YMax &&
                Extent.XMin >= bmpExtent.XMin &&
                Extent.YMin >= bmpExtent.YMin);
        }

        #endregion

        private void LWindow_MouseMove(object sender, MouseEventArgs e)
        {
            mouseLocation = e.Location;
            switch (OpType)
            {
                case OperationType.Pan:
                    if (e.Button != MouseButtons.Left)
                        return;
                    AlterCenter(e.Location.X - mouseLocation.X, e.Location.Y - mouseLocation.Y);
                    Refresh();
                    break;
                case OperationType.ZoomIn:
                case OperationType.ZoomOut:
                    if (e.Button != System.Windows.Forms.MouseButtons.Left)
                        return;
                    Point lu;
                    zoomEndLoc = e.Location;
                    int dx = zoomEndLoc.X - zoomStartLoc.X;
                    int dy = zoomEndLoc.Y - zoomStartLoc.Y;
                    if (dx > 0)
                        lu = zoomStartLoc;
                    else{
                        lu = zoomEndLoc;
                        dx = -dx;
                    }
                    if (dy < 0)
                    {
                        lu.Y += dy;
                        dy = -dy;
                    }
                    Refresh();
                    Graphics g = CreateGraphics();
                    g.DrawRectangle(Pens.Black,new Rectangle(lu.X,lu.Y,dx,dy));
                    break;
                case OperationType.Edit:
                    Refresh();
                    break;
                default:
                    break;
            }
        }
        public new void _Refresh()
        {
            //base.Refresh();
            ForceRedraw();
        }

        public void ForceRedraw()
        {
            PaintEventArgs e = new PaintEventArgs(this.CreateGraphics(),this.Bounds);
            Redraw(this,e,true);
        }

        #endregion
        #region custom event
        public delegate void ScaleChangedEventHandler(object sender,  LScaleChangedEventArgs e);
        public event ScaleChangedEventHandler ScaleChanged;
        void RaiseScaleChangedEvent(object sender, LScaleChangedEventArgs e)
        {
            if (ScaleChanged != null)
                ScaleChanged(sender, e);
        }

        public delegate void CenterAlteredEventHandler(object sender, LCenterAlteredEventArgs e);
        public event CenterAlteredEventHandler CenterAltered;
        void RaiseCenterAlteredEvent(object sender,  LCenterAlteredEventArgs e)
        {
            if (CenterAltered != null)
                CenterAltered(sender, e);
        }

        void LWindow_CenterAltered(object sender, LCenterAlteredEventArgs e)
        {
            Refresh();
        }

        void LWindow_ScaleChanged (object sender, LScaleChangedEventArgs e){
            RedrawBmpBuffer();
            //Refresh();
            ForceRedraw();
        }
        #endregion


        #endregion
    }
}
