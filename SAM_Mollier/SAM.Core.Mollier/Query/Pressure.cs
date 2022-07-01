namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates air pressure from elevation
        /// </summary>
        /// <param name="elevation">Elevation [m]</param>
        /// <returns>Pressure [Pa]</returns>
        public static double Pressure(double elevation)
        {
            return 101325 * System.Math.Pow((1 - 0.0000225577 * elevation), 5.2559);
        }
    }
}
