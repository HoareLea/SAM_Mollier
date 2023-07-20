using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Create
    { 
        public static List<ConstantTemperatureCurve> ConstantTemperatureCurves_DiagramTemperature(Range<double> dryBulbTemperatureRange, double step, double pressure)
        {
            if (dryBulbTemperatureRange == null || double.IsNaN(dryBulbTemperatureRange.Min) || double.IsNaN(dryBulbTemperatureRange.Max) || double.IsNaN(step) || double.IsNaN(pressure))
            {
                return null;
            }

            List<ConstantTemperatureCurve> result = new List<ConstantTemperatureCurve>();

            double dryBulbTemperature = dryBulbTemperatureRange.Min;
            while (dryBulbTemperature <= dryBulbTemperatureRange.Max)
            {
                ConstantTemperatureCurve constantTemperatureCurve = ConstantTemperatureCurve_DiagramTemperature(dryBulbTemperature, pressure);
                if (constantTemperatureCurve != null)
                {
                    result.Add(constantTemperatureCurve);
                }

                dryBulbTemperature += step;
            }

            ConstantTemperatureCurve constantTemperatureCurve_Liquid = ConstantTemperatureCurve_Liquid(dryBulbTemperatureRange.Min, pressure);
            if(constantTemperatureCurve_Liquid != null)
            {
                result.Add(constantTemperatureCurve_Liquid);
            }

            ConstantTemperatureCurve constantTemperatureCurve_Solid = ConstantTemperatureCurve_Solid(dryBulbTemperatureRange.Min, pressure);
            if (constantTemperatureCurve_Solid != null)
            {
                result.Add(constantTemperatureCurve_Solid);
            }

            return result;
        }

        public static List<ConstantTemperatureCurve> ConstantTemperatureCurves_DryBulbTemperature(Range<double> dryBulbTemperatureRange, double step, double pressure)
        {
            if (dryBulbTemperatureRange == null || double.IsNaN(dryBulbTemperatureRange.Min) || double.IsNaN(dryBulbTemperatureRange.Max) || double.IsNaN(step) || double.IsNaN(pressure))
            {
                return null;
            }

            List<ConstantTemperatureCurve> result = new List<ConstantTemperatureCurve>();

            double dryBulbTemperature = dryBulbTemperatureRange.Min;
            while (dryBulbTemperature <= dryBulbTemperatureRange.Max)
            {
                ConstantTemperatureCurve constantTemperatureCurve = ConstantTemperatureCurve_DryBulbTemperature(dryBulbTemperature, pressure);
                if (constantTemperatureCurve != null)
                {
                    result.Add(constantTemperatureCurve);
                }

                dryBulbTemperature += step;
            }

            ConstantTemperatureCurve constantTemperatureCurve_Liquid = ConstantTemperatureCurve_Liquid(dryBulbTemperatureRange.Min, pressure);
            if (constantTemperatureCurve_Liquid != null)
            {
                result.Add(constantTemperatureCurve_Liquid);
            }

            ConstantTemperatureCurve constantTemperatureCurve_Solid = ConstantTemperatureCurve_Solid(dryBulbTemperatureRange.Min, pressure);
            if (constantTemperatureCurve_Solid != null)
            {
                result.Add(constantTemperatureCurve_Solid);
            }

            return result;
        }
    }
}
