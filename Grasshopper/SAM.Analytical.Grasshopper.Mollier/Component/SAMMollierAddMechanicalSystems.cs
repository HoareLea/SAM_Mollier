﻿//using Grasshopper.Kernel;
//using SAM.Analytical.Grasshopper.Mollier.Properties;
//using System;
//using System.Collections.Generic;
//using SAM.Core.Grasshopper;
//using SAM.Core;
//using SAM.Analytical.Mollier;

//namespace SAM.Analytical.Grasshopper.Mollier
//{
//    public class SAMMollierAddMechanicalSystems : GH_SAMComponent
//    {
//        /// <summary>
//        /// Gets the unique ID for this component. Do not change this ID after release.
//        /// </summary>
//        public override Guid ComponentGuid => new Guid("9914f344-75cf-4058-9626-dd51243a37ce");

//        /// <summary>
//        /// The latest version of this component
//        /// </summary>
//        public override string LatestComponentVersion => "1.0.4";

//        /// <summary>
//        /// Provides an Icon for the component.
//        /// </summary>
//        protected override System.Drawing.Bitmap Icon => Resources.SAM_Small;

//        /// <summary>
//        /// Initializes a new instance of the SAM_point3D class.
//        /// </summary>
//        public SAMMollierAddMechanicalSystems()
//          : base("SAMMollier.AddMechanicalSystems", "SAMMollier.AddMechanicalSystems",
//              "Add MechanicalSystems to SAM Analytical Model",
//              "SAM", "Analytical")
//        {
//        }

//        /// <summary>
//        /// Registers all the input parameters for this component.
//        /// </summary>
//        protected override void RegisterInputParams(GH_InputParamManager inputParamManager)
//        {
//            inputParamManager.AddParameter(new GooAnalyticalObjectParam(), "_analytical", "_analytical", "SAM Analytical", GH_ParamAccess.item);

//            GooSystemTypeLibraryParam gooSystemTypeLibraryParam = new GooSystemTypeLibraryParam();
//            gooSystemTypeLibraryParam.Optional = true;
//            inputParamManager.AddParameter(gooSystemTypeLibraryParam, "_systemTypeLibrary_", "_systemTypeLibrary_", "SAM SystemTypeLibrary", GH_ParamAccess.item);

//            GooSpaceParam gooSpaceParam = new GooSpaceParam();
//            gooSpaceParam.Optional = true;
//            inputParamManager.AddParameter(gooSpaceParam, "_spaces_", "_spaces_", "SAM Analytical Spaces", GH_ParamAccess.list);

//            int index = -1;

//            index = inputParamManager.AddTextParameter("_supplyUnitName_", "_supplyUnitName", "Supply Unit Name", GH_ParamAccess.item, "AHU1");
//            //index = inputParamManager.AddTextParameter("_supplyUnitName_", "_supplyUnitName", "Supply Unit Name", GH_ParamAccess.item, "AHU1S");
//            inputParamManager[index].Optional = true;

//            index = inputParamManager.AddTextParameter("_exhaustUnitName_", "_exhaustUnitName_", "Exhaust Unit Name", GH_ParamAccess.item, "AHU1");
//            //index = inputParamManager.AddTextParameter("_exhaustUnitName_", "_exhaustUnitName_", "Exhaust Unit Name", GH_ParamAccess.item, "AHU1E");
//            inputParamManager[index].Optional = true;

//            index = inputParamManager.AddTextParameter("_ventilationRiserName_", "_ventilationRiserName_", "Ventilation Riser Name", GH_ParamAccess.item, "RV1");
//            inputParamManager[index].Optional = true;

//            index = inputParamManager.AddTextParameter("_heatingRiserName_", "_heatingRiserName_", "Heating Riser Name", GH_ParamAccess.item, "RH1");
//            inputParamManager[index].Optional = true;

//            index = inputParamManager.AddTextParameter("_coolingRiserName_", "_coolingRiserName_", "Cooling Riser Name", GH_ParamAccess.item, "RC1");
//            inputParamManager[index].Optional = true;
//        }

