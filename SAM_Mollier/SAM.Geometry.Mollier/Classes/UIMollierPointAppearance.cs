using Newtonsoft.Json.Linq;
using SAM.Core;
using System.Drawing;

namespace SAM.Geometry.Mollier
{
    public class UIMollierPointAppearance : UIMollierAppearance
    {
        public int BorderSize { get; set; } = 1;

        public Color BorderColor { get; set; } = Color.Empty;

        public UIMollierPointAppearance()
        {

        }

        public UIMollierPointAppearance(Color color)
            :base(color)
        {

        }

        public UIMollierPointAppearance(Color color, string label)
            :base(color, label)
        {

        }

        public UIMollierPointAppearance(UIMollierAppearance uIMollierAppearance)
            : base(uIMollierAppearance)
        {

        }

        public UIMollierPointAppearance(UIMollierAppearance uIMollierAppearance, int borderSize, Color borderColor)
            : base(uIMollierAppearance)
        {
            BorderSize = borderSize;
            BorderColor = borderColor;
        }

        public UIMollierPointAppearance(UIMollierPointAppearance uIMollierPointAppearance)
            :base(uIMollierPointAppearance)
        {
            if(uIMollierPointAppearance != null)
            {
                BorderSize = uIMollierPointAppearance.BorderSize;
                BorderColor = uIMollierPointAppearance.BorderColor;
            }
        }

        public UIMollierPointAppearance(UIMollierPointAppearance uIMollierPointAppearance, Color color)
            : base(uIMollierPointAppearance, color)
        {
            if (uIMollierPointAppearance != null)
            {
                BorderSize = uIMollierPointAppearance.BorderSize;
                BorderColor = uIMollierPointAppearance.BorderColor;
            }
        }

        public UIMollierPointAppearance(JObject jObject)
            : base(jObject)
        {
        }

        public override bool FromJObject(JObject jObject)
        {
            bool result = base.FromJObject(jObject);
            if (!result)
            {
                return result;
            }

            if (jObject.ContainsKey("BorderColor"))
            {
                JObject jObject_Color = jObject.Value<JObject>("BorderColor");
                if (jObject_Color != null)
                {
                    SAMColor sAMColor = new SAMColor(jObject_Color);
                    if (sAMColor != null)
                    {
                        BorderColor = sAMColor.ToColor();
                    }
                }
            }

            if (jObject.ContainsKey("BorderSize"))
            {
                BorderSize = jObject.Value<int>("BorderSize");
            }

            return true;
        }
        
        public override JObject ToJObject()
        {
            JObject result = base.ToJObject();
            if(result == null)
            {
                return result;
            }

            if (BorderColor != Color.Empty)
            {
                result.Add("BorderColor", (new SAMColor(BorderColor)).ToJObject());
            }

            result.Add("BorderSize", BorderSize);

            return result;
        }
    }
}
