using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Pact.Provider.Model;
using Pact.Provider.Repositories;

namespace Pact.Provider.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository Repository;

        public ProductsController(IProductRepository productRepository)
        {
            Repository = productRepository;
        }

        // GET /api/products
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            List<Product> products = Repository.List();
            return products;
        }

        // GET /api/products/{id}
        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = Repository.Get(id);

            if (product == null)
            {
                return new NotFoundResult();
            }

            return product;
        }
    }
}
