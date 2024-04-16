using System;
using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public class MollierModel : SAMModel
    {
        private MollierSettings mollierSettings;
        private Dictionary<Type, List<IMollierObject>> dictionary;

        public MollierModel()
        {
            dictionary = new Dictionary<Type, List<IMollierObject>>();
        }
        
        public MollierModel(string name, MollierSettings mollierSettings)
            :base(name)
        {
            this.mollierSettings = mollierSettings == null ? null : new MollierSettings(mollierSettings);
            dictionary = new Dictionary<Type, List<IMollierObject>>();
        }
        
        public MollierModel(string name, MollierSettings mollierSettings, Dictionary<Type, List<IMollierObject>> dictionary)
            :base(name)
        {
            this.mollierSettings = mollierSettings == null ? null : new MollierSettings(mollierSettings);
            this.dictionary = dictionary;
        }

        public bool Add(IMollierObject mollierObject)
        {
            if(mollierObject == null)
            {
                return false;
            }

            if(!dictionary.ContainsKey(mollierObject.GetType()))
            {
                dictionary[mollierObject.GetType()] = new List<IMollierObject>();
            }
            dictionary[mollierObject.GetType()].Add(mollierObject);

            return true;
        }
        
        public bool AddRange(IEnumerable<IMollierObject> mollierObjects)
        {
            if(mollierObjects == null)
            {
                return false;
            }

            foreach(IMollierObject mollierObject in mollierObjects)
            {
                Add(mollierObject);
            }
            return true;
        }
        
        public bool Remove(IMollierObject mollierObject, bool includeNestedObjects = true)
        {
            if(mollierObject == null)
            {
                return false;
            }
            
            List<IMollierObject> mollierObjects = GetMollierObjects(mollierObject.GetType(), false);
            for(int i = mollierObjects.Count - 1; i >= 0; i--)
            {
                IMollierObject mollierObject_Temp = mollierObjects[i];
                if (mollierObject.AlmostEqual(mollierObject_Temp))
                {
                    mollierObjects.Remove(mollierObject_Temp);
                    dictionary[mollierObject.GetType()].Remove(mollierObject_Temp);
                }
            }

            List<IMollierGroup> mollierGroups = GetMollierObjects<IMollierGroup>();
            if(mollierGroups == null || !(mollierObject is IMollierGroupable) || !includeNestedObjects)
            {
                return false;
            }
            foreach(IMollierGroup mollierGroup in mollierGroups)
            {
                IMollierGroupable mollierGroupable = (IMollierGroupable)mollierObject;
                ((MollierGroup)mollierGroup).RemoveObject(mollierGroupable);
            }
            return false;
        }
        
        public void Regenerate()
        {
            if(dictionary == null || dictionary.Count == 0)
            {
                return;
            }
        }

        public void Update<T>(IEnumerable<Tuple<T, T>> mollierObjects, bool includeNestedObjects = true) where T : IMollierObject
        {
            if(mollierObjects == null)
            {
                return;
            }

            foreach(var objectsPair in mollierObjects)
            {
                Update(objectsPair.Item1, objectsPair.Item2, includeNestedObjects);
            }
        }
        
        /// <summary>
        /// Replaces every occurrence of the MollierObject_Old by mollierObject_New
        /// Method has an additional option to search any depth trough all elements
        /// </summary>
        /// <param name="mollierObject_Old">Old mollier object</param>
        /// <param name="mollierObject_New">New mollier object</param>
        /// <param name="includeNestedObjects">includeNestedObjects</param>
        public void Update<T>(T mollierObject_Old, T mollierObject_New, bool includeNestedObjects = true) where T : IMollierObject
        {
            if(mollierObject_Old == null || mollierObject_New == null || mollierObject_Old.GetType() != mollierObject_New.GetType())
            {
                return;
            }


            foreach (KeyValuePair<Type, List<IMollierObject>> keyValuePair in dictionary)
            {
                if (keyValuePair.Value == null || keyValuePair.Value.Count == 0)
                {
                    continue;
                }
                if(includeNestedObjects && (keyValuePair.Key == typeof(MollierGroup) || keyValuePair.Key == typeof(UIMollierGroup)))
                {
                    foreach(MollierGroup mollierGroup in keyValuePair.Value)
                    {
                        mollierGroup.Update((IMollierGroupable)mollierObject_Old, (IMollierGroupable)mollierObject_New, includeNestedObjects);
                    }
                }

                if (!mollierObject_Old.GetType().IsAssignableFrom(keyValuePair.Key))
                {
                    continue;
                }

                for(int i = keyValuePair.Value.Count - 1; i >= 0; i--)
                {
                    if (keyValuePair.Value[i] == (object)mollierObject_Old)
                    {
                        keyValuePair.Value[i] = mollierObject_New;
                    }
                }
            }
        }

        public void Update<T>(T uIMollierObject, bool includeNestedObjects = true) where T : IUIMollierObject
        {
            if (uIMollierObject == null)
            {
                return;
            }

            T mollierObject = GetUIMollierObject<T>(uIMollierObject.Guid, false);
            if(mollierObject != null)
            {
                dictionary[mollierObject.GetType()].Remove(mollierObject);
                dictionary[uIMollierObject.GetType()].Add(uIMollierObject);
                if (!includeNestedObjects)
                {
                    return;
                }
            }

            List<IMollierGroup> mollierGroups = GetMollierObjects<IMollierGroup>();
            if (mollierGroups == null || !(uIMollierObject is IMollierGroupable) || !includeNestedObjects)
            {
                return;
            }

            foreach (IMollierGroup mollierGroup in mollierGroups)
            {
                ((MollierGroup)mollierGroup).Update(uIMollierObject, true);
            }
        }

        public void Clear()
        {
            dictionary?.Clear();
        }
        
        public void Clear<T>() where T: IMollierGroupable
        {
            if (dictionary != null)
            {
                foreach(KeyValuePair<Type, List<IMollierObject>> keyValuePair in dictionary)
                {
                    if (typeof(T).IsAssignableFrom(keyValuePair.Key))
                    {
                        dictionary.Remove(keyValuePair.Key);
                        return;
                    }
                }
            }
        }
        
        public List<IMollierObject> GetMollierObjects(Type type, bool includeNestedObjects = true)
        {
            if (dictionary == null || dictionary.Count == 0 || type == null)
            {
                return null;
            }

            List<IMollierObject> result = new List<IMollierObject>();
            foreach (KeyValuePair<Type, List<IMollierObject>> keyValuePair in dictionary)
            {
                if (keyValuePair.Value == null || keyValuePair.Value.Count == 0)
                {
                    continue;
                }

                if((keyValuePair.Key.IsAssignableFrom(typeof(MollierGroup)) || keyValuePair.Key.IsAssignableFrom(typeof(UIMollierGroup))) && includeNestedObjects)
                {
                    List<IMollierObject> mollierObjects = keyValuePair.Value;
                    foreach (IMollierObject mollierObject in mollierObjects)
                    {
                        result.AddRange(((MollierGroup)mollierObject).GetObjects(type));
                    }
                }    

                if (!type.IsAssignableFrom(keyValuePair.Key))
                {
                    continue;
                }

                foreach (IMollierObject mollierObject in keyValuePair.Value)
                {
                    if (mollierObject == null)
                    {
                        continue;
                    }

                    result.Add(mollierObject);
                }
            }

            return result;
        }

        public T GetUIMollierObject<T>(Guid guid, bool includeNestedObjects = true) where T : IUIMollierObject
        {
            if(dictionary == null || guid == Guid.Empty)
            {
                return default;
            }

            List<IMollierObject> mollierObjects = GetMollierObjects(typeof(T), includeNestedObjects);
            if(mollierObjects == null || mollierObjects.Count == 0)
            {
                return default;
            }

            foreach(IMollierObject mollierObject in mollierObjects)
            {
                if(!(mollierObject is T))
                {
                    continue;
                }

                T uIMollierObject = (T)mollierObject;
                
                if(uIMollierObject.Guid == guid)
                {
                    return uIMollierObject;
                }
            }

            return default;
        }

        public List<T> GetMollierObjects<T>(bool includeNestedObjects = true) where T : IMollierObject
        {
            return GetMollierObjects(typeof(T), includeNestedObjects)?.FindAll(x => x is T)?.ConvertAll(x => (T)(object)x);
        }


    }
}
