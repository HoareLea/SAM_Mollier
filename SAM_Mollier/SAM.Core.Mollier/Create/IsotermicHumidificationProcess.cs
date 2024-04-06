namespace SAM.Core.Mollier
{
    public static partial class Create
    {
        public static IsothermalHumidificationProcess IsothermalHumidificationProcess_ByHumidityRatioDifference(this MollierPoint start, double humidityRatioDifference)
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

            return new IsothermalHumidificationProcess(start, end);
        }

        public static IsothermalHumidificationProcess IsothermalHumidificationProcess_ByRelativeHumidity(this MollierPoint start, double relativeHumidity)
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

            return new IsothermalHumidificationProcess(start, end);
        }
    }
}
