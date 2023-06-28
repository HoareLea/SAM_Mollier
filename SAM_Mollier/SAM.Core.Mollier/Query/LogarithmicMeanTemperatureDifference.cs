namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static double LogarithmicMeanTemperatureDifference(this MollierPoint entering_1, MollierPoint leaving_1, MollierPoint entering_2, MollierPoint leaving_2)
        {
            if (entering_1 == null || leaving_1 == null || entering_2 == null || leaving_2 == null)
            {
                return double.NaN;
            }

            return Core.Query.LogarithmicMeanTemperatureDifference(
                entering_1.DryBulbTemperature,
                leaving_1.DryBulbTemperature,
                entering_2.DryBulbTemperature,
                leaving_2.DryBulbTemperature);

        }
    }
}
