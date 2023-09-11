
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

            return System.Math.Sqrt((System.Math.Pow(mollierPoint_1.DryBulbTemperature - mollierPoint_2.DryBulbTemperature, 2)) + System.Math.Pow(mollierPoint_1.HumidityRatio * 1000 - mollierPoint_2.HumidityRatio * 1000, 2));
        }
        public static double Distance(this IMollierPoint uIMollierPoint_1, IMollierPoint uIMollierPoint_2)
        {
            if (uIMollierPoint_1 == null || uIMollierPoint_2 == null)
            {
                return double.NaN;
            }

            MollierPoint mollierPoint_1 = uIMollierPoint_1 as dynamic;
            MollierPoint mollierPoint_2 = uIMollierPoint_2 as dynamic;

            return Distance(mollierPoint_1, mollierPoint_2);
        }

        //public static double Distance(this IMollierPoint mollierPoint_1, IMollierPoint mollierPoint_2)
        //{
        //    UIMollierPoint uIMollierPoint = null;

        //    MollierPoint mollierPoint = uIMollierPoint;

        //    if(mollierPoint_1 is UIMollierPoint && mollierPoint_2 is UIMollierPoint)
        //    {
        //        return Distance(mollierPoint_1, mollierPoint_2);
        //    }
        //    else if (mollierPoint_1 is MollierPoint && mollierPoint_2 is MollierPoint)
        //    {
        //        return Distance((MollierPoint)mollierPoint_1, (MollierPoint)mollierPoint_2);
        //    }
        //    return double.NaN;
        //}
    }
}
