using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFCore;
using System.Collections;

namespace BookApi.Extentions
{
    public static class ServicesExtensions // static olmak zorunda
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)=> 
            services.AddDbContext<RepositoryContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlConnection"))); 
        public static void ConfigureRepositoryManager(this IServiceCollection services) => services.AddScoped<IRepositoryManager,RepositoryManager>(); 
    }
}
