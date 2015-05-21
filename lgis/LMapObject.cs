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
    public enum GeometryType { 
        Null = 0,
        Point = 1, 
        PolyPolyline = 3, 
        PolyPolygon = 5, 
        Polypoint = 8,
        Polyline, 
        Polygon, 
        Rectangle,
        Unknown 
    };

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
        
        //[0-9][A-Za-z][_] are legal characters
        public string _Name = "NoName";
        public string Name
        {
            get { return _Name; }
            set
            {
                string pattern = @"[0-9A-Za-z_]";
                if (System.Text.RegularExpressions.Regex.IsMatch(value, pattern))
                    _Name = value;
                else
                    throw new Exception("Illegal name: only [0-9A-Za-z_]are allowed");
            }
        }
        public readonly ObjectType ObjectType=ObjectType.Unknown;
        public LMapObject Owner { get; internal set; }
        public virtual LEnvelope Envelope { 
            get
            {
                return LEnvelope.Null;
            }
        }
        public double XMin {get{return Envelope.XMin ;}}
        public double XMax {get{return Envelope.XMax ;}}
        public double YMin {get{return Envelope.YMin ;}}
        public double YMax {get{return Envelope.YMax ;}}
        public double Width { get { return XMax - XMin; } }
        public double Height { get { return YMax - YMin; } }
        #endregion

        #region methods
        /// <summary>
        /// 无参数时返回一个LMapObject.Null对象
        /// </summary>
        public LMapObject () {
            Owner = LMapObject.Null;
        }
        public LMapObject (ObjectType t){
            Owner = LMapObject.Null;
            ObjectType = t;
        }
        public virtual string Info()
        {
            return "No Infomation";
        }
        #endregion


    }
}
