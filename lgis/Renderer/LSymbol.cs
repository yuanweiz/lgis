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
        public SymbolStyle Style = SymbolStyle.Unknown;
        string Label { get; set; }
        public LSymbol(SymbolType type) 
        {
            Type = type;
            Size = 1.0;
        }
        public double Size { get; set; }
        bool Visible = true;
    }

    public class LLineSymbol : LSymbol
    {
        bool Hatch { get; set; }
        public LLineSymbol()
            : base(SymbolType.Line)
        {
            Hatch = false;
        }
    }
    public class LPointSymbol : LSymbol
    {
        public double OffsetX = .0 , OffsetY = .0;
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
        public LPolygonSymbol() : base(SymbolType.Polygon) { }
    }
}
