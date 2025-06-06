namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates ThermalConductivity of Air from dry bulb temperature and relative humidity.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry Bulb Temperature [°C] — measured by a standard thermometer, unaffected by moisture (not wet-bulb or dew point).</param>
        /// <param name="humidityRatio">Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <returns>Air Thermal Conductivity [W/(mK)]</returns>
        public static double ThermalConductivity(double dryBulbTemperature, double humidityRatio)
        {
            if (double.IsNaN(dryBulbTemperature) || double.IsNaN(humidityRatio))
            {
                return double.NaN;
            }

            double lamL = 0.024178 + 0.00007634878 * dryBulbTemperature - 0.00000004663859 * System.Math.Pow(dryBulbTemperature, 2) + 0.00000000004612639 * System.Math.Pow(dryBulbTemperature, 3);
            double lamW = 0.016976 + 0.000057535 * dryBulbTemperature + 0.0000001277125 * System.Math.Pow(dryBulbTemperature, 2) - 0.00000000008951228 * System.Math.Pow(dryBulbTemperature, 3);

            double PHIW = humidityRatio / (0.6222 + humidityRatio);

            return (1 - PHIW) * lamL + PHIW * lamW;
        }

        public static double ThermalConductivity(this MollierPoint mollierPoint)
        {
            if(mollierPoint == null)
            {
                return double.NaN;
            }

            return ThermalConductivity(mollierPoint.DryBulbTemperature, mollierPoint.HumidityRatio);
        }
    }
}
