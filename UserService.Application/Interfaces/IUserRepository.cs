using UserService.Domain.Entities;

namespace UserService.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserProfile>> GetAllAsync();
        Task<UserProfile?> GetByEmailAsync(string email);
        Task<UserProfile?> GetByGuidAsync(Guid userId);

        Task UpdateAsync(UserProfile user);

        Task<bool> SaveChangesAsync();
    }
}
