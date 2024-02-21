using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Create
    { 
        public static List<ConstantTemperatureCurve> ConstantTemperatureCurves_DryBulbTemperature(this MollierRange mollierRange, double step, double pressure, VisibilitySettings visibilitySettings = null, string templateName = null)
        {
            if (double.IsNaN(step) || double.IsNaN(pressure) || mollierRange == null || !mollierRange.IsValid())
            {
                return null;
            }

            List<ConstantTemperatureCurve> result = new List<ConstantTemperatureCurve>();

            double dryBulbTemperature = mollierRange.DryBulbTemperature_Min;
            while (dryBulbTemperature <= mollierRange.DryBulbTemperature_Max)
            {
                bool visible = true;
                if(visibilitySettings != null && templateName != null)
                {
                    visible = visibilitySettings.GetVisible(templateName, ChartDataType.DryBulbTemperature, dryBulbTemperature);
                }

                if(visible)
                {
                    ConstantTemperatureCurve constantTemperatureCurve = ConstantTemperatureCurve_DryBulbTemperature(mollierRange, dryBulbTemperature, pressure);
                    if (constantTemperatureCurve != null)
                    {
                        result.Add(constantTemperatureCurve);
                    }
                }
                
                dryBulbTemperature += step;
            }

            return result;
        }
    }
}
