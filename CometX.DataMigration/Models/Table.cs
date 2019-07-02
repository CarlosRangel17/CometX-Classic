using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using CometX.Application.Attributes;

namespace InternalDataMigration.DataMigration.Models
{
    public class Table
    {
        public string Catalog { get; set; }
        public string Name { get; set; }
        public string Schema { get; set; }
        public string Type { get; set; }
        [PropertyNotMapped]
        public string DataSource { get; set; }
        [PropertyNotMapped]
        public bool Obfuscate { get; set; }
    }
}
