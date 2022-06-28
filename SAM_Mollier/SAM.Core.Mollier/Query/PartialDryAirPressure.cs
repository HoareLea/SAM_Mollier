using System;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Partial Dry Air Pressure [Pa] for given pressure and partial vapour pressure.
        /// </summary>
        /// <param name="pressure">Pressure [Pa]</param>
        /// <param name="partialVapourPressure">Partial vapour pressure [Pa]</param>
        /// <returns>Partial dry air pressure [Pa]</returns>
        public static double PartialDryAirPressure(double pressure, double partialVapourPressure)
        {
            if(double.IsNaN(pressure) || double.IsNaN(partialVapourPressure))
            {
                return double.NaN;
            }

            return pressure - partialVapourPressure;
        }

        /// <summary>
        /// Calculates partial dry air pressure from dry bulb temperature, relative humidity and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="relativeHumidity">Relative humidity (0 - 100)[%]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Partial Dry Air Pressure [Pa]</returns>
        public static double PartialDryAirPressure(double dryBulbTemperature, double relativeHumidity, double pressure)
        {
            if (double.IsNaN(pressure) || double.IsNaN(dryBulbTemperature) || double.IsNaN(relativeHumidity))
            {
                return double.NaN;
            }

            double partialVapourPressure = PartialVapourPressure(dryBulbTemperature, relativeHumidity);
            if(double.IsNaN(partialVapourPressure))
            {
                return double.NaN;
            }

            return PartialDryAirPressure(pressure, partialVapourPressure); 
        }
    }
}
