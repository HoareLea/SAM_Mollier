using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static Dictionary<double, List<MollierPoint>> EnthalpyLine(ChartType chartType = ChartType.Mollier, double enthalpy_Min = Default.EnthalpyMin, double enthalpy_Max = Default.EnthalpyMax, double pressure = Standard.Pressure, double enthalpyStep = 1)
        {
            Dictionary<double, List<MollierPoint>> result = new Dictionary<double, List<MollierPoint>>();

            while (enthalpy_Min <= enthalpy_Max)
            {
                result[enthalpy_Min] = new List<MollierPoint>();
                double humidityRatio_Min = HumidityRatio_ByEnthalpy(-20, enthalpy_Min * 1000);

                double humidityRatio_1 = HumidityRatio_ByEnthalpy(100, enthalpy_Min * 1000);
                double temperature_1 = DryBulbTemperature(enthalpy_Min * 1000, humidityRatio_1);
                double temperature_2 = DryBulbTemperature_ByEnthalpy(enthalpy_Min * 1000, 100, pressure);
                double humidityRatio_2 = HumidityRatio(temperature_2, 100, pressure);

                MollierPoint mollierPoint_1 = new MollierPoint(temperature_1, humidityRatio_1, pressure);
                result[enthalpy_Min].Add(mollierPoint_1);

                if (enthalpy_Min % 10 == 0 && chartType == ChartType.Psychrometric)
                {
                    MollierPoint MollierPoint_2 = new MollierPoint(temperature_2, humidityRatio_2, pressure);

                    double v = 0.0006;
                    double humidityRatio = MollierPoint_2.HumidityRatio + v;
                    double a = (humidityRatio_1 - humidityRatio_2)/(temperature_1 - temperature_2);
                    double b = humidityRatio_2 - a * temperature_2;
                    double temperature = (humidityRatio - b) / a;

                    MollierPoint mollierPoint = new MollierPoint(temperature, humidityRatio, pressure);
                    result[enthalpy_Min].Add(mollierPoint);
                }
                else
                {
                    MollierPoint mollierPoint_2 = new MollierPoint(temperature_2, humidityRatio_2, pressure);
                    result[enthalpy_Min].Add(mollierPoint_2);
                }

                enthalpy_Min += enthalpyStep;
            }
            return result;

        }
    }
}
