using System;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates Specific Heat [J/kg*K]
        /// </summary>
        /// <param name="dryBulbTempearture">Dry Bulb Temparture [C]</param>
        /// <returns>Specific Heat [J/kg*K]</returns>
        public static double SpecificHeat_Air(double dryBulbTempearture)
        {
            if(double.IsNaN(dryBulbTempearture))
            {
                return double.NaN;
            }

            double dryBulbTemperature_Kelvin = dryBulbTempearture + 273.15;

            return 1.9327e-10 * Math.Pow(dryBulbTemperature_Kelvin, 4) - 7.9999e-7 * Math.Pow(dryBulbTemperature_Kelvin, 3) + 1.1407e-3* Math.Pow(dryBulbTemperature_Kelvin, 2) - 4.4890e-1 * dryBulbTemperature_Kelvin + 1.0575e3;
        }
    }
}
