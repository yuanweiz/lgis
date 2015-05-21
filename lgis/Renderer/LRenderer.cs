using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Lgis
{
    public abstract class LRenderer
    {

    }
    public class LSimpleRenderer : LRenderer
    {
    }
    public class LPointRenderer : LRenderer
    {
        public LPointSymbol Symbol;
        public void Render(Graphics g, PointF p)
        {

        }
    }

}
