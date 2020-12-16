using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace BankApp.Shared.Entities
{
    [FirestoreData]
    public class Transaction
    {
        public Transaction()
        {

        }
        public Transaction(DateTime transactionDate, string name, Category category, decimal amount)
        {
            TransactionDate = transactionDate;
            Description = name;
            Category = category;
            Amount = Math.Round(amount, 2);
        }

        [FirestoreProperty]
        public int TransactionId { get; set; }
        [FirestoreProperty]
        public DateTime TransactionDate { get; set; }
        [FirestoreProperty]
        public string Description { get; set; } // döpa om till description.
        [FirestoreProperty]
        public Category Category { get; set; } // byta till string och CategoryId.
        [FirestoreProperty]
        public decimal Amount { get; set; }
    }
}
