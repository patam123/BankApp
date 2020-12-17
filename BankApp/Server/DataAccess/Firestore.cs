using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApp.Shared.Entities;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using Transaction = BankApp.Shared.Entities.Transaction;

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

        public async Task<List<Transaction>> GetTransactions()
        {
            try
            {
                var transactionCollectionRef = firestore.Collection("transactions");
                var transactionSnapshot = await transactionCollectionRef.GetSnapshotAsync();
                var transactionList = new List<Transaction>();
                foreach (var documentSnapshot in transactionSnapshot)
                {
                    if (documentSnapshot.Exists)
                    {
                        var transaction = documentSnapshot.ToDictionary();
                        var jsonTransaction = JsonConvert.SerializeObject(transaction);
                        var newTransaction = JsonConvert.DeserializeObject<Transaction>(jsonTransaction);
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
    }
}
