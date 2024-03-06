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

            return  sensibleGain / (System.Math.Abs(sensibleGain) + System.Math.Abs(latentGain));
        }

        /// <summary>
        /// Calculates Sensible Heat Ratio [-]
        /// </summary>
        /// <param name="specificHeat">Specific Heat [J/kg*K]</param>
        /// <param name="temperature_In">Temperature In [C]</param>
        /// <param name="temperature_Out">Temperature Out [C]</param>
        /// <param name="enthaply_In">Enthalpy In [J/kg]</param>
        /// <param name="enthalpy_Out">Enthalpy Out [J/kg]</param>
        /// <returns>Sensible Heat Ratio (SHR) [-]</returns>
        public static double SensibleHeatRatio(double specificHeat, double temperature_In, double temperature_Out, double enthaply_In, double enthalpy_Out)
        {
            if(double.IsNaN(specificHeat) || double.IsNaN(temperature_In) || double.IsNaN(temperature_Out) || double.IsNaN(enthaply_In) || double.IsNaN(enthalpy_Out))
            {
                return double.NaN;
            }

            return specificHeat * (temperature_Out - temperature_In) / (enthalpy_Out - enthaply_In);
        }

        public static double SensibleHeatRatio(this MollierPoint mollierPoint_1, MollierPoint mollierPoint_2)
        {
            if(mollierPoint_1 == null || mollierPoint_2 == null)
            {
                return double.NaN;
            }

            MollierPoint mollierPoint_1_Temp = mollierPoint_1;
            MollierPoint mollierPoint_2_Temp = mollierPoint_2;
            if(mollierPoint_1.DryBulbTemperature > mollierPoint_2.DryBulbTemperature)
            {
                mollierPoint_1_Temp = mollierPoint_2;
                mollierPoint_2_Temp = mollierPoint_1;
            }

            double sensibleLoad = SensibleLoad(mollierPoint_1_Temp, mollierPoint_2_Temp.DryBulbTemperature - mollierPoint_1_Temp.DryBulbTemperature, 1);
            if(double.IsNaN(sensibleLoad))
            {
                return double.NaN;
            }

            double latentLoad = LatentLoad_ByMassFlow((mollierPoint_2_Temp.HumidityRatio - mollierPoint_1_Temp.HumidityRatio) , 1 * mollierPoint_1_Temp.Density());
            if (double.IsNaN(latentLoad))
            {
                return double.NaN;
            }

            latentLoad *= 1000;

            if (latentLoad == 0 && sensibleLoad == 0)
            {
                return 0;
            }

            return SensibleHeatRatio(sensibleLoad, latentLoad);
        }

        public static double SensibleHeatRatio(this MollierProcess mollierProcess)
        {
            if (mollierProcess == null)
            {
                return double.NaN;
            }

            return SensibleHeatRatio(mollierProcess.Start, mollierProcess.End);

        }
    }
}
