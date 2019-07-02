using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CometX.ConsoleApp.Simulations
{
    public abstract class CometX_Base_Simulation
    {
        protected string ConnectionString { get; private set; }

        public CometX_Base_Simulation(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public abstract void Run();
    }
}
