// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020–2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using System.Text.Json.Nodes;
namespace SAM.Core.Mollier
{

    /// <summary>
    /// Pseudo isothermal humidification proces by Steam where the small rise in dry-bulb temperature is due to the sensible heating effect of the steam.
    /// </summary>
    public class SteamHumidificationProcess : IsothermalHumidificationProcess
    {
        internal SteamHumidificationProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public SteamHumidificationProcess(JsonObject jObject)
            :base(jObject)
        {

        }

        public SteamHumidificationProcess(SteamHumidificationProcess steamHumidificationProcess)
            : base(steamHumidificationProcess)
        {

        }
    }
}
