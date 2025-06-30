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

            // Calculate Saturation Vapor Pressure over ice [Pa] for dry-bulb temperatures below -20°C
            // using the Goff–Gratch equation (1946, revised 1957).
            // This method is valid for sub-zero temperatures down to about -50°C or lower,
            // and is accurate to within ~0.1% near 0 °C. It is widely trusted in meteorology for vapor pressure estimation over ice.
            //
            // References:
            // - https://en.wikipedia.org/wiki/Saturation_vapour_pressure#Goff%E2%80%93Gratch_equation
            // - https://patarnott.com/atms360/pdf_atms360/class2017/VaporPressureIce_SupercooledH20_Murphy.pdf
            //
            // Parameters:
            // dryBulbTemperature: Temperature in degrees Celsius (°C)
            //
            // Returns:
            // Saturation vapor pressure in Pascals (Pa)
            //
            // Valid for temperatures T < -20°C
            if (dryBulbTemperature < -20.0)
            {
                double T = dryBulbTemperature + 273.15; // Convert °C to Kelvin (K)
                double T0 = 273.16;                      // Triple point temperature of water (K)
                double e0 = 6.1173;                      // Saturation vapor pressure at triple point (hPa)

                // Compute the base-10 logarithm of saturation vapor pressure using Goff-Gratch equation for ice
                double log10_e =
                    -9.09718 * (T0 / T - 1.0) -
                    3.56654 * Math.Log10(T0 / T) +
                    0.876793 * (1.0 - T / T0) +
                    Math.Log10(e0);

                double e_hPa = Math.Pow(10.0, log10_e);  // Convert log10 value back to linear scale (hPa)
                return e_hPa * 100.0;                     // Convert from hPa to Pa and return
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
        /// - Uses Glück (1991) for T ≤ 0°C was replaced with IAPWS-2011 Sublimation Curve for T < 0°C
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
                //return 611 * Math.Exp(
                //    -4.909965e-4 +
                //    8.183197e-2 * dryBulbTemperature +
                //    -5.552967e-4 * Math.Pow(dryBulbTemperature, 2) +
                //    -2.228376e-5 * Math.Pow(dryBulbTemperature, 3) +
                //    -6.211808e-7 * Math.Pow(dryBulbTemperature, 4));

                //WIP as IAPWS-2011 Sublimation Curve does not work as expected-----

                // IAPWS-2011 Sublimation Curve
                // From the IAPWS Release on the Pressure along the Sublimation Curve of Ordinary Water Substance (2011).

                // Corrected implementation using the official IAPWS-2011 sublimation curve
                // Valid between 130 K (–143.15°C) and 273.16 K (0.01°C)
                double T = dryBulbTemperature + 273.15; // Temperature in Kelvin

                const double T_t = 273.16; // Triple point temperature [K]
                const double p_t = 611.657; // Triple point pressure [Pa]

                // IAPWS-2011 sublimation equation coefficients
                const double a1 = -13.928169;
                const double a2 = 34.707823;
                const double a3 = -6.733134;

                // IAPWS-2011 sublimation equation implementation
                var theta = T / T_t;
                double ln_p = (a1 * (1 - Math.Pow(theta, -1.5))) +
                              (a2 * (1 - Math.Pow(theta, -1.25))) +
                              (a3 * (1 - Math.Pow(theta, -0.5)));

                double p = p_t * Math.Exp(ln_p);

                return p;

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
