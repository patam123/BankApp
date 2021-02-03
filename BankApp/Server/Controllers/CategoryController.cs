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
    //[Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        Firestore firestore = new Firestore();
        // GET: api/categories/{id}
        [HttpGet]
        [Route("api/categories/{id}")]
        public Task<List<Category>> Get(string id)
        {
            return firestore.GetAllCategories(id);
        }

        // POST: api/categories
        [HttpPost]
        [Route("api/categories")]
        public void Post([FromBody] Category category)
        {
            firestore.AddCategory(category);
        }

        // PUT: api/categories
        [HttpPut]
        [Route("api/categories")]
        public void Put([FromBody] Category category)
        {
            firestore.UpdateCategory(category);
        }


        // DELETE api/categories/5
        [HttpDelete]
        [Route("api/categories/{id}")]
        public void Delete(string id)
        {
            firestore.DeleteCategory(id);
        }
        
        [HttpDelete]
        [Route("api/deleteallcategories/{id}")]
        public void DeleteAll(string id)
        {
            firestore.DeleteAllCategories(id);
        }
    }
}
