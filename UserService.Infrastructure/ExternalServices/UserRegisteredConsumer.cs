using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events;
using UserService.Domain.Entities;
using UserService.Infrastructure.Data;

namespace UserService.Infrastructure.ExternalServices
{
    public class UserRegisteredConsumer(UserProfileDbContext dbContext) : IConsumer<UserRegisteredEvent>
    {
        public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
            var message = context.Message;

            // Check if user already exists.
            var existingUser = await dbContext.UserProfiles.FirstOrDefaultAsync(u => u.Email == message.Email);
            if (existingUser != null) return;

            // If new, create new profile.
            var userProfile = new UserProfile
            {
                Id = Guid.Parse(message.UserId),
                Email = message.Email,
                DisplayName = message.DisplayName,
                IsActive = true
            };

            dbContext.UserProfiles.Add(userProfile);
            await dbContext.SaveChangesAsync();
        }
    }
}
