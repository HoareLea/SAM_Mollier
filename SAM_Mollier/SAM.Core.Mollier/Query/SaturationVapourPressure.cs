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

        /// <summary>
        /// Saturation Vapour Pressure [Pa] for given dry-bulb temperature (ps).
        /// - Uses Glück (1991) for T ≤ 0°C
        /// - Uses IAPWS-IF97 Region 4 (Wagner & Pruß, 1993) for T > 0°C
        /// </summary>
        /// <param name="dryBulbTemperature">Dry Bulb Temperature [°C] — measured by a standard thermometer, unaffected by moisture (not wet-bulb or dew point).</param>
        /// <returns>Saturation Vapour Pressure [Pa]</returns>
        public static double SaturationVapourPressure_IAPWS(double dryBulbTemperature)
        {
            if (double.IsNaN(dryBulbTemperature))
                return double.NaN;

            if (dryBulbTemperature <= 0.0)
            {
                // Glück (1991), Equation 1.1 (T ≤ 0°C)
                return 611 * Math.Exp(
                    -4.909965e-4 +
                    8.183197e-2 * dryBulbTemperature +
                    -5.552967e-4 * Math.Pow(dryBulbTemperature, 2) +
                    -2.228376e-5 * Math.Pow(dryBulbTemperature, 3) +
                    -6.211808e-7 * Math.Pow(dryBulbTemperature, 4));
            }
            else
            {
                // IAPWS-IF97 Region 4 (Wagner & Pruß, 1993)
                double T = dryBulbTemperature + 273.15; // K

                double[] n = new double[]
                {
                    0.11670521452767e4,
                    -0.72421316703206e6,
                    -0.17073846940092e2,
                    0.12020824702470e5,
                    -0.32325550322333e7,
                    0.14915108613530e2,
                    -0.48232657361591e4,
                    0.40511340542057e6,
                    -0.23855557567849,
                    0.65017534844798e3
                };

                double theta = T + n[8] / (T - n[9]);
                double A = theta * theta + n[0] * theta + n[1];
                double B = n[2] * theta * theta + n[3] * theta + n[4];
                double C = n[5] * theta * theta + n[6] * theta + n[7];
                double p = Math.Pow((2 * C) / (-B + Math.Sqrt(B * B - 4 * A * C)), 4); // MPa

                return p * 1e6; // Pa
            }
        }

    }
}
