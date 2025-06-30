namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates specific volume for dry air from dry bulb temperature, relative humidity and pressure v[m³/kg_dryAir].
        /// </summary>
        /// <param name="dryBulbTemperature">Dry Bulb Temperature [°C] — measured by a standard thermometer, unaffected by moisture (not wet-bulb or dew point).</param>
        /// <param name="humidityRatio">Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Dry Air Specific Volume v [m³/kg_dryAir]</returns>
        public static double SpecificVolume(double dryBulbTemperature, double humidityRatio, double pressure)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(humidityRatio) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            double gasConstant_WaterVapour = Constant.GasConstant_WaterVapour;

            double saturationHumidityRatio = SaturationHumidityRatio(dryBulbTemperature, pressure);
            if (double.IsNaN(saturationHumidityRatio))
            {
                return double.NaN;
            }

            if (humidityRatio < saturationHumidityRatio)
            {
                saturationHumidityRatio = humidityRatio;
            }

            //return ((humidityRatio / 0.6222) + 1) * gasConstant_WaterVapour * ((273.15 + dryBulbTemperature) / pressure);

            //return 1 / Density_ByHumidityRatio(dryBulbTemperature, humidityRatio, pressure);

            return (saturationHumidityRatio + 0.622) * gasConstant_WaterVapour * ((273.15 + dryBulbTemperature) / pressure); //old one to be confirmed variable
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
