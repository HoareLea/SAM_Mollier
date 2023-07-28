using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Create
    { 
        public static List<ConstantValueCurve> ConstantValueCurves_Density(this MollierRange mollierRange, Range<double> densityRange, double step, double pressure)
        {
            if(densityRange == null || double.IsNaN(densityRange.Min) || double.IsNaN(densityRange.Max) || double.IsNaN(step) || double.IsNaN(pressure))
            {
                return null;
            }

            List<ConstantValueCurve> result = new List<ConstantValueCurve>();

            double density = densityRange.Min;
            while(density <= densityRange.Max)
            {
                ConstantValueCurve constantValueCurve = ConstantValueCurve_Density(mollierRange, density, pressure);
                if(constantValueCurve != null)
                {
                    result.Add(constantValueCurve);
                }

                density += step;
            }

            return result;
        }

        public static List<ConstantValueCurve> ConstantValueCurves_RelativeHumidity(this MollierRange mollierRange, Range<double> relativeHumidityRange, double step, double pressure)
        {
            if (double.IsNaN(step) || double.IsNaN(pressure) || mollierRange == null || !mollierRange.IsValid())
            {
                return null;
            }

            List<ConstantValueCurve> result = new List<ConstantValueCurve>();

            double relativeHumidity = relativeHumidityRange.Min;
            while (relativeHumidity <= relativeHumidityRange.Max)
            {
                ConstantValueCurve constantValueCurve = ConstantValueCurve_RelativeHumidity(mollierRange, relativeHumidity, pressure);
                if (constantValueCurve != null)
                {
                    result.Add(constantValueCurve);
                }

                relativeHumidity += step;
            }

            return result;
        }

        public static List<ConstantValueCurve> ConstantValueCurves_SpecificVolume(this MollierRange mollierRange, Range<double> specificVolumeRange, double step, double pressure)
        {
            if (specificVolumeRange == null || double.IsNaN(specificVolumeRange.Min) || double.IsNaN(specificVolumeRange.Max) || double.IsNaN(step) || double.IsNaN(pressure) || mollierRange == null || !mollierRange.IsValid())
            {
                return null;
            }

            List<ConstantValueCurve> result = new List<ConstantValueCurve>();

            double specificVolume = specificVolumeRange.Min;
            while (specificVolume <= specificVolumeRange.Max)
            {
                ConstantValueCurve constantValueCurve = ConstantValueCurve_SpecificVolume(mollierRange, specificVolume, pressure);
                if (constantValueCurve != null)
                {
                    result.Add(constantValueCurve);
                }

                specificVolume += step;
            }

            return result;
        }

        public static List<ConstantValueCurve> ConstantValueCurves_WetBulbTemperature(this MollierRange mollierRange, double step, double pressure)
        {
            if (double.IsNaN(step) || double.IsNaN(pressure) || mollierRange == null || !mollierRange.IsValid())
            {
                return null;
            }

            List<ConstantValueCurve> result = new List<ConstantValueCurve>();

            double dryBulbTemperature = mollierRange.DryBulbTemperature_Min;
            while (dryBulbTemperature <= mollierRange.DryBulbTemperature_Max)
            {
                ConstantValueCurve constantValueCurve = ConstantValueCurve_WetBulbTemperature(mollierRange, dryBulbTemperature, pressure);
                if (constantValueCurve != null)
                {
                    result.Add(constantValueCurve);
                }

                dryBulbTemperature += step;
            }

            return result;
        }
    }
}
