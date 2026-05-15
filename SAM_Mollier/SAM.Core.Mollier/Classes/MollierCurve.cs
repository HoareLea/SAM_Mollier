using System.Text.Json.Nodes;
using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public abstract class MollierCurve : IMollierCurve
    {
        protected List<MollierPoint> mollierPoints;

        internal MollierCurve(params MollierPoint[] mollierPoints)
        {
            if (mollierPoints != null)
            {
                this.mollierPoints = new List<MollierPoint>();

                foreach (MollierPoint mollierPoint in mollierPoints)
                {
                    MollierPoint mollierPoint_Temp = mollierPoint == null ? null : new MollierPoint(mollierPoint);

                    this.mollierPoints.Add(mollierPoint_Temp);
                }
            }
        }

        internal MollierCurve(IEnumerable<MollierPoint> mollierPoints)
        {
            if (mollierPoints != null)
            {
                this.mollierPoints = new List<MollierPoint>();

                foreach (MollierPoint mollierPoint in mollierPoints)
                {
                    MollierPoint mollierPoint_Temp = mollierPoint == null ? null : new MollierPoint(mollierPoint);

                    this.mollierPoints.Add(mollierPoint_Temp);
                }
            }
        }

        public MollierCurve(MollierCurve mollierCurve)
        {
            if (mollierCurve?.mollierPoints != null)
            {
                mollierPoints = new List<MollierPoint>();

                foreach (MollierPoint mollierPoint in mollierCurve.mollierPoints)
                {
                    MollierPoint mollierPoint_Temp = mollierPoint == null ? null : new MollierPoint(mollierPoint);

                    mollierPoints.Add(mollierPoint_Temp);
                }
            }
        }

        public MollierCurve(JsonObject jObject)
        {
            FromJsonObject(jObject);
        }

        public MollierPoint Start
        {
            get
            {
                return mollierPoints == null || mollierPoints.Count == 0 ? null : new MollierPoint(mollierPoints[0]);
            }
        }

        public MollierPoint End
        {
            get
            {
                return mollierPoints == null || mollierPoints.Count == 0 ? null : new MollierPoint(mollierPoints[mollierPoints.Count - 1]);
            }
        }

        public double Pressure
        {
            get
            {
                return mollierPoints == null || mollierPoints.Count == 0 || mollierPoints[0] == null ? double.NaN : mollierPoints[0].Pressure;
            }
        }

        public abstract ChartDataType ChartDataType { get; }

        public List<MollierPoint> MollierPoints
        {
            get
            {
                return mollierPoints == null ? null : mollierPoints?.ConvertAll(x => x == null ? null : new MollierPoint(x));
            }
        }

        public virtual bool FromJsonObject(JsonObject jObject)
        {
            if (jObject == null)
            {
                return false;
            }

            if(jObject.ContainsKey("MollierPoints"))
            {
                JsonArray jArray = jObject["MollierPoints"] as JsonArray;
                if(jArray != null)
                {
                    mollierPoints = new List<MollierPoint>();
                    foreach(JsonNode jsonNode_MollierPoint in jArray)
                    {
                        if (!(jsonNode_MollierPoint is JsonObject jObject_MollierPoint))
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
                foreach(MollierPoint mollierPoint in mollierPoints)
                {
                    if(mollierPoint == null)
                    {
                        continue;
                    }

                    jArray.Add(mollierPoint.ToJsonObject());
                }

                jObject.Add("MollierPoints", jArray);
            }

            return jObject;
        }
    }
}
