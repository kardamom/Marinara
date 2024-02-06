using System.Collections;
using System.Linq;
using System.Collections.Generic;
using ExtensionMethods;
using Rhino.Geometry;
using System;


namespace ExtensionMethods
{
    public static class MyExtensions
    {

        public static IEnumerable<IEnumerable<T>> Transpose<T>(IEnumerable<IEnumerable<T>> @this)
        {
            var enumerators = @this.Select(t => t.GetEnumerator()).Where(e => e.MoveNext());

            while (enumerators.Any())
            {
                yield return enumerators.Select(e => e.Current);
                enumerators = enumerators.Where(e => e.MoveNext());
            }
        }

        public static List<List<T>> Transpose<T>(this List<List<T>> lists)
        {
            var longest = lists.Any() ? lists.Max(l => l.Count) : 0;
            List<List<T>> outer = new List<List<T>>(longest);
            for (int i = 0; i < longest; i++)
                outer.Add(new List<T>(lists.Count));
            for (int j = 0; j < lists.Count; j++)
                for (int i = 0; i < longest; i++)
                    outer[i].Add(lists[j].Count > i ? lists[j][i] : default(T));
            return outer;
        }

    }
}
