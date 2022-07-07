using Newtonsoft.Json.Linq;
using System;

namespace SAM.Core.Mollier
{
    public abstract class MollierProcess : IMollierProcess
    {
        private MollierPoint start;
        private MollierPoint end;

        internal MollierProcess(MollierPoint start, MollierPoint end)
        {
            this.start = start == null ? null : new MollierPoint(start);
            this.end = end == null ? null : new MollierPoint(end);
        }

        public MollierProcess(IMollierProcess mollierProcess)
        {
            if(mollierProcess != null)
            {
                start = mollierProcess.Start;
                start = start == null ? null : new MollierPoint(start);


                end = mollierProcess.End;
                end = end == null ? null : new MollierPoint(end);
            }
        }

        public MollierProcess(JObject jObject)
        {
            FromJObject(jObject);
        }

        public MollierPoint Start
        {
            get
            {
                return start == null ? null : new MollierPoint(start);
            }
        }

        public MollierPoint End
        {
            get
            {
                return end == null ? null : new MollierPoint(end);
            }
        }

        public bool FromJObject(JObject jObject)
        {
            if (jObject == null)
            {
                return false;
            }

            start = jObject.ContainsKey("Start") ? new MollierPoint( jObject.Value<JObject>("Start")): null;
            end = jObject.ContainsKey("End") ? new MollierPoint(jObject.Value<JObject>("End")) : null;

            return true;
        }

        public JObject ToJObject()
        {
            JObject jObject = new JObject();
            jObject.Add("_type", Core.Query.FullTypeName(this));

            if (start != null)
            {
                jObject.Add("Start", start.ToJObject());
            }

            if (end != null)
            {
                jObject.Add("End", end.ToJObject());
            }

            return jObject;
        }
    }
}
