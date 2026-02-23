using System.Collections.Immutable;
using CnvProfileCalculation.Domain.ConfigModel;
using CnvProfileCalculation.Domain.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Unite.Data.Entities.Omics.Analysis.Dna.Cnv.Enums;
using Unite.Data.Entities.Omics.Enums;
using Options = CnvProfileCalculation.Domain.ConfigModel.Options;

namespace CnvProfileCalculation.Domain.Services;

public class CnvProfileCalculationService(IOptions<Options> options, ILogger<CnvProfileCalculationService> logger)
{
    class CnvAggregation
    {
        public IList<CnvVariant> Gains { get; set; } = new List<CnvVariant>();
        public IList<CnvVariant> Losses { get; set; } = new List<CnvVariant>();
        public IList<CnvVariant> Neutrals { get; set; }  = new List<CnvVariant>();
    }
    
    public Task<Analysis<CnvProfile>> CalculateCnvProfile(Analysis<CnvVariant> cnvAnalysis)
    {
        if(!options.Value.Genomes.TryGetValue(cnvAnalysis.Genome, out var genomeOptions))
            throw new Exception($"Genome not found: {cnvAnalysis.Genome}");

        var cnvProfileAnalysis = new Analysis<CnvProfile>
        {
            Genome = cnvAnalysis.Genome,
            AnalysisType = cnvAnalysis.AnalysisType,
            DonorKey =  cnvAnalysis.DonorKey,
            MatchedSpecimen =  cnvAnalysis.MatchedSpecimen,
            SpecimenKey = cnvAnalysis.SpecimenKey,
            SpecimenType =  cnvAnalysis.SpecimenType,
            MatchedSpecimenType =  cnvAnalysis.MatchedSpecimenType,
            Path =  cnvAnalysis.Path,
            Reader = cnvAnalysis.Reader,
            Ploidy = cnvAnalysis.Ploidy,
            Purity =  cnvAnalysis.Purity
        };
        
        foreach (var chromosomePair in genomeOptions.Chromosomes)
        {
            var aggregations = new Dictionary<ChromosomeArm, CnvAggregation>();
            var cnvs = cnvAnalysis.Entries.Where(x => x.Chromosome == chromosomePair.Key).ToArray();
            
            foreach (var cnvVariant in cnvs)
            {
                var chromosomeArm = GetChromosomeArm(cnvVariant, chromosomePair.Value);
                if (chromosomeArm.HasValue)
                {
                    if (!aggregations.TryGetValue(chromosomeArm.Value, out var aggregation))
                    {
                        aggregation = new CnvAggregation();
                        aggregations[chromosomeArm.Value] = aggregation;
                    }
                    
                    if (cnvVariant.CnvType == CnvType.Gain)
                    {
                        aggregation.Gains.Add(cnvVariant);
                    }
                    else if (cnvVariant.CnvType == CnvType.Loss)
                    {
                        aggregation.Losses.Add(cnvVariant);
                    }
                    else if (cnvVariant.CnvType == CnvType.Neutral)
                    {
                        aggregation.Neutrals.Add(cnvVariant);
                    }
                }
                else
                {
                    logger.LogError($"Cannot find chromosome arm: cnv({cnvVariant.Start}-{cnvVariant.End}), chromosome({chromosomePair.Key})");
                }
            }

            foreach (var cnvAggregationPair in aggregations)
            {
                var chromosomeArmOptions = chromosomePair.Value.ChromosomeArms[cnvAggregationPair.Key];
                
                var cnvProfile = CalculateCnvProfile(chromosomePair.Key, 
                    cnvAggregationPair.Key, 
                    cnvAggregationPair.Value, 
                    chromosomeArmOptions);
                
                cnvProfileAnalysis.Entries.Add(cnvProfile);
            }
        }
        
        return Task.FromResult(cnvProfileAnalysis);
    }

    private ChromosomeArm? GetChromosomeArm(CnvVariant cnvVariant, ChromosomeOptions chromosomeOptions)
    {
        foreach (var chromosomeArmPair in chromosomeOptions.ChromosomeArms)
        {
            var chromosomeArmOptions = chromosomeArmPair.Value;
            if(cnvVariant.Start >= chromosomeArmOptions.Start && cnvVariant.End <= chromosomeArmOptions.End)
                return chromosomeArmPair.Key;
        }

        return null;
    }

    private CnvProfile CalculateCnvProfile(Chromosome chromosome, 
        ChromosomeArm chromosomeArm, 
        CnvAggregation aggregation, 
        ChromosomeArmOptions chromosomeArmOptions)
    {
        float armLength = chromosomeArmOptions.End - chromosomeArmOptions.Start;

        var gains = aggregation.Gains.OrderBy(x => x.Start);
        
        uint totalGain = CalculateTotalLength(MergeVariants(aggregation.Gains));
        uint totalLoss = CalculateTotalLength(MergeVariants(aggregation.Losses));
        uint totalNeutral = CalculateTotalLength(MergeVariants(aggregation.Neutrals));

        var cnvProfile = new CnvProfile
        {
            Chromosome =  chromosome,
            ChromosomeArm = chromosomeArm,
            Gain = totalGain / armLength,
            Loss = totalLoss / armLength,
            Neutral = totalNeutral / armLength
            
        };

        return cnvProfile;
    }

    private static IList<CnvVariant> MergeVariants(IList<CnvVariant> variants)
    {
        var sortedVariants = variants.OrderBy(x => x.Start).ToList();
        var mergedVariants = new List<CnvVariant>();

        CnvVariant currentRange = null;
        foreach (var variant in sortedVariants)
        {
            if (currentRange == null)
            {
                currentRange = new CnvVariant
                {
                    Start = variant.Start,
                    End = variant.End,
                    Chromosome = variant.Chromosome, 
                    CnvType = variant.CnvType
                };
            }

            if (variant.Start < currentRange.End)
            {
                if(variant.End > currentRange.End)
                    currentRange.End = variant.End;
            }
            else
            {
                mergedVariants.Add(currentRange);
                currentRange = null;
            }

        }

        return mergedVariants;
    }

    private static uint CalculateTotalLength(IList<CnvVariant> variants)
    {
        uint totalLength = 0;
        foreach (var cnvVariant in variants)
        {
            uint length = cnvVariant.End - cnvVariant.Start;
            totalLength += length;
        }
        
        return totalLength;
    }
}