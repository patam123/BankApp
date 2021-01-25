using System;
using System.Collections.Generic;
using System.Text;
using Google.Cloud.Firestore;


namespace BankApp.Shared.Entities
{
    [FirestoreData]
    public class User
    {
        public string Id { get; set; }
        [FirestoreProperty]
        public string FirstName { get; set; }
        [FirestoreProperty]
        public string LastName { get; set; }
        [FirestoreProperty]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
