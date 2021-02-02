using BankApp.Server.DataAccess;
using BankApp.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BankApp.Server.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {

        Firestore firestore = new Firestore();
        // GET: api/transactions
        [HttpGet("{id}")]
        public Task<List<Transaction>> Get(string id)
        {
            return firestore.GetTransactions(id);
        }

        [HttpPut]
        public void Put([FromBody] Transaction transaction)
        {
            firestore.UpdateTransaction(transaction);
        }

        [HttpPost]
        public void Post([FromBody] Transaction transaction)
        {
            firestore.CreateTransaction(transaction);
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            firestore.DeleteTransaction(id);
        }
    }
}
