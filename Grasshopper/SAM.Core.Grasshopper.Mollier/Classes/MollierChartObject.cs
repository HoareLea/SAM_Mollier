// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020–2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using System.Text.Json.Nodes;
using SAM.Core.Mollier;

namespace SAM.Core.Grasshopper.Mollier
{
    public class MollierChartObject : IJSAMObject
    {
        private IUIMollierObject uIMollierObject;

        private ChartType chartType;

        private double z;

        public MollierChartObject(IUIMollierObject uIMollierObject, ChartType chartType, double z)
        {
            this.uIMollierObject = uIMollierObject;
            this.chartType = chartType;
            this.z = z;
        }

        public MollierChartObject(JsonObject jObject)
        {
            FromJsonObject(jObject);
        }

        public MollierChartObject(MollierChartObject mollierChartObject)
        { 
            if(mollierChartObject != null)
            {
                uIMollierObject = mollierChartObject.uIMollierObject;
                chartType = mollierChartObject.chartType;
                z = mollierChartObject.z;
            }
        }

        public IUIMollierObject UIMollierObject
        {
            get
            {
                return uIMollierObject?.Clone();
            }
        }

        public ChartType ChartType
        {
            get
            {
                return chartType;
            }
        }

        public double Z
        {
            get
            {
                return z;
            }
        }

        public IUIMollierAppearance UIMollierAppearance
        {
            get
            {
                return uIMollierObject?.UIMollierAppearance?.Clone();
            }
        }

        public bool FromJsonObject(JsonObject jObject)
        {
            if (jObject == null)
            {
                return false;
            }

            if (jObject.ContainsKey("UIMollierObject"))
            {
                uIMollierObject = Core.Query.IJSAMObject< IUIMollierObject >(jObject["UIMollierObject"] as JsonObject);
            }

            if (jObject.ContainsKey("ChartType"))
            {
                chartType = Core.Query.Enum<ChartType>(jObject["ChartType"]?.GetValue<string>() ?? null);
            }

            if (jObject.ContainsKey("Z"))
            {
                z = jObject["Z"]?.GetValue<double>() ?? default(double);
            }

            return true;
        }
        
        public JsonObject ToJsonObject()
        {
            JsonObject jObject = new JsonObject();
            jObject.Add("_type", Core.Query.FullTypeName(this));

            if(uIMollierObject != null)
            {
                jObject.Add("UIMollierObject", uIMollierObject.ToJsonObject());
            }

            jObject.Add("ChartType", chartType.ToString());

            if(!double.IsNaN(z))
            {
                jObject.Add("Z", z);
            }

            return jObject;
        }
    }
}
