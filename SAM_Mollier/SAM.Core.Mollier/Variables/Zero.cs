namespace SAM.Core.Mollier
{
    /// <summary>
    /// Fluid properties at constant pressure and temperature - fundamental values 
    /// 2. Luft 2.1 Grundlagen
    /// (Prof. Dr.-Ing. habil. Bernd Glück, Zustands- und Stoffwerte 2. überarbeitete und erweiterte Auflage Berlin: Verlag für Bauwesen 1991, ISBN 3-345-00487-9)
    /// </summary>
    public static class Zero
    {
        /// <summary>
        /// Latent Heat of Vapourization (Evaporation heat of water), enthalpy of vaporizarion (r0) - The specific heat of water condensation at temparture 0C [J/kg] de: Verfampfungswärme am Tripelpunkt
        /// (Prof. Dr.-Ing. habil. Bernd Glück, Zustands- und Stoffwerte 2. überarbeitete und erweiterte Auflage Berlin: Verlag für Bauwesen 1991, ISBN 3-345-00487-9 value 2.25)
        /// </summary>
        public const double VapourizationLatentHeat = 2501000;

        /// <summary>
        /// Specific Heat of water vapour at constant pressure and temperature  0C (cpw) [J/kg*K] de: Spezifische Wärmekapazität des Wasserdampfes(Mittelwert) cp W
        ///(Prof. Dr.-Ing. habil. Bernd Glück, Zustands- und Stoffwerte 2. überarbeitete und erweiterte Auflage Berlin: Verlag für Bauwesen 1991, ISBN 3-345-00487-9 value 2.26)
        /// </summary>
        public const double SpecificHeat_WaterVapour = 1860;  //1859

        /// <summary>
        /// Specific Heat of air vapour at constant pressure and temperature 0C (cp) [J/kg*K] de: spezifische Wärmekapazität der treckenen Luft cp L
        /// (Prof. Dr.-Ing. habil. Bernd Glück, Zustands- und Stoffwerte 2. überarbeitete und erweiterte Auflage Berlin: Verlag für Bauwesen 1991, ISBN 3-345-00487-9 value 2.24)
        /// </summary>
        public const double SpecificHeat_Air = 1010; //1006

        /// <summary>
        /// Specific Heat of water vapour at constant pressure and temperature 0C (cw) [J/kg*K] de: Spezifische Wärmekapazität des Wasser cw 
        /// (Prof. Dr.-Ing. habil. Bernd Glück, Zustands- und Stoffwerte 2. überarbeitete und erweiterte Auflage Berlin: Verlag für Bauwesen 1991, ISBN 3-345-00487-9 value 2.30)
        /// </summary>
        public const double SpecificHeat_Water = 4190;  //4219.9

        /// <summary>
        /// Specific Heat of ice at constant pressure and temperature 0C (c_ice) [J/kg*K] de: Spezifische Wärmekapazität des Eises cw fest
        /// (Prof. Dr.-Ing. habil. Bernd Glück, Zustands- und Stoffwerte 2. überarbeitete und erweiterte Auflage Berlin: Verlag für Bauwesen 1991, ISBN 3-345-00487-9 value 2.35)
        /// </summary>
        public const double SpecificHeat_Ice = 2090;

        /// <summary>
        /// Fusion Heat for Ice (r_ice)[J/kg] de: Schmelzwärme des Eises bei 0ºC rsch
        /// (Prof. Dr.-Ing. habil. Bernd Glück, Zustands- und Stoffwerte 2. überarbeitete und erweiterte Auflage Berlin: Verlag für Bauwesen 1991, ISBN 3-345-00487-9 value 2.34)
        /// </summary>
        public const double MeltingHeat_Ice = 334000;
    }

}