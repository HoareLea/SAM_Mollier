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
            
            List<IMollierObject> mollierObjects = GetMollierObjects(mollierObject.GetType());
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
        public void Regenerate()
        {
            if(dictionary == null || dictionary.Count == 0)
            {
                return;
            }
        }

        public List<IMollierObject> GetMollierObjects(Type type)
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


        public List<T> GetMollierObjects<T>() where T : IMollierObject
        {
            return GetMollierObjects(typeof(T))?.FindAll(x => x is T)?.ConvertAll(x => (T)(object)x);
        }


    }
}
