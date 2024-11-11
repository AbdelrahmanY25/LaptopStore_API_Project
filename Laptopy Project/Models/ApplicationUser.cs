using Microsoft.AspNetCore.Identity;

namespace Laptopy_Project.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Product> Products { get; set; }
    }
}
