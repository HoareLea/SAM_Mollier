using System.Drawing;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static Color Color(this IMollierProcess mollierProcess)
        {
            if(mollierProcess == null)
            {
                return System.Drawing.Color.Empty;
            }

            Color result = System.Drawing.Color.Empty;

            MollierProcess mollierProcess_Temp = null;

            if (mollierProcess is UIMollierProcess)
            {
                UIMollierProcess uIMollierProcess = (UIMollierProcess)mollierProcess;

                mollierProcess_Temp = uIMollierProcess.MollierProcess;

                result = uIMollierProcess.UIMollierAppearance != null ? uIMollierProcess.UIMollierAppearance.Color : System.Drawing.Color.Empty;
            }

            if(mollierProcess_Temp == null)
            {
                mollierProcess_Temp = mollierProcess as MollierProcess;
            }

            if(!result.IsEmpty)
            {
                return result;
            }

            result = System.Drawing.Color.DarkGreen;

            if (mollierProcess_Temp == null)
            {
                return result;
            }

            if (mollierProcess_Temp is HeatingProcess)
            {
                return System.Drawing.Color.Red;
            }
            else if (mollierProcess_Temp is CoolingProcess)
            {
                return System.Drawing.Color.Blue;
            }

            return result;
        }
    }
}
