using SAM.Core;

namespace SAM.Geometry.Mollier
{
    public static partial class Create
    {
        public static IReference Reference(this UIMollierPoint uIMollierPoint)
        {
            if (uIMollierPoint == null)
            {
                return null;
            }

            return Core.Create.ObjectReference(uIMollierPoint, x => x.Guid);
        }

        public static IReference Reference(this UIMollierProcess uIMollierProcess)
        {
            if (uIMollierProcess == null)
            {
                return null;
            }

            return Core.Create.ObjectReference(uIMollierProcess, x => x.Guid);
        }

        public static IReference Reference(this UIMollierProcess uIMollierProcess, ProcessReferenceType processReferenceType)
        {
            if(uIMollierProcess == null || processReferenceType == ProcessReferenceType.Undefined)
            {
                return null;
            }

            ObjectReference objectReference = Reference(uIMollierProcess) as ObjectReference;
            if (objectReference == null)
            {
                return null;
            }

            return new PathReference(new ObjectReference[] { objectReference, new ObjectReference(null as System.Type, processReferenceType.ToString()) });
        }
    }
}

