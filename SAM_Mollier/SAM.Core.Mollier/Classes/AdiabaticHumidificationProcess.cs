// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020–2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using System.Text.Json.Nodes;
namespace SAM.Core.Mollier
{
    public class AdiabaticHumidificationProcess : HumidificationProcess
    {
        public override ChartDataType ChartDataType => ChartDataType.AdiabaticHumidificationProcess;

        internal AdiabaticHumidificationProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public AdiabaticHumidificationProcess(JsonObject jObject)
            :base(jObject)
        {

        }

        public AdiabaticHumidificationProcess(AdiabaticHumidificationProcess adiabaticHumidificationProcess)
            : base(adiabaticHumidificationProcess)
        {

        }
    }
}
