using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApp.Shared.Entities;
using Google.Cloud.Firestore;
using Newtonsoft.Json;

namespace BankApp.Server.DataAccess
{
    public class Firestore
    {
        string projectId;
        FirestoreDb firestore;

        public Firestore()
        {
            /// Enter your file path to your own API-key.
            string filepath = @"C:\Users\patri\Downloads\bankapp-2782c-1efd18eca9b1.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);
            projectId = "bankapp-2782c";
            firestore = FirestoreDb.Create(projectId);
        }

        // Category Methods

        public async Task<List<Category>> GetAllCategories(/*string userId*/)
        {

            try
            {
                var categories = firestore.Collection("categories");
                var categoriesSnaphot = await categories.GetSnapshotAsync();
                var categoryList = new List<Category>();

                foreach (var documentSnapshot in categoriesSnaphot)
                {
                    if (documentSnapshot.Exists)
                    {
                        var category = documentSnapshot.ToDictionary();
                        //if (category.Where(x => x.Key == "OwnerId" && (string)x.Value == userId).Count() > 0)
                        //{
                        string jsonCategory = JsonConvert.SerializeObject(category);
                        var newCategory = JsonConvert.DeserializeObject<Category>(jsonCategory);
                        newCategory.Id = documentSnapshot.Id;
                        categoryList.Add(newCategory);
                        //}
                    }
                }

                var sortedCategories = categoryList.OrderBy(x => x.Name).ToList();
                return sortedCategories;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async void AddCategory(Category category)
        {
            try
            {
                var catRef = firestore.Collection("categories");
                await catRef.AddAsync(category);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async void UpdateCategory(Category category)
        {
            try
            {
                var docRef = firestore.Collection("categories").Document(category.Id);
                await docRef.SetAsync(category, SetOptions.Overwrite);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async void DeleteCategory(string id)
        {
            try
            {
                var docRef = firestore.Collection("categories").Document(id);
                await docRef.DeleteAsync();
            }
            catch (Exception)
            {

                throw;
            }

        }

        // Transaction Methods

        public async Task<List<Shared.Entities.Transaction>> GetTransactions()
        {
            try
            {
                var transactionCollectionRef = firestore.Collection("transactions");
                var transactionSnapshot = await transactionCollectionRef.GetSnapshotAsync();
                var transactionList = new List<Shared.Entities.Transaction>();
                foreach (var documentSnapshot in transactionSnapshot)
                {
                    if (documentSnapshot.Exists)
                    {
                        var transaction = documentSnapshot.ToDictionary();
                        object dateValue;
                        transaction.TryGetValue("TransactionDate", out dateValue);

                        int year = int.Parse(dateValue.ToString().Substring(0, 4));
                        int month = int.Parse(dateValue.ToString().Substring(5, 2));
                        int day = int.Parse(dateValue.ToString().Substring(8, 2));
                        DateTime date = new DateTime(year, month, day);

                        transaction.Remove("TransactionDate");
                        transaction.Add("TransactionDate", date);
                        var jsonTransaction = JsonConvert.SerializeObject(transaction);
                        var newTransaction = JsonConvert.DeserializeObject<Shared.Entities.Transaction>(jsonTransaction);
                        newTransaction.Id = documentSnapshot.Id;
                        transactionList.Add(newTransaction);

                    }
                }
                var sortedTransactions = transactionList.OrderBy(x => x.TransactionDate).ToList();
                return transactionList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async void UpdateTransaction(Shared.Entities.Transaction transaction)
        {
            try
            {
                var docRef = firestore.Collection("transactions").Document(transaction.Id);
                var dateString = transaction.TransactionDate.Year.ToString() + "-" + (transaction.TransactionDate.Month < 10 ? "0" + transaction.TransactionDate.Month.ToString() : transaction.TransactionDate.Month.ToString()) + "-" + (transaction.TransactionDate.Day < 10 ? "0" + transaction.TransactionDate.Day.ToString() : transaction.TransactionDate.Day.ToString());
                await docRef.SetAsync(new { TransactionDate = dateString, Amount = transaction.Amount, Description = transaction.Description, CategoryId = transaction.CategoryId, TransactionId = transaction.TransactionId }, SetOptions.Overwrite);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
