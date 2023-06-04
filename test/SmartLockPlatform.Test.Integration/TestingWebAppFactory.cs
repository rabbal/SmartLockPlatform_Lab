using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using SmartLockPlatform.Application.External;
using SmartLockPlatform.Infrastructure.Hosing;
using Testcontainers.PostgreSql;

namespace SmartLockPlatform.Test.Integration;

[CollectionDefinition(nameof(IntegrationTestFixture))]
public class TestFixtureCollection : ICollectionFixture<IntegrationTestFixture> {}

public class IntegrationTestFixture
{
}
public class TestingWebAppFactory<TProgram> : WebApplicationFactory<TProgram>, IAsyncLifetime where TProgram : class
{
    private readonly PostgreSqlContainer _container;

    private static readonly ISmartMqttClient SmartMqttClientFake = A.Fake<ISmartMqttClient>(); 
    
    public TestingWebAppFactory()
    {
        _container = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("db")
            .WithUsername("postgres")
            .WithPassword("1")
            .WithCleanUp(true)
            .Build();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        // How to override settings ...
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                {
                    "ConnectionStrings:Npgsql",
                    _container.GetConnectionString()
                },
            });
        });
        var host = base.CreateHost(builder);

        // How to run a command during startup
        host.InitializeDb();

        return host;
    }


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Don't run `IHostedService`s when running as a test
            services.RemoveAll(typeof(IHostedService));
            services.AddSingleton(SmartMqttClientFake);
        });

        builder.UseEnvironment("Testing");
    }

    public async Task InitializeAsync() => await _container.StartAsync();

    public new async Task DisposeAsync() => await _container.DisposeAsync();
}