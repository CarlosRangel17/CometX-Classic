using System;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace InternalDataMigration.DataMigration.Models
{
    public class Network
    {
        public string ConnectionString { get; set; }
        public string DataSource { get; set; }
        public string InitialCatalog { get; set; }
        public List<Schema> Schemas { get; set; }

        public Network(SqlConnectionStringBuilder conn)
        {
            ConnectionString = conn.ConnectionString;
            DataSource = conn.DataSource;
            InitialCatalog = conn.InitialCatalog;
        }
    }
}
