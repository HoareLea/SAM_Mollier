using System;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static MollierPoint Mid(this MollierPoint mollierPoint_1, MollierPoint mollierPoint_2)
        {
            if (mollierPoint_1 == null || mollierPoint_2 == null || !mollierPoint_1.IsValid() || !mollierPoint_2.IsValid())
            {
                return null;
            }

            return new MollierPoint((mollierPoint_1.DryBulbTemperature + mollierPoint_2.DryBulbTemperature) / 2, (mollierPoint_1.HumidityRatio + mollierPoint_2.HumidityRatio) / 2, mollierPoint_1.Pressure);
        }
    }
}
