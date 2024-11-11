namespace Laptopy_Project.Models
{
    public class ProductImages
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public Product Product { get; set; }
    }
}
