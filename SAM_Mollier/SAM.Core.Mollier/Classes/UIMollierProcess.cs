using Newtonsoft.Json.Linq;
using System.Drawing;

namespace SAM.Core.Mollier
{
    public class UIMollierProcess : IMollierProcess
    {
        private IMollierProcess mollierProcess;
        
        public Color Color { get; set; }
        
        public string Start_Label { get; set; } = null;
        
        public string Process_Label { get; set; } = null;
        
        public string End_Label { get; set; } = null;
        
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
            Start_Label = uIMollierProcess.Start_Label;
            Process_Label = uIMollierProcess.Process_Label;
            End_Label = uIMollierProcess.End_Label;
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
            if (jObject.ContainsKey("Start_Label"))
            {
                Start_Label = jObject.Value<string>("Start_Label");
            }
            if (jObject.ContainsKey("Process_Label"))
            {
                Process_Label = jObject.Value<string>("Process_Label");
            }
            if (jObject.ContainsKey("End_Label"))
            {
                End_Label = jObject.Value<string>("End_Label");
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
            jObject.Add("_type", Core.Query.FullTypeName(this));

            if (Color != Color.Empty)
            {
                jObject.Add("Color", (new SAMColor(Color)).ToJObject());
            }
            if(mollierProcess != null)
            {
                jObject.Add("MollierProcess", mollierProcess.ToJObject());
            }
            if(Start_Label != null)
            {
                jObject.Add("Start_Label", Start_Label);
            }
            if (Process_Label != null)
            {
                jObject.Add("Process_Label", Process_Label);
            }
            if (End_Label != null)
            {
                jObject.Add("End_Label", End_Label);
            }

            return jObject;
        }
    }
}
