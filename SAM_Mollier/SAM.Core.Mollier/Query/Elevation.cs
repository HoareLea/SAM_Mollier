namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates elevation from pressure
        /// </summary>
        /// <param name="pressure">Pressure [Pa]</param>
        /// <returns>Elevation [m]</returns>
        public static double Elevation(double pressure, double tolerance = Tolerance.MacroDistance)
        {
            if(double.IsNaN(pressure))
            {
                return double.NaN;
            }

            return Core.Query.Calculate_BinarySearch(x => Pressure(x), pressure, -1000, 5000, tolerance: tolerance);
        }
    }
}
