using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Specification;
using API.DTOs;
using AutoMapper;
using API.Errors;
using API.Helpers;
using Core.Specifications;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Products> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productsBrandRepo;
        private readonly IGenericRepository<ProductType> _productsTypeRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Products> productsRepo, IGenericRepository<ProductBrand> productsBrandRepo, IGenericRepository<ProductType> productsTypeRepo, IMapper mapper)
        {
            _productsRepo = productsRepo;
            _productsBrandRepo = productsBrandRepo;
            _productsTypeRepo = productsTypeRepo;
            _mapper = mapper;
        }

        [HttpGet]

        public async Task<ActionResult<Pagination<ProductsToReturnDto>>> GetProducts(
            [FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var countSpec = new ProductsWithFiltersForCountSpecification(productParams);

            var totalItems = await _productsRepo.CountAsync(countSpec);
            var products = await _productsRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<ProductsToReturnDto>>(products);

            return Ok(new Pagination<ProductsToReturnDto>(productParams.PageIndex,
                productParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductsToReturnDto>> GetProducts(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = _productsRepo.GetEntityWithSpec(spec);

            if (product.Result == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return _mapper.Map<Products, ProductsToReturnDto>(product.Result);
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
