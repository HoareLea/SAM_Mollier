using Grasshopper.Kernel;
using SAM.Geometry.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;
using SAM.Core.Mollier;
using Newtonsoft.Json.Linq;
using System.Linq;
using SAM.Core.Grasshopper;
using SAM.Core;

namespace SAM.Geometry.Grasshopper.Mollier
{
    public class SAMMollierMollierPointsByPercentage : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("0789f03f-dc02-4849-a684-9e97cd694aa4");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.0";

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.SAM_Mollier;

        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override GH_SAMParam[] Inputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "_mollierPoints", NickName = "_mollierPoints", Description = "Mollier Points", Access = GH_ParamAccess.list }, ParamVisibility.Binding));

                global::Grasshopper.Kernel.Parameters.Param_String @string = null;

                @string = new global::Grasshopper.Kernel.Parameters.Param_String() { Name = "_mollierPointProperty_", NickName = "_mollierPointProperty_", Description = "MollierPointProperty Enum", Access = GH_ParamAccess.item, Optional = true };
                @string.SetPersistentData(MollierPointProperty.DryBulbTemperature.ToString());
                result.Add(new GH_SAMParam(@string, ParamVisibility.Binding));

                global::Grasshopper.Kernel.Parameters.Param_Number @number;

                @number = new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_percentage_", NickName = "_percentage_", Description = "Percentage [0 - 100]", Access = GH_ParamAccess.item, Optional = true };
                @number.SetPersistentData(98.5);
                result.Add(new GH_SAMParam(@number, ParamVisibility.Binding));

                @string = new global::Grasshopper.Kernel.Parameters.Param_String() { Name = "_numberComparisonType_", NickName = "_numberComparisonType_", Description = "NumberComparisonType Enum", Access = GH_ParamAccess.item, Optional = true };
                @string.SetPersistentData(NumberComparisonType.GreaterOrEquals.ToString());
                result.Add(new GH_SAMParam(@string, ParamVisibility.Voluntary));

                global::Grasshopper.Kernel.Parameters.Param_Boolean boolean;

                boolean = new global::Grasshopper.Kernel.Parameters.Param_Boolean() { Name = "_average_", NickName = "_average_", Description = "Average", Access = GH_ParamAccess.item, Optional = true };
                boolean.SetPersistentData(false);
                result.Add(new GH_SAMParam(boolean, ParamVisibility.Voluntary));

                boolean = new global::Grasshopper.Kernel.Parameters.Param_Boolean() { Name = "_unique_", NickName = "_unique_", Description = "Unique", Access = GH_ParamAccess.item, Optional = true };
                boolean.SetPersistentData(false);
                result.Add(new GH_SAMParam(boolean, ParamVisibility.Voluntary));

                @number = new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "minValue_", NickName = "minValue_", Description = "Minimal Value", Access = GH_ParamAccess.item, Optional = true };
                result.Add(new GH_SAMParam(@number, ParamVisibility.Binding));

                boolean = new global::Grasshopper.Kernel.Parameters.Param_Boolean() { Name = "_run", NickName = "_run", Description = "Run", Access = GH_ParamAccess.item, Optional = true };
                boolean.SetPersistentData(false);
                result.Add(new GH_SAMParam(boolean, ParamVisibility.Binding));

                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "value", NickName = "value", Description = "value", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "mollierPoints_In", NickName = "mollierPoints_In", Description = "SAM Mollier Points", Access = GH_ParamAccess.list }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "mollierPoints_Out", NickName = "mollierPoints_Out", Description = "SAM Mollier Points", Access = GH_ParamAccess.list }, ParamVisibility.Voluntary));
                return result.ToArray();
            }
        }

        /// <summary>
        /// Updates PanelTypes for AdjacencyCluster
        /// </summary>
        public SAMMollierMollierPointsByPercentage()
          : base("SAMMollier.MollierPointsByPercentage", "SAMMollier.MollierPointsByPercentage",
              "Calculates MollierPoints By Percentage",
              "SAM", "Mollier")
        {
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index = -1;

            bool run = false;
            index = Params.IndexOfInputParam("_run");
            if (index == -1 || !dataAccess.GetData(index, ref run))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                dataAccess.SetDataList(0, null);
                dataAccess.SetDataList(1, null);
                dataAccess.SetDataList(2, null);
                return;
            }

            if (!run)
                return;

            List<MollierPoint> mollierPoints = new List<MollierPoint>();

            index = Params.IndexOfInputParam("_mollierPoints");
            if (index == -1 || !dataAccess.GetDataList(index, mollierPoints) || mollierPoints == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            MollierPointProperty mollierPointProperty = MollierPointProperty.Undefined;
            index = Params.IndexOfInputParam("_mollierPointProperty_");
            string mollierPointPropertyString = null;
            if (index != -1 && dataAccess.GetData(index, ref mollierPointPropertyString) && !string.IsNullOrWhiteSpace(mollierPointPropertyString))
            {
                mollierPointProperty = Core.Query.Enum<MollierPointProperty>(mollierPointPropertyString);
            }

            if (mollierPointProperty == MollierPointProperty.Undefined)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            double percentage = double.NaN;
            index = Params.IndexOfInputParam("_percentage_");
            if (index != -1)
            {
                dataAccess.GetData(index, ref percentage);
            }

            if (double.IsNaN(percentage))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            NumberComparisonType numberComparisonType = NumberComparisonType.GreaterOrEquals;
            index = Params.IndexOfInputParam("_numberComparisonType_");
            string numberComparisonTypeString = null;
            if (index != -1 && dataAccess.GetData(index, ref numberComparisonTypeString) && !string.IsNullOrWhiteSpace(numberComparisonTypeString))
            {
                numberComparisonType = Core.Query.Enum<NumberComparisonType>(numberComparisonTypeString);
            }

            bool average = false;
            index = Params.IndexOfInputParam("_average_");
            if (index != -1)
            {
                dataAccess.GetData(index, ref average);
            }

            bool unique = false;
            index = Params.IndexOfInputParam("_unique_");
            if (index != -1)
            {
                dataAccess.GetData(index, ref unique);
            }

            double minValue = double.NaN;
            index = Params.IndexOfInputParam("minValue_");
            if (index != -1)
            {
                dataAccess.GetData(index, ref minValue);
            }

            mollierPoints.RemoveAll(x => x == null || !x.IsValid());

            double value = double.NaN;

            List<MollierPoint> mollierPoints_In = new List<MollierPoint>();
            List<MollierPoint> mollierPoints_Out = new List<MollierPoint>();

            if (mollierPoints != null && mollierPoints.Count != 0)
            {
                List<Tuple<double, MollierPoint>> tuples = mollierPoints.ConvertAll(x => new Tuple<double, MollierPoint>(x[mollierPointProperty], x));
                tuples.RemoveAll(x => double.IsNaN(x.Item1));
                if (tuples != null && tuples.Count != 0)
                {
                    if (!double.IsNaN(minValue))
                    {
                        tuples.RemoveAll(x => x.Item1 < minValue);
                    }

                    if (average)
                    {
                        double min = double.MaxValue;
                        double max = double.MinValue;
                        foreach (Tuple<double, MollierPoint> tuple in tuples)
                        {
                            double value_Temp = tuple.Item1;
                            if (value_Temp < min)
                            {
                                min = value_Temp;
                            }
                            if (value_Temp > max)
                            {
                                max = value_Temp;
                            }
                        }

                        value = min + ((max - min) * percentage / 100);
                    }
                    else
                    {
                        List<double> values = tuples.ConvertAll(x => x.Item1);
                        if (unique)
                        {
                            values = values.Distinct().ToList();
                        }

                        values.Sort();

                        int index_Temp = System.Convert.ToInt32(System.Convert.ToDouble(values.Count) * (percentage / 100));
                        if (index_Temp >= values.Count)
                        {
                            index_Temp = values.Count - 1;
                        }

                        value = values[index_Temp];
                    }

                    foreach (Tuple<double, MollierPoint> tuple in tuples)
                    {
                        if (Core.Query.Compare(tuple.Item1, value, numberComparisonType))
                        {
                            mollierPoints_In.Add(tuple.Item2);
                        }
                        else
                        {
                            mollierPoints_Out.Add(tuple.Item2);
                        }
                    }
                }
            }

            index = Params.IndexOfOutputParam("value");
            if (index != -1)
            {
                dataAccess.SetData(index, value);
            }

            index = Params.IndexOfOutputParam("mollierPoints_In");
            if (index != -1)
            {
                dataAccess.SetDataList(index, mollierPoints_In?.ConvertAll(x => new GooMollierPoint(x)));
            }

            index = Params.IndexOfOutputParam("mollierPoints_Out");
            if (index != -1)
            {
                dataAccess.SetDataList(index, mollierPoints_Out?.ConvertAll(x => new GooMollierPoint(x)));
            }
        }
    }
}