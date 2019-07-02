using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CometX.Nethereum.Models
{
    public class RopstenTransaction
    {
        public virtual string blockNumber { get; set; }
        public virtual string timeStamp { get; set; }
        public virtual string hash { get; set; }
        public virtual string nonce { get; set; }
        public virtual string blockHash { get; set; }
        public virtual string from { get; set; }
        public virtual string contractAddress { get; set; }
        public virtual string to { get; set; }
        public virtual string value { get; set; }
        public virtual string tokenName { get; set; }
        public virtual string tokenSymbol { get; set; }
        public virtual string tokenDecimal { get; set; }
        public virtual string transactionIndex { get; set; }
        public virtual string gas { get; set; }
        public virtual string gasPrice { get; set; }
        public virtual string gasUsed { get; set; }
        public virtual string cumulativeGasUsed { get; set; }
        public virtual string input { get; set; }
        public virtual string confirmations { get; set; }


        public int Nonce()
        {
            return Convert.ToInt32(nonce);
        }
    }
}