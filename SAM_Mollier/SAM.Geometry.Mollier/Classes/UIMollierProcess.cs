using Newtonsoft.Json.Linq;
using SAM.Core;
using SAM.Core.Mollier;
using System.Drawing;

namespace SAM.Geometry.Mollier
{
    public class UIMollierProcess : UIMollierCurve, IUIMollierObject, IMollierProcess
    {
        private UIMollierPointAppearance uIMollierPointAppearance_Start;

        private UIMollierPointAppearance uIMollierPointAppearance_End;

        public UIMollierPoint GetUIMollierPoint_Start()
        {
            return new UIMollierPoint(Start, uIMollierPointAppearance_Start);
        }

        public UIMollierPoint GetUIMollierPoint_End()
        {
            return new UIMollierPoint(End, uIMollierPointAppearance_End);
        }

        public UIMollierPointAppearance UIMollierPointAppearance_Start
        {
            get
            {
                return uIMollierPointAppearance_Start;
            }

            set
            {
                uIMollierPointAppearance_Start = value;
            }
        }

        public UIMollierPointAppearance UIMollierPointAppearance_End
        {
            get
            {
                return uIMollierPointAppearance_End;
            }

            set
            {
                uIMollierPointAppearance_End = value;
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
            uIMollierPointAppearance_Start = new UIMollierPointAppearance(color);
            uIMollierPointAppearance_End = new UIMollierPointAppearance(color);
        }

        public UIMollierProcess(MollierProcess mollierProcess, UIMollierAppearance uIMollierAppearance)
            : base(mollierProcess, uIMollierAppearance)
        {
            uIMollierPointAppearance_Start = new UIMollierPointAppearance(uIMollierAppearance.Color);
            uIMollierPointAppearance_End = new UIMollierPointAppearance(uIMollierAppearance.Color);
        }

        public UIMollierProcess(UIMollierProcess uIMollierProcess)
            :base(uIMollierProcess)
        {
            if(uIMollierProcess != null)
            {
                uIMollierPointAppearance_Start = uIMollierProcess.uIMollierPointAppearance_Start?.Clone();
                uIMollierPointAppearance_End = uIMollierProcess.uIMollierPointAppearance_End?.Clone();
            }
        }

        public UIMollierProcess(JObject jObject)
            :base(jObject)
        {

        }

        public override bool FromJObject(JObject jObject)
        {
            bool result = base.FromJObject(jObject);
            if (!result)
            {
                return false;
            }

            if (jObject.ContainsKey("UIMollierPointAppearance_Start"))
            {
                uIMollierPointAppearance_Start = Core.Query.IJSAMObject(jObject.Value<JObject>("UIMollierPointAppearance_Start")) as UIMollierPointAppearance;
            }

            if (jObject.ContainsKey("UIMollierPointAppearance_End"))
            {
                uIMollierPointAppearance_End = Core.Query.IJSAMObject(jObject.Value<JObject>("UIMollierPointAppearance_End")) as UIMollierPointAppearance;
            }

            return true;
        }
        
        public override JObject ToJObject()
        {
            JObject result = base.ToJObject();
            if(result == null)
            {
                return null;
            }

            if (uIMollierPointAppearance_Start != null)
            {
                result.Add("UIMollierPointAppearance_Start", uIMollierPointAppearance_Start.ToJObject());
            }

            if (uIMollierPointAppearance_End != null)
            {
                result.Add("UIMollierPointAppearance_End", uIMollierPointAppearance_End.ToJObject());
            }

            return result;
        }
    }
}
