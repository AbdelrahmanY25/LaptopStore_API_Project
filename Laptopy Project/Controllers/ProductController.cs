using Laptopy_Project.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Laptopy_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult Index(string? filterBy = null)
        {
            var products = productRepository.GetAll();

            if (filterBy != null)
            {
                products = productRepository.GetAll(expression: e => e.Description.Contains(filterBy));
            }

            return Ok(products);
        }

        [HttpGet("Filter")]
        public IActionResult Filter(string? filterByBrand = null, decimal filterByMinPrice = 0, decimal filterByMaxPrice = 0)
        {
            var filteredProducts = productRepository.GetAll();

            if (filterByBrand != null) 
            {
                filteredProducts = productRepository.GetAll(expression: p => p.Name.Contains(filterByBrand));

                if (filterByMinPrice > 0 && filterByMinPrice < filterByMaxPrice)
                {
                    filteredProducts = productRepository.GetAll(expression: p => p.Name.Contains(filterByBrand) && p.Price >= filterByMinPrice && p.Price <= filterByMaxPrice);
                }
            }
            else if (filterByMinPrice > 0 && filterByMinPrice < filterByMaxPrice)
            {
                filteredProducts = productRepository.GetAll(expression: p => p.Price >= filterByMinPrice && p.Price <= filterByMaxPrice);
            }
            
            return Ok(filteredProducts);
        }

        [HttpPost("Details")]
        public IActionResult Details(int productId)
        {
            var product = productRepository.GetOne(expression: p => p.Id == productId);
            if (product != null)
                return Ok(product);

            return NotFound();
        }
    }
}
