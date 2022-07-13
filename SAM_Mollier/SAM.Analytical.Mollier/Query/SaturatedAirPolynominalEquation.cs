using System.Collections.Generic;

namespace SAM.Analytical.Mollier
{
    public static partial class Query
    {
        public static Math.PolynomialEquation SaturatedAirPolynominalEquation(double pressure, double temperature_Min = -20, double temperature_Max = 50, double step = 1.0)
        {
            if(double.IsNaN(pressure))
            {
                return null;
            }

            double temperature = temperature_Min;

            List<double> temperatures = new List<double>();
            List<double> humidityRatios = new List<double>();
            while(temperature < temperature_Max)
            {
                double humidityRatio = Core.Mollier.Query.HumidityRatio(temperature, 100, pressure);

                temperature += step;

                if (double.IsNaN(humidityRatio))
                {
                    continue;
                }

                temperatures.Add(temperature - 1);
                humidityRatios.Add(humidityRatio);
            }

            return Math.Create.PolynomialEquation(humidityRatios, temperatures, 6);
        }
    }
}
