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

        public List<Transaction> Transactions { get; private set; }
        public List<Category> Categories { get; private set; }

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

        public void SetAuthState(ComponentBase source, bool isAuthenticated)
        {
            IsAuth = isAuthenticated;
            NotifyStateChanged(source, "auth");
        }

        public event Action<ComponentBase, string> StateChanged;

        private void NotifyStateChanged(ComponentBase Source, string Property) => StateChanged?.Invoke(Source, Property);

    }
}
