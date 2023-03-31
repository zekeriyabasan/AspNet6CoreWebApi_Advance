using Microsoft.EntityFrameworkCore;
using Repositories.EFCore;
using System.Collections;

namespace BookApi.Extentions
{
    public static class ServicesExtensions // static olmak zorunda
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)=> 
            services.AddDbContext<RepositoryContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlConnection"))); 
            
    }
}
