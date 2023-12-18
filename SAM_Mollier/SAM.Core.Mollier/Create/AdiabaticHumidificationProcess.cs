namespace SAM.Core.Mollier
{
    public static partial class Create
    {
        public static AdiabaticHumidificationProcess AdiabaticHumidificationProcess_ByHumidityRatioDifference(this MollierPoint start, double humidityRatioDifference, double efficiency = 1)
        {
            if (start == null || double.IsNaN(humidityRatioDifference))
            {
                return null;
            }

            MollierPoint end = MollierPoint_ByEnthalpy(start.Enthalpy, start.HumidityRatio + humidityRatioDifference, start.Pressure);
            if (end == null)
            {
                return null;
            }

            return new AdiabaticHumidificationProcess(start, end, efficiency);
        }

        public static AdiabaticHumidificationProcess AdiabaticHumidificationProcess_ByRelativeHumidity(this MollierPoint start, double relativeHumidity, double efficiency = 1)
        {
            if (start == null || double.IsNaN(relativeHumidity))
            {
                return null;
            }

            if(start.RelativeHumidity > relativeHumidity)
            {
                return null;
            }

            double dryBulbTemperature = Query.DryBulbTemperature_ByEnthalpy(start.Enthalpy, relativeHumidity, start.Pressure);
            double humidityRatio = Query.HumidityRatio(dryBulbTemperature, relativeHumidity, start.Pressure);

            MollierPoint end = new MollierPoint(dryBulbTemperature, humidityRatio, start.Pressure);
            if (end == null)
            {
                return null;
            }

            return new AdiabaticHumidificationProcess(start, end, efficiency);
        }
    }
}
