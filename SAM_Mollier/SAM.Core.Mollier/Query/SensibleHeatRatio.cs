namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates Sensible Heat Ratio (factor) (SHR) [0-1]
        /// </summary>
        /// <param name="sensibleGain">Sensible Gain [W]</param>
        /// <param name="latentGain">Latent Gain [W]</param>
        /// <returns>Sensible Heat Ratio (SHR) [-]</returns>
        public static double SensibleHeatRatio(double sensibleGain, double latentGain)
        {
            if (double.IsNaN(sensibleGain) || double.IsNaN(latentGain))
            {
                return double.NaN;
            }

            if(sensibleGain + latentGain == 0)
            {
                return double.NaN;
            }

            return  sensibleGain / (sensibleGain + latentGain);
        }

        /// <summary>
        /// Calculates Sensible Heat Ratio [-]
        /// </summary>
        /// <param name="specificHeat">Specific Heat [kJ/kg*C][</param>
        /// <param name="temperature_In">Temperature In [C]</param>
        /// <param name="temperature_Out">Temperature Out [C]</param>
        /// <param name="enthaply_In">Enthalpy In [kJ/kg]</param>
        /// <param name="enthalpy_Out">Enthalpy Out [kJ/kg]</param>
        /// <returns>Sensible Heat Ratio (SHR) [-]</returns>
        public static double SensibleHeatRatio(double specificHeat, double temperature_In, double temperature_Out, double enthaply_In, double enthalpy_Out)
        {
            if(double.IsNaN(specificHeat) || double.IsNaN(temperature_In) || double.IsNaN(temperature_Out) || double.IsNaN(enthaply_In) || double.IsNaN(enthalpy_Out))
            {
                return double.NaN;
            }

            return specificHeat * (temperature_Out - temperature_In) / (enthalpy_Out - enthaply_In);
        }
    }
}
