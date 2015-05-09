using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lgis
{

    /// <summary>
    /// 最小外包矩形，实现了MBR的加和运算
    /// </summary>
    public class LEnvelope
    {
        /// <summary>
        /// 无效LMapObject对象的外包矩形
        /// </summary>
        public static LEnvelope Null = new LEnvelope();
        public double XMax { get { return xmax; } }
        public double XMin { get { return xmin; } }
        public double YMax { get { return ymax; } }
        public double YMin { get { return ymin; } }
        double
            xmax = double.NaN,
            xmin = double.NaN,
            ymax = double.NaN,
            ymin = double.NaN;
        public LEnvelope() { }
        public LEnvelope(double _xmin, double _ymin, double _xmax, double _ymax)
        {
            xmin = _xmin;
            xmax = _xmax;
            ymin = _ymin;
            ymax = _ymax;
        }

        /// <summary>
        /// MBR的加和运算
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static LEnvelope operator +(LEnvelope a, LEnvelope b)
        {
            //若都是有效的MBR，那么返回两者MBR并集的MBR
            if (!a.IsNull() && !b.IsNull())
            {
                return new LEnvelope(Math.Min(a.xmin, b.xmin),
                    Math.Min(a.ymin, b.ymin),
                    Math.Max(a.xmax, b.xmax),
                    Math.Max(a.ymax, b.ymax));
            }
            //否则返回非空的一个
            else if (a.IsNull())
                return b;
            else
                return a;
        }

        /// <summary>
        /// 检查是否是有效的MBR
        /// </summary>
        /// <returns></returns>
        public bool IsNull()
        {
            return (double.IsNaN(xmin) || double.IsNaN(ymin) || double.IsNaN(xmax) || double.IsNaN(ymax));
        }
        public string Info()
        {
            return "NotImplemented";
        }
    }
}
