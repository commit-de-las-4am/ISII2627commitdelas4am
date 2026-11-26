using Microsoft.Extensions.Configuration;
using LogViewer;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var subscriber = new Subscriber(configuration);
await subscriber.StartConsuming();

Console.WriteLine("Presiona [Enter] para salir.");
Console.ReadLine();