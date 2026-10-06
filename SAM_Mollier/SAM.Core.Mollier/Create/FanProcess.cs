// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
namespace SAM.Core.Mollier
{
    public static partial class Create
    {
        /// <summary>
        /// Creates FanProcess for given specific fan power. The fan power dissipated into the air stream raises
        /// its dry bulb temperature by the pickup temperature, which is ADDED to the inlet dry bulb temperature:
        /// End = Start + PickupTemperature(start, spf). Humidity ratio and pressure are unchanged by a fan.
        /// </summary>
        /// <param name="start">Inlet air state</param>
        /// <param name="spf">Specific Fan Power [W/l/s]</param>
        /// <returns>FanProcess with the outlet dry bulb temperature [C] raised by the pickup temperature [K], or null when start is null, spf is NaN or the pickup temperature cannot be calculated</returns>
        public static FanProcess FanProcess(this MollierPoint start, double spf)
        {
            if (start == null || double.IsNaN(spf))
            {
                return null;
            }

            HeatingProcess heatingProcess = HeatingProcess_ByTemperatureDifference(start, start.PickupTemperature(spf));
            if (heatingProcess == null)
            {
                return null;
            }

            return new FanProcess(start, heatingProcess.End);
        }

        public static FanProcess FanProcess_ByDryBulbTemperature(this MollierPoint start, double dryBulbTemperature)
        {
            if (start == null || double.IsNaN(dryBulbTemperature))
            {
                return null;
            }

            HeatingProcess heatingProcess = HeatingProcess_ByTemperatureDifference(start, System.Math.Abs(start.DryBulbTemperature - dryBulbTemperature));
            if(heatingProcess == null)
            {
                return null;
            }

            return new FanProcess(start, heatingProcess.End);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start">Dry Bulb Temperature [C]</param>
        /// <param name="spf">Specific Fan Power [W/l/s]</param>
        /// <param name="density">Moist Air Density ρ [kg_MoistAir/m3]</param>
        /// <param name="specificHeatCapacity">Specific Heat Capacity of Air [kJ/kgK]</param>
        /// <param name="efficiency">Process efficiency 0-1</param>
        /// <returns></returns>
        public static FanProcess FanProcess(this MollierPoint start, double spf, double density, double specificHeatCapacity)
        {
            if (start == null || double.IsNaN(spf))
            {
                return null;
            }

            return new FanProcess(start, new MollierPoint(start.DryBulbTemperature + Query.PickupTemperature(spf, density, specificHeatCapacity), start.HumidityRatio, start.Pressure));
        }
    }
}
