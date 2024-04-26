using Newtonsoft.Json.Linq;
using SAM.Core.Mollier;
using System.Drawing;

namespace SAM.Geometry.Mollier
{
    public class UICoolingProcess : UIMollierProcess
    {
        private bool realistic = false;

        public UICoolingProcess(CoolingProcess coolingProcess, Color color, bool realistic)
            : base(coolingProcess, color)
        {
            this.realistic = realistic;
        }

        public UICoolingProcess(CoolingProcess coolingProcess, Color color)
            : base(coolingProcess, color)
        {

        }

        public UICoolingProcess(UICoolingProcess uICoolingProcess)
            :base(uICoolingProcess)
        {
            if(uICoolingProcess != null)
            {
                realistic = uICoolingProcess.realistic;
            }
        }

        public UICoolingProcess(JObject jObject) 
            : base(jObject)
        {
        }

        public bool Realistic
        {
            get
            {
                return realistic;
            }
        }

        public bool FromJObject(JObject jObject)
        {
            if(!base.FromJObject(jObject))
            {
                return false;
            }

            if(jObject.ContainsKey("Realistic"))
            {
                realistic = jObject.Value<bool>("Realistic");
            }

            return true;
        }
        
        public JObject ToJObject()
        {
            JObject result = base.ToJObject();
            if(result == null)
            {
                return result;
            }

            result.Add("Realistic", realistic);

            return result;
        }
    }
}
