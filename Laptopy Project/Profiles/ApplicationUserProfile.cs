using AutoMapper;
using Laptopy_Project.Models;
using Laptopy_ProjectI.DTOs;

namespace Laptopy_Project.Profiles
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationUserDTO, ApplicationUser>();
        }
    }
}
