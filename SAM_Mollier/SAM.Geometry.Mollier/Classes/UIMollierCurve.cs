using Newtonsoft.Json.Linq;
using SAM.Core;
using SAM.Core.Mollier;
using System.Collections.Generic;
using System.Drawing;

namespace SAM.Geometry.Mollier
{
    public class UIMollierCurve : IMollierCurve, IUIMollierObject
    {
        private System.Guid guid = System.Guid.NewGuid();

        private MollierCurve mollierCurve;

        private UIMollierAppearance uIMollierAppearance;

        public List<MollierPoint> MollierPoints
        {
            get
            {
                return mollierCurve?.MollierPoints;
            }
        }

        public IUIMollierAppearance UIMollierAppearance
        {
            get
            {
                return uIMollierAppearance;
            }

            set
            {
                uIMollierAppearance = value as UIMollierAppearance;
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
                guid = uIMollierCurve.guid;
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

        public System.Guid Guid
        {
            get
            {
                return guid;
            }
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

            if (jObject.ContainsKey("Guid"))
            {
                guid = Core.Query.Guid(jObject, "Guid");
            }

            return true;
        }
        
        public virtual JObject ToJObject()
        {
            JObject result = new JObject();
            result.Add("_type", Core.Query.FullTypeName(this));

            if (mollierCurve != null)
            {
                result.Add("MollierCurve", mollierCurve.ToJObject());
            }

            if (uIMollierAppearance != null)
            {
                result.Add("UIMollierAppearance", uIMollierAppearance.ToJObject());
            }

            if (guid != System.Guid.Empty)
            {
                result.Add("Guid", guid);
            }

            return result;
        }
    }
}
