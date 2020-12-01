using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApp.Client.Shared
{
    public partial class Transaction
    {
        public Transaction()
        {

        }
        public Transaction(DateTime transactionDate, string name, string category, decimal sum)
        {
            TransactionDate = transactionDate;
            Name = name;
            Category = category;
            Sum = sum;
        }
        [Parameter] public DateTime TransactionDate { get; set; }
        [Parameter] public string Name { get; set; }
        [Parameter] public string Category { get; set; }
        [Parameter] public decimal Sum { get; set; }
    }
}
