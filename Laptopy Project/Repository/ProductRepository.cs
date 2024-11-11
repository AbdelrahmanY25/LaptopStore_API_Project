using Laptopy_Project.Data;
using Laptopy_Project.Models;
using Laptopy_Project.Repository.IRepository;

namespace Laptopy_Project.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
