using SAM.Core.Mollier;
using System.Drawing;

namespace SAM.Geometry.Mollier
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

        public static Color Color(this IMollierPoint mollierPoint)
        {
            if(mollierPoint == null)
            {
                return System.Drawing.Color.Empty;
            }

            MollierPoint mollierPoint_Temp = mollierPoint as MollierPoint;
            if(mollierPoint_Temp == null)
            {
                return System.Drawing.Color.Empty;
            }

            if (mollierPoint is UIMollierPoint)
            {
                IUIMollierAppearance uIMollierAppearance = ((UIMollierPoint)mollierPoint).UIMollierAppearance;
                if (uIMollierAppearance != null)
                {
                    return uIMollierAppearance.Color;
                }
            }

            return System.Drawing.Color.DarkBlue;

        }
    }
}
