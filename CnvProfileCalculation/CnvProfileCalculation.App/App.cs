using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Options = CnvProfileCalculation.Domain.Model.Options;

namespace CnvProfileCalculation;

public sealed class App(IOptions<Options> options, ILogger<App> logger)
{
    public Task RunAsync()
    {
        logger.LogInformation("Running...");
        logger.LogInformation($"DataPath: {options.Value.DataPath}");
        return Task.CompletedTask;
    }
}