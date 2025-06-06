namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates saturation humidity ratio (for relative humidity 100%) from dry bulb temperature, relative humidity and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry Bulb Temperature [°C] — measured by a standard thermometer, unaffected by moisture (not wet-bulb or dew point).</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Saturation humidity ratio [kg_waterVapor/kg_dryAir]</returns>
        public static double SaturationHumidityRatio(double dryBulbTemperature, double pressure)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            double saturationVapourPressure = SaturationVapourPressure(dryBulbTemperature);
            return 0.6222 * saturationVapourPressure / (pressure - saturationVapourPressure); //by Gluck
        }

        public static double SaturationHumidityRatio_ByEnthalpy(double enthalpy, double pressure)
        {
            if(double.IsNaN(enthalpy) || double.IsNaN(pressure))
            {
                return double.NaN;
            }
            
            double dryBulbTemperature = DryBulbTemperature_ByEnthalpy(enthalpy, 100, pressure);
            if(double.IsNaN(dryBulbTemperature))
            {
                return double.NaN;
            }

            return SaturationHumidityRatio(dryBulbTemperature, pressure);
        }

        /// <summary>
        /// Calculates saturation humidity ratio (for relative humidity 100%) for given MollierPoint.
        /// </summary>
        /// <param name="mollierPoint">Mollier Point</param>
        /// <returns>Saturation humidity ratio [kg_waterVapor/kg_dryAir]</returns>
        public static double SaturationHumidityRatio(this MollierPoint mollierPoint)
        {
            if(mollierPoint == null)
            {
                return double.NaN;
            }

            return SaturationHumidityRatio(mollierPoint.DryBulbTemperature, mollierPoint.Pressure);
        }
    }
}
