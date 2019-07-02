using InternalDataMigration.DataMigration.Manager;
using InternalDataMigration.DataMigration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CometX.ConsoleApp.Simulations
{
    public class CometX_DataMigration_Simulation : CometX_Base_Simulation
    {
        protected DataMigrationManager DataMigrationManager = new DataMigrationManager();
        public CometX_DataMigration_Simulation(string connectionString = "") : base(connectionString)
        {
        }

        public override void Run()
        {
            List<Network> networks = GetNetworks();
        }

        public List<Network> GetNetworks()
        {
            return DataMigrationManager.GetSystemInfo();
        }
    }
}
