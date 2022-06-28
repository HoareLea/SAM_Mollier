namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates kinematic viscosity from dry bulb temperature, relative humidity and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="humidityRatio">Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <param name="pressure">Pressure [Pa]</param>
        /// <param name="tolerance">Tolerance</param>
        /// <returns>Kinematic Viscosity [m2/s]</returns>
        public static double KinematicViscosity(double dryBulbTemperature, double humidityRatio, double pressure, double tolerance = Tolerance.Distance)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(humidityRatio) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            if(System.Math.Abs(humidityRatio) <= tolerance)
            {
                double NyL = 0.0000135198 + 0.00000008930841 * dryBulbTemperature + 0.0000000001094808 * System.Math.Pow(dryBulbTemperature, 2) - 0.00000000000003659345 * System.Math.Pow(dryBulbTemperature, 3);
                return NyL * 100000 / pressure;
            }

            double dynamicViscosity = DynamicViscosity(dryBulbTemperature, humidityRatio);
            if(double.IsNaN(dynamicViscosity))
            {
                return double.NaN;
            }

            double density = Density_ByHumidityRatio(dryBulbTemperature, humidityRatio, pressure);
            if(double.IsNaN(density))
            {
                return double.NaN;
            }

            return dynamicViscosity / density;
        }
    }
}
