using Newtonsoft.Json.Linq;
using System.Drawing;

namespace SAM.Core.Mollier
{
    public class UIMollierProcess : IMollierProcess
    {
        private IMollierProcess mollierProcess;
        public Color Color { get; set; }

        public MollierPoint Start
        {
            get
            {
                return mollierProcess?.Start;
            }
        }
        public MollierPoint End
        {
            get
            {
                return mollierProcess?.End;
            }
        }
        public double Pressure
        {
            get
            {

                return mollierProcess == null ? double.NaN : mollierProcess.Pressure;
            }
        }


        public IMollierProcess MollierProcess
        {
            get
            {
                return mollierProcess?.Clone();
            }
        }

        public UIMollierProcess(IMollierProcess mollierProcess, Color color)
        {
            this.mollierProcess = mollierProcess;
            Color = color;
        }

        public UIMollierProcess(UIMollierProcess uIMollierProcess)
        {
            mollierProcess = uIMollierProcess?.MollierProcess;
            Color = uIMollierProcess == null ? Color.Empty : uIMollierProcess.Color;
        }

        public UIMollierProcess(JObject jObject)
        {
            FromJObject(jObject);
        }

        public bool FromJObject(JObject jObject)
        {
            if (jObject == null)
            {
                return false;
            }

            if (jObject.ContainsKey("MollierProcess"))
            {
                mollierProcess = Core.Query.IJSAMObject(jObject.Value<JObject>("MollierProcess")) as IMollierProcess;
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
            return true;
        }
        public JObject ToJObject()
        {
            JObject jObject = new JObject();
            if (Color != Color.Empty)
            {
                jObject.Add("Color", (new SAMColor(Color)).ToJObject());
            }
            if(mollierProcess != null)
            {
                jObject.Add("MollierProcess", mollierProcess.ToJObject());
            }
            return jObject;
        }


    }
}
