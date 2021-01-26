using BankApp.Shared.Entities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApp.Client.Helpers
{
    public class AppStateContainer
    {
        public bool IsAuth { get; private set; }

        public User User { get; set; }

        public List<Transaction> Transactions { get; private set; }
        public List<Category> Categories { get; private set; }
        public List<ExpenseLimit> ExpenseLimits { get; set; }

        public void UpdateCategories(ComponentBase source, List<Category> categories)
        {
            Categories = categories;
            NotifyStateChanged(source, "categories");

        }

        public void UpdateTransactions(ComponentBase source, List<Transaction> transactions)
        {
            Transactions = transactions;
            NotifyStateChanged(source, "transactions");
        }

        public void UpdateExpenseLimits(ComponentBase source, List<ExpenseLimit> expenseLimits)
        {
            ExpenseLimits = expenseLimits;
            NotifyStateChanged(source, "expenseLimits");
        }

        public void SetAuthState(ComponentBase source, User user, bool isAuthenticated)
        {
            IsAuth = isAuthenticated;
            User = user;
            NotifyStateChanged(source, "auth");
        }

        public event Action<ComponentBase, string> StateChanged;

        private void NotifyStateChanged(ComponentBase Source, string Property) => StateChanged?.Invoke(Source, Property);

    }
}
