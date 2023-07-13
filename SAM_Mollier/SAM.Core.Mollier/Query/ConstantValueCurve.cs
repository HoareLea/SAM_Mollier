using System;
using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Query
    { 
        public static ConstantValueCurve ConstantValueCurve_Density(double density, double pressure)
        {
            if(double.IsNaN(density) || double.IsNaN(pressure))
            {
                return null;
            }

            double dryBulbTemperature_1 = DryBulbTemperature_ByDensityAndRelativeHumidity(density, 0, pressure);
            if(double.IsNaN(dryBulbTemperature_1))
            {
                return null;
            }

            double humidityRatio_1 = HumidityRatio(dryBulbTemperature_1, 0, pressure);
            if (double.IsNaN(humidityRatio_1))
            {
                return null;
            }

            MollierPoint mollierPoint_1 = new MollierPoint(dryBulbTemperature_1, humidityRatio_1, pressure);
            if(!mollierPoint_1.IsValid())
            {
                return null;
            }

            double temperature_2 = DryBulbTemperature_ByDensityAndRelativeHumidity(density, 100, pressure);
            if (double.IsNaN(temperature_2))
            {
                return null;
            }

            double humidityRatio_2 = HumidityRatio(temperature_2, 100, pressure);
            if (double.IsNaN(humidityRatio_2))
            {
                return null;
            }

            MollierPoint mollierPoint_2 = new MollierPoint(temperature_2, humidityRatio_2, pressure);
            if (!mollierPoint_2.IsValid())
            {
                return null;
            }

            return new ConstantValueCurve(ChartDataType.Density, density, mollierPoint_1, mollierPoint_2);
        }

        public static ConstantValueCurve ConstantValueCurve_DiagramTemperature(double dryBulbTemperature, double pressure)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(pressure))
            {
                return null;
            }

            double humidityRatio_1 = HumidityRatio(dryBulbTemperature, 0, pressure);
            if(double.IsNaN(humidityRatio_1))
            {
                return null;
            }

            double diagramTemperature_1 = DiagramTemperature(dryBulbTemperature, humidityRatio_1, pressure);
            if(double.IsNaN(diagramTemperature_1))
            {
                return null;
            }

            MollierPoint mollierPoint_1 = new MollierPoint(diagramTemperature_1, humidityRatio_1, pressure);
            if (!mollierPoint_1.IsValid())
            {
                return null;
            }

            double humidityRatio_2 = HumidityRatio(dryBulbTemperature, 100, pressure);
            if (double.IsNaN(humidityRatio_1))
            {
                return null;
            }

            double diagramTemperature_2 = DiagramTemperature(dryBulbTemperature, humidityRatio_2, pressure);
            if (double.IsNaN(diagramTemperature_1))
            {
                return null;
            }

            MollierPoint mollierPoint_2 = new MollierPoint(diagramTemperature_2, humidityRatio_2, pressure);
            if (!mollierPoint_2.IsValid())
            {
                return null;
            }

            return new ConstantValueCurve(ChartDataType.DiagramTemperature, dryBulbTemperature, mollierPoint_1, mollierPoint_2);
        }

        public static ConstantValueCurve ConstantValueCurve_DryBulbTemperature(double dryBulbTemperature, double pressure)
        {
            if (double.IsNaN(dryBulbTemperature) || double.IsNaN(pressure))
            {
                return null;
            }

            MollierPoint mollierPoint_1 = new MollierPoint(dryBulbTemperature, 0, pressure);
            if (!mollierPoint_1.IsValid())
            {
                return null;
            }

            double humidityRatio = HumidityRatio(dryBulbTemperature, 100, pressure);
            if (double.IsNaN(humidityRatio))
            {
                return null;
            }

            MollierPoint mollierPoint_2 = new MollierPoint(dryBulbTemperature, humidityRatio, pressure);
            if(!mollierPoint_2.IsValid())
            {
                return null;
            }

            return new ConstantValueCurve(ChartDataType.DryBulbTemperature, dryBulbTemperature, mollierPoint_1, mollierPoint_2);
        }

        public static ConstantValueCurve ConstantValueCurve_Enthalpy(double enthalpy, double pressure)
        {
            if (double.IsNaN(enthalpy) || double.IsNaN(pressure))
            {
                return null;
            }

            //Relative Humidity = 0
            double dryBulbTemperature_1 = DryBulbTemperature(enthalpy, 0, pressure);
            if(double.IsNaN(dryBulbTemperature_1))
            {
                return null;
            }

            MollierPoint mollierPoint_1 = new MollierPoint(dryBulbTemperature_1, 0, pressure);
            if(!mollierPoint_1.IsValid())
            {
                return null;
            }

            //Relative Humidity = 100
            double dryBulbTemperature_2 = DryBulbTemperature_ByEnthalpy(enthalpy, 100, pressure);
            if(double.IsNaN(dryBulbTemperature_2))
            {
                return null;
            }

            double humidityRatio_2 = HumidityRatio(dryBulbTemperature_2, 100, pressure);
            if (double.IsNaN(humidityRatio_2))
            {
                return null;
            }

            MollierPoint mollierPoint_2 = new MollierPoint(dryBulbTemperature_2, humidityRatio_2, pressure);
            if (!mollierPoint_2.IsValid())
            {
                return null;
            }

            return new ConstantValueCurve(ChartDataType.Enthalpy, enthalpy, mollierPoint_1, mollierPoint_2);
        }

        public static ConstantValueCurve ConstantValueCurve_RelativeHumidity(double relativeHumidity, double pressure, Range<double> dryBulbTemperatureRange)
        {
            if (double.IsNaN(relativeHumidity) || double.IsNaN(pressure) || dryBulbTemperatureRange == null || double.IsNaN(dryBulbTemperatureRange.Min) || double.IsNaN(dryBulbTemperatureRange.Max))
            {
                return null;
            }

            double step = (dryBulbTemperatureRange.Max - dryBulbTemperatureRange.Min) / Math.Truncate(dryBulbTemperatureRange.Max - dryBulbTemperatureRange.Min);
            if(double.IsNaN(step) || double.IsInfinity(step))
            {
                step = 0.1;
            }

            double dryBulbTemperature = dryBulbTemperatureRange.Min;

            List<MollierPoint> mollierPoints = new List<MollierPoint>();
            while(dryBulbTemperature <= dryBulbTemperatureRange.Max)
            {
                double humidityRatio = HumidityRatio(dryBulbTemperature, relativeHumidity, pressure);
                if(!double.IsNaN(humidityRatio))
                {
                    MollierPoint mollierPoint = new MollierPoint(dryBulbTemperature, humidityRatio, pressure);
                    if(mollierPoint.IsValid())
                    {
                        mollierPoints.Add(mollierPoint);
                    }
                }

                dryBulbTemperature += step;
            }

            if(dryBulbTemperature > dryBulbTemperatureRange.Max)
            {
                dryBulbTemperature = dryBulbTemperatureRange.Max;
                double humidityRatio = HumidityRatio(dryBulbTemperature, relativeHumidity, pressure);
                if (!double.IsNaN(humidityRatio))
                {
                    MollierPoint mollierPoint = new MollierPoint(dryBulbTemperature, humidityRatio, pressure);
                    if (mollierPoint.IsValid())
                    {
                        mollierPoints.Add(mollierPoint);
                    }
                }
            }

            if(mollierPoints == null || mollierPoints.Count == 0)
            {
                return null;
            }

            return new ConstantValueCurve(ChartDataType.RelativeHumidity, relativeHumidity, mollierPoints);
        }

        public static ConstantValueCurve ConstantValueCurve_SpecificVolume(double specificVolume, double pressure)
        {
            if(double.IsNaN(specificVolume) || double.IsNaN(pressure))
            {
                return null;
            }

            MollierPoint mollierPoint_1 = Create.MollierPoint_ByRelativeHumidityAndSpecificVolume(0, specificVolume, pressure);
            if (mollierPoint_1 == null || !mollierPoint_1.IsValid())
            {
                return null;
            }

            MollierPoint mollierPoint_2 = Create.MollierPoint_ByRelativeHumidityAndSpecificVolume(100, specificVolume, pressure);
            if (mollierPoint_2 == null || !mollierPoint_2.IsValid())
            {
                return null;
            }

            return new ConstantValueCurve(ChartDataType.SpecificVolume, specificVolume, mollierPoint_1, mollierPoint_2);
        }

        public static ConstantValueCurve ConstantValueCurve_WetBulbTemperature(double wetBulbTemperature, double pressure)
        {
            if (double.IsNaN(wetBulbTemperature) || double.IsNaN(pressure))
            {
                return null;
            }

            double dryBulbTemperature_1 = DryBulbTemperature_ByWetBulbTemperature(wetBulbTemperature, 0, pressure);
            if (double.IsNaN(dryBulbTemperature_1))
            {
                return null;
            }
            double humidityRatio_1 = HumidityRatio(dryBulbTemperature_1, 0, pressure);
            if(double.IsNaN(humidityRatio_1))
            {
                return null;
            }

            MollierPoint mollierPoint_1 = new MollierPoint(dryBulbTemperature_1, humidityRatio_1, pressure);
            if(!mollierPoint_1.IsValid())
            {
                return null;
            }


            double dryBulbTemperature_2 = DryBulbTemperature_ByWetBulbTemperature(wetBulbTemperature, 100, pressure);
            if (double.IsNaN(dryBulbTemperature_2))
            {
                return null;
            }

            double humidityRatio_2 = HumidityRatio(dryBulbTemperature_2, 100, pressure);
            if (double.IsNaN(humidityRatio_2))
            {
                return null;
            }

            MollierPoint mollierPoint_2 = new MollierPoint(dryBulbTemperature_2, humidityRatio_2, pressure);
            if (!mollierPoint_2.IsValid())
            {
                return null;
            }

            return new ConstantValueCurve(ChartDataType.WetBulbTemperature, wetBulbTemperature, mollierPoint_1, mollierPoint_2);
        }
    }
}
