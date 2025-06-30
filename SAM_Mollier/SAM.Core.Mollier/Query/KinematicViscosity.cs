namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates kinematic viscosity from dry bulb temperature, relative humidity and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry Bulb Temperature [°C] — measured by a standard thermometer, unaffected by moisture (not wet-bulb or dew point).</param>
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

        public static double KinematicViscosity(this MollierPoint mollierPoint, double tolerance = Tolerance.Distance)
        {
            if(mollierPoint == null)
            {
                return double.NaN;
            }

            return KinematicViscosity(mollierPoint.DryBulbTemperature, mollierPoint.HumidityRatio, mollierPoint.Pressure, tolerance);
        }
    }
}
