using Newtonsoft.Json.Linq;
using System.Drawing;

namespace SAM.Core.Mollier
{
    public class UIMollierProcess : IMollierProcess, IUIMollierObject
    {
        private MollierProcess mollierProcess;

        private UIMollierAppearance uIMollierAppearance;

        private UIMollierAppearance uIMollierAppearance_Start;

        private UIMollierAppearance uIMollierAppearance_End;

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

        public UIMollierPoint GetUIMollierPoint_Start()
        {
            return new UIMollierPoint(Start, uIMollierAppearance_Start);
        }

        public UIMollierPoint GetUIMollierPoint_End()
        {
            return new UIMollierPoint(End, uIMollierAppearance_End);
        }

        public UIMollierAppearance UIMollierAppearance
        {
            get
            {
                return uIMollierAppearance;
            }

            set
            {
                uIMollierAppearance = value;
            }
        }

        public UIMollierAppearance UIMollierAppearance_Start
        {
            get
            {
                return uIMollierAppearance_Start;
            }

            set
            {
                uIMollierAppearance_Start = value;
            }
        }

        public UIMollierAppearance UIMollierAppearance_End
        {
            get
            {
                return uIMollierAppearance_End;
            }

            set
            {
                uIMollierAppearance_End = value;
            }
        }

        public double Pressure
        {
            get
            {

                return mollierProcess == null ? double.NaN : mollierProcess.Pressure;
            }
        }

        public MollierProcess MollierProcess
        {
            get
            {
                return mollierProcess?.Clone();
            }
        }

        public UIMollierProcess(MollierProcess mollierProcess, Color color)
        {
            this.mollierProcess = mollierProcess;
            uIMollierAppearance = new UIMollierAppearance(color);
            uIMollierAppearance_Start = new UIMollierAppearance(color);
            uIMollierAppearance_End = new UIMollierAppearance(color);
        }

        public UIMollierProcess(UIMollierProcess uIMollierProcess)
        {
            if(uIMollierProcess != null)
            {
                mollierProcess = uIMollierProcess.MollierProcess?.Clone();
                uIMollierAppearance = uIMollierProcess.uIMollierAppearance?.Clone();
                uIMollierAppearance_Start = uIMollierProcess.uIMollierAppearance_Start?.Clone();
                uIMollierAppearance_End = uIMollierProcess.uIMollierAppearance_End?.Clone();
            }
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
                mollierProcess = Core.Query.IJSAMObject(jObject.Value<JObject>("MollierProcess")) as MollierProcess;
            }

            if (jObject.ContainsKey("UIMollierAppearance"))
            {
                uIMollierAppearance = Core.Query.IJSAMObject(jObject.Value<JObject>("UIMollierAppearance")) as UIMollierAppearance;
            }

            if (jObject.ContainsKey("UIMollierAppearance_Start"))
            {
                uIMollierAppearance_Start = Core.Query.IJSAMObject(jObject.Value<JObject>("UIMollierAppearance_Start")) as UIMollierAppearance;
            }

            if (jObject.ContainsKey("UIMollierAppearance_End"))
            {
                uIMollierAppearance_End = Core.Query.IJSAMObject(jObject.Value<JObject>("UIMollierAppearance_End")) as UIMollierAppearance;
            }

            return true;
        }
        
        public JObject ToJObject()
        {
            JObject jObject = new JObject();
            jObject.Add("_type", Core.Query.FullTypeName(this));

            if (mollierProcess != null)
            {
                jObject.Add("MollierProcess", mollierProcess.ToJObject());
            }

            if (uIMollierAppearance != null)
            {
                jObject.Add("UIMollierAppearance", uIMollierAppearance.ToJObject());
            }

            if (uIMollierAppearance_Start != null)
            {
                jObject.Add("UIMollierAppearance_Start", uIMollierAppearance_Start.ToJObject());
            }

            if (uIMollierAppearance_End != null)
            {
                jObject.Add("UIMollierAppearance_End", uIMollierAppearance_End.ToJObject());
            }

            return jObject;
        }
    }
}
