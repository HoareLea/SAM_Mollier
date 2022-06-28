namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates specific volume from dry bulb temperature, relative humidity and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="humidityRatio">Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <returns>Specific Volume [m3/kg]</returns>
        public static double DynamicViscosity(double dryBulbTemperature, double humidityRatio)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(humidityRatio) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            return (humidityRatio + 0.622) * 461.53 * ((273.15 + dryBulbTemperature) / pressure);
        }
    }
}
