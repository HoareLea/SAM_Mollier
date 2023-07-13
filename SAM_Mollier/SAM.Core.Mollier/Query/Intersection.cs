namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static bool Intersection(double humidityRatio_Start_1, double dryBulbTemperature_Start_1, double humidityRatio_End_1, double dryBulbTemperature_End_1, double humidityRatio_Start_2, double dryBulbTemperature_Start_2, double humidityRatio_End_2, double dryBulbTemperature_End_2, out double dryBulbTemperature, out double humidityRatio)
        {
            dryBulbTemperature = double.NaN;
            humidityRatio = double.NaN;

            //x => humidityRatio
            //y => dryBulbTemperature

            //Line 1
            double a1 = dryBulbTemperature_End_1 - dryBulbTemperature_Start_1;
            double b1 = humidityRatio_Start_1 - humidityRatio_End_1;
            double c1 = a1 * humidityRatio_Start_1 + b1 * dryBulbTemperature_Start_1;

            //Line 2
            double a2 = dryBulbTemperature_End_2 - dryBulbTemperature_Start_2;
            double b2 = humidityRatio_Start_2 - humidityRatio_End_2;
            double c2 = a2 * humidityRatio_Start_2 + b2 * dryBulbTemperature_Start_2;

            double det = a1 * b2 - a2 * b1;
            if (det == 0)
            {
                return false;
            }
            humidityRatio = (b2 * c1 - c2 * b1) / det;
            dryBulbTemperature = (a1 * c2 - a2 * c1) / det;

            return true;
        }

        public static bool Intersection(MollierPoint start_1, MollierPoint end_1, MollierPoint start_2, MollierPoint end_2, out MollierPoint intersection)
        {
            intersection = null;

            if(start_1 == null || end_1 == null || start_2 == null || end_2 == null)
            {
                return false;
            }

            if(!Intersection(start_1.HumidityRatio, start_1.DryBulbTemperature, end_1.HumidityRatio, end_1.DryBulbTemperature, start_2.HumidityRatio, start_2.DryBulbTemperature, end_2.HumidityRatio, end_2.DryBulbTemperature, out double dryBulbTemperature, out double humidityRatio))
            {
                return false;
            }

            if(double.IsNaN(humidityRatio) || double.IsNaN(dryBulbTemperature))
            {
                return false;
            }

            intersection = new MollierPoint(dryBulbTemperature, humidityRatio, start_1.Pressure);
            return intersection.IsValid();
        }

        public static bool Intersection(MollierPoint start_1, MollierPoint end_1, MollierPoint start_2, MollierPoint end_2, bool bounded, out MollierPoint intersection, double tolerance = Tolerance.Distance)
        {
            intersection = null;
            if(!Intersection(start_1, end_1, start_2, end_2, out intersection))
            {
                return false;
            }

            if(!bounded)
            {
                return true;
            }

            return On(start_1, end_1, intersection, tolerance) && On(start_2, end_2, intersection, tolerance);
        }
    }
}
