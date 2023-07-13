using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public class ConstantValueCurve : MollierCurve
    {
        private double value;
        private ChartDataType chartDataType;

        internal ConstantValueCurve(ChartDataType chartDataType, double value, params MollierPoint[] mollierPoints)
            :base(mollierPoints)
        {
            this.chartDataType = chartDataType;
            this.value = value;
        }

        internal ConstantValueCurve(ChartDataType chartDataType, double value, IEnumerable<MollierPoint> mollierPoints)
            : base(mollierPoints)
        {
            this.chartDataType = chartDataType;
            this.value = value;
        }

        public ConstantValueCurve(ConstantValueCurve constantValueCurve)
            : base(constantValueCurve)
        {
            if(constantValueCurve != null)
            {
                chartDataType = constantValueCurve.chartDataType;
                value = constantValueCurve.value;
            }
        }

        public ConstantValueCurve(JObject jObject)
            : base(jObject)
        {

        }

        public override ChartDataType ChartDataType
        {
            get
            {
                return chartDataType;
            }
        }

        public double Value
        {
            get
            {
                return value;
            }
        }

        public virtual bool FromJObject(JObject jObject)
        {
            bool result = FromJObject(jObject);
            if(!result)
            {
                return result;
            }

            if(jObject.ContainsKey("Value"))
            {
                value = jObject.Value<double>("Value");
            }

            if (jObject.ContainsKey("ChartDataType"))
            {
                chartDataType = Core.Query.Enum<ChartDataType>(jObject.Value<string>("ChartDataType"));
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

            if(!double.IsNaN(value))
            {
                result.Add("Value", value);
            }

            result.Add("ChartDataType", chartDataType.ToString());

            return result;
        }
    }
}
