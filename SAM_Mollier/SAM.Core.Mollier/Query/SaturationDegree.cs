namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates degree of saturation. Unitless value (ratio) between 0 - 1
        /// </summary>
        /// <param name="mollierPoint"></param>
        /// <returns>Degree of Saturation [-]</returns>
        public static double SaturationDegree(this MollierPoint mollierPoint)
        {
            if(mollierPoint == null)
            {
                return double.NaN;
            }

            return SaturationDegree(mollierPoint.DryBulbTemperature, mollierPoint.Pressure);
        }

        /// <summary>
        /// Calculates degree of saturation. Unitless value (ratio) between 0 - 1
        /// </summary>
        /// <param name="dryBulbTemperature">Dry Bulb Temperature [°C]</param>
        /// <param name="pressure">Pressure [Pa]</param>
        /// <returns>Degree of Saturation [-]</returns>
        public static double SaturationDegree(double dryBulbTemperature, double pressure)
        {
            if (double.IsNaN(dryBulbTemperature) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            double saturationVapourPressure = SaturationVapourPressure(dryBulbTemperature);
            if (double.IsNaN(saturationVapourPressure))
            {
                return double.NaN;
            }

            if (saturationVapourPressure == pressure)
            {
                return double.NaN;
            }

            return (0.622 * saturationVapourPressure) / (pressure - saturationVapourPressure);
        }
    }
}
