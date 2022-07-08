namespace SAM.Core.Mollier
{
    public static partial class Create
    {
        public static MollierPoint MollierPoint_ByWetBulbTemperature(double dryBulbTemperature, double wetBulbTemperature, double pressure)
        {
            if (double.IsNaN(dryBulbTemperature) || double.IsNaN(wetBulbTemperature) || double.IsNaN(pressure))
            {
                return null;
            }

            double humidityRatio = Query.HumidityRatio_ByWetBulbTemperature(dryBulbTemperature, wetBulbTemperature, pressure);

            return new MollierPoint(dryBulbTemperature, humidityRatio, pressure);
        }

        public static MollierPoint MollierPoint_ByEnthalpy(double enthalpy, double humidityRatio, double pressure)
        {
            if(double.IsNaN(enthalpy) || double.IsNaN(humidityRatio) || double.IsNaN(pressure))
            {
                return null;
            }

            double dryBulbTemperature = Query.DryBulbTemperature(enthalpy, humidityRatio);
            if(double.IsNaN(dryBulbTemperature))
            {
                return null;
            }

            return new MollierPoint(dryBulbTemperature, humidityRatio, pressure);
        }

        public static MollierPoint MollierPoint_ByRelativeHumidity(double dryBulbTemperature, double relativeHumidity, double pressure)
        {
            if (double.IsNaN(dryBulbTemperature) || double.IsNaN(relativeHumidity) || double.IsNaN(pressure))
            {
                return null;
            }

            double humidityRatio = Query.HumidityRatio(dryBulbTemperature, relativeHumidity, pressure);
            if(double.IsNaN(humidityRatio))
            {
                return null;
            }

            return new MollierPoint(dryBulbTemperature, humidityRatio, pressure);
        }
    
        public static MollierPoint MollierPoint_ByRelativeHumidityAndSpecificVolume(double relativeHumidity, double specificVolume, double pressure)
        {
            if (double.IsNaN(relativeHumidity) || double.IsNaN(specificVolume) || double.IsNaN(pressure))
            {
                return null;
            }

            double dryBulbTemperature = Query.DryBulbTemperature_ByRelativeHumidityAndSpecificVolume(relativeHumidity, specificVolume, pressure);
            if (double.IsNaN(dryBulbTemperature))
            {
                return null;
            }

            double humidityRatio = Query.HumidityRatio(dryBulbTemperature, relativeHumidity, pressure);
            if (double.IsNaN(humidityRatio))
            {
                return null;
            }

            return new MollierPoint(dryBulbTemperature, humidityRatio, pressure);
        }


    }
}
