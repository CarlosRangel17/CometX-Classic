using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using InternalDataMigration.DataMigration.Models;
using InternalDataMigration.DataMigration.Repository;
using System.Configuration;
using System.Data.SqlClient;

namespace InternalDataMigration.DataMigration.Manager
{
    public class DataMigrationManager
    {
        //protected internal DataMigrationRepository _repo;

        #region constructor(s)
        public DataMigrationManager()
        {
            //_repo = new DataMigrationRepository();
        }

        //public DataMigrationManager(string connectionString)
        //{
        //    _repo = new DataMigrationRepository(connectionString);
        //}
        #endregion

        #region public methods
        public List<Network> GetSystemInfo()
        {
            var networks = new List<Network>();

            try
            {
                foreach (ConnectionStringSettings connectionString in ConfigurationManager.ConnectionStrings ?? new ConnectionStringSettingsCollection())
                {
                    var network = new Network(new SqlConnectionStringBuilder(connectionString.ConnectionString));

                    if (string.IsNullOrWhiteSpace(network.InitialCatalog)) continue;

                    var _dataMigrationRepo = new DataMigrationRepository(connectionString.ConnectionString);

                    network.Schemas = _dataMigrationRepo.GetSchemas();

                    //schemas.ForEach(x => x.Tables = _dataMigrationRepo.GetTables(x.Name));
                    foreach (var schema in network.Schemas)
                    {
                        try
                        {
                            schema.Tables = _dataMigrationRepo.GetTables(schema.Name);
                        }
                        catch (Exception ex)
                        {
                            // May not have access to DB
                        }
                    }

                    networks.Add(network);
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;

                while (ex.InnerException != null)
                {
                    ex = ex.InnerException.InnerException;
                    message += "\n" + ex.Message;
                }

                throw new Exception(message);
            }

            return networks;
        }

        //TODO: Write logic to migrate from one table to another
        public void MigrateTableData(Table table1, Table table2)
        {
            try
            {


            }
            catch (Exception ex)
            {
                var message = ex.Message;

                while (ex.InnerException != null)
                {
                    ex = ex.InnerException.InnerException;
                    message += "\n" + ex.Message;
                }

                throw new Exception(message);
            }
        }
        #endregion
    }
}
