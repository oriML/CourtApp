using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using CourtApp.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Configuration; // Added for IConfigurationBuilder
using System.IO; // Added for Path
using System.Reflection; // Added for Assembly

namespace CourtApp.Api.Tests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting(WebHostDefaults.EnvironmentKey, "Testing"); // Set the environment variable

        // Set the content root to the project directory of the API
        var projectDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../CourtApp.Api"));
        builder.UseContentRoot(projectDir);

        builder.ConfigureAppConfiguration((context, conf) =>
        {
            var testProjectDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            conf.AddJsonFile(Path.Combine(testProjectDir!, "appsettings.json"), optional: false, reloadOnChange: true);
        });

        builder.ConfigureServices(services =>
        {
            // Remove the app's AppDbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add a new AppDbContext registration using an in-memory database for testing
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });

            // Build the service provider
            var sp = services.BuildServiceProvider();

            // Create a scope to obtain a reference to the database contexts
            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AppDbContext>();

                // Ensure the database is created
                db.Database.EnsureCreated();

                // Seed the database with test data if needed
                // db.ContactRequests.AddRange(...
                // db.SaveChanges();
            }
        });
    }
}
