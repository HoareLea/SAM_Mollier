using System;
using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Create
    {
        public static ConstantValueCurve ConstantValueCurve_Density(this MollierRange mollierRange, double density, double pressure)
        {
            if (double.IsNaN(density) || double.IsNaN(pressure) || mollierRange == null || !mollierRange.IsValid())
            {
                return null;
            }

            double dryBulbTemperature_1 = Query.DryBulbTemperature_ByDensityAndRelativeHumidity(density, 0, pressure);
            if (double.IsNaN(dryBulbTemperature_1))
            {
                return null;
            }

            if (dryBulbTemperature_1 > mollierRange.DryBulbTemperature_Max)
            {
                return null;
            }

            double humidityRatio_1 = Query.HumidityRatio(dryBulbTemperature_1, 0, pressure);
            if (double.IsNaN(humidityRatio_1))
            {
                return null;
            }

            if (humidityRatio_1 < mollierRange.HumidityRatio_Min)
            {
                humidityRatio_1 = mollierRange.HumidityRatio_Min;
                dryBulbTemperature_1 = Query.DryBulbTemperature_ByDensityAndHumidityRatio(density, humidityRatio_1, pressure);
            }

            if (humidityRatio_1 > Query.SaturationHumidityRatio(dryBulbTemperature_1, pressure))
            {
                return null;
            }

            MollierPoint mollierPoint_1 = new MollierPoint(dryBulbTemperature_1, humidityRatio_1, pressure);
            if (!mollierPoint_1.IsValid())
            {
                return null;
            }

            double dryBulbTemperature_2 = Query.DryBulbTemperature_ByDensityAndRelativeHumidity(density, 100, pressure);
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

            if(mollierPoint_2.HumidityRatio > mollierRange.HumidityRatio_Max)
            {
                humidityRatio_2 = mollierRange.HumidityRatio_Max;
                dryBulbTemperature_2 = Query.DryBulbTemperature_ByDensityAndHumidityRatio(density, humidityRatio_2, pressure);
                mollierPoint_2 = new MollierPoint(dryBulbTemperature_2, humidityRatio_2, pressure);
            }

            if(mollierPoint_1.DryBulbTemperature < mollierRange.DryBulbTemperature_Min && mollierPoint_2.DryBulbTemperature < mollierRange.DryBulbTemperature_Min)
            {
                return null;
            }

            ConstantValueCurve result = new ConstantValueCurve(ChartDataType.Density, density, mollierPoint_1, mollierPoint_2);
            return result;
        }

        public static ConstantValueCurve ConstantValueCurve_RelativeHumidity(this MollierRange mollierRange, double relativeHumidity, double pressure)
        {
            if (double.IsNaN(relativeHumidity) || double.IsNaN(pressure) || mollierRange == null || !mollierRange.IsValid())
            {
                return null;
            }

            double step = (mollierRange.DryBulbTemperature_Max - mollierRange.DryBulbTemperature_Min) / Math.Truncate(mollierRange.DryBulbTemperature_Max - mollierRange.DryBulbTemperature_Min);
            if (double.IsNaN(step) || double.IsInfinity(step))
            {
                step = 0.1;
            }

            double dryBulbTemperature = Query.DryBulbTemperature_ByHumidityRatio(mollierRange.HumidityRatio_Min, relativeHumidity, pressure);
            dryBulbTemperature = Math.Max(dryBulbTemperature, mollierRange.DryBulbTemperature_Min);

            List<MollierPoint> mollierPoints = new List<MollierPoint>();
            while (dryBulbTemperature <= mollierRange.DryBulbTemperature_Max)
            {
                double humidityRatio = Query.HumidityRatio(dryBulbTemperature, relativeHumidity, pressure);
                if (!double.IsNaN(humidityRatio))
                {
                    if(humidityRatio > mollierRange.HumidityRatio_Max)
                    {
                        humidityRatio = mollierRange.HumidityRatio_Max;
                        dryBulbTemperature = Query.DryBulbTemperature_ByHumidityRatio(humidityRatio, relativeHumidity, pressure);
                    }

                    MollierPoint mollierPoint = new MollierPoint(dryBulbTemperature, humidityRatio, pressure);
                    if (mollierPoint.IsValid())
                    {
                        mollierPoints.Add(mollierPoint);
                    }

                    if(humidityRatio >= mollierRange.HumidityRatio_Max)
                    {
                        break;
                    }
                }

                dryBulbTemperature += step;
            }

            if (dryBulbTemperature > mollierRange.DryBulbTemperature_Max)
            {
                dryBulbTemperature = mollierRange.DryBulbTemperature_Max;
                double humidityRatio = Query.HumidityRatio(dryBulbTemperature, relativeHumidity, pressure);
                if (!double.IsNaN(humidityRatio))
                {
                    MollierPoint mollierPoint = new MollierPoint(dryBulbTemperature, humidityRatio, pressure);
                    if (mollierPoint.IsValid())
                    {
                        mollierPoints.Add(mollierPoint);
                    }
                }
            }

            if (mollierPoints == null || mollierPoints.Count == 0)
            {
                return null;
            }

            return new ConstantValueCurve(ChartDataType.RelativeHumidity, relativeHumidity, mollierPoints);
        }

        public static ConstantValueCurve ConstantValueCurve_SpecificVolume(this MollierRange mollierRange, double specificVolume, double pressure)
        {
            if (double.IsNaN(specificVolume) || double.IsNaN(pressure) || mollierRange == null || !mollierRange.IsValid())
            {
                return null;
            }

            double humidityRatio_1 = Math.Max(0, mollierRange.HumidityRatio_Min);

            MollierPoint mollierPoint_1 = MollierPoint_ByHumidityRatioAndSpecificVolume(humidityRatio_1, specificVolume, pressure);
            if (mollierPoint_1 == null || !mollierPoint_1.IsValid())
            {
                return null;
            }

            if (mollierPoint_1.DryBulbTemperature > mollierRange.DryBulbTemperature_Max)
            {
                return null;
            }

            if (humidityRatio_1 > Query.SaturationHumidityRatio(mollierPoint_1.DryBulbTemperature, pressure))
            {
                return null;
            }


            MollierPoint mollierPoint_2 = MollierPoint_ByRelativeHumidityAndSpecificVolume(100, specificVolume, pressure);
            if (mollierPoint_2 == null || !mollierPoint_2.IsValid())
            {
                return null;
            }

            if(mollierPoint_2.HumidityRatio > mollierRange.HumidityRatio_Max)
            {
                double dryBulbTemperature = Query.DryBulbTemperature_ByHumidityRatioAndSpecificVolume(mollierRange.HumidityRatio_Max, specificVolume, pressure);
                mollierPoint_2 = new MollierPoint(dryBulbTemperature, mollierRange.HumidityRatio_Max, pressure);
            }


            if (mollierPoint_1.DryBulbTemperature < mollierRange.DryBulbTemperature_Min && mollierPoint_2.DryBulbTemperature < mollierRange.DryBulbTemperature_Min)
            {
                return null;
            }

            return new ConstantValueCurve(ChartDataType.SpecificVolume, specificVolume, mollierPoint_1, mollierPoint_2);
        }

        public static ConstantValueCurve ConstantValueCurve_WetBulbTemperature(this MollierRange mollierRange, double wetBulbTemperature, double pressure)
        {
            if (double.IsNaN(wetBulbTemperature) || double.IsNaN(pressure) || mollierRange == null || !mollierRange.IsValid())
            {
                return null;
            }

            double humidityRatio_2 = Query.SaturationHumidityRatio(wetBulbTemperature, pressure);
            if(humidityRatio_2  < mollierRange.HumidityRatio_Min)
            {
                return null;
            }

            MollierPoint mollierPoint_2 = new MollierPoint(wetBulbTemperature, humidityRatio_2, pressure);
            if (!mollierPoint_2.IsValid())
            {
                return null;
            }

            double enthalpy = mollierPoint_2.Enthalpy;
            if(humidityRatio_2 > mollierRange.HumidityRatio_Max)
            {
                humidityRatio_2 = mollierRange.HumidityRatio_Max;
                mollierPoint_2 = MollierPoint_ByEnthalpy(enthalpy, humidityRatio_2, pressure);
            }

            if (mollierPoint_2.DryBulbTemperature > mollierRange.DryBulbTemperature_Max)
            {
                return null;
            }

            double humidityRatio_1 = 0;
            if(humidityRatio_1 < mollierRange.HumidityRatio_Min)
            {
                humidityRatio_1 = mollierRange.HumidityRatio_Min;
            }

            MollierPoint mollierPoint_1 = MollierPoint_ByEnthalpy(enthalpy, humidityRatio_1, pressure);
            if (!mollierPoint_1.IsValid())
            {
                return null;
            }

            if (mollierPoint_1.DryBulbTemperature > mollierRange.DryBulbTemperature_Max)
            {
                double dryBulbTemperature_1 = mollierRange.DryBulbTemperature_Max;

                humidityRatio_1 = Query.HumidityRatio_ByEnthalpy(dryBulbTemperature_1, enthalpy);

                mollierPoint_1 = MollierPoint_ByEnthalpy(enthalpy, humidityRatio_1, pressure);
            }

            if (mollierPoint_1.DryBulbTemperature < mollierRange.DryBulbTemperature_Min && mollierPoint_2.DryBulbTemperature < mollierRange.DryBulbTemperature_Min)
            {
                return null;
            }

            return new ConstantValueCurve(ChartDataType.WetBulbTemperature, wetBulbTemperature, mollierPoint_1, mollierPoint_2);
        }
    }
}