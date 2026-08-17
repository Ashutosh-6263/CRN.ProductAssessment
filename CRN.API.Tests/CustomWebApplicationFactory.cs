using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CRN.API.Tests
{
    public class CustomWebApplicationFactory
        : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            var sqlPassword =
                Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD")
                ?? throw new InvalidOperationException(
                    "MSSQL_SA_PASSWORD environment variable is not set.");

            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:DefaultConnection"] =
                            $"Server=localhost,1433;" +
                            $"Database=CRNProductAssessmentDB;" +
                            $"User Id=sa;" +
                            $"Password={sqlPassword};" +
                            $"TrustServerCertificate=True;" +
                            $"MultipleActiveResultSets=True"
                    });
            });

            builder.ConfigureServices(services =>
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                    "Test",
                    options => { });
            });
        }
    }
}