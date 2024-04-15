using Newtonsoft.Json.Linq;
using System.Drawing;

namespace SAM.Core.Mollier
{
    public class UIMollierLabelAppearance : IUIMollierAppearance
    {
        public int Size { get; set; } = 1;

        public Color Color { get; set; } = Color.Empty;

        public string Text { get; set; } = null;

        public bool Visible { get; set; } = true;

        public UIMollierLabelAppearance()
        {

        }

        public UIMollierLabelAppearance(Color color)
        {
            Color = color;
        }

        public UIMollierLabelAppearance(string text)
        {
            Text = text;
        }

        public UIMollierLabelAppearance(Color color, string text)
        {
            Color = color;
            Text = text;
        }

        public UIMollierLabelAppearance(UIMollierAppearance uIMollierAppearance)
        {
            if(uIMollierAppearance?.UIMollierLabelAppearance != null)
            {
                Color = uIMollierAppearance.UIMollierLabelAppearance.Color;
                Text = uIMollierAppearance.UIMollierLabelAppearance.Text;
                Visible = uIMollierAppearance.UIMollierLabelAppearance.Visible;
                Size = uIMollierAppearance.UIMollierLabelAppearance.Size;
            }
        }

        public UIMollierLabelAppearance(UIMollierLabelAppearance uIMollierLabelAppearance)
        {
            if (uIMollierLabelAppearance != null)
            {
                Color = uIMollierLabelAppearance.Color;
                Text = uIMollierLabelAppearance.Text;
                Visible = uIMollierLabelAppearance.Visible;
                Size = uIMollierLabelAppearance.Size;
            }
        }

        public UIMollierLabelAppearance(UIMollierLabelAppearance uIMollierLabelAppearance, Color color)
        {
            if (uIMollierLabelAppearance != null)
            {
                Text = uIMollierLabelAppearance.Text;
                Visible = uIMollierLabelAppearance.Visible;
                Size = uIMollierLabelAppearance.Size;
            }

            Color = color;
        }

        public UIMollierLabelAppearance(JObject jObject)
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

            if (jObject.ContainsKey("Text"))
            {
                Text = jObject.Value<string>("Text");
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

            if (Text != null)
            {
                jObject.Add("Text", Text);
            }

            jObject.Add("Visible", Visible);

            jObject.Add("Size", Size);

            return jObject;
        }
    }
}
