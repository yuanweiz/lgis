using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lgis
{
    public class LScaleChangedEventArgs : EventArgs
    {
        public double Scale;
        public LScaleChangedEventArgs(double Scale) : base() { this.Scale = Scale; }
    }
    public class LCenterAlteredEventArgs :EventArgs
    {
        public LPoint Center;//new value
        public LCenterAlteredEventArgs(LPoint c) { Center = c; }
    }
}
