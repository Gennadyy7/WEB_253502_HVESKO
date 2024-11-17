using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_253502_HVESKO.API.Data;
using WEB_253502_HVESKO.API.Services.ProductService;
using WEB_253502_HVESKO.Domain.Entities;
using WEB_253502_HVESKO.Domain.Models;

namespace WEB_253502_HVESKO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IProductService _productService;

        public ServicesController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Services/{category}
        [HttpGet("{category?}")]
        public async Task<ActionResult<ResponseData<ListModel<Service>>>> GetServices([FromRoute] string? category = null, [FromQuery] int pageNo = 1, [FromQuery] int pageSize = 3)
        {
            var response = await _productService.GetProductListAsync(category, pageNo, pageSize);
            if (!response.Successfull)
                return NotFound(response.ErrorMessage);

            return Ok(response);
        }

        // GET: api/Services/5
        [HttpGet("id-{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseData<Service>>> GetService([FromRoute] int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            if (!response.Successfull)
                return NotFound(response.ErrorMessage);

            return Ok(response);
        }

        // PUT: api/Services/5
        [HttpPut("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> PutService(int id, Service service)
        {
            await _productService.UpdateProductAsync(id, service, null);
            return Ok();
        }

        // POST: api/Services
        [HttpPost]
        [Authorize(Policy = "admin")]
        public async Task<ActionResult<ResponseData<Service>>> PostService(Service service)
        {
            var response = await _productService.CreateProductAsync(service, null);

            if (!response.Successfull)
                return BadRequest(response.ErrorMessage);

            return CreatedAtAction("GetService", new { id = service.ID }, response);
        }

        // DELETE: api/Services/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> DeleteService(int id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
