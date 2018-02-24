using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace CometX.Application.Models.Table
{
    public class Table
    {
        public List<Column> Columns { get; set; }
        public List<ColumnValue> ValuesList { get; set; }

        public Table()
        {
            Columns = new List<Column>();
            ValuesList = new List<ColumnValue>();
        }
    }
}
