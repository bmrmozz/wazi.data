using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using wazi.data.core.master;
using Microsoft.Extensions.Options;
using wazidataservicetest.Services;
using wazidataservicetest.Repositories;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace wazidataservicetest.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    public class ProductCatalogController : Controller
    {
       ProductService<ProductItem> productservice = null;

        public ProductCatalogController(IOptions<MasterRepository> repository) {
            this.productservice = new ProductService<ProductItem>(repository.Value);
        }

        // GET: api/values
        [HttpGet]
        [Route("GetProductListing")]
        public JsonResult GetProductListing() {
            return Json(this.productservice.GetProductListing());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [Route("GetProduct/Id={Id}")]
        public JsonResult GetProduct(string id) {
            return Json(this.productservice.GetProduct(id));
        }

        //[HttpGet("{categoryid}")]
        //[Route("GetProductsByCategory/categoryid={categoryid}")]
        //public JsonResult GetProductsByCategory(string categoryid) {
        //    return Json(this.productservice.GetProductsByCategory(categoryid));
        //}

        // POST api/values
        [HttpPost]
        [Route("SaveProduct")]
        public void Post([FromBody]ProductItem product) {
            this.productservice.AddProduct(product);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [Route("UpdateProduct/Id={Id}")]
        public void Put(string id, [FromBody]ProductItem product) {
            this.productservice.UpdateProduct(id, product);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Route("RemoveProduct/Id={Id}")]
        public void Delete(string id) {
            this.productservice.RemoveProduct(id);
        }
    }
}
