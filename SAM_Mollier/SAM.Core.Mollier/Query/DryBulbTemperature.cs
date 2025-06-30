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
        /// <param name="pressure">Pressure [Pa]</param>
        /// <returns>Dry-bulb temperature [°C]</returns>
        public static double DryBulbTemperature(double enthalpy, double humidityRatio, double pressure)
        {
            if (double.IsNaN(enthalpy) || double.IsNaN(humidityRatio) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            Func<double, double> func = new Func<double, double>((double temperature) =>
            {
                double saturationTemperature = SaturationTemperature(temperature, pressure);

                return Enthalpy(saturationTemperature, humidityRatio, pressure);
            });

            double vapourizationLatentHeat = Zero.VapourizationLatentHeat / 1000;
            double specificHeat_WaterVapour = Zero.SpecificHeat_WaterVapour / 1000;
            double specificHeat_Air = Zero.SpecificHeat_Air / 1000;

            double result = (enthalpy / 1000 - (humidityRatio * vapourizationLatentHeat)) / (specificHeat_Air + (humidityRatio * specificHeat_WaterVapour));

            if (humidityRatio == 0)
            {
                return result;
            }

            double saturationDryBulbTemperature = Core.Query.Calculate_ByMaxStep(func, enthalpy, -20, 15, out int count, out double enthalpy_Temp, 100, Tolerance.MacroDistance);
            if (!double.IsNaN(saturationDryBulbTemperature) || saturationDryBulbTemperature > result)
            {
                result = saturationDryBulbTemperature;
            }

            return result;
        }

        public static double DryBulbTemperature(double enthalpy, double humidityRatio, double pressure, double saturationHumidityRatio)
        {
            if (double.IsNaN(enthalpy) || double.IsNaN(humidityRatio) || double.IsNaN(pressure) || double.IsNaN(saturationHumidityRatio))
            {
                return double.NaN;
            }

            double vapourizationLatentHeat = Zero.VapourizationLatentHeat / 1000;
            double specificHeat_WaterVapour = Zero.SpecificHeat_WaterVapour / 1000;
            double specificHeat_Air = Zero.SpecificHeat_Air / 1000;
            double specificHeat_Water = Zero.SpecificHeat_Water / 1000;

            return (enthalpy / 1000 - (saturationHumidityRatio * vapourizationLatentHeat)) / (specificHeat_Air + (saturationHumidityRatio * specificHeat_WaterVapour) + (specificHeat_Water * (humidityRatio - saturationHumidityRatio)));
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

            //return Core.Query.Calculate((double x) => Enthalpy_ByRelativeHumidity(x, relativeHumidity, pressure), enthalpy, -50, 99.999);

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
        public static double DryBulbTemperature_ByHumidityRatio(double humidityRatio, double relativeHumidity, double pressure, bool allowRH100 = false)
        {
            if (double.IsNaN(humidityRatio) || double.IsNaN(relativeHumidity) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            return Core.Query.Calculate_ByMaxStep((double x) => HumidityRatio(x, relativeHumidity, pressure, allowRH100) * 1000, humidityRatio * 1000, -50, 50, 100, Tolerance.Distance);


            //OPTION 1
            //return Core.Query.Calculate((double x) => HumidityRatio(x, relativeHumidity, pressure), humidityRatio, -50, 99.999);


            //OPTION 2
            //double result = 50;
            //double humidityRatio_Temp = double.NaN;
            //do
            //{
            //    result -= 0.1;
            //    humidityRatio_Temp = HumidityRatio(result, relativeHumidity, pressure);
            //} while (!double.IsNaN(humidityRatio_Temp) && humidityRatio_Temp > humidityRatio && result > -20);

            //do
            //{
            //    result += 0.0005;
            //    humidityRatio_Temp = HumidityRatio(result, relativeHumidity, pressure);
            //} while (!double.IsNaN(humidityRatio_Temp) && humidityRatio_Temp <= humidityRatio && result <= 50);

            //if (result > 100)
            //{
            //    return double.NaN;
            //}

            //return result;
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
            if (double.IsNaN(humidityRatio) || double.IsNaN(density) || double.IsNaN(pressure))
            {
                return double.NaN;
            }


            return Core.Query.Calculate_BinarySearch((double x) => Density_ByHumidityRatio(x, humidityRatio, pressure), density, -50, 99.999);

            //double result = 110;
            //double density_Temp = double.NaN;
            //do
            //{
            //    result -= 10;
            //    density_Temp = Density_ByHumidityRatio(result, humidityRatio, pressure);
            //} while (!double.IsNaN(density_Temp) && density_Temp > density && result > -20);

            //do
            //{
            //    result += 0.005;
            //    density_Temp = Density_ByHumidityRatio(result, humidityRatio, pressure);
            //} while (!double.IsNaN(density_Temp) && density_Temp <= density && result <= 110);

            //if (result > 100)
            //{
            //    return double.NaN;
            //}

            //return result;
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
            if (double.IsNaN(relativeHumidity) || double.IsNaN(density) || double.IsNaN(pressure))
            {
                return double.NaN;
            }


            return Core.Query.Calculate_BinarySearch((double x) => Density(x, relativeHumidity, pressure), density, -50, 150, true, 100, Tolerance.Distance);

            //double result = 100;
            //double density_Temp = double.NaN;
            //do
            //{
            //    result -= 10;
            //    density_Temp = Density(result, relativeHumidity, pressure);
            //} while (!double.IsNaN(density_Temp) && density_Temp <= density && result > -20);

            //do
            //{
            //    result += 0.005;
            //    density_Temp = Density(result, relativeHumidity, pressure);
            //} while (!double.IsNaN(density_Temp) && density_Temp > density && result <= 100);

            //if (result > 100)
            //{
            //    return double.NaN;
            //}

            //return result;
        }

        /// <summary>
        /// Calculates dry bulb temperature from wet bulb temperature, relative humidity and pressure
        /// </summary>
        /// <param name="relativeHumidity">Relative humidity [%]</param>
        /// <param name="wetBulbTemperature">Wet Bulb Temperature [°C]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Dry-bulb temperature [°C]</returns>
        public static double DryBulbTemperature_ByWetBulbTemperature(double wetBulbTemperature, double relativeHumidity, double pressure)
        {
            if(double.IsNaN(wetBulbTemperature) || double.IsNaN(relativeHumidity) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            //return Core.Query.Calculate((double x) => WetBulbTemperature(x, relativeHumidity, pressure), wetBulbTemperature, 5, 95);
            return Core.Query.Calculate_BinarySearch((double x) => WetBulbTemperature(x, relativeHumidity, pressure), wetBulbTemperature, -20, 100, false, 100, 0.005);
        }

        /// <summary>
        /// Calculates dry bulb temperature from wet bulb temperature, humidity ratio and pressure
        /// </summary>
        /// <param name="humidityRatio">Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <param name="wetBulbTemperature">Wet Bulb Temperature [°C]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Dry-bulb temperature [°C]</returns>
        public static double DryBulbTemperature_ByWetBulbTemperatureAndHumidityRatio(double wetBulbTemperature, double humidityRatio, double pressure)
        {
            if (double.IsNaN(wetBulbTemperature) || double.IsNaN(humidityRatio) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            double pressureRatio = 0.621945 * SaturationVapourPressure(wetBulbTemperature) / (pressure - SaturationVapourPressure(wetBulbTemperature));


            return ((1093 - 0.556 * wetBulbTemperature) * pressureRatio + 0.240 * wetBulbTemperature - humidityRatio * (1093 - wetBulbTemperature)) / (0.444 * humidityRatio + 0.240);
        }

        /// <summary>
        /// Calculates dry bulb temperature from relative humidity, specific volume and pressure
        /// </summary>
        /// <param name="relativeHumidity">Relative humidity [%]</param>
        /// <param name="specificVolume">Specific Volume [m3/kg]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Dry-bulb temperature [°C]</returns>
        public static double DryBulbTemperature_ByRelativeHumidityAndSpecificVolume(double relativeHumidity, double specificVolume, double pressure)
        {
            if (double.IsNaN(relativeHumidity) || double.IsNaN(specificVolume) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            return  Core.Query.Calculate_ByMaxStep((double x) => SpecificVolume(x, HumidityRatio(x, relativeHumidity, pressure), pressure), specificVolume, -50, 50);
        }

        /// <summary>
        /// Calculates dry bulb temperature from relative humidity, specific volume and pressure
        /// </summary>
        /// <param name="humidityRatio">Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <param name="specificVolume">Specific Volume [m3/kg]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Dry-bulb temperature [°C]</returns>
        public static double DryBulbTemperature_ByHumidityRatioAndSpecificVolume(double humidityRatio, double specificVolume, double pressure)
        {
            if (double.IsNaN(humidityRatio) || double.IsNaN(specificVolume) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            return Core.Query.Calculate_ByMaxStep((double x) => SpecificVolume(x, humidityRatio, pressure), specificVolume, -50, 50);
        }

        public static double DryBulbTemperature_ByDiagramTemperature(double diagramTemperature, double humidityRatio, double pressure)
        {
            if (double.IsNaN(diagramTemperature))
            {
                return double.NaN;
            }

            return Core.Query.Calculate_BinarySearch((double x) => DiagramTemperature(x, humidityRatio, pressure), diagramTemperature, -30, 80, increasing: false);
        }

        /// <summary>
        /// Calculates dry bulb temperature
        /// </summary>
        /// <param name="mollierPoint">MollierPoint</param>
        /// <param name="sensibleLoad">Sensible Load [W]</param>
        /// <param name="airFlow">Air Flow [m3/s]</param>
        /// <returns></returns>
        public static double DryBulbTemperature(this MollierPoint mollierPoint, double sensibleLoad, double airFlow)
        {
            if (mollierPoint == null || double.IsNaN(sensibleLoad) || double.IsNaN(airFlow))
            {
                return double.NaN;
            }

            return mollierPoint.DryBulbTemperature + ((sensibleLoad / 1000) / (airFlow * mollierPoint.Density() * mollierPoint.SpecificHeatCapacity_Air()));
        }

        /// <summary>
        /// Calculates humidity ratio for end point for given senisble heat ratio and start point humidity ratio
        /// </summary>
        /// <param name="sensibleHeatRatio">Sensible Heat Ratio (SHR) [-] value from 0 to 1</param>
        /// <param name="specificHeat">Air Specific Heat [J/kg*K]</param>
        /// <param name="dryBulbTemperature_Start">Start Dry Bulb Tempearture [C]</param>
        /// <param name="humidityRatio_End">End Point Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <param name="humidityRatio_Start">Start Point Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <returns>End Point Humidity Ratio [kg_waterVapor/kg_dryAir]</returns>
        public static double DryBulbTemperature_BySensibleHeatRatioAndHumidityRatio(double sensibleHeatRatio, double specificHeat, double dryBulbTemperature_Start, double humidityRatio_End, double humidityRatio_Start)
        {
            if (double.IsNaN(sensibleHeatRatio) || double.IsNaN(specificHeat) || double.IsNaN(dryBulbTemperature_Start) || double.IsNaN(humidityRatio_End) || double.IsNaN(humidityRatio_Start))
            {
                return double.NaN;
            }

            double specificHeat_Air = specificHeat / 1000; //[kJ/kg*K]
            double specificHeat_WaterVapour = Zero.SpecificHeat_WaterVapour / 1000; //[kJ/kg*K]

            double vapourizationLatentHeat = Zero.VapourizationLatentHeat / 1000; //[kJ/kg]

            double divisor = sensibleHeatRatio * (specificHeat_Air + (specificHeat_WaterVapour * humidityRatio_End)) - specificHeat_Air;
            if (divisor == 0)
            {
                return double.NaN;
            }

            double dividend = (sensibleHeatRatio * ((dryBulbTemperature_Start * (specificHeat_Air + specificHeat_WaterVapour * humidityRatio_Start)) + (vapourizationLatentHeat * (humidityRatio_Start - humidityRatio_End)))) - (specificHeat_Air * dryBulbTemperature_Start);

            //return dividend / divisor;
            //return (humidityRatio_Start * specificHeat_WaterVapour * dryBulbTemperature_Start + humidityRatio_Start * vapourizationLatentHeat - 2 * specificHeat_Air * dryBulbTemperature_Start - humidityRatio_End * vapourizationLatentHeat) / (humidityRatio_End * specificHeat_WaterVapour - 2 * specificHeat_Air);
            //to = (cp * ti - SHR * xo * hwe + SHR * hi) / (SHR * cp - cp + SHR * xo * cpw)
            //return (-(sensibleHeatRatio * specificHeat_Air * dryBulbTemperature_Start - (sensibleHeatRatio * humidityRatio_Start - sensibleHeatRatio * humidityRatio_End) * specificHeat_WaterVapour * dryBulbTemperature_Start + (sensibleHeatRatio * humidityRatio_End - sensibleHeatRatio * humidityRatio_Start) * vapourizationLatentHeat)) / (sensibleHeatRatio * specificHeat_Air - specificHeat_Air);
            return (-specificHeat_Air * dryBulbTemperature_Start + sensibleHeatRatio * (specificHeat_Air * dryBulbTemperature_Start + humidityRatio_Start * specificHeat_WaterVapour * dryBulbTemperature_Start + humidityRatio_Start * vapourizationLatentHeat) + humidityRatio_End * vapourizationLatentHeat) / (sensibleHeatRatio * (specificHeat_Air + humidityRatio_End * specificHeat_WaterVapour) - specificHeat_Air);
        }

        public static double DryBulbTemperature_BySensibleHeatRatioAndHumidityRatio(double sensibleHeatRatio, double specificHeat, MollierPoint mollierPoint_Start, double humidityRatio_End)
        {
            if(mollierPoint_Start == null)
            {
                return double.NaN;
            }

            return DryBulbTemperature_BySensibleHeatRatioAndHumidityRatio(sensibleHeatRatio, specificHeat, mollierPoint_Start.DryBulbTemperature, humidityRatio_End, mollierPoint_Start.HumidityRatio);
        }
    }
}
