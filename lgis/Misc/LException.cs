using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lgis
{
    class LOutOfRangeException : Exception 
    {
    }
    class LIOException : System.IO.IOException
    {
        public LIOException(string s) : base(s) { }
    }
}
