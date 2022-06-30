using Newtonsoft.Json.Linq;
using SAM.Core;
using System;

namespace SAM.Analytical.Mollier
{
    public class AirHandlingUnitResult : Result, IAnalyticalObject
    {
        public AirHandlingUnitResult(string name, string source, string reference)
            : base(name, source, reference)
        {

        }

        public AirHandlingUnitResult(Guid guid, string name, string source, string reference)
            : base(guid, name, source, reference)
        {

        }

        public AirHandlingUnitResult(AirHandlingUnitResult airHandlingUnitResult)
            : base(airHandlingUnitResult)
        {

        }

        public AirHandlingUnitResult(JObject jObject)
            : base(jObject)
        {
        }

        public override bool FromJObject(JObject jObject)
        {
            if (!base.FromJObject(jObject))
                return false;

            return true;
        }

        public override JObject ToJObject()
        {
            JObject jObject = base.ToJObject();
            if (jObject == null)
                return null;

            return jObject;
        }
    }
}