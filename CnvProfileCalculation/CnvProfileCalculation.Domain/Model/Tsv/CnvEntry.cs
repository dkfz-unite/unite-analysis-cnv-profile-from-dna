using Unite.Data.Entities.Omics.Analysis.Dna.Cnv.Enums;
using Unite.Data.Entities.Omics.Enums;
using Unite.Essentials.Tsv.Attributes;

namespace CnvProfileCalculation.Domain.Model.Tsv;

public class CnvEntry
{
    [Column("chromosome")]
    public Chromosome Chromosome { get; set; }
    [Column("start")]
    public uint Start { get; set; }
    [Column("end")]
    public uint End { get; set; }
    [Column("type")]
    public CnvType CnvType { get; set; }
}