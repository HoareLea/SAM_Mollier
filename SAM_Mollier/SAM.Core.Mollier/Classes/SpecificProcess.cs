// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020–2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using System.Text.Json.Nodes;
namespace SAM.Core.Mollier
{
    public abstract class SpecificProcess : MollierProcess
    {
        internal SpecificProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public SpecificProcess(JsonObject jObject)
            : base(jObject)
        {

        }

        public SpecificProcess(SpecificProcess specificMollierProcess)
            : base(specificMollierProcess)
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
