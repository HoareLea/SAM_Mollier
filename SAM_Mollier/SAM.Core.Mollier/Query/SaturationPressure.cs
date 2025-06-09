using System;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates saturation pressure [Pa] for a given temperature [°C] using Region 4 formulation.
        /// </summary>
        /// <param name="iAPWSRegion"></param>
        /// <param name="dryBulbTemperature"></param>
        /// <returns></returns>
        public static double SaturationPressure(this IAPWSRegion iAPWSRegion, double dryBulbTemperature)
        {
            switch(iAPWSRegion)
            {
                case Mollier.IAPWSRegion.Region4:
                    double T = dryBulbTemperature + 273.15; // Convert to Kelvin
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
                    double p = Math.Pow((2 * C) / (-B + Math.Sqrt(B * B - 4 * A * C)), 4);

                    return p * 1e6; // MPa to Pa
            }


            return double.NaN;
        }
    }
}
