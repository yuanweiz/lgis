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
    public class LMathDomainException : Exception
    {
        public LMathDomainException(string s) : base(s) { }
    }
    public class LNotImplementedException : Exception
    {
        public LNotImplementedException(string s) : base(s) { }
    }
    public class LTypeMismatchException : Exception
    {
        public LTypeMismatchException(string s) : base(s) { }
    }
}
