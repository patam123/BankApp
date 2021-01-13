using BankApp.Server.DataAccess;
using BankApp.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankApp.Server.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {

        Firestore firestore = new Firestore();
        // GET: api/transactions
        [HttpGet]
        public Task<List<Transaction>> Get()
        {
            return firestore.GetTransactions();
        }

        [HttpPut]
        public void Put([FromBody] Transaction transaction)
        {
            firestore.UpdateTransaction(transaction);
        }
    }
}
