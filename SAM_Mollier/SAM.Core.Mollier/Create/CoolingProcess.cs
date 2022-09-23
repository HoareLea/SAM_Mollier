using System;

namespace SAM.Core.Mollier
{
    public static partial class Create
    {
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
            if(end == null)
            {
                return null;
            }

            if (double.IsNaN(end.RelativeHumidity))
            {
                double temperature_Temp = averageTemperature;
                double humidityRatio_Temp = Query.HumidityRatio(averageTemperature, 100, start.Pressure);
                end = new MollierPoint(temperature_Temp, humidityRatio_Temp, start.Pressure);
                MollierPoint ADP = new MollierPoint(temperature_Temp, humidityRatio_Temp, start.Pressure);

                Func<MollierPoint, MollierPoint, double> calculateDistance = new Func<MollierPoint, MollierPoint, double>((MollierPoint start_Temp, MollierPoint end_Temp) => 
                { 
                    return Math.Sqrt((Math.Pow(start.DryBulbTemperature - end.DryBulbTemperature, 2)) + Math.Pow(start.HumidityRatio - end.HumidityRatio, 2));
                });

                while (calculateDistance.Invoke(start, end) - (calculateDistance.Invoke(start, end) + calculateDistance.Invoke(end, ADP)) * efficiency > 0.01)
                {
                    humidityRatio_Temp += 0.00001;
                    temperature_Temp = Query.DryBulbTemperature_ByHumidityRatio(humidityRatio_Temp, 100, start.Pressure);
                    end = new MollierPoint(temperature_Temp, humidityRatio_Temp, start.Pressure);
                }
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

        public static CoolingProcess CoolingProcess(this MollierPoint start, double dryBulbTemperature, double efficiency = 1)
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
            else if(efficiency == 1)
            {
                double humidityRatio_Saturation = Query.HumidityRatio(dryBulbTemperature, 100, start.Pressure);
                end = new MollierPoint(dryBulbTemperature, humidityRatio_Saturation, start.Pressure);
            }
            else
            {
                Func<double, double> func = new Func<double, double>((double dryBulbTemperature_Temp) =>
                {
                    double humidityRatio_ADP_Temp = Query.HumidityRatio(dryBulbTemperature_Temp, 100, start.Pressure);
                    if (double.IsNaN(humidityRatio_ADP_Temp))
                    {
                        return double.NaN;
                    }


                    MollierPoint molierPoint_ADP_Temp = new MollierPoint(dryBulbTemperature_Temp, humidityRatio_ADP_Temp, start.Pressure);
                    if (molierPoint_ADP_Temp == null)
                    {
                        return double.NaN;
                    }

                    MollierPoint mollierPoint_Temp = MollierPoint_ByFactor(start, molierPoint_ADP_Temp, efficiency);
                    if (mollierPoint_Temp == null)
                    {
                        return double.NaN;
                    }

                    return mollierPoint_Temp.DryBulbTemperature;
                });

                double dryBulbTemperature_ADP = Core.Query.Calculate(func, dryBulbTemperature, -20, start.DryBulbTemperature);
                if (double.IsNaN(dryBulbTemperature_ADP))
                {
                    return null;
                }

                double humidityRatio_ADP = Query.HumidityRatio(dryBulbTemperature_ADP, 100, start.Pressure);
                MollierPoint mollierPoint_ADP = new MollierPoint(dryBulbTemperature_ADP, humidityRatio_ADP, start.Pressure);

                end = MollierPoint_ByFactor(start, mollierPoint_ADP, efficiency);
            }

            return new CoolingProcess(start, end, efficiency);
        }
    }
}
