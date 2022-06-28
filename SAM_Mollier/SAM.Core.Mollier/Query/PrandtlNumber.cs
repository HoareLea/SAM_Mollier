namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates Prandtl Number from dry bulb temperature, relative humidity and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="humidityRatio">Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <param name="pressure">Pressure [Pa]</param>
        /// <param name="tolerance">Tolerance</param>
        /// <returns>Prandtl Number [-]</returns>
        public static double PrandtlNumber(double dryBulbTemperature, double humidityRatio, double pressure, double tolerance = Tolerance.Distance)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(humidityRatio) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            if(System.Math.Abs(humidityRatio) < tolerance)
            {
                return 0.71789 - 0.0001675739 * dryBulbTemperature + 0.0000006514142 * System.Math.Pow(dryBulbTemperature, 2) - 0.0000000006687762 * System.Math.Pow(dryBulbTemperature, 3);
            }

            double dynamicViscosity = DynamicViscosity(dryBulbTemperature, humidityRatio);
            if(double.IsNaN(dynamicViscosity))
            {
                return double.NaN;
            }

            double heatCapacity = HeatCapacity(dryBulbTemperature, humidityRatio);
            if (double.IsNaN(heatCapacity))
            {
                return double.NaN;
            }

            double thermalConductivity = ThermalConductivity(dryBulbTemperature, humidityRatio);
            if(double.IsNaN(thermalConductivity))
            {
                return double.NaN;
            }

            return dynamicViscosity * (heatCapacity * 1000) / thermalConductivity;
        }
    }
}
