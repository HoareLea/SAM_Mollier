﻿namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates relative humidity from dry bulb temperature, humidity ratio and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry Bulb Temperature [°C] — measured by a standard thermometer, unaffected by moisture (not wet-bulb or dew point).</param>
        /// <param name="humidityRatio">Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <param name="clamp">Clamp value Relative Humidity to range 0 - 100</param>
        /// <returns>Relative Humidity (0 - 100) [%]</returns>
        public static double RelativeHumidity(double dryBulbTemperature, double humidityRatio, double pressure, bool allowRH100 = false)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(humidityRatio) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            //double humidityRatio_Temp = humidityRatio / 1000;

            double result = (humidityRatio * pressure / (0.6222 + humidityRatio)) / SaturationVapourPressure(dryBulbTemperature) * 100;

            if(result > 100 && !allowRH100)
            {
                return double.NaN;   //test
            }

            if(result < 0)
            {
                return double.NaN;
            }

            //result = System.Math.Round(result);
            return result;
        }

        /// <summary>
        /// Calculates relative humidity from dry bulb temperature and dew point temperature.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry Bulb Temperature [°C] — measured by a standard thermometer, unaffected by moisture (not wet-bulb or dew point).</param>
        /// <param name="dewPointTemperature">Dew Point Temperature [°C]</param>
        /// <param name="pressure">Pressure [Pa]</param>
        /// <returns>Relative Humidity (0 - 100) [%]</returns>
        public static double RelativeHumidity_ByDewPointTemperature(double dryBulbTemperature, double dewPointTemperature, double pressure)
        {
            if (double.IsNaN(dryBulbTemperature) || double.IsNaN(dewPointTemperature))
            {
                return double.NaN;
            }

            return Core.Query.Calculate((double x) => DewPointTemperature(dryBulbTemperature, x, pressure), dewPointTemperature, 0, 100);
        }

        /// <summary>
        /// Calculates relative humidity from dry bulb temperature, wet bulb temperature and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry Bulb Temperature [°C] — measured by a standard thermometer, unaffected by moisture (not wet-bulb or dew point).</param>
        /// <param name="wetBulbTemperature">Wet bulb temperature [°C]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Relative Humidity (0 - 100) [%]</returns>
        public static double RelativeHumidity_ByWetBulbTemperature(double dryBulbTemperature, double wetBulbTemperature, double pressure)
        {
            if (double.IsNaN(dryBulbTemperature) || double.IsNaN(wetBulbTemperature) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            //Partial Vapour Pressure ratio
            double pressureRatio = HumidityRatio_ByWetBulbTemperature(dryBulbTemperature, wetBulbTemperature, pressure) * pressure / (HumidityRatio_ByWetBulbTemperature(dryBulbTemperature, wetBulbTemperature, pressure) + 0.621945);
            if (double.IsNaN(pressureRatio))
            {
                return double.NaN;
            }

            return System.Math.Min(1, pressureRatio / SaturationVapourPressure(dryBulbTemperature)) * 100;
        }

        public static double RelativeHumidity_ByDryBulbTemperature(double dryBulbTemperature, double relativeHumidity, double pressure)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(relativeHumidity) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            double partialVapourPressure = PartialVapourPressure(dryBulbTemperature, relativeHumidity, pressure);
            if(double.IsNaN(partialVapourPressure))
            {
                return double.NaN;
            }

            double saturationVapourPressure = SaturationVapourPressure(dryBulbTemperature);
            if(double.IsNaN(saturationVapourPressure))
            {
                return double.NaN;
            }

            return RelativeHumidity_ByPartialVapourPressure(partialVapourPressure, saturationVapourPressure);
        }

        public static double RelativeHumidity_ByPartialVapourPressure(double partialVapourPressure, double saturationVapourPressure)
        {
            if(double.IsNaN(partialVapourPressure) || double.IsNaN(saturationVapourPressure))
            { 
                return double.NaN; 
            }

            if(saturationVapourPressure == 0)
            {
                return 0;
            }

            return partialVapourPressure / saturationVapourPressure;
        }


    }
}
