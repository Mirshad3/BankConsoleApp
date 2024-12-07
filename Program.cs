using BankConsoleApp;
using BankConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BankSystemApp
{   

    class Program
    {
        static void Main(string[] args)
        {
            var bankSystem = new BankSystem();
            while (true)
            {
                Console.WriteLine("\nWelcome to AwesomeGIC Bank! What would you like to do?");
                Console.WriteLine("[T] Input transactions");
                Console.WriteLine("[I] Define interest rules");
                Console.WriteLine("[P] Print statement");
                Console.WriteLine("[Q] Quit");

                var choice = Console.ReadLine()?.Trim().ToUpper();
                switch (choice)
                {
                    case "T":
                        HandleTransactions(bankSystem);
                        break;
                    case "I":
                        HandleInterestRules(bankSystem);
                        break;
                    case "P":
                        HandlePrintStatement(bankSystem);
                        break;
                    case "Q":
                        Console.WriteLine("Thank you for banking with AwesomeGIC Bank.");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void HandleTransactions(BankSystem bankSystem)
        {
            while (true)
            {
                Console.WriteLine(
                    "Please enter transaction details in <Date> <Account> <Type> <Amount> format" +
                    "\n(or enter blank to go back to the main menu):");
                var userInput = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(userInput)) break;

                try
                {
                    var parts = userInput.Split();
                    var date = parts[0];
                    var account = parts[1];
                    var txType = parts[2];
                    var amount = decimal.Parse(parts[3]);

                    bankSystem.AddTransaction(date, account, txType, amount);
                    bankSystem.ListTransactions(account);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private static void HandleInterestRules(BankSystem bankSystem)
        {
            while (true)
            {
                Console.WriteLine(
                    "Please enter interest rules details in <Date> <RuleId> <Rate in %> format" +
                    "\n(or enter blank to go back to the main menu):");
                var userInput = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(userInput)) break;

                try
                {
                    var parts = userInput.Split();
                    var date = parts[0];
                    var ruleId = parts[1];
                    var rate = decimal.Parse(parts[2]);

                    bankSystem.AddInterestRule(date, ruleId, rate);
                    bankSystem.ListInterestRules();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private static void HandlePrintStatement(BankSystem bankSystem)
        {
            while (true)
            {
                Console.WriteLine(
                    "Please enter account and month to generate the statement <Account> <Year><Month>" +
                    "\n(or enter blank to go back to the main menu):");
                var userInput = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(userInput)) break;

                try
                {
                    var parts = userInput.Split();
                    var account = parts[0];
                    var yearMonth = parts[1];

                    bankSystem.CalculateInterest(account, yearMonth);
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input. Please follow the specified format.");
                }
            }
        }
    }
}
