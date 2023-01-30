using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Infrastructure.RepositoryImplementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Library.API.DIRegistration
{
    public static class RepositoriesRegistration
    {
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IFilterRepository, FilterRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<INofiticationRepository, NotificationRepository>();
        }
    }
}
