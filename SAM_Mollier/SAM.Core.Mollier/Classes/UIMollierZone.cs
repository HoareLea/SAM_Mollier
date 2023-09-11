using Newtonsoft.Json.Linq;
using System.Drawing;

namespace SAM.Core.Mollier
{
    public class UIMollierZone : IMollierZone, IUIMollierObject
    {
        private MollierZone mollierZone;

        private UIMollierAppearance uIMollierAppearance;

        public UIMollierZone(UIMollierZone uIMollierZone)
        {
            if(uIMollierZone != null)
            {
                mollierZone = uIMollierZone.MollierZone?.Clone();
                uIMollierAppearance = uIMollierZone.UIMollierAppearance?.Clone();
            }
        }
        public UIMollierZone(MollierZone mollierZone)
        {
            this.mollierZone = mollierZone?.Clone();
            uIMollierAppearance = new UIMollierAppearance();
        }
        public UIMollierZone(MollierZone mollierZone, Color color)
        {
            this.mollierZone = mollierZone?.Clone();
            uIMollierAppearance = new UIMollierAppearance(color);
        }
        public UIMollierZone(MollierZone mollierZone, Color color, string text)
        {
            this.mollierZone = mollierZone?.Clone();
            uIMollierAppearance = new UIMollierAppearance(color, text);
        }

        public UIMollierZone(MollierZone mollierZone, UIMollierAppearance uIMollierAppearance)
        {
            this.mollierZone = mollierZone?.Clone();
            this.uIMollierAppearance = uIMollierAppearance?.Clone();
        }
        public UIMollierZone(JObject jObject)
        {
            FromJObject(jObject);
        }
        public MollierZone MollierZone
        {
            get
            {
                return mollierZone;
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
        public bool FromJObject(JObject jObject)
        {
            if (jObject == null)
            {
                return false;
            }

            if (jObject.ContainsKey("MollierZone"))
            {
                mollierZone = new MollierZone(jObject.Value<JObject>("MollierZone"));
            }
            if (jObject.ContainsKey("UIMollierAppearance"))
            {
                uIMollierAppearance = new UIMollierAppearance(jObject.Value<JObject>("UIMollierAppearance"));
            }

            return true;
        }

        public JObject ToJObject()
        {
            JObject result = new JObject();
            result.Add("_type", Core.Query.FullTypeName(this));

            if (mollierZone != null)
            {
                result.Add("MollierZone", mollierZone.ToJObject());
            }
            if (uIMollierAppearance != null)
            {
                result.Add("UIMollierAppearance", uIMollierAppearance.ToJObject());
            }

            return result;
        }
    }
}
