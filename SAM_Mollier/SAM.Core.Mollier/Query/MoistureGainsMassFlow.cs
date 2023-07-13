namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates Moisture Gains Mass Flow Ẇ [kg/s]
        /// </summary>
        /// <param name="latentLoad">Latent Heat Gains Qlat [W]</param>
        /// <param name="vapourizationLatentHeat">Latent Heat of Vapourization r0 [J/kg]</param>
        /// <returns>Moisture Gains Mass Flow Ẇ [kg/s]</returns>
        public static double MoistureGainsMassFlow(double latentLoad, double vapourizationLatentHeat)
        {
            if (double.IsNaN(latentLoad) || double.IsNaN(vapourizationLatentHeat))
            {
                return double.NaN;
            }

            //vapourizationLatentHeat = Zero.VapourizationLatentHeat / 1000;
            double specificHeat_WaterVapour = Zero.SpecificHeat_WaterVapour / 1000;

            return latentLoad / vapourizationLatentHeat;
        }

    }
}
