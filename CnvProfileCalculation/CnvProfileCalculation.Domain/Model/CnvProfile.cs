using Unite.Data.Entities.Omics.Enums;

namespace CnvProfileCalculation.Domain.Model;

public class CnvProfile
{
    public Chromosome Chromosome { get; set; }
    public ChromosomeArm ChromosomeArm { get; set; }
    public float Gain { get; set; }
    public float Loss { get; set; }
    public float Neutral { get; set; }
}