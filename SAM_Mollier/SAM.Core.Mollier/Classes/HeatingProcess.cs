using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public class HeatingProcess : MollierProcess
    {
        public override ChartDataType ChartDataType => ChartDataType.HeatingProcess;
        private double efficiency = 1;
        internal HeatingProcess(MollierPoint start, MollierPoint end, double efficiency = 1)
            : base(start, Query.EndByEfficiency(start, end, efficiency))
        {
            this.efficiency = efficiency;
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
