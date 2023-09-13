using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Drawing;

namespace SAM.Core.Mollier
{
    public class UIMollierGroup : Collection<IMollierGroupable>, IMollierGroup, IUIMollierObject
    {

        private MollierGroup mollierGroup;
        private UIMollierAppearance uIMollierAppearance;
        public UIMollierGroup(MollierGroup mollierGroup)
        {
            this.mollierGroup = mollierGroup;
        }   
        public UIMollierGroup(UIMollierGroup uIMollierGroup)
        {
            mollierGroup = uIMollierGroup.mollierGroup;
        }
        public UIMollierGroup(MollierGroup mollierGroup, UIMollierAppearance uIMollierAppearance)
        {
            this.mollierGroup = mollierGroup;
            this.uIMollierAppearance = uIMollierAppearance;
        }
        public UIMollierGroup(MollierGroup mollierGroup, Color color)
        {
            this.mollierGroup = mollierGroup;
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
        public MollierGroup MollierGroup
        {
            get
            {
                return mollierGroup;
            }
            set
            {
                if(value != null)
                {
                    mollierGroup = value;
                }
            }
        }
        public virtual bool FromJObject(JObject jObject)
        {

            if (jObject.ContainsKey("MollierGroup"))
            {
                mollierGroup = Core.Query.IJSAMObject(jObject.Value<JObject>("MollierGroup")) as MollierGroup;
            }

            if (jObject.ContainsKey("UIMollierAppearance"))
            {
                uIMollierAppearance = Core.Query.IJSAMObject(jObject.Value<JObject>("UIMollierAppearance")) as UIMollierAppearance;
            }

            return true;
        }

        public virtual JObject ToJObject()
        {
            JObject result = new JObject();

            if (mollierGroup != null)
            {
                result.Add("MollierGroup", mollierGroup.ToJObject());
            }

            if (uIMollierAppearance != null)
            {
                result.Add("UIMollierAppearance", uIMollierAppearance.ToJObject());
            }

            return result;
        }
    }
}
