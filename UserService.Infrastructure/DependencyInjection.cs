using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Interfaces;
using UserService.Infrastructure.Data;
using UserService.Infrastructure.ExternalServices;

namespace UserService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            // 1. DbContext.
            services.AddDbContext<UserProfileDbContext>(opt =>
            opt.UseSqlServer(config.GetConnectionString("UserProfileDb")));

            // 2. Application service.
            services.AddScoped<IUserService, Application.Services.UserService>();

            // 3. Infrastructure services.
            services.AddScoped<IUserRepository, Repositories.UserRepository>();

            // 4. MassTransit for RabbitMQ.
            services.AddMassTransit(x =>
            {
                x.AddConsumer<UserRegisteredConsumer>();
                x.AddConsumer<UserDeactivatedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitHost = config["RabbitMQ:HostName"]!;
                    var rabbitUser = config["RabbitMQ:UserName"]!;
                    var rabbitPass = config["RabbitMQ:Password"]!;
                    var rabbitPort = ushort.Parse(config["RabbitMQ:Port"]!);

                    cfg.Host(rabbitHost, rabbitPort, "/", c =>
                    {
                        c.Username(rabbitUser);
                        c.Password(rabbitPass);
                    });

                    cfg.ReceiveEndpoint("user-registration-queue", e =>
                    {
                        e.ConfigureConsumer<UserRegisteredConsumer>(context);
                    });

                    cfg.ReceiveEndpoint("user-deactivation-queue", e =>
                    {
                        e.ConfigureConsumer<UserDeactivatedConsumer>(context);
                    });
                });
            });

            return services;
        }
    }
}
