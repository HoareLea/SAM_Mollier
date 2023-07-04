
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

            MollierPoint mollierPoint_Saturation_N = SaturationMollierPoint(dryBulbTemperature, pressure);
            if(mollierPoint_Saturation_N == null)
            {
                return double.NaN;
            }

            if(humidityRatio < mollierPoint_Saturation_N.HumidityRatio)
            {
                double specificHeat_WaterVapour = Zero.SpecificHeat_WaterVapour / 1000;

                return dryBulbTemperature + humidityRatio * specificHeat_WaterVapour * dryBulbTemperature;
            }

            double enthalpy_M = Enthalpy(dryBulbTemperature, humidityRatio, pressure);
           
            MollierPoint mollierPoint_M = Create.MollierPoint_ByEnthalpy(enthalpy_M, humidityRatio, pressure);

            return mollierPoint_M.DryBulbTemperature;

            //double saturationDryBulbTemperature_M = DryBulbTemperature_ByEnthalpy(enthalpy_M, 100, pressure);

            //MollierPoint MollierPoint_Saturation_M = SaturationMollierPoint(saturationDryBulbTemperature_M, pressure);

            //double humidiRatio_Saturation_M = MollierPoint_Saturation_M.HumidityRatio;
            //double humidityRatio_M = humidityRatio;

            //double dryBulbTemperature_Saturation_M = MollierPoint_Saturation_M.DryBulbTemperature;
            //double dryBulbTemperature_M = mollierPoint_M.DryBulbTemperature;

            //double humidiRatio_Difference = humidityRatio_M - humidiRatio_Saturation_M;
            //double dryBulbTemperature_Difference = dryBulbTemperature_M - dryBulbTemperature_Saturation_M;

            //double directionFactor = humidiRatio_Difference / dryBulbTemperature_Difference;

            //double angle = System.Math.Atan(directionFactor);
        }

        public static double DiagramTemperature(this MollierPoint mollierPoint)
        {
            if(mollierPoint == null)
            {
                return double.NaN;
            }
            
            return DiagramTemperature(mollierPoint.DryBulbTemperature, mollierPoint.HumidityRatio, mollierPoint.Pressure);
        }
    }
}
