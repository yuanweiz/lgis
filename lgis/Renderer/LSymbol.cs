using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Lgis
{
    public abstract class LSymbol
    {
        public readonly SymbolType Type;
        public LinearUnit LinearUnit = LinearUnit.Unknown;
        protected SymbolStyle _Style = SymbolStyle.Unknown;
        public virtual SymbolStyle Style
        {
            get { return _Style; }
            set { _Style = value; }
        }
        string Label { get; set; }
        public LSymbol(SymbolType type) 
        {
            Type = type;
        }
        bool Visible = true;
    }

    public class LLineSymbol : LSymbol
    {
        public Color Color;
        public override SymbolStyle Style
        {
            get { return _Style; }
            set
            {
                switch (value)
                {
                    case SymbolStyle.DashLine:
                    case SymbolStyle.DotLine:
                    case SymbolStyle.SolidLine:
                        _Style = value;
                        return;
                    default:
                        throw new LTypeMismatchException("Only linestyle are accepted");
                }
            }
        }
        public LLineSymbol()
            : base(SymbolType.Line)
        {
        }
        public double Width;
    }
    public class LPointSymbol : LSymbol
    {
        public double Diameter { get; set; }
        public double OffsetX = .0 , OffsetY = .0;
        public LinearUnit LinearUnit = LinearUnit.Unknown;
        double _Height = 2.0,_Width = 2.0;
        public double Height
        {
            get { return _Height; }
            set
            {
                if (value <= 0)
                    throw new LMathDomainException("Size of symbol should be positive");
                _Height = value;
            }
        }
        public double Width
        {
            get { return _Width; }
            set
            {
                if (value <= 0)
                    throw new LMathDomainException("Size of symbol should be positive");
                _Width = value;
            }
        }

        public double OutLineWidth = 1.0;
        public Color FillColor = Color.Azure;
        public Color OutLineColor = Color.Black;
        public LPointSymbol()
            : base(SymbolType.Point)
        {
            
        }
    }
    public class LPolygonSymbol : LSymbol
    {
        public LPolygonSymbol() : base(SymbolType.Polygon) {
            this.Style = SymbolStyle.SolidColorFill;
        }
        public bool Hatch = false;
        public Color FillColor = Color.LightYellow;

        //Outline Related
        public Color OutlineColor = Color.Black;
        public bool Outline = true;
        LLineSymbol OutLineSymbol = new LLineSymbol();
        public double OutlineWidth {
            get
            {
                return OutLineSymbol.Width;
            }
            set {
                OutLineSymbol.Width = value;
            }
        }

        public SymbolStyle OutlineStyle
        {
            get { return OutLineSymbol.Style; }
            set { OutLineSymbol.Style = value; }
        }

    }
}
