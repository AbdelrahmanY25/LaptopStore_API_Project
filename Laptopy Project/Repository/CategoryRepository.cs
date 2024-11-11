using Laptopy_Project.Data;
using Laptopy_Project.Models;
using Laptopy_Project.Repository.IRepository;

namespace Laptopy_Project.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
