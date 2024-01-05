namespace SAM.Core.Mollier
{
    /// <summary>
    /// Fluid properties at constant pressure and temperature 20C
    /// </summary>
    public static class Const20C
    {
        /// <summary>
        /// Latent Heat of Vapourization (Evaporation heat of water), enthalpy of vaporizarion (r0) - The specific heat of water condensation at temparture 20C [J/kg]
        /// </summary>
        public const double VapourizationLatentHeat = 2.450;
        /// <summary>
        /// Specific Heat of water vapour at constant pressure and temeprature 20C (cpw) [J/kg*K]
        /// </summary>
        public const double SpecificHeat_WaterVapour = 1.805;
        /// <summary>
        /// Specific Heat of air vapour at constant pressure and temeprature 20C (cp) [J/kg*K]
        /// </summary>
        public const double SpecificHeat_Air = 1.012;
        /// <summary>
        /// Specific Heat of water vapour at constant pressure and temeprature 20C (cw) [J/kg*K]
        /// </summary>
        public const double SpecificHeat_Water = 4.184;
    }

}