using System.Text.Json.Nodes;
using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public class ConstantTemperatureCurve : ConstantValueCurve
    {
        private Phase phase;

        internal ConstantTemperatureCurve(Phase phase, double value, params MollierPoint[] mollierPoints)
            :base(ChartDataType.DryBulbTemperature, value, mollierPoints)
        {
            this.phase = phase;
        }

        internal ConstantTemperatureCurve(Phase phase, double value, IEnumerable<MollierPoint> mollierPoints)
            : base(ChartDataType.DryBulbTemperature, value, mollierPoints)
        {
            this.phase = phase;
        }

        public ConstantTemperatureCurve(ConstantTemperatureCurve constantTemperatureCurve)
            : base(constantTemperatureCurve)
        {
            if(constantTemperatureCurve != null)
            {
                phase = constantTemperatureCurve.phase;
            }
        }

        public ConstantTemperatureCurve(JsonObject jObject)
            : base(jObject)
        {

        }

        public Phase Phase
        {
            get
            {
                return phase;
            }
        }

        public virtual bool FromJsonObject(JsonObject jObject)
        {
            bool result = FromJsonObject(jObject);
            if(!result)
            {
                return result;
            }

            if (jObject.ContainsKey("Phase"))
            {
                phase = Core.Query.Enum<Phase>(jObject["Phase"]?.GetValue<string>() ?? null);
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

            result.Add("Phase", phase.ToString());

            return result;
        }
    }
}
