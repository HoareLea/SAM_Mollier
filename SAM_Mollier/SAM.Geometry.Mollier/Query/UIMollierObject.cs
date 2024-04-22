using SAM.Core;
using SAM.Core.Mollier;
using System;
using System.Collections.Generic;

namespace SAM.Geometry.Mollier
{
    public static partial class Query
    {
        public static T UIMollierObject<T>(this MollierModel mollierModel, IReference reference) where T : IUIMollierObject
        {
            if (mollierModel == null || reference == null)
            {
                return default;
            }

            List<ObjectReference> objectReferences = new List<ObjectReference>();

            if (reference is PathReference)
            {
                objectReferences.AddRange((PathReference)reference);
            }
            else if (reference is ObjectReference)
            {
                objectReferences.Add((ObjectReference)reference);
            }

            if (objectReferences == null || objectReferences.Count == 0)
            {
                return default;
            }

            Type type = objectReferences[0].Type;
            if (type == null)
            {
                type = typeof(IUIMollierObject);
            }

            if (!Guid.TryParse(objectReferences[0].Reference.Value.ToString(), out Guid guid))
            {
                return default;
            }

            IUIMollierObject uIMollierObject = mollierModel.GetUIMollierObject(type, guid, true);

            if (objectReferences.Count < 2)
            {
                return uIMollierObject is T ? (T)uIMollierObject : default;
            }

            ObjectReference objectReference = objectReferences[1];
            if (objectReference != null)
            {
                UIMollierProcess uIMollierProcess = uIMollierObject as UIMollierProcess;
                if (uIMollierProcess != null)
                {
                    string text = objectReference.Reference.ToString();
                    if (Core.Query.TryGetEnum(text, out ProcessReferenceType processReferenceType))
                    {
                        switch(processReferenceType)
                        {
                            case ProcessReferenceType.Process:
                                return uIMollierProcess is T ? (T)(object)uIMollierProcess : default;

                            case ProcessReferenceType.Start:
                                UIMollierPoint start = uIMollierProcess.GetUIMollierPoint_Start();
                                return start is T ? (T)(object)start : default;

                            case ProcessReferenceType.End:
                                UIMollierPoint end = uIMollierProcess.GetUIMollierPoint_End();
                                return end is T ? (T)(object)end : default;
                        }
                    }
                }
            }

            return default;
        }
    }
}
