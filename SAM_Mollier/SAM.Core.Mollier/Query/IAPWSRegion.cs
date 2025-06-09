
using System;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Returns IAPWS-IF97 region for given pressure [Pa] and temperature [C].
        /// </summary>
        public static IAPWSRegion IAPWSRegion(double dryBulbTemperature, double pressure)
        {
            double dryBulbTempertureK = dryBulbTemperature + 273.15;


            if (dryBulbTempertureK > 1073.15 && dryBulbTempertureK <= 2273.15 && pressure <= 50000000)
            {
                return Mollier.IAPWSRegion.Region5;
            }

            double saturationPressure = SaturationPressure(Mollier.IAPWSRegion.Region4, dryBulbTemperature);

            if (Math.Abs(pressure - saturationPressure) < 100)
            {
                return Mollier.IAPWSRegion.Region4;
            }

            if (dryBulbTempertureK <= 623.15 && pressure <= 100000000.0)
            {
                if (pressure > saturationPressure)
                    return Mollier.IAPWSRegion.Region1;
                else
                    return Mollier.IAPWSRegion.Region2;
            }

            if (dryBulbTempertureK > 623.15 && dryBulbTempertureK <= 863.15 && pressure > 16529200 && pressure <= 100000000)
            {
                return Mollier.IAPWSRegion.Region3;
            }

            return Mollier.IAPWSRegion.Undefined;
        }
    }
}
