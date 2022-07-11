using System.Collections.Generic;

namespace SAM.Analytical.Mollier
{
    public static partial class Query
    {
        public static double SaturatedAirPolynominalEquation(double pressure)
        {
            if(double.IsNaN(pressure))
            {
                return double.NaN;
            }

            double temperature_Min = -20;
            double temperature_Max = 50;

            double step = 1;

            double temperature = temperature_Min;

            while(temperature < temperature_Max)
            {
                double humidityRatio = Core.Mollier.Query.HumidityRatio(temperature, 100, pressure);
                temperature += step;
            }

            throw new System.NotImplementedException();


            //SAM.Math.PolynomialEquation polynomialEquation = new Math.
        }
    }
}
