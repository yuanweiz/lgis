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
        public enum OperationType { Lock, Edit, Pan, None ,ZoomIn,ZoomOut , Select}
        public OperationType OpType;
        public LPoint Center
        {
            get { return _Center; }
            set
            {
                _Center = value;
            }
        }
        //pixel per linearunit
        public new double Scale
        {
            get { return _Scale; }
            set { 
                _Scale = value;
            }
        }
        public LVectorLayer EditingLayer = null;
        LPoint EditingPoint = new LPoint();
        LPolyline EditingLine = new LPolyline();
        LPolygon EditingPolygon = new LPolygon();

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

        Point selectStartLoc ;
        Point selectEndLoc;
        
        //mouse
        Point mouseLocation;
        public LPoint MouseGeographicCorrdinate
        {
            get { return ToGeographicCoordinate(mouseLocation); } 
        }

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
            ZoomToExtent(Layers.Envelope,true);
        }
        public void ZoomToLayer( LLayerGroup l)
        {
            ZoomToExtent(l.Envelope,true);
        }
        public void ZoomToLayer(LLayer l)
        {
            ZoomToExtent(l.Envelope,true);
        }
        public void ZoomToExtent(LEnvelope e,bool ZoomIn)
        {
            double winHeight = Height;
            double winWidth = Width;
            double geoHeight = e.YMax - e.YMin;
            //double lw = Layers.Width;
            double geoWidth = e.XMax - e.XMin;
            if (double.IsNaN(geoHeight) || double.IsNaN(geoWidth) || Width == 0 || geoWidth == 0.0)
                return;
            if (ZoomIn)
            {
                Center.X = (e.XMax + e.XMin) / 2;
                Center.Y = (e.YMax + e.YMin) / 2;
                if (winHeight / winWidth > geoHeight / geoWidth)
                    Scale = winWidth / geoWidth;
                else
                    Scale = winHeight / geoHeight;
                // some alignment around the layer
                //Scale *= .95;
            }
            else
            {
                Center.X = 2* Center.X-(e.XMax + e.XMin) / 2;
                Center.Y = 2* Center.Y - (e.YMax + e.YMin) / 2;
                if (winHeight / winWidth < geoHeight / geoWidth)
                    Scale = Scale * Scale *  geoWidth / winWidth;
                else 
                    Scale = Scale * Scale * geoHeight / winHeight;
                //Not Implemented
            }
            ForceRedraw();
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
                            for (int i = 0; i < vl.Count; ++i)
                            {
                                LPoint p = vl.DataTable.Rows[i]["Geometry"] as LPoint;
                                string text = null;
                                if (pointRenderer.Symbol.ShowLabel)
                                {
                                    text = vl.DataTable.Rows[i][pointRenderer.Symbol.LabelColumn].ToString();
                                }
                                if(p!=null)
                                pointRenderer.Render(g, ToScreenCoordinate(p),text);
                            }
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
        ************************************************/
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
            float dx = (float)(-Width +diff.X * Scale);
            float dy = (float)(-Height - diff.Y * Scale);
            e.Graphics.DrawImage(bmpCache, dx,dy);
            //TODO : DrawTrackingLayers()

            try
            {
                DrawTrackingLine(e.Graphics);
                DrawTrackingPolygon(e.Graphics);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
        void DrawTrackingPolygon(Graphics g )
        {
            if (EditingPolygon.Count < 2)
                return;
            PointF []points = new PointF[EditingPolygon.Count];
            for (int i = 0; i < EditingPolygon.Count; ++i)
                points[i] = ToScreenCoordinate(EditingPolygon[i]);
            g.DrawPolygon(Pens.Black, points);
        }
        void DrawTrackingLine(Graphics g )
        {
            if (EditingLine.Count < 2)
                return;
            PointF []points = new PointF[EditingLine.Count];
            for (int i = 0; i < EditingLine.Count; ++i)
                points[i] = ToScreenCoordinate(EditingLine[i]);
            g.DrawLines(Pens.Black, points);
        }

        void RedrawBmpBuffer()
        {
            bmpCenter = Center.Copy();
            Graphics grLayers = Graphics.FromImage(bmpCache);
            //Clear the bitmap cache
            grLayers.FillRectangle(new SolidBrush(Color.White), 0, 0, 3 * Width, 3 * Height);
            grLayers.TranslateTransform(Width, Height);
            Draw(grLayers, Layers);
            Console.WriteLine("Trigger redraw buffer,Center="+Center.Info());
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
                    selectEndLoc = selectStartLoc = e.Location;
                    break;
                case OperationType.Edit:
                    if (EditingLayer == null)
                    {
                        MessageBox.Show("Please select a layer first");
                        return;
                    }
                    Type t = EditingLayer.GetType();
                    if (t == typeof(LPointLayer))
                    {
                        EditingPoint = ToGeographicCoordinate(e.Location);
                        EditingLayer.Add(EditingPoint);
                        EditingPoint = new LPoint();
                        ForceRedraw();
                    }
                    else if (t == typeof(LLineLayer))
                    {
                        if (EditingLine.Count == 0)
                            EditingLine.Add(ToGeographicCoordinate(e.Location));
                        EditingLine.Add(ToGeographicCoordinate(e.Location));
                    }
                    else if (t == typeof(LPolygonLayer))
                    {
                        if (EditingPolygon.Count == 0)
                            EditingPolygon.Add(ToGeographicCoordinate(e.Location));
                        EditingPolygon.Add(ToGeographicCoordinate(e.Location));
                    }
                    break;
                default:
                    break;
            }
        }
        private void LWindow_MouseUp(object sender, MouseEventArgs e)
        {
            mouseLocation = e.Location;
            // get select region
            Refresh();
            Graphics g = CreateGraphics();
            selectEndLoc = e.Location;
            int dx = selectEndLoc.X - selectStartLoc.X;
            int dy = selectEndLoc.Y - selectStartLoc.Y;
            //If the zoom tracking rectangle is too small
            if (Math.Abs(dx) < 5 && Math.Abs(dy) < 5)
            {
                ZoomByCenter(e.Location);
            }
            LPoint start = ToGeographicCoordinate(selectStartLoc);
            LPoint end = ToGeographicCoordinate(selectEndLoc);
            LEnvelope en = new LEnvelope(
                Math.Min(start.X, end.X),
                Math.Min(start.Y, end.Y),
                Math.Max(start.X, end.X),
                Math.Max(start.Y, end.Y)
                );
            switch (OpType)
            {
                case OperationType.Select:
                    break;
                case OperationType.ZoomIn:
                case OperationType.ZoomOut:
                    if (OpType == OperationType.ZoomIn)
                        ZoomToExtent(en, true);
                    else 
                        ZoomToExtent(en, false);
                    break;
                default:
                    break;
            }

        }

        private void LWindow_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            switch (OpType)
            {
                case OperationType.Edit:
                    mouseLocation = e.Location;
                    if (EditingLayer == null )
                        return;
                    Type t = EditingLayer.GetType();
                    if (t == typeof(LLineLayer))
                    {
                        //Manually Set to null After Tracking
                        if (EditingLine.Count > 1)
                            EditingLayer.Add(EditingLine);
                        EditingLine.RemoveAt(EditingLine.Count - 1);
                        EditingLine = new LPolyline();
                    }
                    else if (t==typeof(LPolygonLayer))
                    {
                        if (EditingPolygon.Count > 2)
                            EditingLayer.Add(EditingPolygon);
                        EditingPolygon.RemoveAt(EditingPolygon.Count - 1);
                        EditingPolygon = new LPolygon();
                    }
                    ForceRedraw();
                    break;
            }
            
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
            switch (OpType)
            {
                case OperationType.Pan:
                    if (e.Button != MouseButtons.Left)
                        return;
                    AlterCenter(e.Location.X - mouseLocation.X, e.Location.Y - mouseLocation.Y);
                    mouseLocation = e.Location;
                    Refresh();
                    break;
                case OperationType.Select:
                case OperationType.ZoomIn:
                case OperationType.ZoomOut:
                    mouseLocation = e.Location;
                    if (e.Button != System.Windows.Forms.MouseButtons.Left)
                        return;
                    selectEndLoc = e.Location;
                    DrawSelectRegion();
                    break;
                case OperationType.Edit:
                    mouseLocation = e.Location;
                    if (EditingLayer == null )
                        return;
                    Type t = EditingLayer.GetType();
                    if (t == typeof(LLineLayer) )
                    {
                        if (EditingLine.Count < 1)
                            return;
                        EditingLine[EditingLine.Count - 1] = ToGeographicCoordinate(e.Location);
                    }
                    else if (t==typeof(LPolygonLayer)  )
                    {
                        if (EditingPolygon.Count < 1)
                            return;
                        EditingPolygon[EditingPolygon.Count - 1] = ToGeographicCoordinate(e.Location);
                    }
                    else
                    {
                        return;
                    }
                    Refresh();
                    break;
                default:
                    break;
            }
        }

        public void ForceRedraw()
        {
            PaintEventArgs e = new PaintEventArgs(this.CreateGraphics(),this.Bounds);
            Redraw(this,e,true);
        }

        private void DrawSelectRegion()
        {
                    int dx = selectEndLoc.X - selectStartLoc.X;
                    int dy = selectEndLoc.Y - selectStartLoc.Y;
                    Point lu= new Point();
                    // XXX:Awful coding style
                    if (dx * dy > 0)
                    {
                        if (dx > 0)
                            lu = selectStartLoc;
                        else
                        {
                            lu = selectEndLoc;
                            dx = -dx;
                            dy = -dy;
                        }
                    }
                    else
                    {
                        if (dx > 0)
                        {
                            lu.X = selectStartLoc.X;
                            lu.Y = selectEndLoc.Y;
                        }
                        else
                        {
                            lu.X = selectEndLoc.X;
                            lu.Y = selectStartLoc.Y;
                            dx = -dx;
                        }
                    }
                    if (dy < 0) dy = -dy;
                    Refresh();
                    Graphics g = CreateGraphics();
                    g.DrawRectangle(Pens.Black,new Rectangle(lu.X,lu.Y,dx,dy));
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
            ForceRedraw();
        }
        #endregion


        #endregion
    }
}
