using System;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates Apparatus Dew Point (ADP)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>Apparatus Dew Point (ADP)</returns>
        public static MollierPoint ApparatusDewPoint(this MollierPoint start, MollierPoint end)
        {
            if (start == null || end == null)
            {
                return null;
            }

            Func<double, double> func = new Func<double, double>((double dryBulbTemperature_Temp) =>
            {
                MollierPoint mollierPoint_ADP = new MollierPoint(dryBulbTemperature_Temp, HumidityRatio(dryBulbTemperature_Temp, 100, start.Pressure), start.Pressure);

                MollierPoint mollierPoint_Project = mollierPoint_ADP.Project(start, end);

                return mollierPoint_Project.Enthalpy;
            });

            double temperature_Temp = 2.27275;

            double dryBulbTemperature_ADP = Core.Query.Calculate_ByDivision(func, end.Enthalpy, temperature_Temp, start.DryBulbTemperature);
            if (double.IsNaN(dryBulbTemperature_ADP))
            {
                dryBulbTemperature_ADP = Core.Query.Calculate_ByDivision(func, end.Enthalpy, -273.15, temperature_Temp);
            }

            if(double.IsNaN(dryBulbTemperature_ADP))
            {
                return null;
            }

            double humidityRatio_ADP = HumidityRatio(dryBulbTemperature_ADP, 100, start.Pressure);

            return new MollierPoint(dryBulbTemperature_ADP, humidityRatio_ADP, start.Pressure);
        }
    }
}
