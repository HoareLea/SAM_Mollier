using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.Core.Mollier
{
    public class MollierZone : IJSAMObject
    {
        private List<MollierPoint> mollierPoints;
        public MollierZone(MollierZone mollierZone)
        {
            //mollierPoints = mollierZone?.mollierPoints?.ConvertAll(x => new MollierPoint(x));

            List<MollierPoint> points = mollierZone.mollierPoints;
            foreach(MollierPoint point in points)
            {
                mollierPoints.Add(new MollierPoint(point));
            }
        }
        public MollierZone(JObject jObject)
        {
            FromJObject(jObject);
        }

        public bool FromJObject(JObject jObject)
        {
            if(jObject == null)
            {
                return false;
            }
            if (jObject.ContainsKey("MollierPoints"))
            {
                mollierPoints = new List<MollierPoint>();

                //mollierPoints = Core.Create.IJSAMObjects<MollierPoint>(jObject.Value<JArray>("MollierPoints"));

                JArray jArray = jObject.Value<JArray>("MollierPoints");
                if(jArray != null)
                {
                    foreach (JObject jObject_MollierPoint in jArray)
                    {
                        if(jObject_MollierPoint == null)
                        {
                            continue;
                        }

                        mollierPoints.Add(new MollierPoint(jObject_MollierPoint));
                    }
                }

            }
            return true;
        }

        public JObject ToJObject()
        {
            JObject jObject = new JObject();
            jObject.Add("_type", Core.Query.FullTypeName(this));
            if(mollierPoints != null)
            {
                JArray jArray = new JArray();
                //mollierPoints.ForEach(x => jArray.Add(x.ToJObject()));
                foreach(MollierPoint point in mollierPoints)
                {
                    jArray.Add(point.ToJObject());
                }

                jObject.Add("MollierPoints", jArray);
            }

            return jObject;
        }
    }
}

