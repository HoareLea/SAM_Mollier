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
    }
}
