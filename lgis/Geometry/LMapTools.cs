using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lgis
{
    public static class LMapTools
    {
        #region internal var
        static double tolerance = 5.0f;
        #endregion

        #region private methods concerning topology
        static bool EnvelopeIntersect(LEnvelope a,LEnvelope b)
        {
            if (a.XMax < b.XMin ||
                a.XMin > b.XMax ||
                a.YMin > b.YMax ||
                a.YMax < b.YMin)
                return false;
            else return true;
        }

        static bool PointOnLineseg( LPoint p,LLineseg ls)
        {
            return false;
        }

        #endregion
    }
}
