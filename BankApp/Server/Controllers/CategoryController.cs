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
    public class CategoryController : ControllerBase
    {

        Firestore firestore = new Firestore();
        [HttpGet]
        [Route("api/categories/{id}")]
        public Task<List<Category>> Get(string id)
        {
            return firestore.GetAllCategories(id);
        }

        [HttpPost]
        [Route("api/categories")]
        public void Post([FromBody] Category category)
        {
            firestore.AddCategory(category);
        }

        [HttpPut]
        [Route("api/categories")]
        public void Put([FromBody] Category category)
        {
            firestore.UpdateCategory(category);
        }


        [HttpDelete]
        [Route("api/categories/{id}")]
        public async Task Delete(string id)
        {
            await firestore.DeleteCategory(id);
        }
        
        [HttpDelete]
        [Route("api/deleteallcategories/{id}")]
        public void DeleteAll(string id)
        {
            firestore.DeleteAllCategories(id);
        }
    }
}
