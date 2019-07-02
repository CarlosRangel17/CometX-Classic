using System;
using CometX.ConsoleApp.Simulations;

namespace CometX.ConsoleApp
{
    public class Program
    {
       protected CometX_Base_Simulation Simulation { get; set; }

        static void Main(string[] args)
        {
            try
            {
                //APA_AttendanceLog_VisitorLog_Simulation.Run();
                //CometXFun_Simulation.Run();
                var Simulation = new CometX_DataMigration_Simulation();
                Simulation.Run();
            }
            catch (Exception ex)
            {
                string message = ex.Message;

                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    message += ex.Message;
                }

                throw new Exception(message);
            }
        }        
    }
}
