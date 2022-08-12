
namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static double Distance(this MollierPoint mollierPoint_1, MollierPoint mollierPoint_2)
        {
            if (mollierPoint_1 == null || mollierPoint_2 == null)
            {
                return double.NaN;
            }

            return System.Math.Sqrt((System.Math.Pow(mollierPoint_1.DryBulbTemperature - mollierPoint_2.DryBulbTemperature, 2)) + System.Math.Pow(mollierPoint_1.HumidityRatio - mollierPoint_2.HumidityRatio, 2));
        }
    }
}
