﻿using System;
using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Create
    { 
        public static ConstantValueCurve ConstantValueCurve_Density(double density, double pressure)
        {
            if(double.IsNaN(density) || double.IsNaN(pressure))
            {
                return null;
            }

            double dryBulbTemperature_1 = Query.DryBulbTemperature_ByDensityAndRelativeHumidity(density, 0, pressure);
            if(double.IsNaN(dryBulbTemperature_1))
            {
                return null;
            }

            double humidityRatio_1 = Query.HumidityRatio(dryBulbTemperature_1, 0, pressure);
            if (double.IsNaN(humidityRatio_1))
            {
                return null;
            }

            MollierPoint mollierPoint_1 = new MollierPoint(dryBulbTemperature_1, humidityRatio_1, pressure);
            if(!mollierPoint_1.IsValid())
            {
                return null;
            }

            double temperature_2 = Query.DryBulbTemperature_ByDensityAndRelativeHumidity(density, 100, pressure);
            if (double.IsNaN(temperature_2))
            {
                return null;
            }

            double humidityRatio_2 = Query.HumidityRatio(temperature_2, 100, pressure);
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

        public static ConstantValueCurve ConstantValueCurve_Enthalpy(double enthalpy, double pressure)
        {
            if (double.IsNaN(enthalpy) || double.IsNaN(pressure))
            {
                return null;
            }

            //Relative Humidity = 0
            double dryBulbTemperature_1 = Query.DryBulbTemperature(enthalpy, 0, pressure);
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
            double dryBulbTemperature_2 = Query.DryBulbTemperature_ByEnthalpy(enthalpy, 100, pressure);
            if(double.IsNaN(dryBulbTemperature_2))
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
                double humidityRatio = Query.HumidityRatio(dryBulbTemperature, relativeHumidity, pressure);
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

            MollierPoint mollierPoint_1 = MollierPoint_ByRelativeHumidityAndSpecificVolume(0, specificVolume, pressure);
            if (mollierPoint_1 == null || !mollierPoint_1.IsValid())
            {
                return null;
            }

            MollierPoint mollierPoint_2 = MollierPoint_ByRelativeHumidityAndSpecificVolume(100, specificVolume, pressure);
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

            double dryBulbTemperature_1 = Query.DryBulbTemperature_ByWetBulbTemperature(wetBulbTemperature, 0, pressure);
            if (double.IsNaN(dryBulbTemperature_1))
            {
                return null;
            }
            double humidityRatio_1 = Query.HumidityRatio(dryBulbTemperature_1, 0, pressure);
            if(double.IsNaN(humidityRatio_1))
            {
                return null;
            }

            MollierPoint mollierPoint_1 = new MollierPoint(dryBulbTemperature_1, humidityRatio_1, pressure);
            if(!mollierPoint_1.IsValid())
            {
                return null;
            }


            double dryBulbTemperature_2 = Query.DryBulbTemperature_ByWetBulbTemperature(wetBulbTemperature, 100, pressure);
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

            return new ConstantValueCurve(ChartDataType.WetBulbTemperature, wetBulbTemperature, mollierPoint_1, mollierPoint_2);
        }
    }
}
