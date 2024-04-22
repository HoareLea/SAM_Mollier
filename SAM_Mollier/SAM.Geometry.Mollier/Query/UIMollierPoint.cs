using SAM.Core.Mollier;
using System.Drawing;

namespace SAM.Geometry.Mollier
{
    public static partial class Query
    {
        public static UIMollierPoint UIMollierPoint(UIMollierProcess uIMollierProcess, ProcessReferenceType processReferenceType)
        {
            if (uIMollierProcess == null || processReferenceType == ProcessReferenceType.Undefined)
            {
                return null;
            }

            switch (processReferenceType)
            {
                case ProcessReferenceType.Process:
                    UIMollierLabelAppearance uIMollierLabelAppearance = (uIMollierProcess.UIMollierAppearance as UIMollierAppearance)?.UIMollierLabelAppearance;
                    return new UIMollierPoint(Core.Mollier.Query.Mid(uIMollierProcess.Start, uIMollierProcess.End), new UIMollierPointAppearance(uIMollierLabelAppearance));

                case ProcessReferenceType.Start:
                    return uIMollierProcess.GetUIMollierPoint_Start();

                case ProcessReferenceType.End:
                    return uIMollierProcess.GetUIMollierPoint_End();
            }

            return null;
        }
    }
}
