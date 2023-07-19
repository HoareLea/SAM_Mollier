using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Create
    { 
        public static List<ConstantValueCurve> ConstantValueCurves_Density(Range<double> densityRange, double step, double pressure)
        {
            if(densityRange == null || double.IsNaN(densityRange.Min) || double.IsNaN(densityRange.Max) || double.IsNaN(step) || double.IsNaN(pressure))
            {
                return null;
            }

            List<ConstantValueCurve> result = new List<ConstantValueCurve>();

            double density = densityRange.Min;
            while(density <= densityRange.Max)
            {
                ConstantValueCurve constantValueCurve = ConstantValueCurve_Density(density, pressure);
                if(constantValueCurve != null)
                {
                    result.Add(constantValueCurve);
                }

                density += step;
            }

            return result;
        }

        public static List<ConstantValueCurve> ConstantValueCurves_DiagramTemperature(Range<double> dryBulbTemperatureRange, double step, double pressure)
        {
            if (dryBulbTemperatureRange == null || double.IsNaN(dryBulbTemperatureRange.Min) || double.IsNaN(dryBulbTemperatureRange.Max) || double.IsNaN(step) || double.IsNaN(pressure))
            {
                return null;
            }

            List<ConstantValueCurve> result = new List<ConstantValueCurve>();

            double dryBulbTemperature = dryBulbTemperatureRange.Min;
            while (dryBulbTemperature <= dryBulbTemperatureRange.Max)
            {
                ConstantValueCurve constantValueCurve = ConstantValueCurve_DiagramTemperature(dryBulbTemperature, pressure);
                if (constantValueCurve != null)
                {
                    result.Add(constantValueCurve);
                }

                dryBulbTemperature += step;
            }

            return result;
        }

        public static List<ConstantValueCurve> ConstantValueCurves_DryBulbTemperature(Range<double> dryBulbTemperatureRange, double step, double pressure)
        {
            if (dryBulbTemperatureRange == null || double.IsNaN(dryBulbTemperatureRange.Min) || double.IsNaN(dryBulbTemperatureRange.Max) || double.IsNaN(step) || double.IsNaN(pressure))
            {
                return null;
            }

            List<ConstantValueCurve> result = new List<ConstantValueCurve>();

            double dryBulbTemperature = dryBulbTemperatureRange.Min;
            while (dryBulbTemperature <= dryBulbTemperatureRange.Max)
            {
                ConstantValueCurve constantValueCurve = ConstantValueCurve_DryBulbTemperature(dryBulbTemperature, pressure);
                if (constantValueCurve != null)
                {
                    result.Add(constantValueCurve);
                }

                dryBulbTemperature += step;
            }

            return result;
        }

        public static List<ConstantValueCurve> ConstantValueCurves_RelativeHumidity(Range<double> relativeHumidityRange, double step, double pressure, Range<double> dryBulbTemperatureRange)
        {
            if (relativeHumidityRange == null || double.IsNaN(relativeHumidityRange.Min) || double.IsNaN(relativeHumidityRange.Max) || dryBulbTemperatureRange == null || double.IsNaN(dryBulbTemperatureRange.Min) || double.IsNaN(dryBulbTemperatureRange.Max) || double.IsNaN(step) || double.IsNaN(pressure))
            {
                return null;
            }

            List<ConstantValueCurve> result = new List<ConstantValueCurve>();

            double relativeHumidity = relativeHumidityRange.Min;
            while (relativeHumidity <= relativeHumidityRange.Max)
            {
                ConstantValueCurve constantValueCurve = ConstantValueCurve_RelativeHumidity(relativeHumidity, pressure, dryBulbTemperatureRange);
                if (constantValueCurve != null)
                {
                    result.Add(constantValueCurve);
                }

                relativeHumidity += step;
            }

            return result;
        }

        public static List<ConstantValueCurve> ConstantValueCurves_SpecificVolume(Range<double> specificVolumeRange, double step, double pressure)
        {
            if (specificVolumeRange == null || double.IsNaN(specificVolumeRange.Min) || double.IsNaN(specificVolumeRange.Max) || double.IsNaN(step) || double.IsNaN(pressure))
            {
                return null;
            }

            List<ConstantValueCurve> result = new List<ConstantValueCurve>();

            double specificVolume = specificVolumeRange.Min;
            while (specificVolume <= specificVolumeRange.Max)
            {
                ConstantValueCurve constantValueCurve = ConstantValueCurve_SpecificVolume(specificVolume, pressure);
                if (constantValueCurve != null)
                {
                    result.Add(constantValueCurve);
                }

                specificVolume += step;
            }

            return result;
        }

        public static List<ConstantValueCurve> ConstantValueCurves_WetBulbTemperature(Range<double> dryBulbTemperatureRange, double step, double pressure)
        {
            if (dryBulbTemperatureRange == null || double.IsNaN(dryBulbTemperatureRange.Min) || double.IsNaN(dryBulbTemperatureRange.Max) || double.IsNaN(step) || double.IsNaN(pressure))
            {
                return null;
            }

            List<ConstantValueCurve> result = new List<ConstantValueCurve>();

            double dryBulbTemperature = dryBulbTemperatureRange.Min;
            while (dryBulbTemperature <= dryBulbTemperatureRange.Max)
            {
                ConstantValueCurve constantValueCurve = ConstantValueCurve_WetBulbTemperature(dryBulbTemperature, pressure);
                if (constantValueCurve != null)
                {
                    result.Add(constantValueCurve);
                }

                dryBulbTemperature += step;
            }

            return result;
        }

        public static List<ConstantValueCurve> ConstantValueCurves_Enthalpy(Range<double> enthalpyRange, double step, double pressure)
        {
            if (enthalpyRange == null || double.IsNaN(enthalpyRange.Min) || double.IsNaN(enthalpyRange.Max) || double.IsNaN(step) || double.IsNaN(pressure))
            {
                return null;
            }

            List<ConstantValueCurve> result = new List<ConstantValueCurve>();

            double enthalpy = enthalpyRange.Min;
            while (enthalpy <= enthalpyRange.Max)
            {
                ConstantValueCurve constantValueCurve = ConstantValueCurve_Enthalpy(enthalpy, pressure);
                if (constantValueCurve != null)
                {
                    result.Add(constantValueCurve);
                }

                enthalpy += step;
            }

            return result;
        }

        public static List<ConstantValueCurve> ConstantValueCurves_Enthalpy(Range<double> dryBulbTemperatureRange, Range<double> humidityRatioRange, double enthalpyStep, double pressure)
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

            return ConstantValueCurves_Enthalpy(new Range<double>(enthalpy_Max, enthalpy_Min), enthalpyStep, pressure);
        }
    }
}
