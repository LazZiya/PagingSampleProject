using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PagingSampleProjectMVC.Data;

namespace PagingSampleProjectMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Get the IWebHost which will host this web application
            var host = CreateHostBuilder(args).Build();

            // Find the service layer within our scope
            using (var scope = host.Services.CreateScope())
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

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
