using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Lgis
{
    public class LWindow
    {
        public readonly System.Windows.Forms.Control Container=null;
        public LPoint Center = null;
        public double Scale
        {
            get { return _Scale; }
            set { _Scale = value; }
        }
        double _Scale = 1.0;
        public LWindow (System.Windows.Forms.Control container,double _x, double _y, double pixelsize,Graphics g)
        {
            Container = container;
            Center = new LPoint(_x, _y);
            Scale = pixelsize;
        }
        public LWindow(System.Windows.Forms.Control container,LPoint center, double pixelsize)
        {
            Container = container;
            Center = new LPoint(center);
            Scale = pixelsize;
        }

        public void AlterCenter(int screendx, int screendy)
        {
            Center.X -= (double)screendx / Scale;
            Center.Y += (double)screendy / Scale;
        }
        public void ZoomToLayer( LLayer l)
        {
            double ch = (double)Container.Height;
            double cw = (double)Container.Width;
            double lh = l.Height;
            double lw = l.Width;
            if (Double.IsNaN(cw) || double.IsNaN(ch) || double.IsNaN(lh) || double.IsNaN(lw))
                throw new Exception("NaN exception ");
            Center.X = (l.XMax + l.XMin) / 2;
            Center.Y = (l.YMax + l.YMin) / 2;
            if (ch / cw > lh / lw)
                Scale = cw / lw;
            else
                Scale = ch / lh;

            // some alignment around the layer
            Scale *= .95;
            Container.Invalidate();
        }
        public void ZoomToLayer(LLayerGroup l)
        {
            double ch = (double)Container.Height;
            double cw = (double)Container.Width;
            double lh = l.Height;
            double lw = l.Width;
            if (Double.IsNaN(cw) || double.IsNaN(ch) || double.IsNaN(lh) || double.IsNaN(lw))
                throw new Exception("NaN exception ");
            Center.X = (l.XMax + l.XMin) / 2;
            Center.Y = (l.YMax + l.YMin) / 2;
            if (ch / cw > lh / lw)
                Scale = cw / lw;
            else
                Scale = ch / lh;
            Container.Invalidate();
            Scale *= .95;
        }

#region Drawing functions and Coordinate convertion
        public void Draw(Graphics g, LLayerGroup lg)
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
                    case ObjectType.Vector:
                    case ObjectType.Raster:
                        Draw(g, (LLayer)o);
                        break;
                    default:
                        throw new Exception("ObjectType can't be Drawn ");
                }
            }
        }
        public void Draw(Graphics g, LLayer l)
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
            double Height = Container.Height, Width = Container.Width;
            newX = (X - Center.X) * Scale + Width / 2;
            newY = (-Y + Center.Y) * Scale + Height / 2;
            return new Point((int)newX, (int)newY);
        }
        public Point ToGeographicCoordinate(Point p) 
        {
            double X = p.X, Y = p.Y;
            double newX, newY;
            double Width = Container.Width;
            double Height = Container.Height;
            newX = (X - Width / 2) / Scale + Center.X;
            newY = (Height / 2 - Y) / Scale + Center.Y;
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
    }
}
