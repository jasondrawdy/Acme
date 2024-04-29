using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AcmeCorpApi.Repository;
using AcmeCorpApi.Models;

namespace AcmeCorpApi.Controllers
{
    [Route("api/customers")]
    public class CustomersController : Controller
    {
        ICustomersRepository _repo;

        public CustomersController(ICustomersRepository repo) 
        {
          _repo = repo;
        }

        // GET api/customers
        [HttpGet()]
        [ProducesResponseType(typeof(List<Customer>), 200)]
        [ProducesResponseType(typeof(List<Customer>), 404)]
        public async Task<ActionResult> Customers()
        {
            var customers = await _repo.GetCustomersAsync();
            if (customers == null) {
              return NotFound();
            }
            return Ok(customers);
        }

        // GET api/customers/1
        [HttpGet("{id}", Name = "GetCustomersRoute")]
        [ProducesResponseType(typeof(Customer), 200)]
        [ProducesResponseType(typeof(Customer), 404)]
        public async Task<ActionResult> Customers(int id)
        {
            var customer = await _repo.GetCustomerAsync(id);
            if (customer == null) {
              return NotFound();
            }
            return Ok(customer);
        }

        // POST api/customers
        [HttpPost()]
        [ProducesResponseType(typeof(Customer), 201)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult> PostCustomer([FromBody]Customer customer)
        {
          if (!ModelState.IsValid) 
            return BadRequest(ModelState);

          var newCustomer = await _repo.InsertCustomerAsync(customer);
          if (newCustomer == null) 
            return BadRequest("Unable to insert customer");
          return CreatedAtRoute("GetCustomersRoute", new { id = newCustomer.Id}, newCustomer);
        }

        // PUT api/customers/1
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(bool), 400)]
        public async Task<ActionResult> PutCustomer(int id, [FromBody]Customer customer)
        {
          if (!ModelState.IsValid)
            return BadRequest(ModelState);

          var status = await _repo.UpdateCustomerAsync(customer);
          if (!status)
            return BadRequest("Unable to update customer");
          return Ok(status);
        }

        // DELETE api/customers/1
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(bool), 404)]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
          var status = await _repo.DeleteCustomerAsync(id);
          if (!status) 
            return NotFound();
          return Ok(status);
        }

        // GET api/customers/states
        [HttpGet("states")]
        [ProducesResponseType(typeof(List<State>), 200)]
        [ProducesResponseType(typeof(List<State>), 404)]
        public async Task<ActionResult> States() 
        {
          var states = await _repo.GetStatesAsync();
          if (states == null)
            return NotFound();
          return Ok(states);
        }
    }
}
