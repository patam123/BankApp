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

        public void SetCategory(Transaction transaction, ChangeEventArgs e) // gör private?
        {
            Category category = Categories.Find(x => x.Name.Equals(e.Value));
            transaction.Category = category;
            StateHasChanged();
            SetCatSum.InvokeAsync(e);
            Console.WriteLine($"{transaction.Description} har lagts till i kategorin \"{category.Name}\"");
            // => skicka till api.
        }
    }
}
