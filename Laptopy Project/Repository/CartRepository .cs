using Laptopy_Project.Data;
using Laptopy_Project.Models;
using Laptopy_Project.Repository.IRepository;

namespace Laptopy_Project.Repository
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
