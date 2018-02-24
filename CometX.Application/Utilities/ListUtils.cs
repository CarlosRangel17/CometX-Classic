using System;
using System.Linq;
using System.Collections.Generic;
using CometX.Application.Extensions.General;

namespace CometX.Application.Utilities
{
    public static class ListUtils
    {
        public static Dictionary<T, string> GetEnumDictionary<T>()
        {
            var returnDictionary = Enum.GetValues(typeof(T))
                .Cast<T>().ToDictionary(at => at, at => ((Enum)Enum.Parse(typeof(T), at.ToString())).ToDescription());
            return returnDictionary;
        }

        public static Dictionary<string, string> GetEnumStringKeyDictionary<T>()
        {
            var returnDictionary = Enum.GetValues(typeof(T)).Cast<T>().ToDictionary(at => at.ToString(), at => ((Enum)Enum.Parse(typeof(T), at.ToString())).ToDescription());
            return returnDictionary;
        }
    }
}
