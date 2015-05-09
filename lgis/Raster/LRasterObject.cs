using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lgis
{
    /// <summary>
    /// 栅格类的基类
    /// </summary>
    public class LRasterObject : LMapObject
    {
        public readonly GeometryType FeatureType ;
        public LRasterObject():base (ObjectType.Raster){}
    }

}