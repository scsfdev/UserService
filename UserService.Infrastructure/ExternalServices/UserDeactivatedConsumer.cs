
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events;
using UserService.Infrastructure.Data;

namespace UserService.Infrastructure.ExternalServices
{
    public class UserDeactivatedConsumer(UserProfileDbContext dbContext) : IConsumer<UserDeactivatedEvent>
    {
        public async Task Consume(ConsumeContext<UserDeactivatedEvent> context)
        {
            var message = context.Message;

            // Check if user exists.
            var existingUser = await dbContext.UserProfiles.FirstOrDefaultAsync(u => u.Email == message.Email);
            if (existingUser == null) return; // No user to deactivate.

            // Deactivate user profile.
            existingUser.IsActive = false;
            existingUser.UpdatedAt = DateTime.UtcNow;
            await dbContext.SaveChangesAsync();
        }
    }
}
