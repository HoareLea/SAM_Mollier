namespace SAM.Core.Mollier
{
    public static partial class Create
    {
        public static IsotermicHumidificationProcess IsotermicHumidificationProcess_ByHumidityRatioDifference(this MollierPoint start, double humidityRatioDifference, double efficiency = 1)
        {
            if (start == null || double.IsNaN(humidityRatioDifference))
            {
                return null;
            }

            MollierPoint end = new MollierPoint(start.DryBulbTemperature, start.HumidityRatio + humidityRatioDifference, start.Pressure);
            if (end == null)
            {
                return null;
            }

            return new IsotermicHumidificationProcess(start, end, efficiency);
        }

        public static IsotermicHumidificationProcess IsotermicHumidificationProcess_ByRelativeHumidity(this MollierPoint start, double relativeHumidity, double efficiency = 1)
        {
            if (start == null || double.IsNaN(relativeHumidity))
            {
                return null;
            }

            if(start.RelativeHumidity > relativeHumidity)
            {
                return null;
            }

            double humidityRatio = Query.HumidityRatio(start.DryBulbTemperature, relativeHumidity, start.Pressure);

            MollierPoint end = new MollierPoint(start.DryBulbTemperature, humidityRatio, start.Pressure);
            if (end == null)
            {
                return null;
            }

            return new IsotermicHumidificationProcess(start, end, efficiency);
        }
    }
}
