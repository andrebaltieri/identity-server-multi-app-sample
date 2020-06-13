using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Api.Products.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {

        [HttpGet("")]
        public string Get()
        {
            return "API de Produtos";
        }

        [HttpGet("products")]
        [Authorize]
        public IEnumerable<Product> Products()
        {
            var products = new List<Product>
            {
                new Product(1, "Mousepad"), new Product(2, "Teclado Gamer"), new Product(3, "Monitor")
            };
            return products;
        }
    }

    public class Product
    {
        public Product(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
