using CnvProfileCalculation.Domain.Model;
using Microsoft.Extensions.Options;
using Unite.Essentials.Extensions;
using Unite.Essentials.Tsv;
using Options = CnvProfileCalculation.Domain.Model.Options;

namespace CnvProfileCalculation.Domain.Repositories;

public class CnvVariantFileRepository(IOptions<Options> options): ICnvVariantRepository
{
    public async Task<IEnumerable<Analysis<CnvVariant>>> GetCnvVariants()
    {
        var optionsValue = options.Value;
        var cnvAnalysisPath = Path.Combine(optionsValue.DataPath, optionsValue.CnvAnalysisFilePath);

        if (!File.Exists(cnvAnalysisPath))
            throw new Exception($"CNV Analysis file not found: {cnvAnalysisPath}");

        var cnvAnalysisFileContent = await File.ReadAllTextAsync(cnvAnalysisPath);
        if(string.IsNullOrEmpty(cnvAnalysisFileContent))
            throw new Exception("Invalid CNV Analysis file");
        
        IEnumerable<CnvAnalysis> cnvAnalyses = TsvReader.Read<CnvAnalysis>(cnvAnalysisFileContent)
            .ToArrayOrNull();

        List<Analysis<CnvVariant>> analyses = new List<Analysis<CnvVariant>>();
        
        foreach (var cnvAnalysis in cnvAnalyses)
        {
            var analysis = new Analysis<CnvVariant>
            {
                DonorKey = cnvAnalysis.Donor,
                SpecimenKey =  cnvAnalysis.Specimen,
                SpecimenType = cnvAnalysis.SpecimenType,
                MatchedSpecimen =  cnvAnalysis.MatchedSpecimen,
                MatchedSpecimenType = cnvAnalysis.MatchedSpecimenType,
                AnalysisType = cnvAnalysis.AnalysisType,
                Genome = cnvAnalysis.Genome,
                Reader = cnvAnalysis.Reader,
                Path = cnvAnalysis.Path,
                Entries = await ReadCnvVariants(cnvAnalysis)
            };
            
            analyses.Add(analysis);
        }

        return analyses;
    }

    private async Task<IList<CnvVariant>> ReadCnvVariants(CnvAnalysis cnvAnalysis)
    {
        var variants = new List<CnvVariant>();

        var path = Path.Combine(options.Value.DataPath, cnvAnalysis.Path);
        if(!File.Exists(path))
            throw new Exception($"CNV data file not found: {path}");
        
        var cnvDataFileContent = await File.ReadAllTextAsync(path);
        if(string.IsNullOrEmpty(cnvDataFileContent))
            throw new Exception("Invalid CNV data file");
        
        IEnumerable<CnvEntry> cnvDataRows = TsvReader.Read<CnvEntry>(cnvDataFileContent)
            .ToArrayOrNull();

        foreach (var cnvDataRow in cnvDataRows)
        {
            var variant = new CnvVariant
            {
                Chromosome =  cnvDataRow.Chromosome,
                Start =  cnvDataRow.Start,
                End = cnvDataRow.End,
                CnvType = cnvDataRow.CnvType
            };
            
            variants.Add(variant);
        }
        
        return variants;
    }
}