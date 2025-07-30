using System;
using System.Collections.Generic;

namespace PropertyBag
{
    public class PropertyBag<T>
    {
        private Dictionary<string, List<T>> kvp = new Dictionary<string, List<T>>(StringComparer.InvariantCultureIgnoreCase);

        public T this[string PropertyName, int pos = 0]
        {
            get
            {
                if (! kvp.ContainsKey(PropertyName))
                {
                    return default(T);
                }

                return kvp[PropertyName][pos];
            }
            set
            {
                if (!kvp.ContainsKey(PropertyName))
                {
                    kvp.Add(PropertyName, new List<T>(pos+32));
                }

                while(kvp[PropertyName].Count < pos)
                {
                    kvp[PropertyName].Add(default(T));
                }

                kvp[PropertyName].Add(value);
            }
        }

        public List<T> this[string PropertyName]
        {
            get
            {
                if (!kvp.ContainsKey(PropertyName))
                {
                    return null;
                }

                return kvp[PropertyName];
            }
            set
            {
                if (!kvp.ContainsKey(PropertyName))
                {
                    kvp.Add(PropertyName, value);
                }
            }
        }

    }
}
