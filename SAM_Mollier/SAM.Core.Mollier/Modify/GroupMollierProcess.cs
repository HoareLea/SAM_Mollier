using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.Core.Mollier
{
    public static partial class Modify
    {
        public static MollierModel GroupMollierProcess(this MollierModel mollierModel, UIMollierProcess uIMollierProcess)
        {
            if(uIMollierProcess == null)
            {
                return mollierModel;
            }

            List<MollierGroup> mollierGroups = mollierModel.GetMollierObjects<MollierGroup>();

            if(mollierGroups == null)
            {
                MollierGroup mollierGroup = new MollierGroup("");
                mollierGroup.Add(uIMollierProcess);
                mollierModel.Add(mollierGroup);
                return mollierModel;
            }

            List<IMollierProcess> mollierProcesses = new List<IMollierProcess>() { uIMollierProcess };

            foreach(MollierGroup mollierGroup in mollierGroups)
            {
                mollierProcesses.AddRange(mollierGroup.GetMollierProcesses());
            }
            
            mollierModel.ClearGroups();
            List<IMollierGroup> newMollierGroups = Query.Group(mollierProcesses);
            mollierModel.AddRange(newMollierGroups);

            return mollierModel;
        }
    }
}
