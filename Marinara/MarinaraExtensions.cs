using System.Collections.Generic;
using System.Linq;

namespace ExtensionMethods
{
    public static class MyExtensions
    {
        public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> @this)
        {
            var enumerators = @this.Select(t => t.GetEnumerator()).Where(e => e.MoveNext());

            while (enumerators.Any())
            {
                yield return enumerators.Select(e => e.Current);
                enumerators = enumerators.Where(e => e.MoveNext());
            }
        }
    }
}