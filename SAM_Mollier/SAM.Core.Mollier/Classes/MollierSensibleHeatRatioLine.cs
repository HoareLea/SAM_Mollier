// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020–2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using System.Text.Json.Nodes;
namespace SAM.Core.Mollier
{
    public class MollierSensibleHeatRatioLine : MollierLine
    {
        private double sensibleHeatRatio;

        public override ChartDataType ChartDataType => ChartDataType.SensibleHeatRatio;

        public MollierSensibleHeatRatioLine(MollierPoint mollierPoint, double sensibleHeatRatio)
            : base(mollierPoint)
        {
            this.sensibleHeatRatio = sensibleHeatRatio;
        }

        public MollierSensibleHeatRatioLine(MollierSensibleHeatRatioLine mollierSensibleHeatRatioLine)
            :base(mollierSensibleHeatRatioLine)
        {
            if(mollierSensibleHeatRatioLine != null)
            {
                sensibleHeatRatio = mollierSensibleHeatRatioLine.sensibleHeatRatio;
            }
        }

        public double SensibleHeatRatio
        {
            get
            {
                return sensibleHeatRatio;
            }
        }

        public virtual bool FromJsonObject(JsonObject jObject)
        {
            bool result = base.FromJsonObject(jObject);
            if (!result)
            {
                return false;
            }

            if(jObject.ContainsKey("SensibleHeatRatio"))
            {
                sensibleHeatRatio = jObject["SensibleHeatRatio"]?.GetValue<double>() ?? default(double);
            }

            return result;
        }

        public virtual JsonObject ToJsonObject()
        {
            JsonObject result = base.ToJsonObject();
            if(result == null)
            {
                return result;
            }

            if(!double.IsNaN(sensibleHeatRatio))
            {
                result.Add("SensibleHeatRatio", sensibleHeatRatio);
            }

            return result;
        }
    }
}
