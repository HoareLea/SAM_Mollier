﻿using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public class HeatingProcess : MollierProcess
    {
        public override ChartDataType ChartDataType => ChartDataType.HeatingProcess;
        internal HeatingProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }
        public HeatingProcess(JObject jObject)
            :base(jObject)
        {

        }

        public HeatingProcess(HeatingProcess heatingProcess)
            : base(heatingProcess)
        {

        }
        public override bool FromJObject(JObject jObject)
        {
            if (!base.FromJObject(jObject))
            {
                return false;
            }

            return true;
        }

        public override JObject ToJObject()
        {
            JObject result = base.ToJObject();
            if (result == null)
            {
                return result;
            }

            return result;
        }
    }
}
