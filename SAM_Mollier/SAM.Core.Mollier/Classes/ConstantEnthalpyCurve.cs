using System.Text.Json.Nodes;
using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public class ConstantEnthalpyCurve : ConstantValueCurve
    {
        private Phase phase;

        internal ConstantEnthalpyCurve(Phase phase, double value, params MollierPoint[] mollierPoints)
            :base(ChartDataType.Enthalpy, value, mollierPoints)
        {
            this.phase = phase;
        }

        internal ConstantEnthalpyCurve(Phase phase, double value, IEnumerable<MollierPoint> mollierPoints)
            : base(ChartDataType.Enthalpy, value, mollierPoints)
        {
            this.phase = phase;
        }

        public ConstantEnthalpyCurve(ConstantEnthalpyCurve constantEnthalpyCurve)
            : base(constantEnthalpyCurve)
        {
            if(constantEnthalpyCurve != null)
            {
                phase = constantEnthalpyCurve.phase;
            }
        }

        public ConstantEnthalpyCurve(JsonObject jObject)
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
