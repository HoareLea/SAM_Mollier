namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates dynamic viscosity from dry bulb temperature, relative humidity and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry Bulb Temperature [°C] — measured by a standard thermometer, unaffected by moisture (not wet-bulb or dew point).</param>
        /// <param name="humidityRatio">Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <returns>Dynamic Viscosity [Pa s]</returns>
        public static double DynamicViscosity(double dryBulbTemperature, double humidityRatio)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(humidityRatio))
            {
                return double.NaN;
            }

            double etaL = 0.0000172436 + 0.0000000504587 * dryBulbTemperature - 0.00000000003923361 * System.Math.Pow(dryBulbTemperature,2)+ 0.00000000000004046118 * System.Math.Pow(dryBulbTemperature, 3);
            double etaW = 0.0000091435 + 0.0000000281979 * dryBulbTemperature + 0.00000000004486993 * System.Math.Pow(dryBulbTemperature, 2) - 0.00000000000004928814 * System.Math.Pow(dryBulbTemperature, 3);
            double PHIW = humidityRatio / (0.6222 + humidityRatio);
            return (1 - PHIW) * etaL + PHIW * etaW;
        }

        public static double DynamicViscosity(this MollierPoint mollierPoint)
        {
            if(mollierPoint == null)
            {
                return double.NaN;
            }

            return DynamicViscosity(mollierPoint.DryBulbTemperature, mollierPoint.HumidityRatio);
        }
    }
}
