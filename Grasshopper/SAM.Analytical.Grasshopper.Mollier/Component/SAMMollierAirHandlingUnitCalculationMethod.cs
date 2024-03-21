using SAM.Analytical.Grasshopper.Mollier.Properties;
using SAM.Analytical.Mollier;
using SAM.Core.Grasshopper;
using System;

namespace SAM.Analytical.Grasshopper.Mollier
{
    public class SAMMollierAirHandlingUnitCalculationMethod : GH_SAMEnumComponent<AirHandlingUnitCalculationMethod>
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("80cbaf15-7cad-43da-ba49-be22ed886148");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.1";

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.SAM_Small;

        /// <summary>
        /// Panel Type
        /// </summary>
        public SAMMollierAirHandlingUnitCalculationMethod()
          : base("SAMMollier.AirHandlingUnitCalculationMethod", "SAMMollier.AirHandlingUnitCalculationMethod",
              "Select Air Handling Unit Calculation Method",
              "SAM", "Mollier")
        {
        }
    }
}