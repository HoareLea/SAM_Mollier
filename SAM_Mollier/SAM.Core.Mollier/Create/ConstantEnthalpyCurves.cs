
using System.Collections.Generic;
using System.Linq;

namespace SAM.Core.Mollier
{
    public static partial class Create
    {
        public static List<ConstantEnthalpyCurve> ConstantEnthalpyCurves(this MollierRange mollierRange, double enthalpy, double pressure, params Phase[] phases)
        {
            if (double.IsNaN(enthalpy) || double.IsNaN(pressure) || mollierRange == null || !mollierRange.IsValid())
            {
                return null;
            }

            Phase[] phases_Temp = phases == null || phases.Length == 0 ? new Phase[] { Phase.Gas, Phase.Liquid} : phases;

            List<ConstantEnthalpyCurve> result = new List<ConstantEnthalpyCurve>();

            double humidityRatio_1 = System.Math.Max(0, mollierRange.HumidityRatio_Min);

            //Mollier Point 1
            double dryBulbTemperature_1 = Query.DryBulbTemperature(enthalpy, humidityRatio_1, pressure);
            if (double.IsNaN(dryBulbTemperature_1))
            {
                return null;
            }

            if (dryBulbTemperature_1 > mollierRange.DryBulbTemperature_Max)
            {
                dryBulbTemperature_1 = mollierRange.DryBulbTemperature_Max;
                humidityRatio_1 = Query.HumidityRatio_ByEnthalpy(dryBulbTemperature_1, enthalpy);
            }

            if (humidityRatio_1 < mollierRange.HumidityRatio_Min)
            {
                humidityRatio_1 = mollierRange.HumidityRatio_Min;
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
            if (dryBulbTemperature_2 > mollierRange.DryBulbTemperature_Max)
            {
                dryBulbTemperature_2 = mollierRange.DryBulbTemperature_Max;
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

            if (humidityRatio_2 > mollierRange.HumidityRatio_Max)
            {
                humidityRatio_2 = mollierRange.HumidityRatio_Max;
                dryBulbTemperature_2 = Query.DryBulbTemperature(enthalpy, humidityRatio_2, pressure);

            }

            if(dryBulbTemperature_2 < mollierRange.DryBulbTemperature_Min)
            {
                dryBulbTemperature_2 = mollierRange.DryBulbTemperature_Min;
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
            if (phases_Temp.Contains(Phase.Liquid) && mollierPoint_2.HumidityRatio < mollierRange.HumidityRatio_Max)
            {       
                double diagramTemperature_1 = Query.DiagramTemperature(mollierPoint_1);
                double diagramTemperature_2 = Query.DiagramTemperature(mollierPoint_2);

                if (mollierPoint_2.HumidityRatio < mollierRange.HumidityRatio_Min)
                {
                    mollierPoint_2 = MollierPoint_ByEnthalpy(enthalpy, mollierRange.HumidityRatio_Min, pressure);
                }

                double dryBulbTemperature_3 = mollierRange.DryBulbTemperature_Min;

                if (Query.Intersection(humidityRatio_1, diagramTemperature_1, humidityRatio_2, diagramTemperature_2, 0, dryBulbTemperature_3, 1, dryBulbTemperature_3, out double diagramTemperature, out double humidityRatio_3))
                {
                    if (humidityRatio_3 > mollierRange.HumidityRatio_Max)
                    {
                        humidityRatio_3 = mollierRange.HumidityRatio_Max;
                    }

                    dryBulbTemperature_3 = Query.DryBulbTemperature(enthalpy, humidityRatio_3, pressure);
                    if (dryBulbTemperature_3 < mollierRange.DryBulbTemperature_Min)
                    {
                        dryBulbTemperature_3 = mollierRange.DryBulbTemperature_Min;
                        humidityRatio_3 = Query.HumidityRatio_ByEnthalpy(dryBulbTemperature_3, enthalpy);
                    }

                    if(humidityRatio_3 > mollierRange.HumidityRatio_Min)
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

        public static List<ConstantEnthalpyCurve> ConstantEnthalpyCurves(this MollierRange mollierRange, Range<double> enthalpyRange, double pressure, double enthalpyStep, IEnumerable<Phase> phases, VisibilitySettings visibilitySettings = null, string templateName = null)
        {
            if(enthalpyRange == null)
            {
                return null;
            }
            
            List<ConstantEnthalpyCurve> result = new List<ConstantEnthalpyCurve>();

            double enthalpy = enthalpyRange.Min;
            while (enthalpy <= enthalpyRange.Max)
            {
                bool visible = true;
                if (visibilitySettings != null && templateName != null)
                {
                    visible = visibilitySettings.GetVisible(templateName, ChartDataType.Enthalpy, enthalpy);
                }

                if(visible)
                {
                    List<ConstantEnthalpyCurve> constantValueCurves = ConstantEnthalpyCurves(mollierRange, enthalpy, pressure, phases?.ToArray());
                    if (constantValueCurves != null)
                    {
                        result.AddRange(constantValueCurves);
                    }
                }

                enthalpy += enthalpyStep;
            }

            return result;
        }

        public static List<ConstantEnthalpyCurve> ConstantEnthalpyCurves_ByHumidityRatioRange(this MollierRange mollierRange, double enthalpyStep, double pressure, params Phase[] phases)
        {
            if (double.IsNaN(enthalpyStep) || double.IsNaN(pressure) || mollierRange == null || !mollierRange.IsValid())
            {
                return null;
            }

            double enthalpy_Max = Query.Enthalpy(mollierRange.DryBulbTemperature_Min, mollierRange.HumidityRatio_Min, pressure);
            if (double.IsNaN(enthalpy_Max))
            {
                return null;
            }

            double enthalpy_Min = Query.Enthalpy(mollierRange.DryBulbTemperature_Max, mollierRange.HumidityRatio_Max, pressure);
            if (double.IsNaN(enthalpy_Max))
            {
                return null;
            }

            enthalpy_Min = enthalpy_Min - (enthalpy_Min % enthalpyStep) - enthalpyStep;
            enthalpy_Max = enthalpy_Max - (enthalpy_Max % enthalpyStep) + enthalpyStep;

            return ConstantEnthalpyCurves(mollierRange, new Range<double>(enthalpy_Max, enthalpy_Min), pressure, enthalpyStep, phases);
        }

    }
}
