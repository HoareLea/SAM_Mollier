// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates Pickup Temperature for given specific fan power, evaluated at the given air state.
        /// </summary>
        /// <remarks>
        /// The result is a temperature RISE, not an absolute temperature: add it to the inlet dry bulb
        /// temperature to obtain the outlet dry bulb temperature.
        /// </remarks>
        /// <param name="mollierPoint">Air state at which density and specific heat capacity are evaluated</param>
        /// <param name="sfp">Specific Fan Power [W/l/s]</param>
        /// <returns>Pickup Temperature rise [K]</returns>
        public static double PickupTemperature(this MollierPoint mollierPoint,  double sfp)
        {
            if(mollierPoint == null || double.IsNaN(sfp))
            {
                return double.NaN;
            }

            return sfp / (mollierPoint.Density() * SpecificHeatCapacity_Air(mollierPoint));
        }

        public static double PickupTemperature(double sfp)
        {
            if (double.IsNaN(sfp))
            {
                return double.NaN;
            }

            return PickupTemperature(sfp, 1.2, 1.005);
        }

        /// <summary>
        /// Calculates Pickup Temperature for given specific fan power (sfp), density and specific heat capacity.
        /// </summary>
        /// <remarks>
        /// The result is a temperature RISE, not an absolute temperature: add it to the inlet dry bulb
        /// temperature to obtain the outlet dry bulb temperature.
        /// </remarks>
        /// <param name="sfp">Specific Fan Power [W/l/s]</param>
        /// <param name="density">Moist Air Density ρ [kg_MoistAir/m3]</param>
        /// <param name="specificHeatCapacity">Specific Heat Capacity of Air [kJ/kgK]</param>
        /// <returns>Pickup Temperature rise [K]</returns>
        public static double PickupTemperature(double sfp, double density, double specificHeatCapacity)
        {
            if (double.IsNaN(sfp) || double.IsNaN(density) || double.IsNaN(specificHeatCapacity))
            {
                return double.NaN;
            }

            return sfp / density * specificHeatCapacity;
        }
    }
}