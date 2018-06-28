using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nhea.Data
{
    public static class ExtensionMethods
    {
        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> enumerable, Func<TSource, TSource, bool> comparer)
        {
            return enumerable.Distinct(new LambdaComparer<TSource>(comparer));
        }
    }
}
