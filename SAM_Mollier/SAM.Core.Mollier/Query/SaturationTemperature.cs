using System;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates saturation temperature (relative humidity 100%) from dry bulb temperature, relative humidity and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Saturation Temperature (for relative humidity 100%)[°C]</returns>
        public static double SaturationTemperature(double dryBulbTemperature, double pressure)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            double humidityRatio = SaturationHumidityRatio(dryBulbTemperature, pressure);
            if(double.IsNaN(humidityRatio))
            {
                return double.NaN;
            }

            return DryBulbTemperature_ByHumidityRatio(humidityRatio, 100, pressure);

        }

        /// <summary>
        /// Calculates saturation temperature (relative humidity 100%) for given MollierPoint.
        /// </summary>
        /// <param name="mollierPoint">MollierPoint</param>
        /// <returns>Saturation Temperature (for relative humidity 100%)[°C]</returns>
        public static double SaturationTemperature(this MollierPoint mollierPoint)
        {
            if(mollierPoint == null)
            {
                return double.NaN;
            }

            return SaturationTemperature(mollierPoint.DryBulbTemperature, mollierPoint.Pressure);
        }

        public static double SaturationTemperature(double partialVapourPressure)
        {
            double partialVapourPressure_Log = Math.Log(partialVapourPressure);

            if (611 <= partialVapourPressure && partialVapourPressure <= 101320)
            {
                return -63.16113 + 5.36859 * partialVapourPressure_Log + 0.973587 * Math.Pow(partialVapourPressure_Log, 2) - 0.0738636 * Math.Pow(partialVapourPressure_Log, 3) + 0.00481832 * Math.Pow(partialVapourPressure_Log, 4);
            }
            else if (103 <= partialVapourPressure && partialVapourPressure < 611)
            {
                return -61.125785 + 8.1386 * partialVapourPressure_Log - 0.07422003 * Math.Pow(partialVapourPressure_Log, 2) + 0.06283721 * Math.Pow(partialVapourPressure_Log, 3) - 0.0027237063 * Math.Pow(partialVapourPressure_Log, 4);
            }

            return double.NaN;

        }

        public static double SaturationTemperature_ByRelativeHumidity(double dryBulbTemperature, double relativeHumidity)
        {
            double partialVapourPressure = PartialVapourPressure(dryBulbTemperature, relativeHumidity);
            if(double.IsNaN(partialVapourPressure))
            {
                return double.NaN;
            }

            return SaturationTemperature(partialVapourPressure);
        }

        public static double SaturationTemperature_ByHumidityRatio(double dryBulbTemperature, double humidtyRatio)
        {
            double partialVapourPressure = PartialVapourPressure_ByHumidityRatio(dryBulbTemperature, humidtyRatio);
            if (double.IsNaN(partialVapourPressure))
            {
                return double.NaN;
            }

            return SaturationTemperature(partialVapourPressure);
        }
    }
}
