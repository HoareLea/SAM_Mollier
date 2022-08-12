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
            if (dryBulbTemperature_Saturation < dryBulbTemperature)
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
            if (start == null || double.IsNaN(efficiency) || double.IsNaN(flowTemperature) || double.IsNaN(returnTemperature))
            {
                return null;
            }
            //check it
            double averageTemperature = (flowTemperature + returnTemperature) / 2;

            double temperatureDifference = start.DryBulbTemperature - averageTemperature;
            double humidityRatioDifference = start.HumidityRatio - Query.HumidityRatio(averageTemperature, 100, start.Pressure);

            MollierPoint end = new MollierPoint(start.DryBulbTemperature - (temperatureDifference * efficiency), start.HumidityRatio - (humidityRatioDifference * efficiency), start.Pressure);

            if ((end.RelativeHumidity).ToString() == (double.NaN).ToString())
            {
                double temperature_Temp = averageTemperature;
                double humidityRatio_Temp = Query.HumidityRatio(averageTemperature, 100, start.Pressure);
                end = new MollierPoint(temperature_Temp, humidityRatio_Temp, start.Pressure);
                MollierPoint ADP = new MollierPoint(temperature_Temp, humidityRatio_Temp, start.Pressure);

                while (CountDistance(start, end) - (CountDistance(start, end) + CountDistance(end, ADP)) * efficiency > 0.01)
                {
                    humidityRatio_Temp += 0.00001;
                    temperature_Temp = Query.DryBulbTemperature_ByHumidityRatio(humidityRatio_Temp, 100, start.Pressure);
                    end = new MollierPoint(temperature_Temp, humidityRatio_Temp, start.Pressure);
                }
            }

            if (end == null)
            {
                return null;
            }

            return new CoolingProcess(start, end, efficiency);
        }
        public static double CountDistance(MollierPoint start, MollierPoint end)
        {
            return System.Math.Sqrt((System.Math.Pow(start.DryBulbTemperature - end.DryBulbTemperature, 2)) + System.Math.Pow(start.HumidityRatio - end.HumidityRatio, 2)); 
        }

    }
}
