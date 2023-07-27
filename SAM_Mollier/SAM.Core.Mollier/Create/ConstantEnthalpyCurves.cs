using System.Collections.Generic;
using System.Linq;

namespace SAM.Core.Mollier
{
    public static partial class Create
    {
        public static List<ConstantEnthalpyCurve> ConstantEnthalpyCurves(double enthalpy, double pressure, Range<double> dryBulbTemperatureRange, Range<double> humidityRatioRange, params Phase[] phases)
        {
            if (double.IsNaN(enthalpy) || double.IsNaN(pressure) || dryBulbTemperatureRange == null || double.IsNaN(dryBulbTemperatureRange.Max) || double.IsNaN(dryBulbTemperatureRange.Min) || humidityRatioRange == null || double.IsNaN(humidityRatioRange.Max) || double.IsNaN(humidityRatioRange.Min))
            {
                return null;
            }


            Phase[] phases_Temp = phases == null || phases.Length == 0 ? new Phase[] { Phase.Gas, Phase.Liquid} : phases;

            List<ConstantEnthalpyCurve> result = new List<ConstantEnthalpyCurve>();

            double humidityRatio_1 = System.Math.Max(0, humidityRatioRange.Min);

            //Mollier Point 1
            double dryBulbTemperature_1 = Query.DryBulbTemperature(enthalpy, humidityRatio_1, pressure);
            if (double.IsNaN(dryBulbTemperature_1))
            {
                return null;
            }

            if (dryBulbTemperature_1 > dryBulbTemperatureRange.Max)
            {
                dryBulbTemperature_1 = dryBulbTemperatureRange.Max;
                humidityRatio_1 = Query.HumidityRatio_ByEnthalpy(dryBulbTemperature_1, enthalpy);
            }

            if (humidityRatio_1 < humidityRatioRange.Min)
            {
                humidityRatio_1 = humidityRatioRange.Min;
                dryBulbTemperature_1 = Query.DryBulbTemperature(enthalpy, humidityRatio_1, pressure);
            }

            MollierPoint mollierPoint_1 = new MollierPoint(dryBulbTemperature_1, humidityRatio_1, pressure);
            if (!mollierPoint_1.IsValid())
            {
                return null;
            }

            //Mollier Point 2
            double dryBulbTemperature_2 = Query.DryBulbTemperature_ByEnthalpy(enthalpy, 100, pressure);
            if (double.IsNaN(dryBulbTemperature_2))
            {
                return null;
            }

            double humidityRatio_2 = 100;
            if (dryBulbTemperature_2 > dryBulbTemperatureRange.Max)
            {
                dryBulbTemperature_2 = dryBulbTemperatureRange.Max;
                humidityRatio_2 = Query.HumidityRatio_ByEnthalpy(dryBulbTemperature_2, enthalpy);
            }
            else
            {
                humidityRatio_2 = Query.HumidityRatio(dryBulbTemperature_2, 100, pressure);
            }

            if (double.IsNaN(humidityRatio_2))
            {
                return null;
            }

            if (humidityRatio_2 > humidityRatioRange.Max)
            {
                humidityRatio_2 = humidityRatioRange.Max;
                dryBulbTemperature_2 = Query.DryBulbTemperature(enthalpy, humidityRatio_2, pressure);

            }

            if(dryBulbTemperature_2 < dryBulbTemperatureRange.Min)
            {
                dryBulbTemperature_2 = dryBulbTemperatureRange.Min;
                humidityRatio_2 = Query.HumidityRatio_ByEnthalpy(dryBulbTemperature_2, enthalpy);
            }

            MollierPoint mollierPoint_2 = new MollierPoint(dryBulbTemperature_2, humidityRatio_2, pressure);
            if (!mollierPoint_2.IsValid())
            {
                return null;
            }

            //Phase Gas
            if (phases_Temp.Contains(Phase.Gas))
            {
                result.Add(new ConstantEnthalpyCurve(Phase.Gas, enthalpy, mollierPoint_1, mollierPoint_2));
            }

            //Phase Liquid
            if (phases_Temp.Contains(Phase.Liquid))
            {                
                double diagramTemperature_1 = Query.DiagramTemperature(mollierPoint_1);
                double diagramTemperature_2 = Query.DiagramTemperature(mollierPoint_2);

                if (mollierPoint_2.HumidityRatio < humidityRatioRange.Min)
                {
                    mollierPoint_2 = MollierPoint_ByEnthalpy(enthalpy, humidityRatioRange.Min, pressure);
                }

                double dryBulbTemperature_3 = dryBulbTemperatureRange.Min;

                if (Query.Intersection(humidityRatio_1, diagramTemperature_1, humidityRatio_2, diagramTemperature_2, 0, dryBulbTemperature_3, 1, dryBulbTemperature_3, out double diagramTemperature, out double humidityRatio_3))
                {
                    if (humidityRatio_3 > humidityRatioRange.Max)
                    {
                        humidityRatio_3 = humidityRatioRange.Max;
                    }

                    dryBulbTemperature_3 = Query.DryBulbTemperature(enthalpy, humidityRatio_3, pressure);
                    if (dryBulbTemperature_3 < dryBulbTemperatureRange.Min)
                    {
                        dryBulbTemperature_3 = dryBulbTemperatureRange.Min;
                        humidityRatio_3 = Query.HumidityRatio_ByEnthalpy(dryBulbTemperature_3, enthalpy);
                    }

                    if(humidityRatio_3 > humidityRatioRange.Min)
                    {
                        MollierPoint mollierPoint_3 = MollierPoint_ByEnthalpy(enthalpy, humidityRatio_3, pressure);
                        if (!mollierPoint_3.IsValid())
                        {
                            return result;
                        }

                        result.Add(new ConstantEnthalpyCurve(Phase.Liquid, enthalpy, mollierPoint_2, mollierPoint_3));
                    }
                }


            }

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

            return ConstantEnthalpyCurves(dryBulbTemperatureRange, new Range<double>(enthalpy_Max, enthalpy_Min), humidityRatioRange, enthalpyStep, pressure);
        }

        public static List<ConstantEnthalpyCurve> ConstantEnthalpyCurves(Range<double> dryBulbTemperatureRange, Range<double> enthalpyRange, Range<double> humidityRatioRange, double step, double pressure)
        {
            if (enthalpyRange == null || double.IsNaN(enthalpyRange.Min) || double.IsNaN(enthalpyRange.Max) || double.IsNaN(step) || double.IsNaN(pressure))
            {
                return null;
            }

            List<ConstantEnthalpyCurve> result = new List<ConstantEnthalpyCurve>();

            double enthalpy = enthalpyRange.Min;
            while (enthalpy <= enthalpyRange.Max)
            {
                List<ConstantEnthalpyCurve> constantEnthalpyCurves = ConstantEnthalpyCurves(enthalpy, pressure, dryBulbTemperatureRange, humidityRatioRange);
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
