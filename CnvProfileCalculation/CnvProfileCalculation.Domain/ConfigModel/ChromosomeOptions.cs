using CnvProfileCalculation.Domain.ConfigModel;
using Unite.Data.Entities.Omics.Enums;

namespace CnvProfileCalculation.Domain.Model;

public class ChromosomeOptions
{
    public IDictionary<ChromosomeArm, ChromosomeArmOptions> ChromosomeArms { get; set; }
}