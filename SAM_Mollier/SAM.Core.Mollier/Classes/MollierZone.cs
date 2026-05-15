using System.Text.Json.Nodes;
using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public class MollierZone : IMollierZone, IMollierGroupable
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
            mollierPoints = new List<MollierPoint>();
            List<MollierPoint> points = mollierZone.mollierPoints;
            foreach(MollierPoint point in points)
            {
                mollierPoints.Add(new MollierPoint(point));
            }
        }
        public MollierZone(JsonObject jObject)
        {
            FromJsonObject(jObject);
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

        public virtual bool FromJsonObject(JsonObject jObject)
        {
            if(jObject == null)
            {
                return false;
            }
            if (jObject.ContainsKey("MollierPoints"))
            {
                mollierPoints = new List<MollierPoint>();

                //mollierPoints = Core.Create.IJSAMObjects<MollierPoint>(jObject["MollierPoints"] as JsonArray);

                JsonArray jArray = jObject["MollierPoints"] as JsonArray;
                if(jArray != null)
                {
                    foreach (JsonNode jsonNode_MollierPoint in jArray)
                    {
                        if (!(jsonNode_MollierPoint is JsonObject jObject_MollierPoint))
                        {
                            continue;
                        }

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

        public virtual JsonObject ToJsonObject()
        {
            JsonObject jObject = new JsonObject();
            jObject.Add("_type", Core.Query.FullTypeName(this));
            if(mollierPoints != null)
            {
                JsonArray jArray = new JsonArray();
                //mollierPoints.ForEach(x => jArray.Add(x.ToJsonObject()));
                foreach(MollierPoint point in mollierPoints)
                {
                    jArray.Add(point.ToJsonObject());
                }

                jObject.Add("MollierPoints", jArray);
            }

            return jObject;
        }
    }
}

