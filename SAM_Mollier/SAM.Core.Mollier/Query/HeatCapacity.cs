namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Heat Capacity of Water
        /// </summary>
        /// <param name="temperature">Temperature [°C]</param>
        /// <returns>Heat Capacity [kJ/kgK]</returns>
        public static double HeatCapacity(double temperature)
        {
            if (temperature < 0 || temperature > 100)
            {
                return double.NaN;
            }

            return System.Math.Pow(0.000000000511 * temperature, 4) - System.Math.Pow(0.00000019 * temperature, 3) + System.Math.Pow(0.0000351 * temperature, 2) - 0.00209 * temperature + 4.22;
        }

        /// <summary>
        /// Heat Capacity of Air
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="humidityRatio">Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <returns>Heat Capacity of Air [kJ/kgK]</returns>
        public static double HeatCapacity(double dryBulbTemperature, double humidityRatio)
        {
            if (double.IsNaN(humidityRatio) || double.IsNaN(dryBulbTemperature))
            {
                return double.NaN;
            }

            double cLL = 1.0065 + 0.000005309587 * dryBulbTemperature + 0.0000004758596 * System.Math.Pow(dryBulbTemperature, 2) - 0.0000000001136145 * System.Math.Pow(dryBulbTemperature, 3);
            double cWL = 1.863 + 0.0002680862 * dryBulbTemperature + 0.0000006794704 * System.Math.Pow(dryBulbTemperature, 2) - 0.0000000002641422 * System.Math.Pow(dryBulbTemperature, 3);

            double humidityRatio_Temp = humidityRatio * 1000;

            return (cLL + humidityRatio_Temp * cWL) / (1 + humidityRatio_Temp);
        }
    }
}
