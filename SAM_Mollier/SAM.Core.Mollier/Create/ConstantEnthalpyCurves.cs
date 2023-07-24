using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Create
    {
        public static List<ConstantEnthalpyCurve> ConstantEnthalpyCurves(double enthalpy, double pressure, Range<double> dryBulbTemperatureRange)
        {
            if (double.IsNaN(enthalpy) || double.IsNaN(pressure) || dryBulbTemperatureRange == null || double.IsNaN(dryBulbTemperatureRange.Max) || double.IsNaN(dryBulbTemperatureRange.Min))
            {
                return null;
            }

            //Phase Gas

            //Relative Humidity = 0 or RelativeHumidity for max dry bulb temperature
            double dryBulbTemperature_1 = Query.DryBulbTemperature(enthalpy, 0, pressure);
            if (double.IsNaN(dryBulbTemperature_1))
            {
                return null;
            }

            double humidityRatio_1 = 0;
            if(dryBulbTemperature_1 > dryBulbTemperatureRange.Max)
            {
                dryBulbTemperature_1 = dryBulbTemperatureRange.Max;
                humidityRatio_1 = Query.HumidityRatio_ByEnthalpy(dryBulbTemperature_1, enthalpy);
            }

            MollierPoint mollierPoint_1 = new MollierPoint(dryBulbTemperature_1, humidityRatio_1, pressure);
            if (!mollierPoint_1.IsValid())
            {
                return null;
            }

            //Relative Humidity = 100
            double dryBulbTemperature_2 = Query.DryBulbTemperature_ByEnthalpy(enthalpy, 100, pressure);
            if (double.IsNaN(dryBulbTemperature_2))
            {
                return null;
            }

            double humidityRatio_2 = Query.HumidityRatio(dryBulbTemperature_2, 100, pressure);
            if (double.IsNaN(humidityRatio_2))
            {
                return null;
            }

            MollierPoint mollierPoint_2 = new MollierPoint(dryBulbTemperature_2, humidityRatio_2, pressure);
            if (!mollierPoint_2.IsValid())
            {
                return null;
            }

            List<ConstantEnthalpyCurve> result = new List<ConstantEnthalpyCurve>();
            result.Add(new ConstantEnthalpyCurve(Phase.Gas, enthalpy, mollierPoint_1, mollierPoint_2));

            //Phase Liquid
            double dryBulbTemperature_3 = dryBulbTemperatureRange.Min;
            if (double.IsNaN(dryBulbTemperature_3))
            {
                return null;
            }

            if(!Query.Intersection(humidityRatio_1, dryBulbTemperature_1, humidityRatio_2, dryBulbTemperature_2, 0, dryBulbTemperature_3, 1, dryBulbTemperature_3, out double diagramTemperature, out double humidityRatio_3))
            {
                return result;
            }

            dryBulbTemperature_3 = Query.DryBulbTemperature(enthalpy, humidityRatio_3, pressure);

            MollierPoint mollierPoint_3 = new MollierPoint(dryBulbTemperature_3, humidityRatio_3, pressure);
            if (!mollierPoint_3.IsValid())
            {
                return result;
            }
            result.Add(new ConstantEnthalpyCurve(Phase.Liquid, enthalpy, mollierPoint_2, mollierPoint_3));

            return result;
        }

        public static List<ConstantEnthalpyCurve> ConstantEnthalpyCurves_ByHumidityRatioRange(Range<double> dryBulbTemperatureRange, Range<double> humidityRatioRange, double enthalpyStep, double pressure)
        {
            if (dryBulbTemperatureRange == null || double.IsNaN(dryBulbTemperatureRange.Min) || double.IsNaN(dryBulbTemperatureRange.Max) || humidityRatioRange == null || double.IsNaN(humidityRatioRange.Min) || double.IsNaN(humidityRatioRange.Max) || double.IsNaN(enthalpyStep) || double.IsNaN(pressure))
            {
                return null;
            }

            double enthalpy_Max = Query.Enthalpy(dryBulbTemperatureRange.Min, humidityRatioRange.Min, pressure);
            if (double.IsNaN(enthalpy_Max))
            {
                return null;
            }

            double enthalpy_Min = Query.Enthalpy(dryBulbTemperatureRange.Max, humidityRatioRange.Max, pressure);
            if (double.IsNaN(enthalpy_Max))
            {
                return null;
            }

            enthalpy_Min = enthalpy_Min - (enthalpy_Min % enthalpyStep) - enthalpyStep;
            enthalpy_Max = enthalpy_Max - (enthalpy_Max % enthalpyStep) + enthalpyStep;

            return ConstantEnthalpyCurves(dryBulbTemperatureRange, new Range<double>(enthalpy_Max, enthalpy_Min), enthalpyStep, pressure);
        }

        public static List<ConstantEnthalpyCurve> ConstantEnthalpyCurves(Range<double> dryBulbTemperatureRange, Range<double> enthalpyRange, double step, double pressure)
        {
            if (enthalpyRange == null || double.IsNaN(enthalpyRange.Min) || double.IsNaN(enthalpyRange.Max) || double.IsNaN(step) || double.IsNaN(pressure))
            {
                return null;
            }

            List<ConstantEnthalpyCurve> result = new List<ConstantEnthalpyCurve>();

            double enthalpy = enthalpyRange.Min;
            while (enthalpy <= enthalpyRange.Max)
            {
                List<ConstantEnthalpyCurve> constantEnthalpyCurves = ConstantEnthalpyCurves(enthalpy, pressure, dryBulbTemperatureRange);
                if (constantEnthalpyCurves != null && constantEnthalpyCurves.Count > 0)
                {
                    result.AddRange(constantEnthalpyCurves);
                }

                enthalpy += step;
            }

            return result;
        }

    }
}
