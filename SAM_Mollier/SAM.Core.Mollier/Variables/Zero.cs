namespace SAM.Core.Mollier
{
    /// <summary>
    /// Fluid properties at constant pressure and temperature 0C
    /// </summary>
    public static class Zero
    {
        /// <summary>
        /// Latent Heat of Vapourization (Evaporation heat of water), enthalpy of vaporizarion (r0) - The specific heat of water condensation at temparture 0C [J/kg]
        /// </summary>
        public const double VapourizationLatentHeat = 2501000;

        /// <summary>
        /// Specific Heat of water vapour at constant pressure and temeprature 0C [J/kg*K]
        /// </summary>
        public const double SpecificHeat_WaterVapour = 1860;  //1859

        /// <summary>
        /// Specific Heat of air vapour at constant pressure and temeprature 0C [J/kg*K]
        /// </summary>
        public const double SpecificHeat_Air = 1005; //1006

        /// <summary>
        /// Specific Heat of water vapour at constant pressure and temeprature 0C [J/kg*K]
        /// </summary>
        public const double SpecificHeat_Water = 4190;  //4219.9

        /// <summary>
        /// Specific Heat of ice at constant pressure and temeprature 0C [J/kg*K]
        /// </summary>
        public const double SpecificHeat_Ice = 2090;

        /// <summary>
        /// Fusion Heat for Ice [J/kg]
        /// </summary>
        public const double MeltingHeat_Ice = 334000;
    }

}