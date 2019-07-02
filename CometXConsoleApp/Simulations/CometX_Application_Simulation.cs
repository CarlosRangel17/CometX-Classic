using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using CometX.Application.Managers;
using CometX.Application.Models;
using CometX.ConsoleApp.Models.APAAcct;
using CometX.ConsoleApp.Models.APALog;

namespace CometX.ConsoleApp.Simulations
{
    public static class CometX_Application_Simulation
    {
        public static readonly CometXManager _cometXManager_Acct = new CometXManager("APATables_ACCTEntities");
        public static readonly CometXManager _cometXManager_APALog = new CometXManager("APALog");

        public static void Run()
        {
            // Testing out Any 
            //var result_singleCondition = Any_SingleCondition();
            //var result_multiCondition = Any_MultiCondition();

            // Testing out Sorted Table
            //var result_skipTakeCondition = SortedTable_SkipTakeCondition<APA_ApplicationLog>();
            //var result_skipTakeWhereCondition = SortedTable_SkipTakeWhereCondition();

            // Test out INSERT
            var result_insertWithContext = Insert_WithContext();
        }

        public static bool Any_SingleCondition()
        {
            return _cometXManager_Acct.Any<UserSetting>(x => x.Username == "A04744" && x.FirstName == "Carlos");
        }

        public static bool Any_MultiCondition()
        {
            return _cometXManager_Acct.Any<UserSetting>(x => x.Username == "A04744" && x.FirstName == "" && x.LastName == "" && x.IsEntryValidationRequired == true);
        }

        public static APA_ApplicationLog Insert_WithContext()
        {
            var entity = new APA_ApplicationLog
            {
                Date = DateTime.Now,
                Thread = "TEST",
                Level = "TEST",
                Logger = "TEST",
                Message = "TEST",
                Exception = "",
                ApplicationName = "TEST",
                ApplicationLibrary = "TEST",
                Username = "TEST",
                Filename = "TEST"
            };

            _cometXManager_APALog.InsertWithContext(ref entity);
            return entity;
        }

        public static List<T> SortedTable_SkipTakeCondition<T>() where T : new()
        {
            return _cometXManager_APALog.GetSortedTable<T>(SortDirection.Descending, "Id", 0, 10);
        }

        public static List<APA_ApplicationLog> SortedTable_SkipTakeWhereCondition() 
        {
            var today = DateTime.Now;
            return _cometXManager_APALog.GetSortedTable<APA_ApplicationLog>(SortDirection.Descending, "Id", 0, 10, x => x.Date < today);
        }
    }
}
