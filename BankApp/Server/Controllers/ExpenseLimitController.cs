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
    [Route("api/expenselimits")]
    [ApiController]
    public class ExpenseLimitController : ControllerBase
    {

        Firestore firestore = new Firestore();

        // GET api/<ExpenseLimitController>/5
        [HttpGet("{id}")]
        public Task<List<ExpenseLimit>> Get(string id)
        {
            return firestore.GetExpenseLimits(id);
        }

        // POST api/<ExpenseLimitController>
        [HttpPost]
        public void Post([FromBody] ExpenseLimit expenseLimit)
        {
            firestore.AddExpenseLimit(expenseLimit);
        }

        // PUT api/<ExpenseLimitController>/5
        [HttpPut]
        public void Put([FromBody] ExpenseLimit expenseLimit)
        {
            firestore.UpdateExpenseLimit(expenseLimit);
        }

        // DELETE api/<ExpenseLimitController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            firestore.DeleteExpenseLimit(id);
        }
    }
}
