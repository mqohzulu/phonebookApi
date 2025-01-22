using ContactManagement.Application.Common.Interfaces;
using ContactManagement.Application.Interfaces;
using ContactManagement.Infrastructure.Authentication;
using ContactManagement.Infrastructure.Persistance.Repositories;
using ContactManagement.Infrastructure.Persistance;
using ContactManagement.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using ContactManagement.Infrastructure.Settings;
using ContactManagement.Application.Authentication.Common.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ContactManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure( this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddTransient<IAuthService, AuthService>();

            services.AddSingleton<IRabbitMQService, RabbitMQService>();

            services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            return services;
        }
    }

}
