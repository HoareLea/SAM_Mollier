using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Drawing;

namespace SAM.Core.Mollier
{
    public class UIMollierGroup : MollierGroup, IUIMollierObject
    {
        private System.Guid guid = System.Guid.NewGuid();

        private UIMollierAppearance uIMollierAppearance;
        
        public UIMollierGroup(MollierGroup mollierGroup)
            : base(mollierGroup)
        {

        }   
        
        public UIMollierGroup(UIMollierGroup uIMollierGroup)
            : base(uIMollierGroup)
        {
            if(uIMollierGroup != null)
            {
                uIMollierAppearance = uIMollierGroup.uIMollierAppearance == null ? null : new UIMollierAppearance(uIMollierGroup.uIMollierAppearance);
                guid = uIMollierGroup.guid;
            }
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

        public System.Guid Guid
        {
            get
            {
                return guid;
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

            if (jObject.ContainsKey("Guid"))
            {
                guid = Core.Query.Guid(jObject, "Guid");
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

            if (guid != System.Guid.Empty)
            {
                result.Add("Guid", guid);
            }

            return result;
        }
    }
}
