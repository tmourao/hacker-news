using System.Collections.Generic;
using System.Linq;

namespace News.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool HasData<T>(this IEnumerable<T> data)
        {
            return data != null && data.Any();
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> data)
        {
            return data == null || !data.Any();
        }
    }
}
