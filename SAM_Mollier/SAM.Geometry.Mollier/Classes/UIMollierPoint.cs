using System.Text.Json.Nodes;
using SAM.Core;
using SAM.Core.Mollier;
using System.Drawing;

namespace SAM.Geometry.Mollier
{
    public class UIMollierPoint : MollierPoint, IUIMollierObject
    {
        private System.Guid guid = System.Guid.NewGuid();

        private UIMollierPointAppearance uIMollierPointAppearance;

        public virtual IUIMollierAppearance UIMollierAppearance
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

        public UIMollierPoint(JsonObject jObject)
            : base(jObject)
        {
            FromJsonObject(jObject);
        }

        public UIMollierPoint(UIMollierProcess uIMollierProcess, ProcessReferenceType processReferenceType)
            :this(Query.UIMollierPoint(uIMollierProcess, processReferenceType))
        {

        }

        public System.Guid Guid
        {
            get
            {
                return guid;
            }
        }

        public virtual bool FromJsonObject(JsonObject jObject)
        {
            if (jObject == null || !base.FromJsonObject(jObject))
            {
                return false;
            }

            if (jObject.ContainsKey("UIMollierPointAppearance"))
            {
                uIMollierPointAppearance = new UIMollierPointAppearance(jObject["UIMollierPointAppearance"] as JsonObject);
            }

            if (jObject.ContainsKey("Guid"))
            {
                guid = Core.Query.Guid(jObject, "Guid");
            }

            return true;
        }
        
        public virtual JsonObject ToJsonObject()
        {
            JsonObject result = base.ToJsonObject();
            if (result == null)
            {
                return null;
            }

            if (uIMollierPointAppearance != null)
            {
                result.Add("UIMollierPointAppearance", uIMollierPointAppearance.ToJsonObject());
            }

            if (guid != System.Guid.Empty)
            {
                result.Add("Guid", guid);
            }

            return result;
        }
    }
}
