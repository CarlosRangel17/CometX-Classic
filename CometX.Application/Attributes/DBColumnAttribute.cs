using System;

namespace CometX.Application.Attributes
{
    public class DBColumnAttribute : Attribute
    {
        public string Name { get; set; }
        public DBColumnAttribute(string name)
        {
            this.Name = name;
        }
    }
}
