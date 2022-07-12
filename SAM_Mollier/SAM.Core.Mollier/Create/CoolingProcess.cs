namespace SAM.Core.Mollier
{
    public static partial class Create
    {
        public static CoolingProcess CoolingProcess(this MollierPoint start, double dryBulbTemperature)
        {
            if (start == null || double.IsNaN(dryBulbTemperature))
            {
                return null;
            }

            if (dryBulbTemperature > start.DryBulbTemperature)
            {
                return null;
            }

            double humidityRatio = Query.HumidityRatio(dryBulbTemperature, 100, start.Pressure);

            MollierPoint end = new MollierPoint(dryBulbTemperature, humidityRatio, start.Pressure);
            if (end == null)
            {
                return null;
            }

            return new CoolingProcess(start, end);
        }

        public static CoolingProcess CoolingProcess_ByTemperatureDifference(this MollierPoint start, double temperatureDifference)
        {
            if (start == null || double.IsNaN(temperatureDifference))
            {
                return null;
            }

            double dryBulbTemperature = start.DryBulbTemperature - temperatureDifference;
            double humidityRatio = Query.HumidityRatio(dryBulbTemperature, 100, start.Pressure);

            MollierPoint end = new MollierPoint(dryBulbTemperature, humidityRatio, start.Pressure);
            if (end == null)
            {
                return null;
            }

            return new CoolingProcess(start, end);
        }

        public static CoolingProcess CoolingProcess_ByEnthalpyDifference(this MollierPoint start, double enthalpyDifference)
        {
            if (start == null || double.IsNaN(enthalpyDifference))
            {
                return null;
            }

            double enthalpy = start.Enthalpy - enthalpyDifference;

            double dryBulbTemperature = Query.DryBulbTemperature_ByEnthalpy(enthalpy, 100, start.Pressure);
            double humidityRatio = Query.HumidityRatio(dryBulbTemperature, 100, start.Pressure);

            MollierPoint end = MollierPoint_ByEnthalpy(dryBulbTemperature, humidityRatio, start.Pressure);
            if (end == null)
            {
                return null;
            }

            return new CoolingProcess(start, end);
        }

        public static CoolingProcess CoolingProcess_ByMedium(this MollierPoint start, double flowTemperature, double returnTemperature, double efficiency)
        {
            if(start == null || double.IsNaN(efficiency) || double.IsNaN(flowTemperature) || double.IsNaN(returnTemperature))
            {
                return null;
            }

            double averageTemperature = (flowTemperature + returnTemperature) / 2;

            double temperatureDifference = start.DryBulbTemperature - averageTemperature;
            double humidityRatioDifference = start.HumidityRatio - Query.HumidityRatio(averageTemperature, 100, start.Pressure);

            MollierPoint end = new MollierPoint(start.DryBulbTemperature - (temperatureDifference * efficiency), start.HumidityRatio - (humidityRatioDifference * efficiency), start.Pressure);
            if(end == null)
            {
                return null;
            }

            return new CoolingProcess(start, end);
        }
    }
}
