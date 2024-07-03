using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ecommapp.Models;
using ecommapp.Services;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;

namespace ecommapp.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SupplierController : ControllerBase
    {
       private readonly IRepository<Supplier> _repository;

       public SupplierController(IRepository<Supplier> repository)
       {
        _repository = repository;
       }

        [Authorize(Roles = "Admin")]
        [HttpGet]
       public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers([FromQuery] int skip, [FromQuery]int take)
       {
        var suppliers = await _repository.GetAllAsync(skip, take);
        return Ok(suppliers);
       }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<Supplier>> UpsertSupplier([FromBody] Supplier supplier)
        {
            var upsertedSupplier = await _repository.UpsertAsync(supplier);
            return Ok(upsertedSupplier);
        } 
    }
}