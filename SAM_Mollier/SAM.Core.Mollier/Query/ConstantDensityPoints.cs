using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Query
    { 
        public static Dictionary<double, List<MollierPoint>> ConstantDensityPoints(double density_Min = Default.DensityMin, double density_Max = Default.DensityMax, double pressure = Standard.Pressure, double densityStep = 0.02, double dryBulbTemperature_Min = Default.DryBulbTemperatureMin, double dryBulbTemperature_Max = Default.DryBulbTemperatureMax, double humidityRatio_Min = Default.HumidityRatioMin, double humidityRatio_Max = Default.HumidityRatioMax)
        {
            Dictionary<double, List<MollierPoint>> result = new Dictionary<double, List<MollierPoint>>();
            while (density_Min <= density_Max)
            {
                result[density_Min] = new List<MollierPoint>();
                double temperature_1 = DryBulbTemperature_ByDensityAndRelativeHumidity(density_Min, 0, pressure);
                double humidityRatio_1 = HumidityRatio(temperature_1, 0, pressure);
                MollierPoint mollierPoint_1 = new MollierPoint(temperature_1, humidityRatio_1, pressure);

                double temperature_2 = DryBulbTemperature_ByDensityAndRelativeHumidity(density_Min, 100, pressure);
                double humidityRatio_2 = HumidityRatio(temperature_2, 100, pressure);
                MollierPoint mollierPoint_2 = new MollierPoint(temperature_2, humidityRatio_2, pressure);

                List<MollierPoint> points = ShortenLineByEndPoints(mollierPoint_1, mollierPoint_2, humidityRatio_Min, humidityRatio_Max, dryBulbTemperature_Min, dryBulbTemperature_Max);
                if(points != null && points.Count > 1)
                {
                    result[density_Min].Add(points[0]);
                    result[density_Min].Add(points[1]);
                }

                density_Min += densityStep;
            }
            return result;

        }
    }
}
