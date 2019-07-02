using System;
using CometX.Application.Attributes;

namespace CometX.ConsoleApp.Models.APAAcct
{
    public class UserSetting
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsEntryValidationRequired { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        [PropertyNotMapped]
        public string CreatedOnString { get; set; }
        public UserSetting()
        {

        }

        public UserSetting(string username)
        {
            Username = username;
            FirstName = "";
            LastName = "";
            IsEntryValidationRequired = true;
            IsActive = true;
            CreatedOn = DateTime.Now;
        }
        public void SetDateStrings()
        {
            CreatedOnString = CreatedOn.ToString("MM/dd/yyyy");
        }
    }
}
