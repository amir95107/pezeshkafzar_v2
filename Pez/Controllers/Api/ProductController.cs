using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Pezeshkafzar_v2.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pezeshkafzar_v2.Controllers.Api
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductController(IProductRepository repository)
        {
            _repository = repository;
        }

        // GET: api/<ProductController>
        [HttpGet]
        public async Task<ActionResult<List<Products>>> Get()
            => await _repository.GetAllAsync();

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> Get(Guid id)
            => await _repository.FindAsync(id);

        // POST api/<ProductController>
        [HttpPatch]
        public async Task Post([FromBody] ProductDto contract)
        {
            if (contract is null)
                throw new ArgumentNullException(nameof(contract));

            var product = await _repository.FindAsync(contract.Id);
            if (contract is null)
                throw new ArgumentNullException(nameof(product));

            product.Price = contract.Price;
            product.PriceAfterDiscount = contract.PriceAfterDiscount;
            product.Stock = contract.Stock;
            product.ModifiedAt=DateTime.Now;

            _repository.Modify(product);
            await _repository.SaveChangesAsync();
        }

        public class ProductDto
        {
            public Guid Id { get; set; }
            public decimal Price { get; set; }
            public decimal PriceAfterDiscount { get; set; }
            public int Stock { get; set; }
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
