using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public class MixingProcess : MollierProcess
    {
        public override ChartDataType ChartDataType => ChartDataType.MixingProcess;
        private double efficiency = 1;
        internal MixingProcess(MollierPoint start, MollierPoint end, double efficiency = 1)
            : base(start, Query.EndByEfficiency(start, end, efficiency))
        {
            this.efficiency = efficiency;
        }

        public MixingProcess(JObject jObject)
            :base(jObject)
        {

        }

        public MixingProcess(MixingProcess mixingProcess)
            : base(mixingProcess)
        {

        }
        public override bool FromJObject(JObject jObject)
        {
            if (!base.FromJObject(jObject))
            {
                return false;
            }

            if (jObject.ContainsKey("Efficiency"))
            {
                efficiency = jObject.Value<double>("Efficiency");
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

            if (!double.IsNaN(efficiency))
            {
                result.Add("Efficiency", efficiency);
            }

            return result;
        }
    }
}
