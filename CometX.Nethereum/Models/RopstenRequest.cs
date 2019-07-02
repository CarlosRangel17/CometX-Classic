using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CometX.Nethereum.Models
{ 
    public class RopstenRequest
    {
        public virtual string status { get; set; }
        public virtual string message { get; set; }
        public virtual List<RopstenTransaction> result { get; set; }
    }
}