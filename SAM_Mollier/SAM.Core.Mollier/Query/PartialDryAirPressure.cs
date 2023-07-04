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
                double humidityRatio = HumidityRatio(dryBulbTemperature, relativeHumidity, pressure);
                if (double.IsNaN(humidityRatio))
                {
                    return double.NaN;
                }

                return PartialVapourPressure_ByHumidityRatio(humidityRatio, pressure);
            }

            return PartialDryAirPressure(pressure, partialVapourPressure);
        }

        /// <summary>
        /// Calculates PartialDryAirPressure from humidityRatio and Pressure (Prof. Dr.-Ing. habil. Bernd Glück, Zustands- und Stoffwerte 2. überarbeitete und erweiterte Auflage Berlin: Verlag für Bauwesen 1991, ISBN 3-345-00487-9 equation 2.7)
        /// </summary>
        /// <param name="humidityRatio">Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <param name="pressure">Pressure [Pa]</param>
        /// <returns>Partial Dry Air Pressure [Pa]</returns>
        public static double PartialDryAirPressure_ByHumidityRatio(double humidityRatio, double pressure)
        {
            if(double.IsNaN(humidityRatio) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            return 0.6222 / (0.6222 + humidityRatio) * pressure;
        }

        public static double PartialDryAirPressure(this MollierPoint mollierPoint)
        {
            if(mollierPoint == null)
            {
                return double.NaN;
            }

            return PartialDryAirPressure(mollierPoint.DryBulbTemperature, mollierPoint.RelativeHumidity, mollierPoint.Pressure);
        }
    }
}
