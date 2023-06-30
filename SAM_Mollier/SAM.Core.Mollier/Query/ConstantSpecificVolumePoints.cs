using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Query
    { 
        public static Dictionary<double, List<MollierPoint>> ConstantSpecificVolumePoints(double specific_volume_Min = Default.SpecificVolumeMin, double specific_volume_Max = Default.SpecificVolumeMax, double pressure = Standard.Pressure, double specificVolumeStep = 0.05, double dryBulbTemperature_Min = Default.DryBulbTemperatureMin, double dryBulbTemperature_Max = Default.DryBulbTemperatureMax, double humidityRatio_Min = Default.HumidityRatioMin, double humidityRatio_Max = Default.HumidityRatioMax)
        {
            Dictionary<double, List<MollierPoint>> result = new Dictionary<double, List<MollierPoint>>();

            while (specific_volume_Min <= specific_volume_Max)
            {
                result[specific_volume_Min] = new List<MollierPoint>();

                MollierPoint mollierPoint_1 = Create.MollierPoint_ByRelativeHumidityAndSpecificVolume(0, specific_volume_Min, pressure);
                if(mollierPoint_1 == null || !mollierPoint_1.IsValid())
                {
                    specific_volume_Min += specificVolumeStep;
                    continue;
                }

                MollierPoint mollierPoint_2 = Create.MollierPoint_ByRelativeHumidityAndSpecificVolume(100, specific_volume_Min, pressure);
                if (mollierPoint_2 == null || !mollierPoint_2.IsValid())
                {
                    specific_volume_Min += specificVolumeStep;
                    continue;
                }

                

                List<MollierPoint> points = ShortenLineByEndPoints(mollierPoint_1, mollierPoint_2, humidityRatio_Min, humidityRatio_Max, dryBulbTemperature_Min, dryBulbTemperature_Max);
                if(points == null || points.Count < 2)
                {
                    specific_volume_Min += specificVolumeStep;
                    continue;
                }
                
                result[specific_volume_Min].Add(points[0]);
                result[specific_volume_Min].Add(points[1]);

                specific_volume_Min += specificVolumeStep;
            }

            return result;

        }
    }
}
