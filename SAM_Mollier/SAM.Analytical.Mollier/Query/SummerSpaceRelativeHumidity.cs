namespace SAM.Analytical.Mollier
{
    public static partial class Query
    {
        public static double SummerSpaceRelativeHumidity(this AirHandlingUnitResult airHandlingUnitResult)
        {
            if(airHandlingUnitResult == null)
            {
                return double.NaN;
            }

            if(!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerSpaceRelativeHumidty, out double result))
            {
                return double.NaN;
            }

            return result;
        }
    }
}