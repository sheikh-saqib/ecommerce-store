using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Specification;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Products> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productsBrandRepo;
        private readonly IGenericRepository<ProductType> _productsTypeRepo;

        public ProductsController(IGenericRepository<Products> productsRepo, IGenericRepository<ProductBrand> productsBrandRepo, IGenericRepository<ProductType> productsTypeRepo)
        {
            _productsRepo = productsRepo;
            _productsBrandRepo = productsBrandRepo;
            _productsTypeRepo = productsTypeRepo;
        }

        [HttpGet]

        public async Task<ActionResult<List<Products>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await _productsRepo.ListAsync(spec);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> GetProducts(int id)
        {
            return await _productsRepo.GetByIdAsync(id);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlySet<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productsBrandRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlySet<ProductType>>> GetProductTypes()
        {
            return Ok(await _productsTypeRepo.ListAllAsync());
        }

    }
}
