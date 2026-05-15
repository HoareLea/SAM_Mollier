// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020–2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using System.Text.Json.Nodes;
using SAM.Core.Mollier;
using System;

namespace SAM.Weather.Mollier
{
    public class WeatherMollierPoint : MollierPoint
    {
        private DateTime dateTime;

        public WeatherMollierPoint(MollierPoint mollierPoint, DateTime dateTime)
            :base(mollierPoint)
        {
            this.dateTime = dateTime;
        }

        public WeatherMollierPoint(WeatherMollierPoint weatherMollierPoint)
            : base(weatherMollierPoint)
        {
            if(weatherMollierPoint != null)
            {
                dateTime = weatherMollierPoint.dateTime;
            }
        }

        public WeatherMollierPoint(JsonObject jObject)
            : base(jObject)
        {

        }

        public DateTime DateTime
        {
            get
            {
                return dateTime;
            }
        }

        public override JsonObject ToJsonObject()
        {
            JsonObject result = base.ToJsonObject();
            if(result == null)
            {
                return null;
            }

            result.Add("DateTime", dateTime);

            return result;
        }

        public override bool FromJsonObject(JsonObject jObject)
        {
            if(!base.FromJsonObject(jObject))
            {
                return false;
            }

            if(jObject.ContainsKey("DateTime"))
            {
                dateTime = jObject["DateTime"]?.GetValue<DateTime>() ?? default(DateTime);
            }

            return true;
        }
    }
}
