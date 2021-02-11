using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApp.Shared.Entities;
using System.Net.Http;
using Newtonsoft.Json;
using BankApp.Client.Helpers;

namespace BankApp.Client.Shared
{
    public partial class IndividualTransaction
    {
        public IndividualTransaction()
        {

        }

        [Parameter] public bool IsTransactionPage { get; set; }
        [Parameter] public Transaction Transaction { get; set; }
        [Parameter] public List<Category> Categories { get; set; }
        [Parameter] public EventCallback SetCatSum { get; set; }

        [Inject] public HttpClient Http { get; set; }
        [Inject] AppStateContainer AppState { get; set; }
        protected override void OnInitialized()
        {

        }
        public async Task SetCategory(Transaction transaction, ChangeEventArgs e)
        {
            transaction.CategoryId = (string)e.Value;


            await SetCatSum.InvokeAsync(e);
            await Http.SendJsonAsync(HttpMethod.Put, "api/transactions", transaction);
            StateHasChanged();
        }

        private async Task DeleteTransaction(string id)
        {
            await Http.DeleteAsync("api/transactions/" + id);
            var updatedTransactions = await Http.GetJsonAsync<List<Transaction>>("api/transactions/" + AppState.User.Id);
            AppState.UpdateTransactions(this, updatedTransactions);
        }
    }
}
