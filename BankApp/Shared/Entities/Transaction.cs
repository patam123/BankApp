using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApp.Shared.Entities
{
    public class Transaction
    {
        public Transaction()
        {

        }
        public Transaction(DateTime transactionDate, string name, Category category, decimal amount)
        {
            TransactionDate = transactionDate;
            Name = name;
            Category = category;
            Amount = Math.Round(amount, 2);
        }

        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
        public decimal Amount { get; set; }
    }
}
