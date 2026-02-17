using CnvProfileCalculation.Domain.Model;
using Microsoft.Extensions.Options;
using Unite.Data.Entities.Omics.Analysis.Dna.Cnv.Enums;
using Unite.Data.Entities.Omics.Enums;
using Options = CnvProfileCalculation.Domain.Model.Options;

namespace CnvProfileCalculation.Domain.Services;

public class CnvProfileCalculationService(IOptions<Options> options)
{
    class CnvAggregation
    {
        public IList<CnvVariant> Gains { get; set; } = new List<CnvVariant>();
        public IList<CnvVariant> Losses { get; set; } = new List<CnvVariant>();
        public IList<CnvVariant> Neutrals { get; set; }  = new List<CnvVariant>();
    }
    
    public Analysis<CnvProfile> CalculateCnvProfile(Analysis<CnvVariant> cnvAnalysis)
    {
        if(!options.Value.Genomes.TryGetValue(cnvAnalysis.Genome, out var genomeOptions))
            throw new Exception($"Genome not found: {cnvAnalysis.Genome}");

        var cnvProfileAnalysis = new Analysis<CnvProfile>();
        
        foreach (var chromosomePair in genomeOptions.Chromosomes)
        {
            var aggregation = new Dictionary<ChromosomeArm, CnvAggregation>();
            var cnvs = cnvAnalysis.Entries.Where(x => x.Chromosome == chromosomePair.Key).ToArray();
            
            foreach (var cnvVariant in cnvs)
            {
                var chromosomeArm = GetChromosomeArm(cnvVariant, chromosomePair.Value);
                if (cnvVariant.CnvType == CnvType.Gain)
                {
                    aggregation[chromosomeArm].Gains.Add(cnvVariant);
                }
                else if (cnvVariant.CnvType == CnvType.Loss)
                {
                    aggregation[chromosomeArm].Losses.Add(cnvVariant);
                }
                else if (cnvVariant.CnvType == CnvType.Neutral)
                {
                    aggregation[chromosomeArm].Neutrals.Add(cnvVariant);
                }
            }

            foreach (var cnvAggregationPair in aggregation)
            {
                var cnvProfile = CalculateCnvProfile(chromosomePair.Key, cnvAggregationPair.Key, cnvAggregationPair.Value);
                cnvProfileAnalysis.Entries.Add(cnvProfile);
            }
        }
        
        return cnvProfileAnalysis;
    }

    private ChromosomeArm GetChromosomeArm(CnvVariant cnvVariant, ChromosomeOptions chromosomeOptions)
    {
        //TODO: implement
        throw new NotImplementedException();
    }

    private CnvProfile CalculateCnvProfile(Chromosome chromosome, ChromosomeArm chromosomeArm, CnvAggregation aggregation)
    {
        //TODO: implement
        throw new NotImplementedException();
    }
}