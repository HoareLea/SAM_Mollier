using SAM.Core.Mollier;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Analytical.Mollier
{
    public static partial class Create
    {
        public static AirHandlingUnit AirHandlingUnit(IEnumerable<IMollierProcess> mollierProcesses_Supply, IEnumerable<IMollierProcess> mollierProcesses_Extract)
        {
            if ((mollierProcesses_Supply == null || mollierProcesses_Supply.Count() == 0) && (mollierProcesses_Extract == null || mollierProcesses_Extract.Count() == 0))
            {
                return null;
            }

            AirHandlingUnit result = new AirHandlingUnit("AHU", double.NaN, double.NaN);
            if(mollierProcesses_Supply != null)
            {
                List<SimpleEquipment> simpleEquipments = new List<SimpleEquipment>();

                foreach(IMollierProcess mollierProcess in mollierProcesses_Supply)
                {
                    SimpleEquipment simpleEquipment = SimpleEquipment(mollierProcess);
                    if(simpleEquipment == null)
                    {
                        continue;
                    }

                    simpleEquipments.Add(simpleEquipment);
                }

                result.AddSimpleEquipments(FlowClassification.Supply, simpleEquipments.ToArray());
            }

            if (mollierProcesses_Extract != null)
            {
                List<SimpleEquipment> simpleEquipments = new List<SimpleEquipment>();

                foreach (IMollierProcess mollierProcess in mollierProcesses_Extract)
                {
                    SimpleEquipment simpleEquipment = SimpleEquipment(mollierProcess);
                    if (simpleEquipment == null)
                    {
                        continue;
                    }

                    simpleEquipments.Add(simpleEquipment);
                }

                result.AddSimpleEquipments(FlowClassification.Extract, simpleEquipments.ToArray());
            }


            return result;
        }
    }
}