namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static double WetBulbTemperature(double dryBulbTemperature, double relativeHumidity, double pressure)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(relativeHumidity) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            double enthalpy = Enthalpy_ByRelativeHumidity(dryBulbTemperature, relativeHumidity, pressure);
            if(double.IsNaN(enthalpy))
            {
                return double.NaN;
            }

            return Core.Query.Calculate_ByMaxStep((double x) => Enthalpy_ByRelativeHumidity(x, 100, pressure), enthalpy, -20, 50);


            //double enthalpy = Enthalpy_ByRelativeHumidity(dryBulbTemperature, relativeHumidity, pressure);

            double result = -4;
            double enthaply_Temp = double.NaN;
            do
            {
                result += 5;
                enthaply_Temp = Enthalpy_ByRelativeHumidity(result, 100, pressure);

            } while (!double.IsNaN(enthaply_Temp) && enthaply_Temp <= enthalpy);

            do
            {
                result -= 0.01;
                enthaply_Temp = Enthalpy_ByRelativeHumidity(result, 100, pressure);

            } while (!double.IsNaN(enthaply_Temp) && enthaply_Temp > enthalpy);

            return result;
        }

        public static double WetBulbTemperature(this MollierPoint mollierPoint)
        {
            if(mollierPoint == null)
            {
                return double.NaN;
            }

            return WetBulbTemperature(mollierPoint.DryBulbTemperature, mollierPoint.RelativeHumidity, mollierPoint.Pressure);
        }
    }
}
