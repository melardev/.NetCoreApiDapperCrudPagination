using ApiCoreDapperCrudPagination.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ApiCoreDapperCrudPagination.Infrastructure.Extensions
{
    public static class IoCExtensions
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ITodoService, TodoService>();
            services.AddTransient<ITodoService, TodoStoredProceduresService>();
        }
    }
}