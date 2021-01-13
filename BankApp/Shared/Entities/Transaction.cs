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
        public string Id { get; set; }
        [FirestoreProperty]
        public string TransactionId { get; set; }
        [FirestoreProperty]
        public DateTime TransactionDate { get; set; }
        [FirestoreProperty]
        public string Description { get; set; }
        [FirestoreProperty]
        public string CategoryId { get; set; }
        [FirestoreProperty]
        public double Amount { get; set; }
    }
}
