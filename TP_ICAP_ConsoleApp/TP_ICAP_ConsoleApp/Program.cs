// See https://aka.ms/new-console-template for more information
using TP_ICAP_ConsoleApp.Logic;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TP_ICAP_ConsoleApp;
using TP_ICAP_ConsoleApp.Containers;
using TP_ICAP_ConsoleApp.Controllers;

using IHost host = CreateHostBuilder(args).Build();
using var scope = host.Services.CreateScope();

var services = scope.ServiceProvider;

try 
{
    services.GetRequiredService<App>().Run(args);
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}

static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton<IMatchAlgorithms, MatchAlgorithms>();
            services.AddSingleton<IFastBookOrdered, FastBookOrdered>();
            services.AddSingleton<ISellOrders, SellOrders>();
            services.AddSingleton<IOrdersProcessor, OrdersProcessor>();
            services.AddSingleton<App> ();
        });
}



