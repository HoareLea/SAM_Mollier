﻿namespace SAM.Core.Mollier
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

            double saturationVapourPressure = SaturationVapourPressure(mollierPoint.DryBulbTemperature);
            if (double.IsNaN(saturationVapourPressure) || saturationVapourPressure == 0)
            {
                return double.NaN;
            }

            double partialVapourPressure = PartialVapourPressure(mollierPoint);
            if(double.IsNaN(partialVapourPressure))
            {
                return double.NaN;
            }

            return partialVapourPressure / saturationVapourPressure;

        }
    }
}
