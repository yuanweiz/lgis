using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lgis
{
    public static class LMapTools
    {
        public enum SelectMode { Partly, Fully };

        #region internal var
        //static double tolerance = 5.0f;
        static double eps = 1e-5;
        #endregion

        #region methods concerning topology
        #region Private
        static bool EnvelopeIntersect(LEnvelope a,LEnvelope b)
        {
            if (a.XMax < b.XMin ||
                a.XMin > b.XMax ||
                a.YMin > b.YMax ||
                a.YMax < b.YMin)
                return false;
            else return true;
        }

        static bool PointOnLineseg( LPoint C,LLineseg ls ,double tolerance)
        {
            /************************
             *    /C 
             *   / |
             *  /  |
             * A---D------------B
             * define pos as AD / AB ;
             * */
            LVector v1 = ls.B - ls.A;
            LVector v2 = C - ls.A;
            double pos;
            double dist;
            if (v1 * v1 < eps)
                pos = (v1 * v2) / (v1 * v1);
            else
                return false;
            if (pos > 1)
                dist = (C - ls.B).Norm();
            else if (pos < 0)
                dist = (C - ls.A).Norm();
            else {
                dist = (v2 -v1 * pos).Norm();
            }
            if (dist < tolerance)
                return true;
            else return false;
        }

        static bool PointInPolygon(LPoint p, LPolygon plg)
        {   //TODO:
            /*****************************
             * some special circumstances should be taken into account
             *                   
             *                    /\ 
             *                   /  \                 / E
             *                  /    \        C  D  / 
             *    /------------/  A   \-------\  //
             *   /                      B      \/ 
             *  /                               F
             * /
            when judging whether A is inside polygon, counting B/C twice
             * should be avoided, a posible solution is ignore all horizonal 
             * lines (e.g. lineseg BC)
            * Another problems occurs when A.Y=D.Y, then in lineseg DF and DE
             * point D will be counted twice, a solution is 
             * 
            */

            if (!PointInEnvelope(p, plg.Envelope))
                return false;
            bool up1, up2;
            LPoint A, B;
            int cnt=0;
            A = plg[plg.Count - 1];
            up1 = (A.Y > p.Y); 
            foreach (LLineseg ls in plg.Edges) { 
                A = ls.A; B = ls.B;
                // as explained above
                if (A.Y == B.Y)
                    continue;
                up1 = (A.Y > p.Y);
                up2 = (B.Y > p.Y);
                if (up1 == up2)
                    continue;
                else
                {
                    double x = B.X + (p.Y - B.Y) / (A.Y - B.Y) * (A.X - B.X);
                    if (x > p.X)
                        cnt++;
                }
            }
            if ((cnt & 1) != 0)
                return true;
            else return false;
        }

        static bool PointInEnvelope(LPoint p, LEnvelope e)
        {
            return p.X > e.XMin && p.X < e.XMax && p.Y > e.YMin && p.Y < e.YMax;
        }

        static bool PointOnPolyline(LPoint p, LPolyline pll, double tolerance)
        {
            bool flag = false;
            foreach (LLineseg ls in pll.Edges)
            {
                flag = flag && PointOnLineseg(p, ls, tolerance);
                if (flag)
                    return true;
            }
            return false;
        }

        static bool PointInPolyPolygon(LPoint p, LPolyPolygon pplg)
        {
            bool flag = false;
            for (int i = 0; i < pplg.Count; ++i)
            {
                flag = flag && PointInPolygon(p, pplg[i]);
                if (flag)
                    return true;
            }
            return false;
        }

        static bool PointOnPoint(LPoint searchpoint, LPoint p ,double tolerance)
        {
            return (p - searchpoint).Norm() < tolerance;
        }

        static bool EnvelopeWithinRegion(LEnvelope mbr, LEnvelope select_region)
        {
            return select_region.XMax > mbr.XMax &&
                select_region.XMin < mbr.XMin &&
                select_region.YMax > mbr.YMax &&
                select_region.YMin < mbr.YMin;
        }
        static bool PolygonFullyWithinEnvelope(LPolygon plg, LEnvelope e)
        {
            return EnvelopeWithinRegion(plg.Envelope ,e );
        }
        static bool PolygonPartlyWithinEnvelope(LPolygon plg, LEnvelope e)
        {

            LLineseg ls;
            for (int i = 0; i < plg.Count-1;++i)
            {
                ls = new LLineseg(plg[i], plg[i + 1]);
                if (LinesegPartlyWithinEnvelope(ls, e))
                    return true;
            }
            ls = new LLineseg(plg[plg.Count - 1], plg[0]);
            if (LinesegPartlyWithinEnvelope(ls, e))
                return true;
            return false;
        }
        static bool PolylineFullyWithinEnvelope(LPolyline pll, LEnvelope e)
        {
            return EnvelopeWithinRegion(pll.Envelope,e);
        }
        static bool LinesegFullyWithinEnvelope(LLineseg ls, LEnvelope e)
        {
            return EnvelopeWithinRegion(ls.Envelope, e);
        }
        static bool LinesegPartlyWithinEnvelope(LLineseg ls, LEnvelope e)
        {
            const int up = 8, down = 4, left = 1, right = 2;
            LEnvelope mbr = ls.Envelope;
            int posA = PosCode(ls.A, e);
            int posB = PosCode(ls.B, e);
            int posAB = posA | posB;
            // if A or B is inside the envelope
            if (posA == 0 || posB == 0)
                return true;
            // if A and B are on the same side (up/down/left/right) 
            // of envelope
            else if ((posA & posB) != 0)
                return false;
            //four directions
            double x, y;
            double x1 = ls.A.X , x2=ls.B.X,y1=ls.A.Y,y2=ls.B.Y;
            if ((posAB|up)!=0) // may intersect with upper bound
            {
                x = x1 + (x2 - x1)/(y2-y1)*(e.YMax-y1);
                if ((x - e.XMin) * (x - e.XMax) < 0)
                    return true;
            }
            else if ((posAB | down) != 0)
            {
                x = x1 + (x2 - x1)/(y2-y1)*(e.YMin-y1);
                if ((x - e.XMin) * (x - e.XMax) < 0)
                    return true;
            }
            else if ((posAB | left) != 0)
            {
                y = y1 + (y2 - y1) / (x2 - x1) * (e.XMin - x1);
                if ((y - e.YMin) * (y - e.YMax) < 0)
                    return true;
            }
            else if ((posAB | right) != 0)
            {
                y = y1 + (y2 - y1) / (x2 - x1) * (e.XMax - x1);
                if ((y - e.YMin) * (y - e.YMax) < 0)
                    return true;
            }
            return false;
        }

        private static int PosCode(LPoint p, LEnvelope e)
        {
            //using Sutherland-cohen algorithm
            /*******************************
             *          |           | 
             *     8|1  |     8     |   2|8
             *          |           | 
             *  --------|------------------
             *          |           | 
             *      1   |     0     |   2 
             *          |           | 
             *  --------|------------------
             *          |           | 
             *      1|4 |     4     |   2|4 
             *          |           | 
             *          |           | 
             * *********|***********************/
            int lr = 0;//left-right
            int ud = 0;//up-down
            if (p.X > e.XMax)
                lr = 2;
            else if (p.X < e.XMin)
                lr = 1;
            if (p.Y > e.YMax)
                ud = 8;
            else if (p.Y < e.YMin)
                ud = 4;
            return lr & ud;

        }
        #endregion

        #region public
        public static bool FullyWithinEnvelope(LVectorObject vo, LEnvelope e)
        {
            return EnvelopeWithinRegion(vo.Envelope, e);
        }

        public static bool PartlyWithinEnvelope(LVectorObject vo, LEnvelope e)
        {
            switch (vo.GeometryType)
            {
                case GeometryType.Point:
                    return EnvelopeWithinRegion(vo.Envelope, e);
                case GeometryType.Polygon:
                    return PolygonPartlyWithinEnvelope((LPolygon)vo, e);
                case GeometryType.Polypoint:
                    foreach (LPoint p in vo.Vertices)
                    {
                        if (PointInEnvelope(p, e))
                            return true;
                    }
                    return false;
                case GeometryType.Polyline:
                case GeometryType.PolyPolyline:
                    foreach (LLineseg ls in vo.Edges)
                    {
                        if (LinesegPartlyWithinEnvelope(ls, e))
                            return true;
                    }
                    return false;
                default: return false;
            }
        }

        #endregion

        #endregion

        #region Public methods concerning linear transformation
        public static Matrix3D GetRotateMatrix(double angle_degree, LPoint center)
        {
            double angle = angle_degree * Math.PI / 180.0;
            double cos = Math.Cos(angle), sin = Math.Sin(angle);
            double xc=center.X,yc=center.Y;
            double []arr = { cos, -sin, 0, sin, cos, 0, 0, 0, 1 };
            Matrix3D rot = new Matrix3D(arr);
            rot[0, 2] = xc - cos * xc + sin * yc;
            rot[1, 2] = yc - sin * xc - cos * yc;
            return rot;
        }

        public static void LinearTransform(LVectorLayer l ,Matrix3D rot)
        {
            switch (l.LayerType)
            {
                case LayerType.Raster:
                    break;
                case LayerType.Vector:
                    foreach (LVectorObject vo in (LVectorLayer)l)
                        LinearTransform(vo, rot);
                    break;
            }
        }

        //FIXME:bad implementation
        public static void LinearTransform(LVectorObject vo , Matrix3D rot){
            LPoint newp;
            foreach (LPoint p in vo.Vertices)
            {
                newp = rot * p;
                p.X = newp.X;
                p.Y = newp.Y;
            }
        }

        #endregion

        #region public method concerning selection

        //TODO:
        public static IEnumerable<LVectorObject> SelectByRegion(LVectorLayer l, LEnvelope e ,SelectMode mode)
        {
            switch (mode)
            {
                default:
                case SelectMode.Partly:
                    return from vectorobject in l
                           where FullyWithinEnvelope((LPoint)vectorobject, e)
                           select vectorobject;
                case SelectMode.Fully:
                    return from vectorobject in l
                           where PartlyWithinEnvelope((LPoint)vectorobject, e)
                           select vectorobject;
            }
        }
        #endregion

        #region methods concerning editing
        #endregion
    }
}
