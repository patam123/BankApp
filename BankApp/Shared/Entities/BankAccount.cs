using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Shared.Entities
{
    public class BankAccount
    {
        public int Id { get; set; }
        public List<Transaction> Transactions { get; set; }
        public decimal Amount { get; set; }
    }
}
