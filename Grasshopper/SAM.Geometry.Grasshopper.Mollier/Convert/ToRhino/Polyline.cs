using Rhino.Geometry;
using SAM.Core.Mollier;
using SAM.Geometry.Mollier;
using System.Collections.Generic;

namespace SAM.Geometry.Grasshopper.Mollier
{
    public static partial class Convert
    {
        public static Polyline ToRhino_Polyline(this IMollierProcess mollierProcess, ChartType chartType, double z = 0)
        {
            if(mollierProcess == null)
            {
                return null;
            }

            if(mollierProcess is UIMollierProcess)
            {
                return ToRhino_Polyline((UIMollierProcess)mollierProcess, chartType, z);
            }

            return ToRhino_Polyline(mollierProcess is UIMollierProcess ? ((UIMollierProcess)mollierProcess).MollierProcess : mollierProcess as MollierProcess, chartType, z);
        }

        public static Polyline ToRhino_Polyline(this UIMollierProcess uIMollierProcess, ChartType chartType, double z = 0)
        {
            MollierProcess mollierProcess = uIMollierProcess?.MollierProcess;
            if(mollierProcess == null)
            {
                return null;
            }

            Polyline result = null;
            if (uIMollierProcess is UICoolingProcess && ((UICoolingProcess)uIMollierProcess).Realistic)
            {
                List<MollierPoint> mollierPoints = Core.Mollier.Query.ProcessMollierPoints((CoolingProcess)mollierProcess);
                result = Core.Grasshopper.Mollier.Convert.ToRhino_Polyline(mollierPoints, chartType, true, z);
            }
            else
            {
                result = Core.Grasshopper.Mollier.Convert.ToRhino_Polyline(mollierProcess, chartType, z);
            }

            return result;
        }
    }
}
