using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carrot.Contracts.Services;
using Carrot.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Carrot.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();

            // Create a new scope
            using (var scope = webHost.Services.CreateScope())
            {
                // Get the Accounts from Okta To Initialize the Database (In Memory)
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var inMemoryDatabase = configuration.GetValue<bool>("InMemoryDatabase", false);
                if (inMemoryDatabase)
                {
                    var oktaService = scope.ServiceProvider.GetRequiredService<IOktaService>();
                    var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
                    var users = await oktaService.GetUsersAsync();
                    if (users is { Count: > 0 })
                    {
                        await accountService.UpdateAccounts(users);
                    }
                }
            }

            await webHost.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
