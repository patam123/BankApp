﻿using System;
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
            string filepath = @"bankapp-2782c-1efd18eca9b1.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);
            projectId = "bankapp-2782c";
            firestore = FirestoreDb.Create(projectId);
        }

        // Category Methods

        public async Task<List<Category>> GetAllCategories(string userId)
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
                        if (category.Where(x => x.Key == "OwnerId" && (string)x.Value == userId).Count() > 0)
                        {

                            string jsonCategory = JsonConvert.SerializeObject(category);
                            var newCategory = JsonConvert.DeserializeObject<Category>(jsonCategory);
                            newCategory.Id = documentSnapshot.Id;
                            categoryList.Add(newCategory);
                        }
                    }
                }
                var defaultCategory = await GetDefaultCategory();
                categoryList.Add(defaultCategory);
                var sortedCategories = categoryList.OrderBy(x => x.Name).ToList();
                return sortedCategories;
            }
            catch (Exception)
            {

                Console.WriteLine("Något gick fel");
                return new List<Category>();
            }

        }
        public async void AddCategory(Category category)
        {
            try
            {
                var docRef = firestore.Collection("categories");
                await docRef.AddAsync(category);
            }
            catch (Exception)
            {

                Console.WriteLine("Något gick fel");
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

                Console.WriteLine("Något gick fel");
            }
        }

        public async Task DeleteCategory(string id)
        {
            try
            {
                var docRef = firestore.Collection("categories").Document(id);
                var result = await docRef.DeleteAsync();
                await UpdateTransactionsOnCategoryDelete(id);
                await DeleteExpenseLimitsOnCategoryDelete(id);
            }
            catch (Exception)
            {

                Console.WriteLine("Något gick fel");
            }

        }

        public async void DeleteAllCategories(string id)
        {
            try
            {
                var snapshot = await firestore.Collection("categories").GetSnapshotAsync();
                var documents = snapshot.Documents;

                var userDocuments = documents.Where(x => x.GetValue<string>("OwnerId") == id);

                while (userDocuments.Count() > 0)
                {
                    foreach (var document in userDocuments)
                    {
                        await document.Reference.DeleteAsync();
                    }

                    snapshot = await firestore.Collection("categories").GetSnapshotAsync();
                    documents = snapshot.Documents;

                    userDocuments = documents.Where(x => x.GetValue<string>("OwnerId") == id);
                }

            }
            catch (Exception)
            {

                Console.WriteLine("Något gick fel");
            }
        }

        public async Task<Category> GetDefaultCategory()
        {
            try
            {
                var documentSnapshot = await firestore.Collection("categories").Document("nm5tXgPCOwfavR9dbF2w").GetSnapshotAsync();

                var category = documentSnapshot.ToDictionary();
                string jsonCategory = JsonConvert.SerializeObject(category);
                var newCategory = JsonConvert.DeserializeObject<Category>(jsonCategory);
                newCategory.Id = documentSnapshot.Id;

                return newCategory;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Transaction Methods

        public async Task<List<Shared.Entities.Transaction>> GetTransactions(string userId)
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
                        if (transaction.Where(x => x.Key == "OwnerId" && (string)x.Value == userId).Count() > 0)
                        {

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
                }
                var sortedTransactions = transactionList.OrderByDescending(x => x.TransactionDate).ToList();
                return sortedTransactions;
            }
            catch (Exception)
            {
                Console.WriteLine("Något gick fel");
                return new List<Shared.Entities.Transaction>();
            }
        }

        public async void CreateTransaction(Shared.Entities.Transaction transaction)
        {
            try
            {
                var docRef = firestore.Collection("transactions");
                var dateString = transaction.TransactionDate.Year.ToString() + "-" + (transaction.TransactionDate.Month < 10 ? "0" + transaction.TransactionDate.Month.ToString() : transaction.TransactionDate.Month.ToString()) + "-" + (transaction.TransactionDate.Day < 10 ? "0" + transaction.TransactionDate.Day.ToString() : transaction.TransactionDate.Day.ToString());
                await docRef.AddAsync(new { TransactionDate = dateString, Amount = transaction.Amount, Description = transaction.Description, CategoryId = transaction.CategoryId, TransactionId = transaction.TransactionId, OwnerId = transaction.OwnerId });
            }
            catch (Exception)
            {

                Console.WriteLine("Något gick fel");
            }

        }

        public async void UpdateTransaction(Shared.Entities.Transaction transaction)
        {
            try
            {
                var docRef = firestore.Collection("transactions").Document(transaction.Id);
                var dateString = transaction.TransactionDate.Year.ToString() + "-" + (transaction.TransactionDate.Month < 10 ? "0" + transaction.TransactionDate.Month.ToString() : transaction.TransactionDate.Month.ToString()) + "-" + (transaction.TransactionDate.Day < 10 ? "0" + transaction.TransactionDate.Day.ToString() : transaction.TransactionDate.Day.ToString());
                await docRef.SetAsync(new { TransactionDate = dateString, Amount = transaction.Amount, Description = transaction.Description, CategoryId = transaction.CategoryId, TransactionId = transaction.TransactionId, OwnerId = transaction.OwnerId }, SetOptions.Overwrite);
            }
            catch (Exception)
            {

                Console.WriteLine("Något gick fel");
            }
        }

        public async Task UpdateTransactionsOnCategoryDelete(string categoryId)
        {
            try
            {
                var snapshot = await firestore.Collection("transactions").GetSnapshotAsync();
                var documents = snapshot.Documents;

                var transactionsWithDeletedCategoryId = documents.Where(x => x.GetValue<string>("CategoryId") == categoryId);
                while (transactionsWithDeletedCategoryId.Count() > 0)
                {
                    foreach (var transaction in transactionsWithDeletedCategoryId)
                    {
                        await transaction.Reference.UpdateAsync("CategoryId", "nm5tXgPCOwfavR9dbF2w");
                    }
                    snapshot = await firestore.Collection("transactions").GetSnapshotAsync();
                    documents = snapshot.Documents;

                    transactionsWithDeletedCategoryId = documents.Where(x => x.GetValue<string>("CategoryId") == categoryId);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Något gick fel");
            }
        }

        public async void DeleteTransaction(string id)
        {
            try
            {
                var docRef = firestore.Collection("transactions").Document(id);
                await docRef.DeleteAsync();
            }
            catch (Exception)
            {

                Console.WriteLine("Något gick fel");
            }
        }

        public async void DeleteAllTransactions(string id)
        {
            try
            {
                var snapshot = await firestore.Collection("transactions").GetSnapshotAsync();
                var documents = snapshot.Documents;

                var userDocuments = documents.Where(x => x.GetValue<string>("OwnerId") == id);

                while (userDocuments.Count() > 0)
                {
                    foreach (var document in userDocuments)
                    {
                        await document.Reference.DeleteAsync();
                    }

                    snapshot = await firestore.Collection("transactions").GetSnapshotAsync();
                    documents = snapshot.Documents;

                    userDocuments = documents.Where(x => x.GetValue<string>("OwnerId") == id);
                }

            }
            catch (Exception)
            {

                Console.WriteLine("Något gick fel");
            }
        }

        // Expense Limit Methods

        public async Task<List<ExpenseLimit>> GetExpenseLimits(string userId)
        {
            try
            {
                var expenseRef = firestore.Collection("expenseLimits");
                var expenseSnapshot = await expenseRef.GetSnapshotAsync();
                var expenseList = new List<ExpenseLimit>();

                foreach (var documentSnapshot in expenseSnapshot)
                {
                    if (documentSnapshot.Exists)
                    {
                        var expenseLimit = documentSnapshot.ToDictionary();
                        if (expenseLimit.Where(x => x.Key == "OwnerId" && (string)x.Value == userId).Count() > 0)
                        {

                            object startDateValue;
                            object endDateValue;

                            expenseLimit.TryGetValue("StartDate", out startDateValue);

                            int startYear = int.Parse(startDateValue.ToString().Substring(0, 4));
                            int startMonth = int.Parse(startDateValue.ToString().Substring(5, 2));
                            int startDay = int.Parse(startDateValue.ToString().Substring(8, 2));
                            DateTime startDate = new DateTime(startYear, startMonth, startDay);

                            expenseLimit.Remove("StartDate");
                            expenseLimit.Add("StartDate", startDate);

                            expenseLimit.TryGetValue("EndDate", out endDateValue);

                            int endYear = int.Parse(endDateValue.ToString().Substring(0, 4));
                            int endMonth = int.Parse(endDateValue.ToString().Substring(5, 2));
                            int endDay = int.Parse(endDateValue.ToString().Substring(8, 2));
                            DateTime endDate = new DateTime(endYear, endMonth, endDay);

                            expenseLimit.Remove("EndDate");
                            expenseLimit.Add("EndDate", endDate);


                            string jsonExpenseLimit = JsonConvert.SerializeObject(expenseLimit);
                            var newExpenseLimit = JsonConvert.DeserializeObject<ExpenseLimit>(jsonExpenseLimit);
                            newExpenseLimit.Id = documentSnapshot.Id;
                            expenseList.Add(newExpenseLimit);
                        }
                    }
                }

                return expenseList;
            }
            catch (Exception)
            {

                Console.WriteLine("Något gick fel");
                return new List<ExpenseLimit>();
            }

        }

        public async void AddExpenseLimit(ExpenseLimit expenseLimit)
        {
            try
            {
                var docRef = firestore.Collection("expenseLimits");
                var startDateString = expenseLimit.StartDate.Year.ToString() + "-" + (expenseLimit.StartDate.Month < 10 ? "0" + expenseLimit.StartDate.Month.ToString() : expenseLimit.StartDate.Month.ToString()) + "-" + (expenseLimit.StartDate.Day < 10 ? "0" + expenseLimit.StartDate.Day.ToString() : expenseLimit.StartDate.Day.ToString());
                var endDateString = expenseLimit.EndDate.Year.ToString() + "-" + (expenseLimit.EndDate.Month < 10 ? "0" + expenseLimit.EndDate.Month.ToString() : expenseLimit.EndDate.Month.ToString()) + "-" + (expenseLimit.EndDate.Day < 10 ? "0" + expenseLimit.EndDate.Day.ToString() : expenseLimit.EndDate.Day.ToString());
                var newExpenseLimit = new { Name = expenseLimit.Name, CategoryId = expenseLimit.CategoryId, OwnerId = expenseLimit.OwnerId, Amount = expenseLimit.Amount, StartDate = startDateString, EndDate = endDateString };
                await docRef.AddAsync(newExpenseLimit);
            }
            catch (Exception)
            {

                Console.WriteLine("Något gick fel");
            }
        }

        public async void UpdateExpenseLimit(ExpenseLimit expenseLimit)
        {
            try
            {
                var docRef = firestore.Collection("expenseLimits").Document(expenseLimit.Id);
                var startDateString = expenseLimit.StartDate.Year.ToString() + "-" + (expenseLimit.StartDate.Month < 10 ? "0" + expenseLimit.StartDate.Month.ToString() : expenseLimit.StartDate.Month.ToString()) + "-" + (expenseLimit.StartDate.Day < 10 ? "0" + expenseLimit.StartDate.Day.ToString() : expenseLimit.StartDate.Day.ToString());
                var endDateString = expenseLimit.EndDate.Year.ToString() + "-" + (expenseLimit.EndDate.Month < 10 ? "0" + expenseLimit.EndDate.Month.ToString() : expenseLimit.EndDate.Month.ToString()) + "-" + (expenseLimit.EndDate.Day < 10 ? "0" + expenseLimit.EndDate.Day.ToString() : expenseLimit.EndDate.Day.ToString());

                await docRef.SetAsync(new { Name = expenseLimit.Name, CategoryId = expenseLimit.CategoryId, OwnerId = expenseLimit.OwnerId, Amount = expenseLimit.Amount, StartDate = startDateString, EndDate = endDateString }, SetOptions.Overwrite);
            }
            catch (Exception)
            {

                Console.WriteLine("Något gick fel");
            }

        }

        public async void DeleteExpenseLimit(string id)
        {
            try
            {
                var docRef = firestore.Collection("expenseLimits").Document(id);
                await docRef.DeleteAsync();
            }
            catch (Exception)
            {

                Console.WriteLine("Något gick fel");
            }
        }

        public async void DeleteAllExpenseLimits(string id)
        {
            try
            {
                var snapshot = await firestore.Collection("expenseLimits").GetSnapshotAsync();
                var documents = snapshot.Documents;

                var userDocuments = documents.Where(x => x.GetValue<string>("OwnerId") == id);

                while (userDocuments.Count() > 0)
                {
                    foreach (var document in userDocuments)
                    {
                        await document.Reference.DeleteAsync();
                    }

                    snapshot = await firestore.Collection("expenseLimits").GetSnapshotAsync();
                    documents = snapshot.Documents;

                    userDocuments = documents.Where(x => x.GetValue<string>("OwnerId") == id);
                }

            }
            catch (Exception)
            {

                Console.WriteLine("Något gick fel");
            }
        }

        public async Task DeleteExpenseLimitsOnCategoryDelete(string categoryId)
        {
            try
            {
                var snapshot = await firestore.Collection("expenseLimits").GetSnapshotAsync();
                var documents = snapshot.Documents;

                var expenseLimitsWithDeletedCategoryId = documents.Where(x => x.GetValue<string>("CategoryId") == categoryId);
                while (expenseLimitsWithDeletedCategoryId.Count() > 0)
                {
                    foreach (var transaction in expenseLimitsWithDeletedCategoryId)
                    {
                        await transaction.Reference.DeleteAsync();
                    }
                    snapshot = await firestore.Collection("expenseLimits").GetSnapshotAsync();
                    documents = snapshot.Documents;

                    expenseLimitsWithDeletedCategoryId = documents.Where(x => x.GetValue<string>("CategoryId") == categoryId);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Något gick fel");
            }
        }

        // User methods

        public async Task<User> GetUser(string id)
        {
            try
            {
                var newUser = new User();
                var userRef = firestore.Collection("users").Document(id);
                var userSnapshot = await userRef.GetSnapshotAsync();
                if (userSnapshot.Exists)
                {
                    var user = userSnapshot.ToDictionary();
                    var jsonUser = JsonConvert.SerializeObject(user);
                    newUser = JsonConvert.DeserializeObject<User>(jsonUser);
                    newUser.Id = userSnapshot.Id;
                    newUser.Password = "l";
                }
                return newUser;
            }
            catch (Exception)
            {
                Console.WriteLine("Något gick fel");
                return new User();
            }
        }

        public async void CreateUser(User user)
        {
            try
            {

                await firestore.Collection("users").Document(user.Id).SetAsync(user);

            }
            catch (Exception)
            {

                Console.WriteLine("Något gick fel");
            }
        }

        public async Task<string> UpdateUser(User user)
        {
            try
            {
                var result = await firestore.Collection("users").Document(user.Id).SetAsync(user, SetOptions.Overwrite);
                var time = result.UpdateTime.ToString();
                if (!string.IsNullOrEmpty(time))
                {
                    return "Användaren uppdaterad.";
                }
                else
                {
                    return "Något gick fel. Uppdateringen misslyckades.";
                }
            }
            catch (Exception)
            {

                return "Något gick fel. Uppdateringen misslyckades.";
            }
        }
        public async Task<string> DeleteUser(string id)
        {
            try
            {
                var result = await firestore.Collection("users").Document(id).DeleteAsync();
                var time = result.UpdateTime.ToString();
                if (!string.IsNullOrEmpty(time))
                {
                    return "Användaren raderades.";
                }
                else
                {
                    return "Något gick fel. Användaren kunde inte raderas.";
                }
            }
            catch (Exception)
            {
                return "Något gick fel. Användaren kunde inte raderas.";
            }
        }

    }
}
