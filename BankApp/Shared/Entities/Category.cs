using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace BankApp.Shared.Entities
{
    [FirestoreData]
    public class Category
    {
        public string Id { get; set; }

        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public string Color { get; set; }

        [FirestoreProperty]
        public string OwnerId { get; set; }

        public double TransactionSum { get; set; }
        public bool IsBeingModified { get; set; }
    }
}
