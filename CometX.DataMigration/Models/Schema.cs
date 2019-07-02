using CometX.Application.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalDataMigration.DataMigration.Models
{
    public class Schema
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        [PropertyNotMapped]
        public string DataSource { get; set; }
        [PropertyNotMapped]
        public List<Table> Tables { get; set; }
    }
}
