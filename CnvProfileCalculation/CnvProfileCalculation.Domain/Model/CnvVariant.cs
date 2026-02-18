using Unite.Data.Entities.Omics.Analysis.Dna.Cnv.Enums;
using Unite.Data.Entities.Omics.Enums;

namespace CnvProfileCalculation.Domain.Model;

public class CnvVariant
{
    public Chromosome Chromosome { get; set; }
    public uint Start { get; set; } = 0;
    public uint End { get; set; } = 0;
    public CnvType CnvType { get; set; }
}