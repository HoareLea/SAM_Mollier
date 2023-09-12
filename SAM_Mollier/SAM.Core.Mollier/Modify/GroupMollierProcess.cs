using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.Core.Mollier
{
    public static partial class Modify
    {
        public static MollierModel GroupMollierProcesses(this MollierModel mollierModel, IEnumerable<IMollierProcess> mollierProcesses)
        {
            if(mollierProcesses == null)
            {
                return mollierModel;
            }

            List<MollierGroup> mollierGroups = mollierModel.GetMollierObjects<MollierGroup>();

            if(mollierGroups == null)
            {
                MollierGroup mollierGroup = new MollierGroup("");
                mollierModel.Add(mollierGroup);
                mollierGroups = new List<MollierGroup> { mollierGroup };
            }

            List<IMollierProcess> newMollierProcesses = new List<IMollierProcess>(mollierProcesses);

            foreach(MollierGroup mollierGroup in mollierGroups)
            {
                newMollierProcesses.AddRange(mollierGroup.GetMollierProcesses());
            }
            
            mollierModel.Clear<IMollierGroup>();
            List<IMollierGroup> newMollierGroups = Query.Group(newMollierProcesses);
            mollierModel.AddRange(newMollierGroups);

            return mollierModel;
        }
    
    }
}
