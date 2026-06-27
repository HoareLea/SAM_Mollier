// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020–2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using System.Text.Json.Nodes;
namespace SAM.Core.Mollier
{
    public class UndefinedProcess : MollierProcess
    {
        public override ChartDataType ChartDataType => ChartDataType.UndefinedProcess;
        internal UndefinedProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public UndefinedProcess(JsonObject jObject)
            :base(jObject)
        {

        }

        public UndefinedProcess(UndefinedProcess undefinedProcess)
            : base(undefinedProcess)
        {

        }
        public override bool FromJsonObject(JsonObject jObject)
        {
            if (!base.FromJsonObject(jObject))
            {
                return false;
            }

            return true;
        }

        public override JsonObject ToJsonObject()
        {
            JsonObject result = base.ToJsonObject();
            if (result == null)
            {
                return result;
            }

            return result;
        }
    }
}
