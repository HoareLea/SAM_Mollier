using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public class MollierZone : IJSAMObject
    {
        private List<MollierPoint> mollierPoints;

        public MollierZone(IEnumerable<MollierPoint> mollierPoints)
        {
            if (mollierPoints != null)
            {
                this.mollierPoints = new List<MollierPoint>();
                foreach(MollierPoint mollierPoint in mollierPoints)
                {
                    this.mollierPoints.Add(mollierPoint.Clone());
                }
            }
        }

        public MollierZone(MollierZone mollierZone)
        {
            //mollierPoints = mollierZone?.mollierPoints?.ConvertAll(x => new MollierPoint(x));
            mollierPoints = new List<MollierPoint>();
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

        public MollierPoint GetCenter()
        {
            if (mollierPoints == null || mollierPoints.Count == 0)
            {
                return null;
            }

            double dryBulbTemperature = 0, humidityRatio = 0;
            foreach(MollierPoint mollierPoint in mollierPoints)
            {
                dryBulbTemperature += mollierPoint.DryBulbTemperature;
                humidityRatio += mollierPoint.HumidityRatio;
            }
            dryBulbTemperature /= mollierPoints.Count;
            humidityRatio /= mollierPoints.Count;

            return new MollierPoint(dryBulbTemperature, humidityRatio, mollierPoints[0].Pressure);
        }

        public List<MollierPoint> MollierPoints
        {
            get
            {
                return mollierPoints?.ConvertAll(x => new MollierPoint(x));
            }
        }

        public virtual bool FromJObject(JObject jObject)
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

        public virtual JObject ToJObject()
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

