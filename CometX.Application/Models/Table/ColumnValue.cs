using System.Collections.Generic;

namespace CometX.Application.Models.Table
{
    public class ColumnValue
    {
        public List<Value> Values { get; set; }
        public Dictionary<string, string> Keys { get; set; }
    }
}
