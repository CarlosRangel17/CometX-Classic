using System;
using CometX.Application.Managers;
using CometX.ConsoleApp.Models.APAServices;

namespace CometX.ConsoleApp.Simulations
{
    public static class APA_AttendanceLog_VisitorLog_Simulation
    {
        public static readonly CometXManager _cometXManager = new CometXManager("APAServices");

        public static void Run()
        {
            // Create Attendance Log object [1]
            var visitorLog_1 = new APA_AttendanceLog_VisitorLog
            {
                BadgeNumber = "E04744",
                CheckOutStatus = "IN",
                VisitorName = "Carlos Rangel",
                VisitorCompany = "Sogeti Capgemeni",
                PersonDeptVisited = "APA",
                EventID = null
            };

            Checkin(visitorLog_1);

            // Create Attendance Log object [2]
            var visitorLog_2 = new APA_AttendanceLog_VisitorLog
            {
                BadgeNumber = "E04755",
                CheckOutStatus = "IN",
                VisitorName = "Puneet Mittal",
                VisitorCompany = "Sogeti Capgemeni",
                PersonDeptVisited = "APA",
                EventID = null
            };

            Checkin(visitorLog_2);

            var member_1 = FindMember("E04744");

            var member_2 = FindMember("E04755");

            Checkout(member_1);
            Checkout(member_2);
        }

        public static APA_AttendanceLog_VisitorLog FindMember(string badgeId)
        {
            // Initialize CometXManager object 
            return _cometXManager.FirstOrDefault<APA_AttendanceLog_VisitorLog>(x => x.BadgeNumber == badgeId && x.CheckOutStatus == "IN");
        }

        public static void Checkout(APA_AttendanceLog_VisitorLog visitorLog)
        {
            // Check-Out logic
            visitorLog.CheckOutDatetime = DateTime.Now;

            var badgeNumber = visitorLog.BadgeNumber;
            // Update
            _cometXManager.Update(visitorLog, x => x.BadgeNumber == visitorLog.BadgeNumber && x.CheckOutStatus == "IN");

        }

        public static void Checkin(APA_AttendanceLog_VisitorLog visitorLog)
        {
            // Check-In logic
            visitorLog.CheckInDateTime = DateTime.Now;

            // Insert
            _cometXManager.Insert(visitorLog);
        }
    }
}
