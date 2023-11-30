// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TP_ICAP_ConsoleApp;
using TP_ICAP_ConsoleApp.Containers;
using TP_ICAP_ConsoleApp.Controllers;
using TP_ICAP_ConsoleApp.Logic;

using IHost host = CreateHostBuilder(args).Build();
using var scope = host.Services.CreateScope();

var services = scope.ServiceProvider;

try
{
    services.GetRequiredService<App>().Run(args);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton<IPriceTimePriorityAlgorithm, PriceTimePriorityAlgorithm>();
            services.AddSingleton<IProRataAlgorithm, ProRataAlgorithm>();
            services.AddSingleton<IFastBookOrdered, FastBookOrdered>();
            services.AddSingleton<ISellOrders, SellOrders>();
            services.AddSingleton<IOrdersProcessor, OrdersProcessor>();
            services.AddSingleton<App>();
        });
}



