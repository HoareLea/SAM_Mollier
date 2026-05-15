// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020–2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using System.Text.Json.Nodes;
using SAM.Core;
using SAM.Core.Mollier;
using System.Drawing;
using SAM.Geometry.Planar;

namespace SAM.Geometry.Mollier
{
    public class UIMollierLabelAppearance : IUIMollierAppearance
    {
        public int Size { get; set; } = 1;

        public Color Color { get; set; } = Color.Empty;

        public string Text { get; set; } = null;

        public bool Visible { get; set; } = true;

        public Vector2D  Vector2D { get; set;} = null;

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
                Vector2D = uIMollierAppearance.UIMollierLabelAppearance.Vector2D == null ? null : new Vector2D(uIMollierAppearance.UIMollierLabelAppearance.Vector2D);
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
                Vector2D = uIMollierLabelAppearance.Vector2D == null ? null : new Vector2D(uIMollierLabelAppearance.Vector2D);
            }
        }

        public UIMollierLabelAppearance(UIMollierLabelAppearance uIMollierLabelAppearance, Color color)
        {
            if (uIMollierLabelAppearance != null)
            {
                Text = uIMollierLabelAppearance.Text;
                Visible = uIMollierLabelAppearance.Visible;
                Size = uIMollierLabelAppearance.Size;
                Vector2D = uIMollierLabelAppearance.Vector2D == null ? null : new Vector2D(uIMollierLabelAppearance.Vector2D);
            }

            Color = color;
        }

        public UIMollierLabelAppearance(JsonObject jObject)
        {
            FromJsonObject(jObject);
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

            if (jObject.ContainsKey("Text"))
            {
                Text = jObject["Text"]?.GetValue<string>() ?? null;
            }

            if (jObject.ContainsKey("Visible"))
            {
                Visible = jObject["Visible"]?.GetValue<bool>() ?? default(bool);
            }

            if (jObject.ContainsKey("Size"))
            {
                Size = jObject["Size"]?.GetValue<int>() ?? default(int);
            }

            if (jObject.ContainsKey("Vector2D"))
            {
                Vector2D = new Vector2D(jObject["Vector2D"] as JsonObject);
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

            if (Text != null)
            {
                jObject.Add("Text", Text);
            }

            jObject.Add("Visible", Visible);

            jObject.Add("Size", Size);

            if (Vector2D != null)
            {
                jObject.Add("Vector2D", Vector2D.ToJsonObject());
            }

            return jObject;
        }
    }
}