//        /// <summary>
//        /// Registers all the output parameters for this component.
//        /// </summary>
//        protected override void RegisterOutputParams(GH_OutputParamManager outputParamManager)
//        {
//            outputParamManager.AddParameter(new GooJSAMObjectParam<SAMObject>(), "Analytical", "Analytical", "SAM Analytical", GH_ParamAccess.item);
//            outputParamManager.AddParameter(new GooSystemParam(), "MechanicalSystems", "MechanicalSystems", "MechanicalSystems", GH_ParamAccess.list);
//        }

//        /// <summary>
//        /// This is the method that actually does the work.
//        /// </summary>
//        /// <param name="dataAccess">
//        /// The DA object is used to retrieve from inputs and store in outputs.
//        /// </param>
//        protected override void SolveInstance(IGH_DataAccess dataAccess)
//        {
//            IAnalyticalObject analyticalObject = null;
//            if (!dataAccess.GetData(0, ref analyticalObject) || analyticalObject == null)
//            {
//                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
//                return;
//            }

//            SystemTypeLibrary systemTypeLibrary = null;
//            dataAccess.GetData(1, ref systemTypeLibrary);

//            if (systemTypeLibrary == null)
//                systemTypeLibrary = ActiveSetting.Setting.GetValue<SystemTypeLibrary>(AnalyticalSettingParameter.DefaultSystemTypeLibrary);

//            List<Space> spaces = new List<Space>();
//            if (!dataAccess.GetDataList(2, spaces))
//                spaces = null;

//            string supplyUnitName = null;
//            dataAccess.GetData(3, ref supplyUnitName);
//            if (string.IsNullOrEmpty(supplyUnitName))
//                supplyUnitName = "AHU1";

//            string exhaustUnitName = null;
//            dataAccess.GetData(4, ref exhaustUnitName);
//            if (string.IsNullOrEmpty(exhaustUnitName))
//                exhaustUnitName = "AHU1";

//            string ventilationRiserName = null;
//            dataAccess.GetData(5, ref ventilationRiserName);
//            if (string.IsNullOrEmpty(ventilationRiserName))
//                ventilationRiserName = "RV1";

//            string heatingRiserName = null;
//            dataAccess.GetData(6, ref heatingRiserName);
//            if (string.IsNullOrEmpty(heatingRiserName))
//                heatingRiserName = "RH1";

//            string coolingRiserName = null;
//            dataAccess.GetData(7, ref coolingRiserName);
//            if (string.IsNullOrEmpty(coolingRiserName))
//                coolingRiserName = "RC1";

//            List<MechanicalSystem> mechanicalSystems = null;

//            AnalyticalModel analyticalModel = null;
//            if (analyticalObject is AnalyticalModel)
//            {
//                analyticalModel = new AnalyticalModel((AnalyticalModel)analyticalObject);
//            }

//            if (analyticalModel != null)
//            {
//                AdjacencyCluster adjacencyCluster = analyticalModel.AdjacencyCluster;

//                mechanicalSystems = adjacencyCluster.AddMechanicalSystems(systemTypeLibrary, spaces, supplyUnitName, exhaustUnitName, ventilationRiserName, heatingRiserName, coolingRiserName);
//                if (mechanicalSystems != null && mechanicalSystems.Count > 0)
//                {
//                    analyticalModel = new AnalyticalModel(analyticalModel, adjacencyCluster);

//                    List<AirHandlingUnit> airHandlingUnits = adjacencyCluster.GetObjects<AirHandlingUnit>();
//                    if(airHandlingUnits != null)
//                    {
//                        foreach(AirHandlingUnit airHandlingUnit in airHandlingUnits)
//                        {
//                            AirHandlingUnitResult airHandlingUnitResult = Analytical.Mollier.Create.AirHandlingUnitResult(analyticalModel, airHandlingUnit.Name, out List<Core.Mollier.IMollierProcess> mollierProcesses);
//                            if (airHandlingUnit != null && airHandlingUnitResult != null)
//                            {
//                                adjacencyCluster = analyticalModel.AdjacencyCluster;
//                                adjacencyCluster.AddObject(airHandlingUnitResult);
//                                adjacencyCluster.AddRelation(airHandlingUnit, airHandlingUnitResult);
//                            }
//                        }
//                    }

//                    analyticalObject = new AnalyticalModel(analyticalModel, adjacencyCluster);
//                }
//            }

//            dataAccess.SetData(0, analyticalObject);
//            dataAccess.SetDataList(1, mechanicalSystems);
//        }
//    }
//}