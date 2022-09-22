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
            double dryBulbTemperature_Start_Saturation = Query.DryBulbTemperature_ByHumidityRatio(start.HumidityRatio, 100, start.Pressure);

            if (dryBulbTemperature_Start_Saturation < dryBulbTemperature)
            {
                end = new MollierPoint(dryBulbTemperature, start.HumidityRatio, start.Pressure);
            }
            else
            {
                double humidityRatio_Saturation = Query.HumidityRatio(dryBulbTemperature, 100, start.Pressure);
                end = new MollierPoint(dryBulbTemperature, humidityRatio_Saturation, start.Pressure);
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

            return CoolingProcess(start, start.DryBulbTemperature - temperatureDifference);
        }

        public static CoolingProcess CoolingProcess_ByEnthalpyDifference(this MollierPoint start, double enthalpyDifference)
        {
            if (start == null || double.IsNaN(enthalpyDifference))
            {
                return null;
            }

            MollierPoint end = MollierPoint_ByEnthalpy(start.Enthalpy - enthalpyDifference, start.HumidityRatio, start.Pressure);

            return CoolingProcess(start, end.DryBulbTemperature);
        }

        public static CoolingProcess CoolingProcess_ByMedium(this MollierPoint start, double flowTemperature, double returnTemperature, double efficiency)
        {
            if (start == null || double.IsNaN(efficiency) || double.IsNaN(flowTemperature) || double.IsNaN(returnTemperature))
            {
                return null;
            }
            //check it - find better equation describing average coil temperature
            double averageTemperature = (flowTemperature + returnTemperature) / 2;

            double temperatureDifference = start.DryBulbTemperature - averageTemperature;
            double humidityRatioDifference = start.HumidityRatio - Query.HumidityRatio(averageTemperature, 100, start.Pressure);

            MollierPoint end = new MollierPoint(start.DryBulbTemperature - (temperatureDifference * efficiency), start.HumidityRatio - (humidityRatioDifference * efficiency), start.Pressure);

            if (double.IsNaN(end.RelativeHumidity))
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

        public static CoolingProcess CoolingProcess_ByMediumAndDryBulbTemperature(this MollierPoint start, double flowTemperature, double returnTemperature, double dryBulbTemperature)
        {
            if(start == null)
            {
                return null;
            }

            double dewPointTemperature = start.DewPointTemperature();

            double dryBulbTemperature_ADP = (returnTemperature + flowTemperature) / 2;
            double humidityRatio_ADP = Query.HumidityRatio(dryBulbTemperature_ADP, 100, start.Pressure);

            MollierPoint mollierPoint_ADP = new MollierPoint(dryBulbTemperature_ADP, humidityRatio_ADP, start.Pressure);

            if (dewPointTemperature < dryBulbTemperature)
            {
                return CoolingProcess(start, dryBulbTemperature);
            }

            double humidityRatio = (humidityRatio_ADP * (dryBulbTemperature - start.DryBulbTemperature) - start.HumidityRatio * (dryBulbTemperature - dryBulbTemperature_ADP)) / (dryBulbTemperature_ADP - start.DryBulbTemperature);

            MollierPoint mollierPoint_End = new MollierPoint(dryBulbTemperature, humidityRatio, start.Pressure);
            double efficiency = (start.Enthalpy - mollierPoint_End.Enthalpy) / (start.Enthalpy - mollierPoint_ADP.Enthalpy);
            return CoolingProcess_ByMedium(start, flowTemperature, returnTemperature, efficiency);
        }

        public static double CountDistance(MollierPoint start, MollierPoint end)
        {
            return System.Math.Sqrt((System.Math.Pow(start.DryBulbTemperature - end.DryBulbTemperature, 2)) + System.Math.Pow(start.HumidityRatio - end.HumidityRatio, 2)); 
        }
    }
}
