using Laptopy_Project.Data;
using Laptopy_Project.Models;
using Laptopy_Project.Repository.IRepository;

namespace Laptopy_Project.Repository
{
    public class ContactUsRepository : Repository<ContactUs>, IContactUsRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ContactUsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
