using System;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates dry bulb temperature from enthalpy and humidity ratio.
        /// </summary>
        /// <param name="enthalpy">Moist air Enthalpy[J / kg]</param>
        /// <param name="humidityRatio">Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <returns>Dry-bulb temperature [°C]</returns>
        public static double DryBulbTemperature(double enthalpy, double humidityRatio)
        {
            if(double.IsNaN(enthalpy) || double.IsNaN(humidityRatio))
            {
                return double.NaN;
            }

            double result = -25;
            double enthalpy_Temp = double.NaN;
            do
            {
                result += 1;
                enthalpy_Temp = 1.01 * result + humidityRatio * (2501 + 1.86 * result);
            }
            while (!double.IsNaN(enthalpy_Temp) && enthalpy_Temp <= enthalpy && result <= 120);

            int i = 0;
            do
            {
                result -= 0.001;
                enthalpy_Temp = 1.01 * result + humidityRatio * (2501 + 1.86 * result);
                i++;
            }
            while (!double.IsNaN(enthalpy_Temp) && enthalpy_Temp > enthalpy && i <= 10000);

            return result;
            //return ((enthalpy / 1000) - 2501 * humidityRatio) / (1.005 + humidityRatio * 1.86);
        }

        /// <summary>
        /// Calculates dry bulb temperature from enthalpy, pressure and relative humidity
        /// </summary>
        /// <param name="enthalpy">Moist air Enthalpy[J / kg]</param>
        /// <param name="relativeHumidity">Relative humidity [%]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Dry-bulb temperature [°C]</returns>
        public static double DryBulbTemperature_ByEnthalpy(double enthalpy, double relativeHumidity, double pressure)
        {
            if (double.IsNaN(enthalpy) || double.IsNaN(relativeHumidity) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            double result = 50;
            double enthalpy_Temp = double.NaN;
            do
            {
                result -= 0.1;
                enthalpy_Temp = Enthalpy_ByRelativeHumidity(result, relativeHumidity, pressure);
            } while (!double.IsNaN(enthalpy_Temp) && enthalpy_Temp > enthalpy && result > -20);

            do
            {
                result += 0.0005;
                enthalpy_Temp = Enthalpy_ByRelativeHumidity(result, relativeHumidity, pressure);
            } while (!double.IsNaN(enthalpy_Temp) && enthalpy_Temp <= enthalpy && result <= 50);

            if (result > 100)
            {
                return double.NaN;
            }

            return result;
        }

        /// <summary>
        /// Calculates dry bulb temperature from humidity ratio, relative humidity and pressure
        /// </summary>
        /// <param name="humidityRatio">Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <param name="relativeHumidity">Relative humidity [%]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Dry-bulb temperature [°C]</returns>
        public static double DryBulbTemperature_ByHumidityRatio(double humidityRatio, double relativeHumidity, double pressure)
        {
            double result = 50;
            double humidityRatio_Temp = double.NaN;
            do
            {
                result -= 0.1;
                humidityRatio_Temp = HumidityRatio(result, relativeHumidity, pressure);
            } while (!double.IsNaN(humidityRatio_Temp) && humidityRatio_Temp > humidityRatio && result > -20);

            do
            {
                result += 0.0005;
                humidityRatio_Temp = HumidityRatio(result, relativeHumidity, pressure);
            } while (!double.IsNaN(humidityRatio_Temp) && humidityRatio_Temp <= humidityRatio && result <= 50);

            if (result > 100)
            {
                return double.NaN;
            }

            return result;
        }

        /// <summary>
        /// Calculates dry bulb temperature from density, humidity ratio and pressure
        /// </summary>
        /// <param name="humidityRatio">Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <param name="density">Density [kg/m3]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Dry-bulb temperature [°C]</returns>
        public static double DryBulbTemperature_ByDensityAndHumidityRatio(double density, double humidityRatio, double pressure)
        {
            double result = 110;
            double density_Temp = double.NaN;
            do
            {
                result -= 10;
                density_Temp = Density_ByHumidityRatio(result, humidityRatio, pressure);
            } while (!double.IsNaN(density_Temp) && density_Temp > density && result > -20);

            do
            {
                result += 0.005;
                density_Temp = Density_ByHumidityRatio(result, humidityRatio, pressure);
            } while (!double.IsNaN(density_Temp) && density_Temp <= density && result <= 110);

            if (result > 100)
            {
                return double.NaN;
            }

            return result;
        }

        /// <summary>
        /// Calculates dry bulb temperature from humidity ratio, relative humidity and pressure
        /// </summary>
        /// <param name="relativeHumidity">Relative humidity [%]</param>
        /// <param name="density">Density [kg/m3]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Dry-bulb temperature [°C]</returns>
        public static double DryBulbTemperature_ByDensityAndRelativeHumidity(double density, double relativeHumidity, double pressure)
        {
            double result = 100;
            double density_Temp = double.NaN;
            do
            {
                result -= 10;
                density_Temp = Density(result, relativeHumidity, pressure);
            } while (!double.IsNaN(density_Temp) && density_Temp <= density && result > -20);

            do
            {
                result += 0.005;
                density_Temp = Density(result, relativeHumidity, pressure);
            } while (!double.IsNaN(density_Temp) && density_Temp > density && result <= 100);

            if (result > 100)
            {
                return double.NaN;
            }

            return result;
        }
    }
}
