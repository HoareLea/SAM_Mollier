// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020–2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using System.Text.Json.Nodes;
namespace SAM.Core.Mollier
{
    public class RoomProcess : SpecificProcess
    {
        public override ChartDataType ChartDataType => ChartDataType.RoomProcess;

        internal RoomProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public RoomProcess(JsonObject jObject)
            : base(jObject)
        {

        }

        public RoomProcess(RoomProcess roomProcess)
            : base(roomProcess)
        {

        }
    }
}
