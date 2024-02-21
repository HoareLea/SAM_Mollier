using Newtonsoft.Json.Linq;
using System.Drawing;

namespace SAM.Core.Mollier
{
    public class UIMollierAppearance : IJSAMObject
    {
        public int Size { get; set; } = 1;

        public Color Color { get; set; } = Color.Empty;

        public string Label { get; set; } = null;

        public bool Visible { get; set; } = true;

        public UIMollierAppearance()
        {

        }

        public UIMollierAppearance(Color color)
        {
            Color = color;
        }

        public UIMollierAppearance(Color color, string label)
        {
            Color = color;
            Label = label;
        }

        public UIMollierAppearance(UIMollierAppearance uIMollierAppearance)
        {
            if(uIMollierAppearance != null)
            {
                Color = uIMollierAppearance.Color;
                Label = uIMollierAppearance.Label;
                Visible = uIMollierAppearance.Visible;
                Size = uIMollierAppearance.Size;
            }
        }

        public UIMollierAppearance(UIMollierAppearance uIMollierAppearance, Color color)
        {
            if (uIMollierAppearance != null)
            {
                Label = uIMollierAppearance.Label;
                Visible = uIMollierAppearance.Visible;
                Size = uIMollierAppearance.Size;
            }

            Color = color;
        }

        public UIMollierAppearance(JObject jObject)
        {
            FromJObject(jObject);
        }

        public virtual bool FromJObject(JObject jObject)
        {
            if (jObject == null)
            {
                return false;
            }
            
            if (jObject.ContainsKey("Color"))
            {
                JObject jObject_Color = jObject.Value<JObject>("Color");
                if (jObject_Color != null)
                {
                    SAMColor sAMColor = new SAMColor(jObject_Color);
                    if (sAMColor != null)
                    {
                        Color = sAMColor.ToColor();
                    }
                }
            }

            if (jObject.ContainsKey("Label"))
            {
                Label = jObject.Value<string>("Label");
            }

            if (jObject.ContainsKey("Visible"))
            {
                Visible = jObject.Value<bool>("Visible");
            }

            if (jObject.ContainsKey("Size"))
            {
                Size = jObject.Value<int>("Size");
            }

            return true;
        }
        
        public virtual JObject ToJObject()
        {
            JObject jObject = new JObject();
            jObject.Add("_type", Core.Query.FullTypeName(this));

            if (Color != Color.Empty)
            {
                jObject.Add("Color", (new SAMColor(Color)).ToJObject());
            }

            if (Label != null)
            {
                jObject.Add("Label", Label);
            }

            jObject.Add("Visible", Visible);

            jObject.Add("Size", Size);

            return jObject;
        }
    }
}
