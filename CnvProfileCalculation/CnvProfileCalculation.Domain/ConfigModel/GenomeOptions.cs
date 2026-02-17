using CnvProfileCalculation.Domain.Model;
using Unite.Data.Entities.Omics.Enums;

namespace CnvProfileCalculation.Domain.ConfigModel;

public class GenomeOptions
{
    public IDictionary<Chromosome, ChromosomeOptions> Chromosomes { get; set; }
}