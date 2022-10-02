namespace SAM.Analytical.Mollier
{
    public static partial class Query
    {
        public static double SummerSupplyFanTemperature(this AirHandlingUnitResult airHandlingUnitResult)
        {
            if(airHandlingUnitResult == null)
            {
                return double.NaN;
            }

            if(!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerSupplyFanTemperature, out double result))
            {
                return double.NaN;
            }

            return result;
        }
    }
}