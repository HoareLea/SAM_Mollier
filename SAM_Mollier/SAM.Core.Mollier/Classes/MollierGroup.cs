using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
                Name = mollierGroup.Name;
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
                        List<IMollierGroupable> objects = ((MollierGroup)mollierGroupable).GetObjects(type, includeNestedObjects);
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
        
        public void RemoveObject(IMollierGroupable mollierGroupable, bool includeNestedObjects = true)
        {
            if (mollierGroupable == null)
            {
                return;
            }

            int count = Count;

            for (int i = count - 1; i >= 0; i--)
            {
                if (includeNestedObjects && this[i] is MollierGroup)
                {
                    ((MollierGroup)this[i]).RemoveObject(mollierGroupable, includeNestedObjects);
                }

                if (mollierGroupable.AlmostEqual(this[i]))
                {
                    RemoveAt(i);
                    continue;
                }

                if (mollierGroupable is IUIMollierObject && this[i]?.GetType() == mollierGroupable.GetType() && ((IUIMollierObject)this[i]).Guid == ((IUIMollierObject)mollierGroupable).Guid)
                {
                    RemoveAt(i);
                    continue;
                }
            }

            //int i = 0; 
            //while(i < Count)
            //{
            //    if(includeNestedObjects && this[i] is MollierGroup)
            //    {
            //        ((MollierGroup)this[i]).RemoveObject(mollierGroupable, includeNestedObjects);
            //    }
            //    if (mollierGroupable.AlmostEqual(this[i]))
            //    {
            //        RemoveAt(i);
            //    }
            //    else if(mollierGroupable is IUIMollierObject && this[i]?.GetType() == mollierGroupable.GetType() && ((IUIMollierObject)this[i]).Guid == ((IUIMollierObject)mollierGroupable).Guid)
            //    {
            //        RemoveAt(i);
            //    }
            //    else
            //    {
            //        i++;
            //    }
            //}
        }

        public void Update(IUIMollierObject uIMollierObject, bool includeNestedObjects = true)
        {
            if(!(uIMollierObject is IMollierGroupable))
            {
                return;
            }

            for (int i = 0; i < Count; i++)
            {
                if (includeNestedObjects && this[i] is MollierGroup)
                {
                    ((MollierGroup)this[i]).Update(uIMollierObject, includeNestedObjects);
                }

                if (this[i]?.GetType() == uIMollierObject.GetType() && ((IUIMollierObject)this[i]).Guid == uIMollierObject.Guid)
                {
                    this[i] = uIMollierObject as IMollierGroupable;
                }
            }


            //int i = 0;
            //while (i < Count)
            //{
            //    if (includeNestedObjects && this[i] is MollierGroup)
            //    {
            //        ((MollierGroup)this[i]).Update(uIMollierObject, includeNestedObjects);
            //    }

            //    if (this[i]?.GetType() == uIMollierObject.GetType() && ((IUIMollierObject)this[i]).Guid == uIMollierObject.Guid)
            //    {
            //        this[i] = uIMollierObject as IMollierGroupable;
            //    }
            //    else
            //    {
            //        i++;
            //    }
            //}
        }

        /// <summary>
        /// Replaces every occurrence of the MollierObject_Old by mollierObject_New
        /// Method has an additional option to search any depth trough all elements
        /// </summary>
        /// <param name="mollierGroupable_Old">Old mollier object</param>
        /// <param name="mollierGroupable_New">New mollier object</param>
        /// <param name="includeNestedObjects">includeNestedObjects</param>
        public void Update<T>(T mollierGroupable_Old, T mollierGroupable_New, bool includeNestedObjects = true) where T : IMollierGroupable
        {
            for(int i = Count - 1; i >= 0; i--)
            {
                if(includeNestedObjects && this[i] is MollierGroup)
                {
                    ((MollierGroup)this[i]).Update(mollierGroupable_Old, mollierGroupable_New, includeNestedObjects);
                }

                if (this[i] == (object)mollierGroupable_Old)
                {
                    this[i] = mollierGroupable_New;
                }
            }
        }
        

        public IEnumerator<IMollierGroupable> GetEnumerator()
        {
            return base.GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        List<IMollierGroupable> Objects
        {
            get
            {
                return this.ToList();
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
    }
}
