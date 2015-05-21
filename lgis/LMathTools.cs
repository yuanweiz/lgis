using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lgis
{
    public static class LMathTools
    {
        static double UnitsInMeter( LinearUnit unit)
        {
            switch (unit)
            {
                case LinearUnit.Foot:
                    return 0.3048 ;
                case LinearUnit.Mile:
                    return 1609.344;
                case LinearUnit.KiloMiter:
                    return 1000 ;
                case LinearUnit.Meter:
                default:
                    return 1.0 ;
            }
        }
        public static double UnitTransform(double val,LinearUnit from, LinearUnit to)
        {
            return val * UnitsInMeter(from) / UnitsInMeter(to);

        }
    }
}
