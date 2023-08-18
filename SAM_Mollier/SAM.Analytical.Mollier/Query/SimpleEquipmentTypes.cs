using SAM.Core.Mollier;
using System.Collections.Generic;

namespace SAM.Analytical.Mollier
{
    public static partial class Query
    {
        public static List<System.Type> SimpleEquipmentTypes(this IEnumerable<IMollierProcess> mollierProcesses)
        {
            if(mollierProcesses == null)
            {
                return null;
            }

            List<System.Type> result = new List<System.Type>();
            foreach(IMollierProcess mollierProcess in mollierProcesses)
            {
                System.Type type = mollierProcess?.SimpleEquipmentType();
                if(type == null)
                {
                    continue;
                }

                result.Add(type);
            }

            return result;
        }
    }
}