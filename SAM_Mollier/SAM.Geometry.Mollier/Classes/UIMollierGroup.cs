using System.Text.Json.Nodes;
using SAM.Core.Mollier;
using System.Drawing;

namespace SAM.Geometry.Mollier
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

        public IUIMollierAppearance UIMollierAppearance
        {
            get
            {
                return uIMollierAppearance;
            }
            set
            {
                if(value != null)
                {
                    uIMollierAppearance = value as UIMollierAppearance;
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

        public override bool FromJsonObject(JsonObject jObject)
        {
            if(jObject == null || !base.FromJsonObject(jObject))
            {
                return false;
            }

            if (jObject.ContainsKey("UIMollierAppearance"))
            {
                uIMollierAppearance = Core.Query.IJSAMObject(jObject["UIMollierAppearance"] as JsonObject) as UIMollierAppearance;
            }

            if (jObject.ContainsKey("Guid"))
            {
                guid = Core.Query.Guid(jObject, "Guid");
            }

            return true;
        }

        public override JsonObject ToJsonObject()
        {
            JsonObject result = base.ToJsonObject();
            
            if(result == null)
            {
                return null;
            }

            if (uIMollierAppearance != null)
            {
                result.Add("UIMollierAppearance", uIMollierAppearance.ToJsonObject());
            }

            if (guid != System.Guid.Empty)
            {
                result.Add("Guid", guid);
            }

            return result;
        }
    }
}
