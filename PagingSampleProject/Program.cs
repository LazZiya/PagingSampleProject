using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PagingSampleProject.Data;

namespace PagingSampleProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Get the IWebHost which will host this web application
            var host = CreateWebHostBuilder(args).Build();
            
            // Find the service layer within our scope
            using(var scope = host.Services.CreateScope())
            {
                // Get the instance of ApplicationContext in our services layer
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();

                // Call DataGenerator to create sample data
                DataGenerator.Initialize(services);
            }
            
            // Continue to run the application
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
