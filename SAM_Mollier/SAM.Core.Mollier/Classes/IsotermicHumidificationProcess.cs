// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020–2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using System.Text.Json.Nodes;
namespace SAM.Core.Mollier
{
    public class IsothermalHumidificationProcess : HumidificationProcess
    {
        public override ChartDataType ChartDataType => ChartDataType.IsothermalHumidificationProcess;

        internal IsothermalHumidificationProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public IsothermalHumidificationProcess(JsonObject jObject)
            :base(jObject)
        {

        }

        public IsothermalHumidificationProcess(IsothermalHumidificationProcess isothermalHumidificationProcess)
            : base(isothermalHumidificationProcess)
        {

        }
    }
}
