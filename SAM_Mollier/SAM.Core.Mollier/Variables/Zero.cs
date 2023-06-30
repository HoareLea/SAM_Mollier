namespace SAM.Core.Mollier
{
    /// <summary>
    /// Fluid properties at constant pressure and temperature 0C
    /// </summary>
    public static class Zero
    {
        /// <summary>
        /// Latent Heat of Vapourization (Evaporation heat of water) - The specific heat of water condensation at temparture 0C [J/kg]
        /// </summary>
        public const double VapourizationLatentHeat = 2501000;

        /// <summary>
        /// Specific Heat of water vapour at constant pressure and temeprature 0C [J/kg*K]
        /// </summary>
        public const double SpecificHeat_WaterVapour = 1859;

        /// <summary>
        /// Specific Heat of air vapour at constant pressure and temeprature 0C [J/kg*K]
        /// </summary>
        public const double SpecificHeat_Air = 1006;

        /// <summary>
        /// Specific Heat of water vapour at constant pressure and temeprature 0C [J/kg*K]
        /// </summary>
        public const double SpecificHeat_Water = 4219.9;
    }

}