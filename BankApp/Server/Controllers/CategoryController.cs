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
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        Firestore firestore = new Firestore();
        // GET: api/categories
        [HttpGet("{id}")]
        public Task<List<Category>> Get(string id)
        {
            return firestore.GetAllCategories(id);
        }

        // POST: api/categories
        [HttpPost]
        public void Post([FromBody] Category category)
        {
            firestore.AddCategory(category);
        }

        // PUT: api/categories
        [HttpPut]
        public void Put([FromBody] Category category)
        {
            firestore.UpdateCategory(category);
        }

        // GET api/categories/5


        // PUT api/categories/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/categories/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            firestore.DeleteCategory(id);
        }
    }
}
