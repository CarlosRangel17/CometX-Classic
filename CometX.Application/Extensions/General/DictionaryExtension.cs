using System;
using System.Collections.Generic;

namespace CometX.Application.Extensions.General
{
    public static class DictionaryExtension
    {
        public static void AddRange<T>(this ICollection<T> target, IEnumerable<T> source)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (source == null) throw new ArgumentNullException(nameof(source));
            foreach (var element in source) target.Add(element);
        }
    }
}
