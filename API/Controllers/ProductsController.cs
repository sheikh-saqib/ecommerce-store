﻿using Infrastructure.Data;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;

        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]

        public async Task<ActionResult<List<Products>>> GetProducts()
        {
            var products = await _repo.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> GetProducts(int id)
        {
            return await _repo.GetProductByIdAsync(id);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlySet<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _repo.GetProductBrandAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlySet<ProductType>>> GetProductTypes()
        {
            return Ok(await _repo.GetProductTypeAsync());
        }

    }
}
