using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Lgis
{
    public abstract class LRenderer
    {
        virtual public void Render (Graphics g){
            throw new LNotImplementedException("Can't render this kind of symbol");
        }
    }
    public class LSimpleRenderer : LRenderer
    {

    }

}
