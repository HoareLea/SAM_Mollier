﻿namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates density from dry bulb temperature, relative humidity and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Density [kg/m3]</returns>
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
    }
}
