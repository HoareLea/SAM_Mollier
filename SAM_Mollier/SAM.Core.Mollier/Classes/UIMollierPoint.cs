using Newtonsoft.Json.Linq;
using System.Drawing;

namespace SAM.Core.Mollier
{
    public class UIMollierPoint : MollierPoint, IUIMollierObject
    {
        private UIMollierAppearance uIMollierAppearance;

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

        public UIMollierPoint(MollierPoint mollierPoint, Color color, string label)
            : base(mollierPoint)
        {
            uIMollierAppearance = new UIMollierAppearance(color, label);
        }

        public UIMollierPoint(MollierPoint mollierPoint, Color color)
            : base(mollierPoint)
        {

            uIMollierAppearance = new UIMollierAppearance(color);
        }

        public UIMollierPoint(MollierPoint mollierPoint, UIMollierAppearance uIMollierAppearance)
            : base(mollierPoint)
        {
            this.uIMollierAppearance = uIMollierAppearance?.Clone();
        }

        public UIMollierPoint(MollierPoint mollierPoint)
            : base(mollierPoint)
        {
            uIMollierAppearance = new UIMollierAppearance();
        }   

        public UIMollierPoint(UIMollierPoint uIMollierPoint)
            : base(uIMollierPoint)
        {
            if(uIMollierPoint != null)
            {
                uIMollierAppearance = uIMollierPoint.uIMollierAppearance?.Clone();
            }
        }

        public UIMollierPoint(JObject jObject)
            : base(jObject)
        {
            FromJObject(jObject);
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
