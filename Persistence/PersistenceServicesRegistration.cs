using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Persistence.Database;
using Microsoft.EntityFrameworkCore;
using Application.Contracts.Interfaces.Common;
using Application.Contracts.Interfaces.seeders;
using Persistence.Repositories.Common;
using Persistence.Seeders;
using StackExchange.Redis;

namespace Persistence
{
    public static class PersistenceServicesRegistration
    {
        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            // services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<ICurrencySeeder, CurrencySeeder>();
            services.AddScoped<ICategorySeeder, CategorySeeder>();
            services.AddScoped<IDosageSeeder, DosageSeeder>();
            services.AddScoped<IUnitSeeder, UnitSeeder>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
            });

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var options = new ConfigurationOptions
                {
                    EndPoints = { "localhost:6379" },
                    AbortOnConnectFail = false, // 🔥 DO NOT crash app
                    ConnectRetry = 5,
                    ConnectTimeout = 5000
                };

                return ConnectionMultiplexer.Connect(options);
            });

            return services;
        }
    }
}