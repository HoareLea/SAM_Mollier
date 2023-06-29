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

        /// <summary>
        /// Calculates enthalpy for end point for given sensible heat ratio
        /// </summary>
        /// <param name="sensibleHeatRatio">Sensible Heat Ratio (SHR) [-] value from 0 to 1 </param>
        /// <param name="specificHeat">Specific Heat [J/kg*K]</param>
        /// <param name="dryBulbTemperature_Start">Start Dry Bulb Tempearture [C]</param>
        /// <param name="dryBulbTemperature_End">End Dry Bulb Tempearture [C]</param>
        /// <param name="enthalpy_Start">Start Enthalpy [J/kg]</param>
        /// <returns>End Enthalpy [J/kg]</returns>
        public static double Enthalpy_BySensibleHeatRatio(double sensibleHeatRatio, double specificHeat, double dryBulbTemperature_Start, double dryBulbTemperature_End, double enthalpy_Start)
        {
            if(double.IsNaN(sensibleHeatRatio) || double.IsNaN(specificHeat) || double.IsNaN(dryBulbTemperature_Start) || double.IsNaN(dryBulbTemperature_End) || double.IsNaN(enthalpy_Start))
            {
                return double.NaN;
            }

            if(sensibleHeatRatio == 0 || (dryBulbTemperature_End - dryBulbTemperature_Start) == 0)
            {
                return double.NaN;
            }

            return enthalpy_Start - (specificHeat * (dryBulbTemperature_Start - dryBulbTemperature_End) / sensibleHeatRatio);
        }

        public static double Enthalpy_BySensibleHeatRatio(double sensibleHeatRatio, double specificHeat, MollierPoint mollierPoint_Start, double dryBulbTemperature_End)
        {
            if(mollierPoint_Start == null)
            {
                return double.NaN;
            }

            return Enthalpy_BySensibleHeatRatio(sensibleHeatRatio, specificHeat, mollierPoint_Start.DryBulbTemperature, dryBulbTemperature_End, mollierPoint_Start.Enthalpy);
        }
    }
}
