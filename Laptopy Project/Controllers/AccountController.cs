using AutoMapper;
using E_Commerce506API.Utility;
using Laptopy_Project.DTOs;
using Laptopy_Project.Models;
using Laptopy_ProjectI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Laptopy_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 RoleManager<IdentityRole> roleManager,
                                 IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(ApplicationUserDTO userDTO)
        {
            if (roleManager.Roles.IsNullOrEmpty())
            {
                await roleManager.CreateAsync(new(SD.adminRole));
                await roleManager.CreateAsync(new(SD.customerRole));
            }

            if(ModelState.IsValid)
            {
                var user = mapper.Map<ApplicationUser>(userDTO);

                var result = await userManager.CreateAsync(user, userDTO.Password);

                if(result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, SD.customerRole);
                    await signInManager.SignInAsync(user, false);

                    return Ok();
                }
                return BadRequest();
            }

            return BadRequest();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var user = await userManager.FindByNameAsync(loginDTO.UserName);

            if (user != null)
            {
                var res = await userManager.CheckPasswordAsync(user, loginDTO.Password);

                if(res)
                {
                    await signInManager.SignInAsync(user, loginDTO.RemeberMe);  
                    return Ok();
                }
                else
                {
                    ModelState.AddModelError("Error", "There is error in Password or username");
                }
            }
            else
            {
                ModelState.AddModelError("Error", "There is error in Password or username");
            }

            return NotFound();
        }

        [HttpDelete("Logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }
    }
}
