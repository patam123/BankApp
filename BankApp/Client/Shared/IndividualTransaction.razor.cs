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

        public void SetCategory(Transaction transaction, ChangeEventArgs e) // gör private => gör en ny metod att kalla på i .razorkomponent
        {
            Category category = Categories.Find(x => x.Name.Equals(e.Value));
            transaction.Category = category;
            Console.WriteLine($"{transaction.Name} har lagts till i kategorin \"{category.Name}\"");
            // => skicka till api.
        }
    }
}
