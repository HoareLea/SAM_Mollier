using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Query
    { 
        public static Dictionary<double, List<MollierPoint>> SpecificVolumeLine(double specific_volume_Min = Default.SpecificVolumeMin, double specific_volume_Max = Default.SpecificVolumeMax, double pressure = Standard.Pressure, double specificVolumeStep = 0.05)
        {
            Dictionary<double, List<MollierPoint>> result = new Dictionary<double, List<MollierPoint>>();

            while (specific_volume_Min <= specific_volume_Max)
            {
                result[specific_volume_Min] = new List<MollierPoint>();

                MollierPoint mollierPoint_1 = Create.MollierPoint_ByRelativeHumidityAndSpecificVolume(0, specific_volume_Min, pressure);
                result[specific_volume_Min].Add(mollierPoint_1);
                MollierPoint mollierPoint_2 = Create.MollierPoint_ByRelativeHumidityAndSpecificVolume(100, specific_volume_Min, pressure);
                result[specific_volume_Min].Add(mollierPoint_2);

                specific_volume_Min += specificVolumeStep;
            }

            return result;

        }
    }
}
