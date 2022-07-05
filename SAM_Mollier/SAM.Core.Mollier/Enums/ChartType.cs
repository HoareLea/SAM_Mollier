using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.Core.Mollier
{
    public enum ChartType
    {
        [Description("Undefined")] Undefined,
        [Description("Mollier")] Mollier,
        [Description("Psychrometric")] Psychrometric
    }
}
