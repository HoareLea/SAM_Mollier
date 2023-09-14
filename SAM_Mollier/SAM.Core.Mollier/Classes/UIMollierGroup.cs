using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Drawing;

namespace SAM.Core.Mollier
{
    public class UIMollierGroup : MollierGroup, IUIMollierObject
    {
        private UIMollierAppearance uIMollierAppearance;
        public UIMollierGroup(MollierGroup mollierGroup)
            : base(mollierGroup)
        {

        }   
        public UIMollierGroup(UIMollierGroup uIMollierGroup)
            : base(uIMollierGroup)
        {

        }
        public UIMollierGroup(MollierGroup mollierGroup, UIMollierAppearance uIMollierAppearance)
            : base(mollierGroup)
        {
            this.uIMollierAppearance = uIMollierAppearance;
        }
        public UIMollierGroup(MollierGroup mollierGroup, Color color)
            : base(mollierGroup)
        {
            this.uIMollierAppearance = new UIMollierAppearance(color, mollierGroup.Name);
        }

        public UIMollierAppearance UIMollierAppearance
        {
            get
            {
                return uIMollierAppearance;
            }
            set
            {
                if(value != null)
                {
                    uIMollierAppearance = value;
                }
            }
        }
        public override bool FromJObject(JObject jObject)
        {
            if(jObject == null || !base.FromJObject(jObject))
            {
                return false;
            }

            if (jObject.ContainsKey("UIMollierAppearance"))
            {
                uIMollierAppearance = Core.Query.IJSAMObject(jObject.Value<JObject>("UIMollierAppearance")) as UIMollierAppearance;
            }

            return true;
        }

        public override JObject ToJObject()
        {
            JObject result = base.ToJObject();
            
            if(result == null)
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
