using System.Text.Json.Nodes;
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

        public ConstantValueCurve(JsonObject jObject)
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

        public virtual bool FromJsonObject(JsonObject jObject)
        {
            bool result = FromJsonObject(jObject);
            if(!result)
            {
                return result;
            }

            if(jObject.ContainsKey("Value"))
            {
                value = jObject["Value"]?.GetValue<double>() ?? default(double);
            }

            if (jObject.ContainsKey("ChartDataType"))
            {
                chartDataType = Core.Query.Enum<ChartDataType>(jObject["ChartDataType"]?.GetValue<string>() ?? null);
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

            if(!double.IsNaN(value))
            {
                result.Add("Value", value);
            }

            result.Add("ChartDataType", chartDataType.ToString());

            return result;
        }
    }
}
