using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SAM.Core.Mollier
{
    public class MollierGroup : Collection<IMollierGroupable>, IMollierGroup
    {
        public string Name { get; set; }

        public MollierGroup(string name)
        {
            Name = name;
        }

        public MollierGroup(MollierGroup mollierGroup)
        {
            if(mollierGroup != null)
            {
                foreach(IMollierGroupable mollierGroupable in mollierGroup)
                {
                    Add(mollierGroupable);
                }
            }
        }

        public MollierGroup(JObject jObject)
        {
            FromJObject(jObject);
        }

        // TODO: [Maciek]
        public List<IMollierGroupable> GetObjects(Type type, bool includeNestedObjects = true)
        {
            if(type == null)
            {
                return null;
            }

            List<IMollierGroupable> result = new List<IMollierGroupable>();
            foreach (IMollierGroupable mollierGroupable in this)
            {
                if (type.IsAssignableFrom(mollierGroupable.GetType()))
                {
                    result.Add(mollierGroupable);
                }

                if (includeNestedObjects)
                {
                    if (mollierGroupable is MollierGroup)
                    {
                        List<IMollierGroupable> objects = ((MollierGroup)mollierGroupable).GetObjects(type, includeNestedObjects); ;
                        if (objects != null)
                        {
                            result.AddRange(objects);
                        }
                    }
                }
            }
            return result;
        }


        public List<T> GetObjects<T>(bool includeNestedObjects = true) where T: IMollierGroupable
        {
            return GetObjects(typeof(T), includeNestedObjects)?.FindAll(x => x is T)?.ConvertAll(x => (T)(object)x);
        }

        public List<IMollierProcess> GetMollierProcesses()
        {
            return GetObjects<IMollierProcess>(true);
        }

        public List<IMollierPoint> GetMollierPoints()
        {
            return GetObjects<IMollierPoint>(true);
        }

        public void RemoveObject(IMollierGroupable mollierGroupable, bool includeNestedObjects = true)
        {
            int i = 0; 
            while(i < Count)
            {
                if(includeNestedObjects && this[i] is MollierGroup)
                {
                    RemoveObject(this[i], includeNestedObjects);
                }
                if (mollierGroupable.AlmostEqual(this[i]))
                {
                    RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        } 
        public virtual bool FromJObject(JObject jObject)
        {
            if(jObject == null)
            {
                return false;
            }

            if(jObject.ContainsKey("Name"))
            {
                Name = jObject.Value<string>("Name");
            }

            if(jObject.ContainsKey("Objects"))
            {
                List<IMollierGroupable> mollierGroupables =  Core.Create.IJSAMObjects<IMollierGroupable>(jObject.Value<JArray>("Objects"));
                mollierGroupables?.ForEach(x => Add(x));
            }

            return true;
        }

        public IEnumerator<IMollierGroupable> GetEnumerator()
        {
            return base.GetEnumerator();
        }

        public virtual JObject ToJObject()
        {
            JObject jObject = new JObject();
            jObject.Add("_type", Core.Query.FullTypeName(this));

            if(Name != null)
            {
                jObject.Add("Name", Name);
            }

            JArray jArray = new JArray();
            foreach (IMollierGroupable mollierGroupable in this)
            {
                jArray.Add(mollierGroupable.ToJObject());
            }

            jObject.Add("Objects", jArray);

            return jObject;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
