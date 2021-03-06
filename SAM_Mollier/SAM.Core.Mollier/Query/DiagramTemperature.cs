
namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates humidity ratio from dry bulb temperature, humidity ratio and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="humidityRatio">Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <returns>Diagram Temperature [°C]</returns>
        public static double DiagramTemperature(double dryBulbTemperature, double humidityRatio)
        {
            if (double.IsNaN(humidityRatio) || double.IsNaN(dryBulbTemperature))
            {
                return double.NaN;
            }

            return dryBulbTemperature + humidityRatio * 1.86 * dryBulbTemperature;
        }

        public static double DiagramTemperature(MollierPoint mollierPoint)
        {
            if(mollierPoint == null)
            {
                return double.NaN;
            }

            return DiagramTemperature(mollierPoint.DryBulbTemperature, mollierPoint.HumidityRatio);
        }
    }
}
