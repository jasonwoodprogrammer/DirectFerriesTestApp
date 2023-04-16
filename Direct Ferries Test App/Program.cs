using Direct_Ferries_Test_App;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<IDataService, DataService>();
        services.AddTransient<ILogger, Logger>();
    })
    .Build();

var dataService = new DataService(host.Services.GetRequiredService<ILogger>());

await dataService.ProcessData();
