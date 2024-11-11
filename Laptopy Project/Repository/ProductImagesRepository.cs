using Laptopy_Project.Data;
using Laptopy_Project.Repository.IRepository;

namespace Laptopy_Project.Repository
{
    public class ProductImagesRepository : Repository<ProductImagesRepository>, IProductImagesRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ProductImagesRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
