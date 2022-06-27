namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates enthalpy from dry bulb temperature and humidity ratio.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="humidityRatio">Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <returns>Enthalpy [J/kg]</returns>
        public static double Enthalpy(double dryBulbTemperature, double humidityRatio)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(humidityRatio))
            {
                return double.NaN;
            }

            if(dryBulbTemperature < -50)
            {
                return double.NaN;
            }

            if(dryBulbTemperature < 100)
            {
                double result = 1.005 * dryBulbTemperature + humidityRatio * (2501 + 1.86 * dryBulbTemperature);
                result = result * 1000;
                return result;
                //From Recknagel Sprenger 07/08 page 133
            }

            return double.NaN;
        }

        /// <summary>
        /// Calculates enthalpy from dry bulb temperature, relative humidity and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="relativeHumidity">Relative humidity [%]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Enthalpy [J/kg]</returns>
        public static double Enthalpy_ByRelativeHumidity(double dryBulbTemperature, double relativeHumidity, double pressure)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(relativeHumidity) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            if(dryBulbTemperature < -50)
            {
                return double.NaN;
                //'it is too cold'
            }

            if(dryBulbTemperature < 100)
            {
                double humidityRatio = HumidityRatio(dryBulbTemperature, relativeHumidity, pressure);
                if(!double.IsNaN(humidityRatio))
                {
                    return Enthalpy(dryBulbTemperature, humidityRatio);
                }
            }

            return double.NaN;
        }
    }
}
