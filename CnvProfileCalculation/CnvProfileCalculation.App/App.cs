using CnvProfileCalculation.Domain.Model;
using CnvProfileCalculation.Domain.Repositories;
using CnvProfileCalculation.Domain.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Options = CnvProfileCalculation.Domain.ConfigModel.Options;

namespace CnvProfileCalculation;

public sealed class App(IOptions<Options> options, 
    ILogger<App> logger,
    ICnvVariantRepository cnvVariantRepository,
    CnvProfileCalculationService  cnvProfileCalculationService,
    ICnvProfileRepository cnvProfileRepository)
{
    public async Task RunAsync()
    {
        logger.LogInformation("Running...");
        logger.LogInformation($"DataPath: {options.Value.DataPath}");

        //TODO: use a service instead of direct usage of repository
        var cnvAnalyses = await cnvVariantRepository.GetCnvVariants();
       
        List<Analysis<CnvProfile>> analyses = new List<Analysis<CnvProfile>>();
        foreach (var cnvAnalysis in cnvAnalyses)
        {
            var profiles = await cnvProfileCalculationService.CalculateCnvProfile(cnvAnalysis);
            analyses.Add(profiles);
            
            /*foreach (var profilesEntry in profiles.Entries)
            {
                logger.LogInformation($"CNV Profile: ch {profilesEntry.Chromosome}, cha {profilesEntry.ChromosomeArm}, g {profilesEntry.Gain}, l {profilesEntry.Loss}, n {profilesEntry.Neutral}");
            }*/
        }
        
        cnvProfileRepository.StoreCnvProfileAnalyses(analyses);
    }
}