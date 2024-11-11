using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Laptopy_Project.Models
{
    public class Cart
    {
        public int ProductId { get; set; }
        public string ApplicationUserId { get; set; }

        [Required]
        [Range(1, 100)]
        public int BookedProducts { get; set; }

        [ValidateNever]
        public Product Product{ get; set; }

        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
