using Laptopy_Project.Models;

namespace Laptopy_Project.DTOs
{
    public class ShoppingCart
    {
        public List<Cart> Carts { get; set; }
        public double TotalPrice { get; set; }
    }
}
