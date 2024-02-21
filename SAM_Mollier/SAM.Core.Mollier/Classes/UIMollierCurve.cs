using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace SAM.Core.Mollier
{
    public class UIMollierCurve : IMollierCurve, IUIMollierObject
    {
        private MollierCurve mollierCurve;

        private UIMollierAppearance uIMollierAppearance;

        public List<MollierPoint> MollierPoints
        {
            get
            {
                return mollierCurve?.MollierPoints;
            }
        }

        public UIMollierAppearance UIMollierAppearance
        {
            get
            {
                return uIMollierAppearance;
            }

            set
            {
                uIMollierAppearance = value;
            }
        }

        public ChartDataType ChartDataType
        {
            get
            {
                return mollierCurve == null ? ChartDataType.Undefined : mollierCurve.ChartDataType;
            }
        }

        public double Pressure
        {
            get
            {

                return mollierCurve == null ? double.NaN : mollierCurve.Pressure;
            }
        }

        public MollierCurve MollierCurve
        {
            get
            {
                return mollierCurve?.Clone();
            }
        }

        public MollierPoint Start
        {
            get
            {
                return mollierCurve?.Start;
            }
        }

        public MollierPoint End
        {
            get
            {
                return mollierCurve?.End;
            }
        }

        public UIMollierCurve(MollierCurve mollierCurve, Color color)
        {
            this.mollierCurve = mollierCurve;
            uIMollierAppearance = new UIMollierAppearance(color);
        }

        public UIMollierCurve(UIMollierCurve uIMollierCurve)
        {
            if(uIMollierCurve != null)
            {
                mollierCurve = uIMollierCurve.MollierCurve?.Clone();
                uIMollierAppearance = uIMollierCurve.uIMollierAppearance?.Clone();
            }
        }

        public UIMollierCurve(MollierCurve mollierCurve, UIMollierAppearance uIMollierAppearance)
        {
            this.mollierCurve = mollierCurve?.Clone();
            this.uIMollierAppearance = uIMollierAppearance?.Clone();
        }

        public UIMollierCurve(JObject jObject)
        {
            FromJObject(jObject);
        }

        public virtual bool FromJObject(JObject jObject)
        {
            if (jObject == null)
            {
                return false;
            }

            if (jObject.ContainsKey("MollierCurve"))
            {
                mollierCurve = Core.Query.IJSAMObject(jObject.Value<JObject>("MollierCurve")) as MollierCurve;
            }

            if (jObject.ContainsKey("UIMollierAppearance"))
            {
                uIMollierAppearance = Core.Query.IJSAMObject(jObject.Value<JObject>("UIMollierAppearance")) as UIMollierAppearance;
            }

            return true;
        }
        
        public virtual JObject ToJObject()
        {
            JObject jObject = new JObject();
            jObject.Add("_type", Core.Query.FullTypeName(this));

            if (mollierCurve != null)
            {
                jObject.Add("MollierCurve", mollierCurve.ToJObject());
            }

            if (uIMollierAppearance != null)
            {
                jObject.Add("UIMollierAppearance", uIMollierAppearance.ToJObject());
            }

            return jObject;
        }
    }
}
