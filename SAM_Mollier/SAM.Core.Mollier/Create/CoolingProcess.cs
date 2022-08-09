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

            MollierPoint end = null;

            double dryBulbTemperature_Saturation = Query.DryBulbTemperature_ByHumidityRatio(start.HumidityRatio, 100, start.Pressure);
            if(dryBulbTemperature_Saturation < dryBulbTemperature)
            {
                end = new MollierPoint(dryBulbTemperature, start.HumidityRatio, start.Pressure);
            }
            else
            {
                double humidityRatio = Query.HumidityRatio(dryBulbTemperature, 100, start.Pressure);

                end = new MollierPoint(dryBulbTemperature, humidityRatio, start.Pressure);
            }

            if (end == null)
            {
                return null;
            }

            return new CoolingProcess(start, end, 1);
        }

        public static CoolingProcess CoolingProcess_ByTemperatureDifference(this MollierPoint start, double temperatureDifference)
        {
            if (start == null || double.IsNaN(temperatureDifference))
            {
                return null;
            }

            return CoolingProcess_ByTemperatureDifference(start, start.DryBulbTemperature - temperatureDifference);
        }

        public static CoolingProcess CoolingProcess_ByEnthalpyDifference(this MollierPoint start, double enthalpyDifference)
        {
            if (start == null || double.IsNaN(enthalpyDifference))
            {
                return null;
            }

            double enthalpy = start.Enthalpy - enthalpyDifference;

            double dryBulbTemperature = Query.DryBulbTemperature_ByEnthalpy(start.Enthalpy - enthalpyDifference, start.RelativeHumidity, start.Pressure);

            double dryBulbTemperature_Saturation = Query.DryBulbTemperature_ByEnthalpy(enthalpy, 100, start.Pressure);
            double humidityRatio_Saturation = Query.HumidityRatio(dryBulbTemperature_Saturation, 100, start.Pressure);

            MollierPoint end = null;
            if (dryBulbTemperature_Saturation < dryBulbTemperature)
            {
                end = new MollierPoint(dryBulbTemperature, start.HumidityRatio, start.Pressure);
            }
            else
            {
                end = MollierPoint_ByEnthalpy(dryBulbTemperature_Saturation, humidityRatio_Saturation, start.Pressure);
            }

            if (end == null)
            {
                return null;
            }

            return new CoolingProcess(start, end, 1);
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

            return new CoolingProcess(start, end, efficiency);
        }
    }
}
