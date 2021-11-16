using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StyleParserCS.utils
{
    public static class Extensions
    {
        public static void AddRange<T>(this IList<T> list, IList<T> other)
        {
            foreach (T item in other)
            {
                list.Add(item);
            }
        }

        public static TV GetValue<TK, TV>(this IDictionary<TK, TV> dict, TK key, TV defaultValue = default(TV))
        {
            return dict.TryGetValue(key, out TV value) ? value : defaultValue;
        }
    }
}
