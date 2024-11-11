using Laptopy_Project.DTOs;
using Laptopy_Project.Models;
using Laptopy_Project.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace Laptopy_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository cartRepository;
        private readonly UserManager<ApplicationUser> userManger;

        public CartController(ICartRepository cartRepository, UserManager<ApplicationUser> userManger)
        {
            this.cartRepository = cartRepository;
            this.userManger = userManger;
        }

        [HttpGet]
        public IActionResult BayProduct(int productId)
        {
            var appUser = userManger.GetUserId(User);

            //if (appUser == null)
            //    return RedirectToAction("Login", "Account");

            Cart cart = new Cart()
            {
                ApplicationUserId = appUser,
                ProductId = productId
            };

            return Ok(cart);
        }

        [HttpPost("BayProduct")]
        public IActionResult BayProduct(Cart cart)
        {
            var oldUser = cartRepository.GetOne(expression: c => c.ProductId == cart.ProductId && c.ApplicationUserId == cart.ApplicationUserId);

            if (oldUser != null)
            {
                oldUser.BookedProducts += cart.BookedProducts;
                cartRepository.Commit();
            }
            else
            {
                cartRepository.Add(cart);
                cartRepository.Commit();
            }

            return Created();
        }

        [HttpPost("Index")]
        public IActionResult Index()
        {
            var products = cartRepository.GetAll([p => p.Product.Category], p => p.ApplicationUserId == userManger.GetUserId(User)).ToList();

            if (products != null)
            {
                ShoppingCart shoppingCart = new ShoppingCart()
                {
                    Carts = products,
                    TotalPrice = (double)products.Sum(p => p.BookedProducts * p.Product.Price)
                };

                return Ok(shoppingCart);
            }

            return Ok();
        }

        [HttpPut("Increment")]
        public IActionResult Increment(int productId)
        {
            var ApplicationUserId = userManger.GetUserId(User);

            var product = cartRepository.GetOne(expression: e => e.ApplicationUserId == ApplicationUserId && e.ProductId == productId);

            if (product != null)
            {
                product.BookedProducts++;
                cartRepository.Commit();

                return Ok();
            }

            return NotFound();
        }

        [HttpPut("Decrement")]
        public IActionResult Decrement(int productId)
        {
            var ApplicationUserId = userManger.GetUserId(User);

            var product = cartRepository.GetOne(expression: e => e.ApplicationUserId == ApplicationUserId && e.ProductId == productId);

            if (product != null)
            {
                product.BookedProducts--;

                if (product.BookedProducts > 0)
                    cartRepository.Commit();
                else
                    product.BookedProducts = 1;

                return Ok();
            }

            return NotFound();
        }

        [HttpPut("Delete")]
        public IActionResult Delete(int productId)
        {
            var ApplicationUserId = userManger.GetUserId(User);

            var product = cartRepository.GetOne(expression: e => e.ApplicationUserId == ApplicationUserId && e.ProductId == productId);

            if (product != null)
            {
                cartRepository.Delete(product);
                cartRepository.Commit();

                return Ok();
            }

            return NotFound();
        }

        [HttpPost("Pay")]
        public IActionResult Pay()
        {
            var ApplicationUserId = userManger.GetUserId(User);

            var cartProduct = cartRepository.GetAll([e => e.Product], e => e.ApplicationUserId == ApplicationUserId).ToList();

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/checkout/success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancel",
            };

            foreach (var item in cartProduct)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                        UnitAmount = (long)item.Product.Price * 100,
                    },
                    Quantity = item.BookedProducts,
                });
            }

            var service = new SessionService();
            var session = service.Create(options);
            return Created(session.Url, cartProduct);
        }
    }
}
