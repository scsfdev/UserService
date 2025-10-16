using AutoMapper;
using UserService.Application.Dtos;
using UserService.Domain.Entities;

namespace UserService.Application.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // Entity to Dtos
            CreateMap<UserProfile, UserProfileDto>();

            // Dto to Entity
            CreateMap<UserProfileDto, UserProfile>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
