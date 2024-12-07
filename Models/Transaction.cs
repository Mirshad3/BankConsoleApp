using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankConsoleApp.Models
{
    public class Transaction
    {
        public string Date { get; set; }
        public string Account { get; set; }
        public string TxType { get; set; }
        public decimal Amount { get; set; }
        public string TxnId { get; set; }

        public Transaction(string date, string account, string txType, decimal amount, string txnId)
        {
            Date = date;
            Account = account;
            TxType = txType;
            Amount = amount;
            TxnId = txnId;
        }
    }

}
