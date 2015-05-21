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

    //the type of LVectorLayer
    public enum LayerType{ Vector,Raster ,Network,Unknown}
    public enum FeatureType { Polygon, Line, Point,Unknown };
    public enum LinearUnit { Meter,Degree,Foot,Unknown};
    public enum AngularUnit { Degree,Unknown}


    public enum SymbolType { Line, Polygon, Point };
    public enum SymbolStyle
    {
        Unknown,

        //Line
        SolidLine,
        DashLine,
        DotLine,

        //Point marker
        ImageMarker,
        CircleMarker,

        //Polygon
        TransparentFill,
        SolidColorFill
    }
    
}
