using System;
using CometX.Application.Attributes;

namespace CometX.ConsoleApp.Models.APALog
{
    public class APA_ApplicationLog
    {
        [PrimaryKey]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Thread { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationLibrary { get; set; }
        public string Username { get; set; }
        public string Filename { get; set; }
        public int? LogGroup { get; set; }
        [PropertyNotMapped]
        public string DateString { get; set; }

        public APA_ApplicationLog()
        {

        }
    }
}
