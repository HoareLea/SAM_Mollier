using SAM.Core.Mollier;
using System.Collections.Generic;

namespace SAM.Weather.Mollier
{
    public static partial class Query
    {
        public static List<WeatherMollierPoint> WeatherMollierPoints(this WeatherDay weatherDay, int year, int month, int day)
        {
            List<MollierPoint> mollierPoints = weatherDay?.MollierPoints();
            if(mollierPoints == null)
            {
                return null;
            }

            List<WeatherMollierPoint> result = new List<WeatherMollierPoint>();
            for(int i=0; i < mollierPoints.Count; i++)
            {
                MollierPoint mollierPoint = mollierPoints[i];
                if(mollierPoint == null)
                {
                    continue;
                }

                result.Add(new WeatherMollierPoint(mollierPoint, new System.DateTime(year, month, day, i, 0, 0)));
            }

            return result;
        }

        public static List<WeatherMollierPoint> WeatherMollierPoints(this WeatherYear weatherYear)
        {
            List<WeatherDay> weatherDays = weatherYear?.WeatherDays;
            if(weatherDays == null)
            {
                return null;
            }

            System.DateTime dateTime = new System.DateTime(weatherYear.Year, 1, 1);

            List<WeatherMollierPoint> result = new List<WeatherMollierPoint>();
            for (int i = 0; i < weatherDays.Count; i++)
            {
                List<WeatherMollierPoint> weatherMollierPoints = weatherDays[i].WeatherMollierPoints(dateTime.Year, dateTime.Month, dateTime.Day);

                dateTime.AddDays(1);

                if (weatherMollierPoints == null)
                {
                    continue;
                }

                result.AddRange(weatherMollierPoints);
            }

            return result;
        }

        public static List<WeatherMollierPoint> WeatherMollierPoints(this WeatherData weatherData)
        {
            List<WeatherYear> weatherYears = weatherData?.WeatherYears;
            if(weatherYears == null)
            {
                return null;
            }

            List<WeatherMollierPoint> result = new List<WeatherMollierPoint>();
            foreach(WeatherYear weatherYear in weatherYears)
            {
                List<WeatherMollierPoint> weatherMollierPoints = weatherYear?.WeatherMollierPoints();
                if(weatherMollierPoints == null)
                {
                    continue;
                }

                result.AddRange(weatherMollierPoints);
            }

            return result;
        }
    }
}
