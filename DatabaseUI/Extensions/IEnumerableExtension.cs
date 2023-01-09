using System.Collections.Generic;
using System;

namespace DatabaseUI.Extensions
{
    public static class IEnumerableExtension
    {
        public static int IndexOf<T>(this IEnumerable<T> values, Func<T, bool> func)
        {
            int i = 0;
            foreach (var value in values)
            {
                if (func.Invoke(value)) return i;
                i++;
            }
            return -1;
        }
    }
}
