using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lgis
{
    /// <summary>
    /// LMapObject的类型,Vector和Raster是实体对象，Layer与LayerGroup是容器类型
    /// </summary>
    public enum ObjectType { Vector, Raster, Layer,LayerGroup, Unknown };

    /// <summary>
    /// 矢量要素类的类型
    /// </summary>
    public enum FeatureType { Point, Polygon, Polyline, PolyPolyline, PolyPolygon, Polypoint,Rectangle,Unknown };

    /// <summary>
    /// 提供了图层与要素类的基类
    /// </summary>
    public class LMapObject
    {
        /// <summary>
        /// 静态成员Null，它是所有LMapObject对象的Owner
        /// </summary>
        public static LMapObject Null = new LMapObject();

        #region 属性
        /// <summary>
        /// Null对象是任何LMapObject对象（包括自身）的默认父对象(Owner)
        /// </summary>
        public string Name = "NoName";
        public readonly ObjectType ObjectType=ObjectType.Unknown;
        public LMapObject Owner { get; internal set; }
        public LEnvelope Envelope { get; internal set; }
        public double XMin {get{return Envelope.XMin ;}}
        public double XMax {get{return Envelope.XMax ;}}
        public double YMin {get{return Envelope.YMin ;}}
        public double YMax {get{return Envelope.YMax ;}}
        public double Width { get { return XMax - XMin; } }
        public double Height { get { return YMax - YMin; } }
        #endregion

        /// <summary>
        /// 无参数时返回一个LMapObject.Null对象
        /// </summary>
        public LMapObject () {
            Owner = LMapObject.Null;
            Envelope = LEnvelope.Null;
        }
        public LMapObject (ObjectType t){
            Owner = LMapObject.Null;
            Envelope = LEnvelope.Null;
            ObjectType = t;
        }

        /// <summary>
        /// 改变一个MapObject对象的MBR，需要由派生类显式调用
        /// </summary>
        internal virtual void RefreshEnvelope()
        {
            //由于Null对象没有MBR，不需要实现此方法
            //其他任何LMapObject派生类均需要提供实现
            //要求的实现形式为
            //
            //public override void RefreshEnvelope(){
            //    // implementation here
            //    _Owner.RefreshEnvelope();
            //}
            
            if (this != LMapObject.Null)
                throw new NotImplementedException("RefreshEnvelope() not implemented");
        }
    }

    /// <summary>
    /// 矢量要素类的基类
    /// </summary>
    public class LVectorObject : LMapObject
    {
        public FeatureType FeatureType { get; protected set; }
        public LVectorObject() : base(ObjectType.Vector) { FeatureType = Lgis.FeatureType.Unknown; }
        public LVectorObject(FeatureType t) : base(ObjectType.Vector) { FeatureType = t; }
    }

    /// <summary>
    /// 栅格类的基类
    /// </summary>
    public class LRasterObject : LMapObject
    {
        public readonly FeatureType FeatureType ;
        public LRasterObject():base (ObjectType.Raster){}
    }

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
    }
}
