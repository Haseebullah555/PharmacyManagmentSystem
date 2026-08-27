
using Application.Contracts.UserManagement;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories.UserManagement;

namespace Identity
{
    public static class IdentityServicesRegistration
    {
        public static IServiceCollection ConfigureIdentityServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICurrentUserRepository, CurrentUserRepository>();

            return services;
        }
    }
}