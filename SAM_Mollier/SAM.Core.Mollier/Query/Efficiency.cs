namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Process Efficiency in percent [%]
        /// </summary>
        /// <param name="dryBulbTemperature_1">Start Dry Bulb Temperature</param>
        /// <param name="dryBulbTemperature_2">End Dry Bulb Temperature</param>
        /// <param name="supplyTemperature">Supply Temperature</param>
        /// <param name="returnTemperature">Return Temperature</param>
        /// <param name="tolerance">Rounding Tolerance</param>
        /// <returns>Efficiency [%]</returns>
        public static double Efficiency(double dryBulbTemperature_1, double dryBulbTemperature_2, double supplyTemperature, double returnTemperature, double tolerance = Tolerance.MacroDistance)
        {
            if(double.IsNaN(dryBulbTemperature_1) || double.IsNaN(dryBulbTemperature_2) || double.IsNaN(supplyTemperature) || double.IsNaN(returnTemperature))
            {
                return double.NaN;
            }

            double meanTemperature = (supplyTemperature + returnTemperature) / 2;

            double result = Core.Query.AlmostEqual(System.Math.Abs(dryBulbTemperature_1 - meanTemperature), 0) ? 0 : System.Math.Abs((dryBulbTemperature_1 - dryBulbTemperature_2) / (dryBulbTemperature_1 - meanTemperature));
            result = Core.Query.Round(result, tolerance) * 100;

            return result;
        }
    }
}
