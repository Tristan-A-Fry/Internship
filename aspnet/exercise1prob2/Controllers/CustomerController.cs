using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using exercise1prob2.Models;
using exercise1prob2.Services;

namespace exercise1prob2.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IRepository<Customer> _repository;

        public CustomerController(IRepository<Customer> repository)
        {
            _repository = repository;
        }

        [HttpGet] 
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers([FromQuery] int skip, [FromQuery]int take)
        {
            var customers = await _repository.GetAllAsync(skip, take);
            return Ok(customers);
        }

        [HttpPut]
        public async Task<ActionResult<Customer>> UpsertCustomers([FromBody] Customer customers)
        {
            var upsertedCustomers = await _repository.UpsertAsync(customers);
            return Ok(upsertedCustomers);
        }
    }
}