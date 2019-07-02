using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalDataMigration.DataMigration.Queries
{
    public class Query
    {
        public static string SELECT_SCHEMAS = "SELECT database_id AS Id, name AS Name, create_date AS DateCreated FROM sys.databases;";
        public static string SELECT_TABLES = "SELECT TABLE_CATALOG AS Catalog, TABLE_SCHEMA AS [Schema], TABLE_Name AS Name, TABLE_TYPE AS Type FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE';";
    }
}
