using UserService.Application.Dtos;

namespace UserService.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserProfileDto>> GetAllUserProfileAsync();
        Task<UserProfileDto?> GetUserProfileByEmailAsync(string email);
        Task<UserProfileDto?> GetUserProfileByGuidAsync(Guid userId);
        
        Task<bool> UpdateUserProfileAsync(UserProfileDto updateProfileDto);
    }
}
