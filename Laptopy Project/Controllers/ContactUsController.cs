using Laptopy_Project.Models;
using Laptopy_Project.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Laptopy_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
        private readonly IContactUsRepository contactUsRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public ContactUsController(IContactUsRepository contactUsRepository, UserManager<ApplicationUser> userManager)
        {
            this.contactUsRepository = contactUsRepository;
            this.userManager = userManager;
        }

        [HttpPost("SendMessage")]
        public IActionResult SendMessage(ContactUs contactUs)
        {
            contactUsRepository.Add(contactUs);
            contactUsRepository.Commit();
            return Created();
        }
    }
}
