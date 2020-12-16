using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Shared.Entities
{
    public class BankAccount
    {
        public string Id { get; set; }
        public List<Category> Categories { get; set; }
        public List<Transaction> Transactions { get; set; }
        public decimal Amount { get; set; } // kanske inte behövs? ta summan av alla transaktioner.
    }
}
