using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Shared.Entities
{
    [FirestoreData]
    public class ExpenseLimit
    {
        public string Id { get; set; }
        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public string CategoryId { get; set; }
        [FirestoreProperty]
        public string OwnerId { get; set; }
        [FirestoreProperty]
        public double Amount { get; set; }
        [FirestoreProperty]
        public DateTime StartDate { get; set; }
        [FirestoreProperty]
        public DateTime EndDate { get; set; }
    }
}
