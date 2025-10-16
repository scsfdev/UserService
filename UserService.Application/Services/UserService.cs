using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Dtos;
using UserService.Application.Interfaces;

namespace UserService.Application.Services
{
    // TODO: Standardize return types and error handling

    public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
    {
        public async Task<IEnumerable<UserProfileDto>> GetAllUserProfileAsync()
        {
            var userList = await userRepository.GetAllAsync();

            return mapper.Map<IEnumerable<UserProfileDto>>(userList);
        }

        public async Task<UserProfileDto?> GetUserProfileByEmailAsync(string email)
        {
            var user = await userRepository.GetByEmailAsync(email);
            
            return mapper.Map<UserProfileDto?>(user);
        }

        public async Task<UserProfileDto?> GetUserProfileByGuidAsync(Guid userId)
        {
            var user = await userRepository.GetByGuidAsync(userId);

            return mapper.Map<UserProfileDto?>(user);
        }
        
        public async Task<bool> UpdateUserProfileAsync(UserProfileDto updateProfileDto)
        {
            // Currently, this function will only update DisplayName.
            // In future, we can extend it to update other fields as needed (Eg: Profile Pic, Bio, etc).

            var user = await userRepository.GetByGuidAsync(updateProfileDto.Id);
            if(user == null)
            {
                return false;
            }

            user.DisplayName = updateProfileDto.DisplayName;
            user.UpdatedAt = DateTime.UtcNow;
            await userRepository.UpdateAsync(user);
            return await userRepository.SaveChangesAsync();
        }
    }
}
