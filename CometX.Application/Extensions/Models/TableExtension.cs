using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using CometX.Application.Models.Table;
using CometX.Application.Extensions.General;

namespace CometX.Application.Extensions.Models
{
    public static class TableExtension
    {
        public static void ToTableModel<T>(this Table table, IEnumerable<T> records) where T : new()
        {
            if (table.Columns == null) table.Columns = new List<Column>();
            if (table.ValuesList == null) table.ValuesList = new List<ColumnValue>();

            Type recordType = typeof(T);
            List<PropertyInfo> properties = typeof(T).GetProperties().ToList();

            properties.ForEach(x => table.Columns.Add(new Column
            {
                Name = x.Name,
                Type = x.PropertyType.ToString()
            }));


            foreach (var record in records)
            {
                var values = new List<Value>();

                properties.ForEach(x => values.Add(new Value
                {
                    Name = x.Name,
                    Type = x.PropertyType.ToString(),
                    DataValue = x.GetValue(record) != null ? x.GetValue(record).ToString() : "",
                    IsPrimaryKey = x.HasPrimaryKeyAttribute()
                }));

                table.ValuesList.Add(new ColumnValue
                {
                    Values = values,
                    Keys = values.Any(x => x.IsPrimaryKey) ? values.Where(x => x.IsPrimaryKey).ToDictionary(x => x.Name, x => x.DataValue) : new Dictionary<string, string>()
                });
            }
        }
    }
}
