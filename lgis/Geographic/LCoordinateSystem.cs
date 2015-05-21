using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lgis
{
    public enum LinearUnit { Meter,Degree,Foot,Unknown};
    public enum AngularUnit { Degree,Unknown}
    public class LProjectedCoordinateSystem
    {
        #region preset projectedCSs
        public static LProjectedCoordinateSystem
            Beijing_1954_3_Degree_GK_CM_114E = new LProjectedCoordinateSystem(
                LGeographicCoordinateSystem.GCS_Beijing_1954,
                "Beijing_1954_3_Degree_GK_CM_114E",
                114.0,
                1.0,
                "Gauss_Kruger",
                LinearUnit.Meter);
        #endregion
        internal LProjectedCoordinateSystem(LGeographicCoordinateSystem geographicCS,
            string name,
            double centralMeridian,
            double scaleFactor,
            string projection,
            LinearUnit linearUnit)
        {
            Name = name;
            GeographicCS = geographicCS;
            CentralMeridian = centralMeridian;
            ScaleFactor = scaleFactor;
            Projection = projection;
            LinearUnit = linearUnit;
        }
        string Name;
        LGeographicCoordinateSystem GeographicCS;
        double CentralMeridian;
        double ScaleFactor;
        string Projection;
        LinearUnit LinearUnit;
    }
    public class LGeographicCoordinateSystem
    {
        #region preset GeographicCSs
        public static LGeographicCoordinateSystem
            GCS_WGS_1984 = new LGeographicCoordinateSystem("GCS_WGS_1984", "WGS_1984",AngularUnit.Degree, 6378137.0, 6356752.314245179, 298.257223563),
            GCS_Beijing_1954 = new LGeographicCoordinateSystem("GCS_Beijing_1954", "Krasovsky_1940", AngularUnit.Degree, 6378245.0, 6356863.018773047, 298.3);
        #endregion

        internal LGeographicCoordinateSystem (string name ,
            string spheroid,
            AngularUnit angularUint,
            double semimajorAxis,
            double semiminorAxis,
            double inverseFlattening)
        {
            Name = name;
            Spheroid = spheroid;
            this.AngularUnit = angularUint;
            SemimajorAxis = semimajorAxis;
            SemiminorAxis = semiminorAxis;
            InverseFlattening = inverseFlattening;
        }
        #region properties
        string Name;
        string Spheroid;
        AngularUnit AngularUnit;
        double SemimajorAxis;
        double SemiminorAxis;
        double InverseFlattening;
        #endregion

    }


}
