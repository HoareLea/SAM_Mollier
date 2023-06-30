namespace SAM.Core.Mollier
{
    /// <summary>
    /// Standard temperature and pressure (STP) are standard sets of conditions for experimental measurements to be established to allow comparisons to be made between different sets of data.
    /// </summary>
    public static class Standard
    {
        /// <summary>
        /// Standard Pressure [Pa].
        /// </summary>
        public const double Pressure = 101325;

        /// <summary>
        /// Standard Temperature [C].
        /// </summary>
        public const double Temperature = 20;

        /// <summary>
        /// Specific Heat of water vapour at constant pressure and temeprature 20C [J\kg*K]
        /// </summary>
        public const double SpecificHeat_Water = 4184.4;
    }
}