using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Query
    { 
        public static Dictionary<double, List<MollierPoint>> DensityLine(double density_Min = Default.DensityMin, double density_Max = Default.DensityMax, double pressure = Standard.Pressure, double densityStep = 0.02, double dryBulbTemperatureMin = Default.DryBulbTemperatureMin, double dryBulbTemperatureMax = Default.DryBulbTemperatureMax, double humidityRatioMin = Default.HumidityRatioMin, double humidityRatioMax = Default.HumidityRatioMax)
        {
            Dictionary<double, List<MollierPoint>> result = new Dictionary<double, List<MollierPoint>>();
            while (density_Min <= density_Max)
            {
                result[density_Min] = new List<MollierPoint>();
                double temperature_1 = DryBulbTemperature_ByDensityAndRelativeHumidity(density_Min, 0, pressure);

                double humidityRatio_1 = HumidityRatio(temperature_1, 0, pressure);

                MollierPoint mollierPoint_1 = new MollierPoint(temperature_1, humidityRatio_1, pressure);
                result[density_Min].Add(mollierPoint_1);

                double temperature_2 = DryBulbTemperature_ByDensityAndRelativeHumidity(density_Min, 100, pressure);
                double humidityRatio_2 = HumidityRatio(temperature_2, 100, pressure);
                MollierPoint mollierPoint_2 = new MollierPoint(temperature_2, humidityRatio_2, pressure);
                result[density_Min].Add(mollierPoint_2);
                double a = (temperature_1 - temperature_2) / (humidityRatio_1 - humidityRatio_2);
                density_Min += densityStep;
            }
            return result;

        }
    }
}
