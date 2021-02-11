using BankApp.Server.DataAccess;
using BankApp.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BankApp.Server.Controllers
{
    
    [ApiController]
    public class ExpenseLimitController : ControllerBase
    {

        Firestore firestore = new Firestore();

        [HttpGet]
        [Route("api/expenselimits/{id}")]
        public Task<List<ExpenseLimit>> Get(string id)
        {
            return firestore.GetExpenseLimits(id);
        }

        [HttpPost]
        [Route("api/expenselimits")]
        public void Post([FromBody] ExpenseLimit expenseLimit)
        {
            firestore.AddExpenseLimit(expenseLimit);
        }

        [HttpPut]
        [Route("api/expenselimits")]
        public void Put([FromBody] ExpenseLimit expenseLimit)
        {
            firestore.UpdateExpenseLimit(expenseLimit);
        }

        [HttpDelete]
        [Route("api/expenselimits/{id}")]
        public void Delete(string id)
        {
            firestore.DeleteExpenseLimit(id);
        }

        [HttpDelete]
        [Route("api/deleteallexpenselimits/{id}")]
        public void DeleteAll(string id)
        {
            firestore.DeleteAllExpenseLimits(id);
        }
    }
}
