using BankConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BankConsoleApp
{
    public class BankSystem
    {
        private readonly Dictionary<string, List<Transaction>> _accounts;
        private readonly List<InterestRule> _interestRules;

        public BankSystem()
        {
            _accounts = new Dictionary<string, List<Transaction>>();
            _interestRules = new List<InterestRule>();
        }

        public void ValidateTransaction(string date, string account, string txType, decimal amount)
        {
            if (!Regex.IsMatch(date, @"^\d{8}$"))
                throw new ArgumentException("Date must be in YYYYMMdd format.");

            if (txType.ToUpper() != "D" && txType.ToUpper() != "W")
                throw new ArgumentException("Transaction type must be D (deposit) or W (withdrawal).");

            if (amount <= 0 || decimal.Round(amount, 2) != amount)
                throw new ArgumentException("Amount must be greater than zero with up to two decimal places.");

            if (txType.ToUpper() == "W" && (!_accounts.ContainsKey(account) || !_accounts[account].Any()))
                throw new ArgumentException("The first transaction for an account cannot be a withdrawal.");

            if (txType.ToUpper() == "W" && GetBalance(account) < amount)
                throw new ArgumentException("Insufficient balance. Transaction declined.");
        }

        public void AddTransaction(string date, string account, string txType, decimal amount)
        {
            ValidateTransaction(date, account, txType, amount);
            var txnId = GenerateTransactionId(date, account);
            var transaction = new Transaction(date, account, txType, amount, txnId);

            if (!_accounts.ContainsKey(account))
                _accounts[account] = new List<Transaction>();

            _accounts[account].Add(transaction);
        }

        private string GenerateTransactionId(string date, string account)
        {
            var count = _accounts.ContainsKey(account) ? _accounts[account].Count(t => t.Date == date) + 1 : 1;
            return $"{date}-{count:D2}";
        }

        public decimal GetBalance(string account)
        {
            if (!_accounts.ContainsKey(account)) return 0;

            return _accounts[account]
                .Sum(txn => txn.TxType.ToUpper() == "D" ? txn.Amount : -txn.Amount);
        }

        public void ListTransactions(string account)
        {
            if (!_accounts.ContainsKey(account) || !_accounts[account].Any())
            {
                Console.WriteLine($"No transactions found for account {account}.");
                return;
            }

            Console.WriteLine($"Account: {account}");
            Console.WriteLine("| Date | Txn Id | Type | Amount |");
            foreach (var txn in _accounts[account])
            {
                Console.WriteLine($"| {txn.Date} | {txn.TxnId,-11} | {txn.TxType,-4} | {txn.Amount,8:F2} |");
            }
        }

        public void ValidateInterestRule(string date, string ruleId, decimal rate)
        {
            if (!Regex.IsMatch(date, @"^\d{8}$"))
                throw new ArgumentException("Date must be in YYYYMMdd format.");

            if (rate <= 0 || rate >= 100)
                throw new ArgumentException("Interest rate must be greater than 0 and less than 100.");
        }

        public void AddInterestRule(string date, string ruleId, decimal rate)
        {
            ValidateInterestRule(date, ruleId, rate);
            _interestRules.RemoveAll(r => r.Date == date);
            _interestRules.Add(new InterestRule(date, ruleId, rate));
            _interestRules.Sort((x, y) => string.Compare(x.Date, y.Date, StringComparison.Ordinal));
        }

        public void ListInterestRules()
        {
            if (!_interestRules.Any())
            {
                Console.WriteLine("No interest rules defined.");
                return;
            }

            Console.WriteLine("Interest rules:");
            Console.WriteLine("| Date | RuleId | Rate (%) |");
            foreach (var rule in _interestRules)
            {
                Console.WriteLine($"| {rule.Date} | {rule.RuleId,-6} | {rule.Rate,8:F2} |");
            }
        }

        public void CalculateInterest(string account, string yearMonth)
        {
            if (!_accounts.ContainsKey(account))
            {
                Console.WriteLine($"No transactions found for account {account}.");
                return;
            }

            var transactions = _accounts[account]
                .Where(t => t.Date.StartsWith(yearMonth))
                .ToList();

            if (!transactions.Any())
            {
                Console.WriteLine($"No transactions for {account} in {yearMonth}.");
                return;
            }

            var balance = 0m;
            Console.WriteLine($"Account: {account}");
            Console.WriteLine("| Date | Txn Id | Type | Amount | Balance |");

            foreach (var txn in transactions)
            {
                balance += txn.TxType.ToUpper() == "D" ? txn.Amount : -txn.Amount;
                Console.WriteLine(
                    $"| {txn.Date} | {txn.TxnId,-11} | {txn.TxType,-4} | {txn.Amount,8:F2} | {balance,8:F2} |");
            }
        }
    }

}
