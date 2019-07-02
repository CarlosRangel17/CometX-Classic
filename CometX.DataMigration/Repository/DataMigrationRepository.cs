using System;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using InternalDataMigration.DataMigration.Models;
using InternalDataMigration.DataMigration.Queries;
using CometX.Application.Utilities;

namespace InternalDataMigration.DataMigration.Repository
{
    public class DataMigrationRepository
    {
        private static string ConnectionString { get; set; }
        private readonly SqlUtils SqlUtil;

        #region constructor(s)
        public DataMigrationRepository()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["APAMASTER"].ConnectionString;
            SqlUtil = new SqlUtils(ConnectionString);
        }
        public DataMigrationRepository(string connectionString = "")
        {
            ConnectionString = connectionString ?? ConfigurationManager.ConnectionStrings["APAMASTER"].ConnectionString;
            SqlUtil = new SqlUtils(ConnectionString);
        }
        #endregion

        #region public methods
        public List<Schema> GetSchemas()
        {
            var records = new List<Schema>();

            try
            {
                records = SqlUtil.GetMultipleInfo<Schema>(Query.SELECT_SCHEMAS, null, true);
            }
            catch (Exception ex)
            {
                var message = "The following error(s) were thrown @method GetSchemas:\n" + ex.Message;

                while (ex.InnerException != null)
                {
                    ex = ex.InnerException.InnerException;
                    message += "\n" + ex.Message;
                }

                //RollBack(RollbackCase.GetSchemas);
                throw new Exception(message);
            }

            return records;
        }

        public List<Table> GetTables(string catalog)
        {
            var records = new List<Table>();

            try
            {
                var query = string.Format("USE {0} {1}", catalog, Query.SELECT_TABLES);
                records = SqlUtil.GetMultipleInfo<Table>(query, null, true);
            }
            catch (Exception ex)
            {
                var message = "The following error(s) were thrown @method GetTables:\n" + ex.Message;

                while (ex.InnerException != null)
                {
                    ex = ex.InnerException.InnerException;
                    message += "\n" + ex.Message;
                }

                //RollBack(RollbackCase.GetTables);
                throw new Exception(message);
            }

            return records;
        }
        #endregion

    }
}
