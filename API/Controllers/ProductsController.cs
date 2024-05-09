using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]

        public string GetProducts()
        {
            return "this wil be a list of products";
        }

        [HttpGet("{id}")]
        public string GetProducts(int id)
        {
            return "this is the product";
        }

    }
}
