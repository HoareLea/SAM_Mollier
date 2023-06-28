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
    }
}
