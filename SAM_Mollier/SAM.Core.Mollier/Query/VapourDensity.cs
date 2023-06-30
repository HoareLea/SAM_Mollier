namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calcilates Vapour Density [kg/m3]
        /// </summary>
        /// <param name="mollierPoint"></param>
        /// <returns>Vapour Density [kg/m3]</returns>
        public static double VapourDensity(this MollierPoint mollierPoint)
        {
            if(mollierPoint == null)
            {
                return double.NaN;
            }

            double dryBulbTemperature = mollierPoint.DryBulbTemperature;
            if (double.IsNaN(dryBulbTemperature))
            {
                return double.NaN;
            }

            double partialVapourPressure = PartialVapourPressure(mollierPoint);
            if(double.IsNaN(partialVapourPressure))
            {
                return double.NaN;
            }

            return partialVapourPressure / (461.5 * (dryBulbTemperature + 273.15)); // Where: 461.5 => Specific gas constant for water vapour J/(kg*K) indywidualna stała gazowa pary wodnej

        }
    }
}
