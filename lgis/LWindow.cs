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
        #region Public Properties and fields
        public LPoint Center
        {
            get { return _Center; }
            set { 
                _Center = value;
            }
        }
        public new double Scale
        {
            get { return _Scale; }
            set { 
                _Scale = value;
            }
        }
        public LLayerGroup Layers
        {
            get { return _Layers; }
            set
            {
                _Layers = value;
            }
        }

        LLayerGroup _Layers = new LLayerGroup();
        LPoint _Center = new LPoint(0,0);
        double _Scale = 1.0;
#endregion

        #region private fields

        LPolygon editingPolygon = new LPolygon();
        LPolyline editingPolyline = new LPolyline();
        LPoint editingPoint = new LPoint();

        #endregion

        #region Methods

        /// <summary>
        /// Defalut Constructor, inherited from UserControl
        /// </summary>
        public LWindow()
        {
            InitializeComponent();
        }
        
        #region Drawing Functions

        public void AlterCenter(int screendx, int screendy)
        {
            Center.X -= (double)screendx / Scale;
            Center.Y += (double)screendy / Scale;
        }

        public void DrawLayer (LLayerGroup lg){
            Layers = lg;
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
                    throw new NotImplementedException("Raster datatype under development");
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
            g.DrawLines(Pens.Black, pts);
        }

        void DrawPolygon(Graphics g, LPolygon pg)
        {
            Point[] pts = new Point[pg.Count];
            for (int i = 0; i < pg.Count; ++i)
            {
                pts[i] = ToScreenCoordinate(pg[i]);
            }
            g.DrawPolygon(Pens.Black, pts);
        }


        #endregion

        #region Winform-related events
        private void LWindow_Load(object sender, EventArgs e)
        {

        }

        private void LWindow_Paint(object sender, PaintEventArgs e)
        {
                Draw(e.Graphics, Layers);
        }
        #endregion
#endregion
    }
}
