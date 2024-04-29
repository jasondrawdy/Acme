using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AcmeCorpApi.Repository;
using AcmeCorpApi.Models;

namespace AcmeCorpApi.Controllers
{
    [Route("api/orders")]
    public class OrdersController : Controller
    {
        IOrdersRepository _repo;

        public OrdersController(IOrdersRepository repo) 
        {
          _repo = repo;
        }

        // GET api/orders
        [HttpGet()]
        [ProducesResponseType(typeof(List<Order>), 200)]
        [ProducesResponseType(typeof(List<Order>), 404)]
        public async Task<ActionResult> Orders()
        {
            var orders = await _repo.GetOrdersAsync();
            if (orders == null) {
              return NotFound();
            }
            return Ok(orders);
        }

        // GET api/orders/1
        [HttpGet("{id}", Name = "GetOrdersRoute")]
        [ProducesResponseType(typeof(Order), 200)]
        [ProducesResponseType(typeof(Order), 404)]
        public async Task<ActionResult> Orders(int id)
        {
            var order = await _repo.GetOrderAsync(id);
            if (order == null) {
              return NotFound();
            }
            return Ok(order);
        }

        // POST api/orders
        [HttpPost()]
        [ProducesResponseType(typeof(Order), 201)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult> PostOrder([FromBody]Order order)
        {
          if (!ModelState.IsValid) 
            return BadRequest(ModelState);

          var newOrder = await _repo.InsertOrderAsync(order);
          if (newOrder == null) 
            return BadRequest("Unable to insert order");
          return CreatedAtRoute("GetOrdersRoute", new { id = newOrder.Id}, newOrder);
        }

        // PUT api/orders/1
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(bool), 400)]
        public async Task<ActionResult> PutOrder(int id, [FromBody]Order order)
        {
          if (!ModelState.IsValid)
            return BadRequest(ModelState);

          var status = await _repo.UpdateOrderAsync(order);
          if (!status)
            return BadRequest("Unable to update order");
          return Ok(status);
        }

        // DELETE api/orders/1
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(bool), 404)]
        public async Task<ActionResult> DeleteOrder(int id)
        {
          var status = await _repo.DeleteOrderAsync(id);
          if (!status) 
            return NotFound();
          return Ok(status);
        }
    }
}
