using Newtonsoft.Json.Linq;
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

        public MollierCurve(JObject jObject)
        {
            FromJObject(jObject);
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

        public virtual bool FromJObject(JObject jObject)
        {
            if (jObject == null)
            {
                return false;
            }

            if(jObject.ContainsKey("MollierPoints"))
            {
                JArray jArray = jObject.Value<JArray>("MollierPoints");
                if(jArray != null)
                {
                    mollierPoints = new List<MollierPoint>();
                    foreach(JObject jObject_MollierPoint in jArray)
                    {
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
                foreach(MollierPoint mollierPoint in mollierPoints)
                {
                    if(mollierPoint == null)
                    {
                        continue;
                    }

                    jArray.Add(mollierPoint.ToJObject());
                }

                jObject.Add("MollierPoints", jArray);
            }

            return jObject;
        }
    }
}
