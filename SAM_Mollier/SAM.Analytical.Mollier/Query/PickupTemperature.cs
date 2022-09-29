using SAM.Core.Mollier;

namespace SAM.Analytical.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates Pickup Temperature for given specific fan power
        /// </summary>
        /// <param name="mollierPoint"></param>
        /// <param name="sfp">Specific Fan Power [W/l/s]</param>
        /// <returns></returns>
        public static double PickupTemperature(this MollierPoint mollierPoint,  double sfp)
        {
            if(mollierPoint == null || double.IsNaN(sfp))
            {
                return double.NaN;
            }

            return sfp / (mollierPoint.Density() * Core.Mollier.Query.HeatCapacity(mollierPoint));
        }

        public static double PickupTemperature(this double dryBulbTemperature, double sfp)
        {
            if (double.IsNaN(dryBulbTemperature) || double.IsNaN(sfp))
            {
                return double.NaN;
            }

            return sfp / 1.2 * 1.005;
        }
    }
}