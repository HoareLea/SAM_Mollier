namespace SAM.Analytical.Mollier
{
    public static partial class Query
    {
        public static double SummerSupplyFanRelativeHumidity(this AirHandlingUnitResult airHandlingUnitResult)
        {
            if(airHandlingUnitResult == null)
            {
                return double.NaN;
            }

            if(!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerSupplyFanRelativeHumidty, out double result))
            {
                return double.NaN;
            }

            return result;
        }
    }
}