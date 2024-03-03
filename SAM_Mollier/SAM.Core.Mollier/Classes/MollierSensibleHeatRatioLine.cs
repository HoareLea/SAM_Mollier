using Newtonsoft.Json.Linq;

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

        public virtual bool FromJObject(JObject jObject)
        {
            bool result = base.FromJObject(jObject);
            if (!result)
            {
                return false;
            }

            if(jObject.ContainsKey("SensibleHeatRatio"))
            {
                sensibleHeatRatio = jObject.Value<double>(sensibleHeatRatio);
            }

            return result;
        }

        public virtual JObject ToJObject()
        {
            JObject result = base.ToJObject();
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
