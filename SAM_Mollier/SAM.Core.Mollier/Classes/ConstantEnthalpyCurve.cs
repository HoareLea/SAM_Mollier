using Newtonsoft.Json.Linq;
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

        public ConstantEnthalpyCurve(JObject jObject)
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

        public virtual bool FromJObject(JObject jObject)
        {
            bool result = FromJObject(jObject);
            if(!result)
            {
                return result;
            }

            if (jObject.ContainsKey("Phase"))
            {
                phase = Core.Query.Enum<Phase>(jObject.Value<string>("Phase"));
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

            result.Add("Phase", phase.ToString());

            return result;
        }
    }
}
