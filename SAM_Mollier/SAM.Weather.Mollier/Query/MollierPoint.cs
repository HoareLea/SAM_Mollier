namespace SAM.Weather.Mollier
{
    public static partial class Query
    {
        public static Core.Mollier.MollierPoint MollierPoint(this WeatherHour weatherHour, double pressure = double.NaN)
        {
            if (weatherHour == null)
            {
                return null;
            }

            double dryBulbTemperature = weatherHour[WeatherDataType.DryBulbTemperature];
            if (double.IsNaN(dryBulbTemperature))
            {
                return null;
            }

            double relativeHumidity = weatherHour[WeatherDataType.RelativeHumidity];
            if (double.IsNaN(relativeHumidity))
            {
                return null;
            }

            double pressure_Temp = pressure;

            if (double.IsNaN(pressure_Temp))
            {
                pressure_Temp = weatherHour[WeatherDataType.AtmosphericPressure];
            }

            if (double.IsNaN(pressure_Temp))
            {
                pressure_Temp = Core.Mollier.Standard.Pressure;
            }

            if (double.IsNaN(pressure_Temp))
            {
                return null;
            }

            return Core.Mollier.Create.MollierPoint_ByRelativeHumidity(dryBulbTemperature, relativeHumidity, pressure_Temp);
        }
    }
}
