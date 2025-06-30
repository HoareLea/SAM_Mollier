
using System;
using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Query_WIP
    {
        public static class Zero
        {
            /// <summary>
            /// Specific heat capacity of dry air at constant pressure [kJ/kg·°C]
            /// </summary>
            public static double cp_air => 1.006;

            /// <summary>
            /// Specific heat capacity of water vapor at constant pressure [kJ/kg·°C]
            /// </summary>
            public static double cp_vapor => 1.86;

            /// <summary>
            /// Specific heat capacity of liquid water [kJ/kg·°C]
            /// </summary>
            public static double cp_liquid => 4.186;

            /// <summary>
            /// Latent heat of vaporization of water at approximately 0°C [kJ/kg]
            /// (varies slightly with temperature, typically 2501 kJ/kg at 0°C)
            /// </summary>
            public static double latentHeat => 2501.0;
        }

        public static double DiagramTemperature(double dryBulbTemperature, double humidityRatio, double pressure)
        {
            double W_sat_db = HumidityRatio(dryBulbTemperature, 100.0, pressure);

            // Unsaturated case - simply return dry bulb
            if (humidityRatio <= W_sat_db)
                return dryBulbTemperature;

            // Supersaturated case: find extended enthalpy path
            double h_initial = Enthalpy(dryBulbTemperature, humidityRatio, pressure);

            // Iterative solution (bisection) to find temperature at intersection with extended enthalpy line
            double T_lower = -100.0;
            double T_upper = dryBulbTemperature;
            double T_diag = dryBulbTemperature;

            const int maxIter = 50;
            const double tol = 1e-6;

            for (int i = 0; i < maxIter; i++)
            {
                T_diag = (T_lower + T_upper) / 2.0;
                double phaseTemperature = T_diag < 0 ? T_diag : dryBulbTemperature;
                double W_sat = HumidityRatio(T_diag, 100.0, pressure);

                //// Assume excess moisture leaves at diagram temperature (liquid or solid based on temperature)
                //double W_liquid_removed = humidityRatio - W_sat;

                //// Enthalpy at current guess T_diag, considering moisture removal
                //double h_current = Enthalpy(T_diag, W_sat, pressure)
                //    + W_liquid_removed * Zero.cp_liquid * T_diag;

                // Correct enthalpy balance: removing latent heat properly
                double h_current = Enthalpy(T_diag, W_sat, pressure)
                                   + (humidityRatio - W_sat) * Zero.latentHeat;

                double h_diff = h_current - h_initial;

                if (Math.Abs(h_diff) < tol)
                    break;

                if (h_diff > 0)
                    T_upper = T_diag;
                else
                    T_lower = T_diag;
            }

            return T_diag;
        }

        // Using existing SAM methods for consistency

        public static double Enthalpy(double T, double W, double pressure)
        {
            return Query.Enthalpy(T, W, pressure);
        }

        public static double HumidityRatio(double T, double relativeHumidity, double pressure)
        {
            double p_ws = Query.SaturationVapourPressure(T);
            double p_w = relativeHumidity / 100.0 * p_ws;
            return 0.621945 * p_w / (pressure - p_w);
        }


    }
}
