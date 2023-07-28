using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Create
    { 
        public static List<ConstantTemperatureCurve> ConstantTemperatureCurves_DryBulbTemperature(this MollierRange mollierRange, double step, double pressure)
        {
            if (double.IsNaN(step) || double.IsNaN(pressure) || mollierRange == null || mollierRange.IsValid())
            {
                return null;
            }

            List<ConstantTemperatureCurve> result = new List<ConstantTemperatureCurve>();

            double dryBulbTemperature = mollierRange.DryBulbTemperature_Min;
            while (dryBulbTemperature <= mollierRange.DryBulbTemperature_Max)
            {
                ConstantTemperatureCurve constantTemperatureCurve = ConstantTemperatureCurve_DryBulbTemperature(mollierRange, dryBulbTemperature, pressure);
                if (constantTemperatureCurve != null)
                {
                    result.Add(constantTemperatureCurve);
                }

                dryBulbTemperature += step;
            }

            //ConstantTemperatureCurve constantTemperatureCurve_Liquid = ConstantTemperatureCurve_Liquid(dryBulbTemperatureRange.Min, pressure);
            //if (constantTemperatureCurve_Liquid != null)
            //{
            //    result.Add(constantTemperatureCurve_Liquid);
            //}

            //ConstantTemperatureCurve constantTemperatureCurve_Solid = ConstantTemperatureCurve_Solid(dryBulbTemperatureRange.Min, pressure);
            //if (constantTemperatureCurve_Solid != null)
            //{
            //    result.Add(constantTemperatureCurve_Solid);
            //}

            return result;
        }
    }
}
