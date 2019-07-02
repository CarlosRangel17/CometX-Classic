using System;

namespace CometX.ConsoleApp.Models.APAServices
{
    public class APA_AttendanceLog_VisitorLog
    {
        public string VisitorName { get; set; }

        public string VisitorCompany { get; set; }

        public string PersonDeptVisited { get; set; }

        public string BadgeNumber { get; set; }

        public int? EventID { get; set; }

        public DateTime CheckInDateTime { get; set; }

        public DateTime? CheckOutDatetime { get; set; }

        public string CheckOutStatus { get; set; }
    }
}
