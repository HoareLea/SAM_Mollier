﻿using SAM.Core.Grasshopper;
using System;
using SAM.Core.Grasshopper.Mollier.Properties;
using SAM.Core.Mollier;

namespace SAM.Weather.Grasshopper
{
    public class SAMMollierMollierPointProperty : GH_SAMEnumComponent<MollierPointProperty>
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("d41c0352-d17c-43d8-962d-ef05ad8d9343");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.0";

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.SAM_Small;

        /// <summary>
        /// Zone Type Enum Component
        /// </summary>
        public SAMMollierMollierPointProperty()
          : base("SAMWeather.MollierPointProperty", "SAMWeather.MollierPointProperty",
              "Select MollierPointProperty",
              "SAM", "Mollier")
        {
        }
    }
}