using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace SAM.Core.Mollier
{
    public class UIMollierProcess : UIMollierCurve, IUIMollierObject, IMollierProcess
    {
        private UIMollierAppearance uIMollierAppearance_Start;

        private UIMollierAppearance uIMollierAppearance_End;

        public UIMollierPoint GetUIMollierPoint_Start()
        {
            return new UIMollierPoint(Start, uIMollierAppearance_Start);
        }

        public UIMollierPoint GetUIMollierPoint_End()
        {
            return new UIMollierPoint(End, uIMollierAppearance_End);
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

        public MollierProcess MollierProcess
        {
            get
            {
                return (MollierCurve as MollierProcess)?.Clone();
            }
        }

        public UIMollierProcess(MollierProcess mollierProcess, Color color)
            :base(mollierProcess, color)
        {
            uIMollierAppearance_Start = new UIMollierAppearance(color);
            uIMollierAppearance_End = new UIMollierAppearance(color);
        }

        public UIMollierProcess(UIMollierProcess uIMollierProcess)
            :base(uIMollierProcess)
        {
            if(uIMollierProcess != null)
            {
                uIMollierAppearance_Start = uIMollierProcess.uIMollierAppearance_Start?.Clone();
                uIMollierAppearance_End = uIMollierProcess.uIMollierAppearance_End?.Clone();
            }
        }

        public UIMollierProcess(JObject jObject)
            :base(jObject)
        {

        }

        public virtual bool FromJObject(JObject jObject)
        {
            bool result = base.FromJObject(jObject);
            if (!result)
            {
                return false;
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
        
        public virtual JObject ToJObject()
        {
            JObject result = base.ToJObject();
            if(result == null)
            {
                return null;
            }

            if (uIMollierAppearance_Start != null)
            {
                result.Add("UIMollierAppearance_Start", uIMollierAppearance_Start.ToJObject());
            }

            if (uIMollierAppearance_End != null)
            {
                result.Add("UIMollierAppearance_End", uIMollierAppearance_End.ToJObject());
            }

            return result;
        }
    }
}
