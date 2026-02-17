using CnvProfileCalculation.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Options = CnvProfileCalculation.Domain.Model.Options;

namespace CnvProfileCalculation;

public sealed class App(IOptions<Options> options, 
    ILogger<App> logger,
    ICnvVariantRepository cnvVariantRepository)
{
    public async Task RunAsync()
    {
        logger.LogInformation("Running...");
        logger.LogInformation($"DataPath: {options.Value.DataPath}");

        var cnvs = await cnvVariantRepository.GetCnvVariants();
        //TODO: use a service instead of direct usage of repository
    }
}