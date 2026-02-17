using CnvProfileCalculation;
using CnvProfileCalculation.Domain.Model;
using CnvProfileCalculation.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

// Register services
builder.Services.AddSingleton<ICnvVariantRepository, CnvVariantFileRepository>();
builder.Services.AddTransient<App>();

builder.Services.
    AddOptions<Options>()
    .Bind(builder.Configuration.GetSection("Options"));

builder.Logging.AddConsole();

using var host = builder.Build();

var app = host.Services.GetRequiredService<App>();
await app.RunAsync();