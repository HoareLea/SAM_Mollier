using System.Text.Json.Nodes;
using SAM.Core;
using SAM.Core.Mollier;
using System.Drawing;

namespace SAM.Geometry.Mollier
{
    public class UIMollierAppearance : IUIMollierAppearance
    {
        public int Size { get; set; } = 1;

        public Color Color { get; set; } = Color.Empty;

        public bool Visible { get; set; } = true;

        public UIMollierLabelAppearance UIMollierLabelAppearance { get; set; } = null;

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
            UIMollierLabelAppearance = new UIMollierLabelAppearance(Color.Empty, label);
        }

        public UIMollierAppearance(UIMollierAppearance uIMollierAppearance)
        {
            if(uIMollierAppearance != null)
            {
                Color = uIMollierAppearance.Color;
                UIMollierLabelAppearance = uIMollierAppearance.UIMollierLabelAppearance == null ? null :  new UIMollierLabelAppearance(uIMollierAppearance.UIMollierLabelAppearance);
                Visible = uIMollierAppearance.Visible;
                Size = uIMollierAppearance.Size;
            }
        }

        public UIMollierAppearance(UIMollierAppearance uIMollierAppearance, Color color)
        {
            if (uIMollierAppearance != null)
            {
                UIMollierLabelAppearance = uIMollierAppearance.UIMollierLabelAppearance == null ? null : new UIMollierLabelAppearance(uIMollierAppearance.UIMollierLabelAppearance);
                Visible = uIMollierAppearance.Visible;
                Size = uIMollierAppearance.Size;
            }

            Color = color;
        }

        public UIMollierAppearance(JsonObject jObject)
        {
            FromJsonObject(jObject);
        }

        public string Label
        {
            get
            {
                return UIMollierLabelAppearance?.Text;
            }

            set
            {
                if(UIMollierLabelAppearance == null)
                {
                    UIMollierLabelAppearance = new UIMollierLabelAppearance(value);
                }
                else
                {
                    UIMollierLabelAppearance.Text = value;
                }
            }
        }

        public virtual bool FromJsonObject(JsonObject jObject)
        {
            if (jObject == null)
            {
                return false;
            }
            
            if (jObject.ContainsKey("Color"))
            {
                JsonObject jObject_Color = jObject["Color"] as JsonObject;
                if (jObject_Color != null)
                {
                    SAMColor sAMColor = new SAMColor(jObject_Color);
                    if (sAMColor != null)
                    {
                        Color = sAMColor.ToColor();
                    }
                }
            }

            if (jObject.ContainsKey("UIMollierLabelAppearance"))
            {
                UIMollierLabelAppearance = new UIMollierLabelAppearance(jObject["UIMollierLabelAppearance"] as JsonObject);
            }

            if (jObject.ContainsKey("Visible"))
            {
                Visible = jObject["Visible"]?.GetValue<bool>() ?? default(bool);
            }

            if (jObject.ContainsKey("Size"))
            {
                Size = jObject["Size"]?.GetValue<int>() ?? default(int);
            }

            return true;
        }
        
        public virtual JsonObject ToJsonObject()
        {
            JsonObject jObject = new JsonObject();
            jObject.Add("_type", Core.Query.FullTypeName(this));

            if (Color != Color.Empty)
            {
                jObject.Add("Color", (new SAMColor(Color)).ToJsonObject());
            }

            if (UIMollierLabelAppearance != null)
            {
                jObject.Add("UIMollierLabelAppearance", UIMollierLabelAppearance.ToJsonObject());
            }

            jObject.Add("Visible", Visible);

            jObject.Add("Size", Size);

            return jObject;
        }
    }
}
