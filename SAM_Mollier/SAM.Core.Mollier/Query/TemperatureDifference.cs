
namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static double TemperatureDifference(this MollierPoint start, MollierPoint end, MollierPoint mollierPoint)
        {
            if(start == null || end == null || mollierPoint == null)
            {
                return double.NaN;
            }

            MollierPoint mollierPoint_Project = start.Project(end, mollierPoint);
            if(mollierPoint_Project == null)
            {
                return double.NaN;
            }

            return mollierPoint.DryBulbTemperature - mollierPoint_Project.DryBulbTemperature;
        }
    }
}
