
using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates humidity ratio from dry bulb temperature, humidity ratio and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry Bulb Temperature [°C] — measured by a standard thermometer, unaffected by moisture (not wet-bulb or dew point).</param>
        /// <param name="humidityRatio">Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <param name="pressure">Pressure [Pa]</param>
        /// <param name="phase">Default phase for 0C (only for case where dry bulb temperature set to 0 and phase is not a gas)</param>
        /// <returns>Diagram Temperature [°C]</returns>
        public static double DiagramTemperature(double dryBulbTemperature, double humidityRatio, double pressure, Phase phase = Phase.Solid)
        {
            if (double.IsNaN(humidityRatio) || double.IsNaN(dryBulbTemperature))
            {
                return double.NaN;
            }
            
            double humidityRatio_Saturation = SaturationHumidityRatio(dryBulbTemperature, pressure);
            if(double.IsNaN(humidityRatio_Saturation) || humidityRatio_Saturation >= humidityRatio)
            {
                double specificHeat_WaterVapour = Zero.SpecificHeat_WaterVapour / 1000;
                double specificHeat_Air = Zero.SpecificHeat_Air / 1000;

                return (specificHeat_Air + humidityRatio * specificHeat_WaterVapour) * dryBulbTemperature;
            }

            double enthalpy = Enthalpy(dryBulbTemperature, humidityRatio, pressure);

            double dryBulbTemperature_1 = DryBulbTemperature(enthalpy, 0, pressure);

            double diagramTemperature_1 = DiagramTemperature(dryBulbTemperature_1, 0, pressure);

            double dryBulbTemperature_2 = DryBulbTemperature_ByEnthalpy(enthalpy, 100, pressure);
            double humidityRatio_2 = HumidityRatio(dryBulbTemperature_2, 100, pressure);

            double diagramTemperature_2 = DiagramTemperature(dryBulbTemperature_2, humidityRatio_2, pressure);

            if (!Intersection(0, diagramTemperature_1, humidityRatio_2, diagramTemperature_2, humidityRatio, 0, humidityRatio, 1, out double result, out double humidityRatio_Result))
            {
                return double.NaN;
            }

            return result;

            //result = result + (humidityRatio - humidityRatio_Saturation) * Zero.SpecificHeat_Water / 1000 * System.Math.Abs(result);

            //if (dryBulbTemperature > 0)
            //{
            //    return result;
            //}

            //if (dryBulbTemperature == 0)
            //{
            //    if (phase != Phase.Solid)
            //    {
            //        return result;
            //    }
            //}

            //return result - System.Math.Abs((humidityRatio - humidityRatio_Saturation) * (-Zero.MeltingHeat_Ice / 1000 + Zero.SpecificHeat_Ice / 1000 * result));
        }

        public static double DiagramTemperature(this MollierPoint mollierPoint, Phase phase = Phase.Solid)
        {
            if(mollierPoint == null)
            {
                return double.NaN;
            }
            
            return DiagramTemperature(mollierPoint.DryBulbTemperature, mollierPoint.HumidityRatio, mollierPoint.Pressure, phase);
        }
    }
}
