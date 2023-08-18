using System.Collections.Generic;

namespace SAM.Weather.Mollier
{
    public static partial class Query
    {
        public static List<Core.Mollier.MollierPoint> MollierPoints(this WeatherDay weatherDay, double pressure = double.NaN)
        {
            if(weatherDay == null)
            {
                return null;
            }

            List<double> dryBulbTemperatures = weatherDay.GetValues(WeatherDataType.DryBulbTemperature);
            if(dryBulbTemperatures == null || dryBulbTemperatures.Count == 0)
            {
                return null;
            }
            
            List<double> relativeHumidities = weatherDay.GetValues(WeatherDataType.RelativeHumidity);
            if (relativeHumidities == null || relativeHumidities.Count == 0)
            {
                return null;
            }

            List<double> pressures = weatherDay.GetValues(WeatherDataType.AtmosphericPressure);

            List<Core.Mollier.MollierPoint> result = new List<Core.Mollier.MollierPoint>();
            for (int i = 0; i < dryBulbTemperatures.Count; i++)
            {
                double dryBulbTemperature = dryBulbTemperatures[i];
                if(double.IsNaN(dryBulbTemperature))
                {
                    result.Add(null);
                    continue;
                }

                if(i >= relativeHumidities.Count)
                {
                    result.Add(null);
                    continue;
                }

                double relativeHumidity = relativeHumidities[i];
                if (double.IsNaN(dryBulbTemperature))
                {
                    result.Add(null);
                    continue;
                }

                double pressure_Temp = pressure;

                if (double.IsNaN(pressure_Temp))
                {
                    if (pressures != null && i < pressures.Count)
                    {
                        pressure_Temp = pressures[i];
                    }
                }

                if (double.IsNaN(pressure_Temp))
                {
                    pressure_Temp = Core.Mollier.Standard.Pressure;
                }

                if (double.IsNaN(pressure_Temp))
                {
                    continue;
                }

                Core.Mollier.MollierPoint mollierPoint = Core.Mollier.Create.MollierPoint_ByRelativeHumidity(dryBulbTemperature, relativeHumidity, pressure_Temp);
                if(mollierPoint == null)
                {
                    continue;
                }

                result.Add(mollierPoint);
            }

            return result;
        }
    }
}
