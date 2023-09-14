using Newtonsoft.Json.Linq;
using System.Drawing;

namespace SAM.Core.Mollier
{
    public class UIMollierZone : MollierZone, IUIMollierObject
    {
        private UIMollierAppearance uIMollierAppearance;

        public UIMollierZone(UIMollierZone uIMollierZone)
            :base(uIMollierZone)
        {
            if(uIMollierZone != null)
            {
                uIMollierAppearance = uIMollierZone.UIMollierAppearance?.Clone();
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

            return result;
        }
    }
}
