using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankConsoleApp.Models
{
    public class InterestRule
    {
        public string Date { get; set; }
        public string RuleId { get; set; }
        public decimal Rate { get; set; }

        public InterestRule(string date, string ruleId, decimal rate)
        {
            Date = date;
            RuleId = ruleId;
            Rate = rate;
        }
    }
}
