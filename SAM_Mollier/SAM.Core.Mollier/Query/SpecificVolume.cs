namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates specific volume from dry bulb temperature, relative humidity and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="humidityRatio">Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Specific Volume [m3/kg]</returns>
        public static double SpecificVolume(double dryBulbTemperature, double humidityRatio, double pressure)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(humidityRatio) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            return (humidityRatio + 0.622) * 461.53 * ((273.15 + dryBulbTemperature) / pressure);
        }

        public static double SpecificVolume(this MollierPoint mollierPoint)
        {
            if(mollierPoint == null)
            {
                return double.NaN;
            }

            return SpecificVolume(mollierPoint.DryBulbTemperature, mollierPoint.HumidityRatio, mollierPoint.Pressure);
        }
    }
}
