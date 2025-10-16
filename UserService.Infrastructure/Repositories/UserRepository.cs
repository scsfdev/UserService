using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Infrastructure.Data;

namespace UserService.Infrastructure.Repositories
{
    public class UserRepository(UserProfileDbContext dbContext) : IUserRepository
    {
        public async Task<IEnumerable<UserProfile>> GetAllAsync()
        {
            var users = await dbContext.UserProfiles.Where(u=>u.IsActive).ToListAsync();
            return users;
        }

        public async Task<UserProfile?> GetByEmailAsync(string email)
        {
            return await dbContext.UserProfiles.FirstOrDefaultAsync(u => u.Email == email);
        }

        public Task<UserProfile?> GetByGuidAsync(Guid userId)
        {
            return dbContext.UserProfiles.FirstOrDefaultAsync(u => u.Id == userId);
        }


        public Task UpdateAsync(UserProfile user)
        {
            dbContext.UserProfiles.Update(user);
            return Task.CompletedTask;
        }


        public async Task<bool> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync() > 0;
        }

    }
}
