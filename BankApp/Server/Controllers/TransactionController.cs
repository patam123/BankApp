using BankApp.Server.DataAccess;
using BankApp.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BankApp.Server.Controllers
{
    //[Route("api/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {

        Firestore firestore = new Firestore();
        // GET: api/transactions
        [HttpGet/*("{id}")*/]
        [Route("api/transactions/{id}")]
        public Task<List<Transaction>> Get(string id)
        {
            return firestore.GetTransactions(id);
        }

        [HttpPut]
        [Route("api/transactions")]
        public void Put([FromBody] Transaction transaction)
        {
            firestore.UpdateTransaction(transaction);
        }

        [HttpPost]
        [Route("api/transactions")]
        public void Post([FromBody] Transaction transaction)
        {
            firestore.CreateTransaction(transaction);
        }

        [HttpDelete/*("{id}")*/]
        [Route("api/transactions/{id}")]
        public void Delete(string id)
        {
            firestore.DeleteTransaction(id);
        }

        [HttpDelete]
        [Route("api/deletealltransactions/{id}")]
        public void DeleteAll(string id)
        {
            firestore.DeleteAllTransactions(id);
        }
    }
}
