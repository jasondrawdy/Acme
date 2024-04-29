using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AcmeCorpApi.Repository;
using AcmeCorpApi.Models;

namespace AcmeCorpApi.Controllers
{
    [Route("api/products")]
    public class ProductsController : Controller
    {
        IProductsRepository _repo;

        public ProductsController(IProductsRepository repo) 
        {
          _repo = repo;
        }

        // GET api/products
        [HttpGet()]
        [ProducesResponseType(typeof(List<Product>), 200)]
        [ProducesResponseType(typeof(List<Product>), 404)]
        public async Task<ActionResult> Products()
        {
            var products = await _repo.GetProductsAsync();
            if (products == null) {
              return NotFound();
            }
            return Ok(products);
        }

        // GET api/products/1
        [HttpGet("{id}", Name = "GetProductsRoute")]
        [ProducesResponseType(typeof(Product), 200)]
        [ProducesResponseType(typeof(Product), 404)]
        public async Task<ActionResult> Products(int id)
        {
            var product = await _repo.GetProductAsync(id);
            if (product == null) {
              return NotFound();
            }
            return Ok(product);
        }

        // POST api/products
        [HttpPost()]
        [ProducesResponseType(typeof(Product), 201)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult> PostProduct([FromBody]Product product)
        {
          if (!ModelState.IsValid) 
            return BadRequest(ModelState);

          var newProduct = await _repo.InsertProductAsync(product);
          if (newProduct == null) 
            return BadRequest("Unable to insert product");
          return CreatedAtRoute("GetProductsRoute", new { id = newProduct.Id}, newProduct);
        }

        // PUT api/products/1
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(bool), 400)]
        public async Task<ActionResult> PutProduct(int id, [FromBody]Product product)
        {
          if (!ModelState.IsValid)
            return BadRequest(ModelState);

          var status = await _repo.UpdateProductAsync(product);
          if (!status)
            return BadRequest("Unable to update product");
          return Ok(status);
        }

        // DELETE api/products/1
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(bool), 404)]
        public async Task<ActionResult> DeleteProduct(int id)
        {
          var status = await _repo.DeleteProductAsync(id);
          if (!status) 
            return NotFound();
          return Ok(status);
        }
    }
}
