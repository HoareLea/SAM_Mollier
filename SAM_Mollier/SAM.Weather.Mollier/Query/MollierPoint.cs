namespace SAM.Weather.Mollier
{
    public static partial class Query
    {
        public static Core.Mollier.MollierPoint MollierPoint(this WeatherHour weatherHour)
        {
            if(weatherHour == null)
            {
                return null;
            }

            double dryBulbTemperature = weatherHour[WeatherDataType.DryBulbTemperature];
            if(double.IsNaN(dryBulbTemperature))
            {
                return null;
            }
            
            double relativeHumidity = weatherHour[WeatherDataType.RelativeHumidity];
            if (double.IsNaN(relativeHumidity))
            {
                return null;
            }

            double pressure = weatherHour[WeatherDataType.AtmosphericPressure];
            if (double.IsNaN(pressure))
            {
                return null;
            }

            return Core.Mollier.Create.MollierPoint_ByRelativeHumidity(dryBulbTemperature, relativeHumidity, pressure);
        }
    }
}
