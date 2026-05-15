using System.Text.Json.Nodes;
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

        public UICoolingProcess(JsonObject jObject) 
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

        public bool FromJsonObject(JsonObject jObject)
        {
            if(!base.FromJsonObject(jObject))
            {
                return false;
            }

            if(jObject.ContainsKey("Realistic"))
            {
                realistic = jObject["Realistic"]?.GetValue<bool>() ?? default(bool);
            }

            return true;
        }
        
        public JsonObject ToJsonObject()
        {
            JsonObject result = base.ToJsonObject();
            if(result == null)
            {
                return result;
            }

            result.Add("Realistic", realistic);

            return result;
        }
    }
}
