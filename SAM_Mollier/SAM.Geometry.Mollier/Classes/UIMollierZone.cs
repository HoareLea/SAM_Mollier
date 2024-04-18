using Newtonsoft.Json.Linq;
using SAM.Core;
using SAM.Core.Mollier;
using System.Drawing;

namespace SAM.Geometry.Mollier
{
    public class UIMollierZone : MollierZone, IUIMollierObject
    {
        private System.Guid guid = System.Guid.NewGuid();
        
        private UIMollierAppearance uIMollierAppearance;

        public UIMollierZone(UIMollierZone uIMollierZone)
            :base(uIMollierZone)
        {
            if(uIMollierZone != null)
            {
                uIMollierAppearance = (uIMollierZone.UIMollierAppearance as UIMollierAppearance)?.Clone();
                guid = uIMollierZone.guid;
            }
        }

        public UIMollierZone(MollierZone mollierZone)
            : base(mollierZone)
        {
            uIMollierAppearance = new UIMollierAppearance();
        }
        
        public UIMollierZone(MollierZone mollierZone, Color color)
            : base(mollierZone)
        {
            uIMollierAppearance = new UIMollierAppearance(color);
        }
        
        public UIMollierZone(MollierZone mollierZone, Color color, string text)
            : base(mollierZone)
        {
            uIMollierAppearance = new UIMollierAppearance(color, text);
        }

        public UIMollierZone(MollierZone mollierZone, UIMollierAppearance uIMollierAppearance)
            : base(mollierZone)
        {
            this.uIMollierAppearance = uIMollierAppearance?.Clone();
        }

        public UIMollierZone(JObject jObject)
            : base(jObject)
        {
            FromJObject(jObject);
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

            if (jObject.ContainsKey("UIMollierAppearance"))
            {
                uIMollierAppearance = new UIMollierAppearance(jObject.Value<JObject>("UIMollierAppearance"));
            }

            if(jObject.ContainsKey("Guid"))
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

            if (uIMollierAppearance != null)
            {
                result.Add("UIMollierAppearance", uIMollierAppearance.ToJObject());
            }

            if(guid != System.Guid.Empty)
            {
                result.Add("Guid", guid);
            }

            return result;
        }
    }
}
