using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApp.Shared.Entities;

namespace BankApp.Client.Shared
{
    public partial class IndividualTransaction
    {
        public IndividualTransaction()
        {

        }

        [Parameter] public Transaction Transaction { get; set; }
        [Parameter] public List<Category> Categories { get; set; }
        [Parameter] public EventCallback SetCatSum { get; set; }
        [Parameter] public Category CurrentCategory { get; set; } // överflödig, ta bort

        protected override void OnInitialized()
        {
            CurrentCategory = Categories.Find(x => x.Id.Equals(Transaction.CategoryId));
        }
        public void SetCategory(Transaction transaction, ChangeEventArgs e) // gör private?
        {
            Category category = Categories.Find(x => x.Id.Equals(e.Value));
            Transaction.CategoryId = category.Id;

            SetCatSum.InvokeAsync(e);
            Console.WriteLine($"{transaction.Description} har lagts till i kategorin \"{category.Name}\"");
            // => skicka till api.
        }
    }
}
