namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates temperature conductivity (Thermal diffusivity) from dry bulb temperature, relative humidity and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="humidityRatio">Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <param name="pressure">Pressure [Pa]</param>
        /// <param name="tolerance">Tolerance</param>
        /// <returns>Temperature Conductivity (Thermal diffusivity) [???]</returns>
        public static double TemperatureConductivity(double dryBulbTemperature, double humidityRatio, double pressure, double tolerance = Tolerance.Distance)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(humidityRatio) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            if(System.Math.Abs(humidityRatio) < tolerance)
            {
                double aL = 0.0000188328 + 0.0000001286753 * dryBulbTemperature + 0.0000000001680101 * System.Math.Pow(dryBulbTemperature, 2) - 0.0000000000001240072 * System.Math.Pow(dryBulbTemperature, 3);
                return aL * 100000 / pressure;
            }

            double thermalConductivity = ThermalConductivity(dryBulbTemperature, humidityRatio);
            if(double.IsNaN(thermalConductivity))
            {
                return double.NaN;
            }


            double heatCapacity = HeatCapacity(dryBulbTemperature, humidityRatio);
            if(double.IsNaN(heatCapacity))
            {
                return double.NaN;
            }


            double density = Density_ByHumidityRatio(dryBulbTemperature, humidityRatio, pressure);
            if(double.IsNaN(density))
            {
                return double.NaN;
            }


            return thermalConductivity / (heatCapacity * 1000) / density;
        }

        public static double TemperatureConductivity(this MollierPoint mollierPoint, double tolerance = Tolerance.Distance)
        {
            if(mollierPoint == null)
            {
                return double.NaN;
            }

            return TemperatureConductivity(mollierPoint.DryBulbTemperature, mollierPoint.HumidityRatio, mollierPoint.Pressure, tolerance);
        }
    }
}
