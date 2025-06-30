namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates saturation MollierPoint (for relative humidity 100%) from dry bulb temperature, relative humidity and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry Bulb Temperature [°C] — measured by a standard thermometer, unaffected by moisture (not wet-bulb or dew point).</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>MollierPoint (for relative humidity 100%)</returns>
        public static MollierPoint SaturationMollierPoint(double dryBulbTemperature, double pressure)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(pressure))
            {
                return null;
            }

            double humidityRatio_Saturation = SaturationHumidityRatio(dryBulbTemperature, pressure);
            if(double.IsNaN(humidityRatio_Saturation))
            {
                return null;
            }

            double dryBulbTemperature_Saturation = DryBulbTemperature_ByHumidityRatio(humidityRatio_Saturation, 100, pressure);
            if(double.IsNaN(dryBulbTemperature_Saturation))
            {
                return null;
            }

            return new MollierPoint(dryBulbTemperature_Saturation, humidityRatio_Saturation, pressure);
        }

        /// <summary>
        /// Calculates saturation MollierPoint (for relative humidity 100%) for given MollierPoint.
        /// </summary>
        /// <param name="mollierPoint">MollierPoint</param>
        /// <returns>MollierPoint (for relative humidity 100%)</returns>
        public static MollierPoint SaturationMollierPoint(this MollierPoint mollierPoint)
        {
            if (mollierPoint == null)
            {
                return null;
            }

            return SaturationMollierPoint(mollierPoint.DryBulbTemperature, mollierPoint.Pressure);

        }
    }
}
