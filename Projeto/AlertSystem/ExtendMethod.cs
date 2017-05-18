using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertSystem
{
    public static class MetodoExtensao
    {
        public static IEnumerable<TResult> ZipDeTres<T1, T2, T3, TResult>(
            this IEnumerable<T1> primeiro,
            IEnumerable<T2> segundo,
            IEnumerable<T3> terceiro,
            Func<T1, T2, T3, TResult> func)
        {
            using (var e1 = primeiro.GetEnumerator())
            using (var e2 = segundo.GetEnumerator())
            using (var e3 = terceiro.GetEnumerator())
            {
                while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext())
                    yield return func(e1.Current, e2.Current, e3.Current);
            }
        }
    }

}
