﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Data;

namespace Lgis
{
    public abstract class LRenderer
    {
        public LVectorLayer Layer;
        public static double ToPixels(double dpi, double val, LinearUnit unit){
            if (unit == LinearUnit.Pixel)
                return 1.0;
            double nInches = 12.0 * LMathTools.UnitTransform(val, unit, LinearUnit.Foot);
            return dpi * nInches;
        }
    }
    public class LPolygonRenderer : LRenderer
    {
        public virtual void Render(Graphics g, PointF[] points)
        {
            throw new LNotImplementedException("Can't Render this polygon feature");
        }
    }

    public class LSimplePolygonRenderer  : LPolygonRenderer
    {
        public LPolygonSymbol Symbol = new LPolygonSymbol();
        public override void Render(Graphics g, PointF[] points)
        {
            if (Symbol == null )
                return;

            //FillColor
            switch (Symbol.Style)
            {
                case SymbolStyle.SolidColorFill:
                    SolidBrush fillBrush = new SolidBrush(Symbol.FillColor);
                    g.FillPolygon(fillBrush, points);
                    break;
                default:
                    break;
            }
            //Outline
            if (Symbol.Outline)
            {
                float width =(float) ToPixels( g.DpiX, Symbol.OutlineWidth , Symbol.LinearUnit);
                Pen outlinePen = new Pen(Symbol.OutlineColor,width);
                switch (Symbol.OutlineStyle)
                {
                    case SymbolStyle.DashLine:
                        outlinePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        break;
                    case SymbolStyle.DotLine:
                        outlinePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                        break;
                    case SymbolStyle.DashDotLine:
                        outlinePen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                        break;
                    case SymbolStyle.SolidLine:
                    default:
                        outlinePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                        break;
                }
                g.DrawPolygon(outlinePen, points);
            }
        }
    }

    public class LUniqueValuePolygonRenderer : LPolygonRenderer
    {

        public LUniqueValuePolygonRenderer() { }
        public LUniqueValuePolygonRenderer(DataColumn c)
        {
            IComparable iComp = c.DataType as IComparable;
            if (iComp == null)
                throw new LTypeMismatchException("the type of column is not comparable");
        }
        public override void Render(Graphics g, PointF[] points)
        {
            //TODO:
        }
    }

    public class LLineRenderer :LRenderer
    {
        public LLineSymbol Symbol = new LLineSymbol();

        public LLineRenderer()
        {
            Symbol.Style = SymbolStyle.SolidLine;
            Symbol.Width = 0.001;
        }
        public void Render(Graphics g, PointF[] points)
        {
            Pen pen = new Pen(Symbol.Color);
            pen.Width = (float)ToPixels(g.DpiX, Symbol.Width, Symbol.LinearUnit);
            switch (Symbol.Style)
            {
                case SymbolStyle.DashLine:
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    break;
                case SymbolStyle.DotLine:
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    break;
                case SymbolStyle.DashDotLine:
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                    break;
                case SymbolStyle.SolidLine:
                default:
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                    break;
            }
            pen.Width = (float)ToPixels(g.DpiX, Symbol.Width, Symbol.LinearUnit);
            pen.Color = Symbol.Color;
            g.DrawLines(pen, points);
        }
    }

    public class LPointRenderer : LRenderer
    {
        public LPointRenderer() { }
        public LPointRenderer(LPointLayer layer) { Layer = layer; }
        public LPointSymbol Symbol = new LPointSymbol();
        public void Render(Graphics g, PointF p ,string text=null)
        {

            if (Symbol == null )
                return;
            float size = (float)ToPixels(g.DpiX, Symbol.Diameter, Symbol.LinearUnit);
            float startx = p.X - size /2;
            float starty = p.Y - size /2;
            float outlineWidth = (float)ToPixels(g.DpiX, Symbol.OutLineWidth, Symbol.LinearUnit);

            #region render shape
            Pen outlinePen = new Pen(Symbol.OutLineColor,outlineWidth);
            SolidBrush fillBrush = new SolidBrush(Symbol.FillColor);
            switch (Symbol.Style)
            {
                case SymbolStyle.CircleMarker:
                    g.FillEllipse(fillBrush,startx,starty,size,size);
                    g.DrawEllipse(outlinePen, startx, starty, size, size);
                    break;
                case SymbolStyle.TextMarker:
                    break;
                default:
                    break;
            }
            #endregion

            #region Render Text
            if (Symbol.ShowLabel)
            {
                if (Layer!=null && text!=null)
                {
                    g.DrawString(text, Symbol.Font, Brushes.Black, p);
                }
                else
                {
                    //use default int id
                }
            }
            #endregion
        }
    }

}
