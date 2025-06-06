using System;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Saturation Vapour Pressure [Pa] for given dry-bulb temperature (ps). Gluck (1.1 and 1.4) temerature range from -20 to 100
        /// </summary>
        /// <param name="dryBulbTemperature">Dry Bulb Temperature [°C] — measured by a standard thermometer, unaffected by moisture (not wet-bulb or dew point).</param>
        /// <returns>Saturation Vapour Pressure [Pa]</returns>
        public static double SaturationVapourPressure(double dryBulbTemperature)
        {
            if (double.IsNaN(dryBulbTemperature))
            {
                return double.NaN;
            }

            if (dryBulbTemperature < 0.01) //from -20C to 0.01C max 0.00% (Gluck 1.1)
            {
                return 611 * Math.Exp(-4.909965e-4 + 8.183197e-2 * dryBulbTemperature - 5.552967e-4 * Math.Pow(dryBulbTemperature, 2) - 2.228376e-5 * Math.Pow(dryBulbTemperature, 3) - 6.211808e-7 * Math.Pow(dryBulbTemperature, 4)); 
            }

            if (dryBulbTemperature <= 100)//from 0.01C to 100C max 0.02% (Gluck 1.4)
            {
                return 611 * Math.Exp(-1.91275e-4 + 7.258e-2 * dryBulbTemperature - 2.939e-4 * Math.Pow(dryBulbTemperature, 2) + 9.841e-7 * Math.Pow(dryBulbTemperature, 3) - 1.92e-9 * Math.Pow(dryBulbTemperature, 4));
            }

            //if (dryBulbTemperature < 0)
            //{
            //    return 4.689 * Math.Pow((1.486 + dryBulbTemperature / 100), 12.3);
            //}

            //if (dryBulbTemperature <= 100)
            //{
            //    return 611.213 * Math.Exp(0.07257 * dryBulbTemperature - 0.0002937 * Math.Pow(dryBulbTemperature, 2) + 0.000000981 * Math.Pow(dryBulbTemperature, 3) - 0.000000001901 * Math.Pow(dryBulbTemperature, 4));
            //}

            return double.NaN;
        }

        public static double SaturationVapourPressure(this MollierPoint mollierPoint)
        {
            if(mollierPoint == null)
            {
                return double.NaN;
            }

            return SaturationVapourPressure(mollierPoint.DryBulbTemperature);
        }
    }
}
