using System;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates Specific Heat for Air [J/kg*K]
        /// </summary>
        /// <param name="dryBulbTempearture">Dry Bulb Temparture [C]</param>
        /// <returns>Air Specific Heat [J/kg*K]</returns>
        public static double SpecificHeat_Air(double dryBulbTempearture)
        {
            if(double.IsNaN(dryBulbTempearture))
            {
                return double.NaN;
            }

            double dryBulbTemperature_Kelvin = dryBulbTempearture + 273.15;

            return 1.9327e-10 * Math.Pow(dryBulbTemperature_Kelvin, 4) - 7.9999e-7 * Math.Pow(dryBulbTemperature_Kelvin, 3) + 1.1407e-3* Math.Pow(dryBulbTemperature_Kelvin, 2) - 4.4890e-1 * dryBulbTemperature_Kelvin + 1.0575e3;
        }

        /// <summary>
        /// Calculates Specific Heat for Water Vapour [J/kg*K]
        /// </summary>
        /// <param name="dryBulbTempearture">Dry Bulb Temparture [C]</param>
        /// <returns>Water Vapour Specific Heat [J/kg*K]</returns>
        public static double SpecificHeat_WaterVapour(double dryBulbTempearture)
        {
            if (double.IsNaN(dryBulbTempearture))
            {
                return double.NaN;
            }

            double dryBulbTemperature_Kelvin = dryBulbTempearture + 273.15;

            return 9e-12 * Math.Pow(dryBulbTemperature_Kelvin, 4) - 9e-8 * Math.Pow(dryBulbTemperature_Kelvin, 3) + 0.0002 * Math.Pow(dryBulbTemperature_Kelvin, 2) - 0.5115 * dryBulbTemperature_Kelvin + 1692.7;
        }

        /// <summary>
        /// Calculates Specific Heat for Water [J/kg*K]
        /// </summary>
        /// <param name="dryBulbTempearture">Dry Bulb Temparture [C]</param>
        /// <returns>Water Specific Heat [J/kg*K]</returns>
        public static double SpecificHeat_Water(double dryBulbTempearture)
        {
            if (double.IsNaN(dryBulbTempearture))
            {
                return double.NaN;
            }

            double dryBulbTemperature_Kelvin = dryBulbTempearture + 273.15;

            return 5e-07*Math.Pow(dryBulbTemperature_Kelvin, 4) - 0.0002 * Math.Pow(dryBulbTemperature_Kelvin, 3) + 0.0328 * Math.Pow(dryBulbTemperature_Kelvin, 2) - 1.99 * dryBulbTemperature_Kelvin + 4215.6;
        }
    }
}
