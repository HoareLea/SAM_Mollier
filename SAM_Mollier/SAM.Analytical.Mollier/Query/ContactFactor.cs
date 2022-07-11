using SAM.Core.Mollier;

namespace SAM.Analytical.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates Contact Factor
        /// </summary>
        /// <param name="point_1"></param>
        /// <param name="point_2"></param>
        /// <returns></returns>
        public static double ContactFactor(this MollierPoint point_1, MollierPoint point_2)
        {
            if (point_1 == null || point_2 == null)
            {
                return double.NaN;
            }

            throw new System.NotImplementedException();

            //return sfp / (mollierPoint.Density() * Core.Mollier.Query.HeatCapacity(mollierPoint));
        }
    }
}