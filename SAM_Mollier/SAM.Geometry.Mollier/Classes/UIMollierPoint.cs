using Newtonsoft.Json.Linq;
using SAM.Core;
using SAM.Core.Mollier;
using System.Drawing;

namespace SAM.Geometry.Mollier
{
    public class UIMollierPoint : MollierPoint, IUIMollierObject
    {
        private System.Guid guid = System.Guid.NewGuid();

        private UIMollierPointAppearance uIMollierPointAppearance;

        public IUIMollierAppearance UIMollierAppearance
        {
            get
            {
                return uIMollierPointAppearance;
            }

            set
            {
                if(value == null)
                {
                    uIMollierPointAppearance = null;
                }
                else if(value is UIMollierPointAppearance)
                {
                    uIMollierPointAppearance = new UIMollierPointAppearance((UIMollierPointAppearance)value);
                }
                else if (value is UIMollierAppearance)
                {
                    if(uIMollierPointAppearance == null)
                    {
                        uIMollierPointAppearance = new UIMollierPointAppearance();
                    }

                    uIMollierPointAppearance = new UIMollierPointAppearance((UIMollierAppearance)value, uIMollierPointAppearance.BorderSize, uIMollierPointAppearance.BorderColor);
                }
            }
        }

        public UIMollierPoint(MollierPoint mollierPoint, Color color, Color labelColor, string label)
            : base(mollierPoint)
        {
            uIMollierPointAppearance = new UIMollierPointAppearance(color, labelColor, label);
        }

        public UIMollierPoint(MollierPoint mollierPoint, Color color)
            : base(mollierPoint)
        {

            uIMollierPointAppearance = new UIMollierPointAppearance(color);
        }

        public UIMollierPoint(MollierPoint mollierPoint, UIMollierPointAppearance uIMollierPointAppearance)
            : base(mollierPoint)
        {
            this.uIMollierPointAppearance = uIMollierPointAppearance?.Clone();
        }

        public UIMollierPoint(MollierPoint mollierPoint)
            : base(mollierPoint)
        {
            uIMollierPointAppearance = new UIMollierPointAppearance();
        }   

        public UIMollierPoint(UIMollierPoint uIMollierPoint)
            : base(uIMollierPoint)
        {
            if(uIMollierPoint != null)
            {
                uIMollierPointAppearance = uIMollierPoint.uIMollierPointAppearance?.Clone();
                guid = uIMollierPoint.guid;
            }
        }

        public UIMollierPoint(JObject jObject)
            : base(jObject)
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

        public bool FromJObject(JObject jObject)
        {
            if (jObject == null || !base.FromJObject(jObject))
            {
                return false;
            }

            if (jObject.ContainsKey("UIMollierPointAppearance"))
            {
                uIMollierPointAppearance = new UIMollierPointAppearance(jObject.Value<JObject>("UIMollierPointAppearance"));
            }

            if (jObject.ContainsKey("Guid"))
            {
                guid = Core.Query.Guid(jObject, "Guid");
            }

            return true;
        }
        
        public JObject ToJObject()
        {
            JObject result = base.ToJObject();
            if (result == null)
            {
                return null;
            }

            if (uIMollierPointAppearance != null)
            {
                result.Add("UIMollierPointAppearance", uIMollierPointAppearance.ToJObject());
            }

            if (guid != System.Guid.Empty)
            {
                result.Add("Guid", guid);
            }

            return result;
        }
    }
}
