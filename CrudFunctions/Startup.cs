using CrudFunctions.Data.Repository;
using CrudFunctions.Domain.Abstractions;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

[assembly: FunctionsStartup(typeof(CrudFunctions.Startup))]

namespace CrudFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddSingleton(new CosmosClient(config["CosmosDbConnectionString"]));
            builder.Services.AddTransient(typeof(ICosmosRepository<>), typeof(CosmosRepository<>));
        }
    }
}