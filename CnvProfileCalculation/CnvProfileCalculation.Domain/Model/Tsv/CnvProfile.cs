using Unite.Data.Entities.Omics.Enums;
using Unite.Essentials.Tsv.Attributes;

namespace CnvProfileCalculation.Domain.Model.Tsv;

public class CnvProfile
{
    [Column("chromosome")]
    public Chromosome Chromosome { get; set; }
    [Column("chromosome_arm")]
    public ChromosomeArm ChromosomeArm { get; set; }
    [Column("gain")]
    public float Gain { get; set; }
    [Column("loss")]
    public float Loss { get; set; }
    [Column("neutral")]
    public float Neutral { get; set; }
}