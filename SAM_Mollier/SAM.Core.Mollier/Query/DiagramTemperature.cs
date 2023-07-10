
namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates humidity ratio from dry bulb temperature, humidity ratio and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="humidityRatio">Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <param name="pressure">Pressure [Pa]</param>
        /// <returns>Diagram Temperature [°C]</returns>
        public static double DiagramTemperature(double dryBulbTemperature, double humidityRatio, double pressure)
        {
            if (double.IsNaN(humidityRatio) || double.IsNaN(dryBulbTemperature))
            {
                return double.NaN;
            }

            double specificHeat_WaterVapour = Zero.SpecificHeat_WaterVapour / 1000;
            double specificHeat_Air = Zero.SpecificHeat_Air / 1000;

            //double result = dryBulbTemperature + System.Math.Abs((humidityRatio * specificHeat_WaterVapour) * dryBulbTemperature);
            double result = (specificHeat_Air + humidityRatio * specificHeat_WaterVapour) * dryBulbTemperature;

            //double saturationTemperature = SaturationTemperature(dryBulbTemperature, pressure);
            //if (dryBulbTemperature < saturationTemperature)
            //{
            //    double enthalpy = Query.Enthalpy(dryBulbTemperature, humidityRatio, pressure);

            //    if(TryFindDiagramTemperature(enthalpy, humidityRatio, pressure, out double diagramTemperature))
            //    {
            //        return diagramTemperature;
            //    }

            //    //return saturationTemperature - (saturationTemperature + dryBulbTemperature) / 2;
            //}
            return result;

        }

        public static double DiagramTemperature(this MollierPoint mollierPoint)
        {
            if (mollierPoint == null)
            {
                return double.NaN;
            }

            return DiagramTemperature(mollierPoint.DryBulbTemperature, mollierPoint.HumidityRatio, mollierPoint.Pressure);
        }
    }
}
