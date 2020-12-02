using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApp.Shared
{
    public class Transaction
    {
        public Transaction()
        {

        }
        public Transaction(DateTime transactionDate, string name, Category category, decimal sum)
        {
            TransactionDate = transactionDate;
            Name = name;
            Category = category;
            Sum = Math.Round(sum, 2);
        }

        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
        public decimal Sum { get; set; }
    }
}
