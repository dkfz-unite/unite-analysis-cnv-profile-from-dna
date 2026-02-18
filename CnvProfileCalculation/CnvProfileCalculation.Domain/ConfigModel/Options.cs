namespace CnvProfileCalculation.Domain.ConfigModel;

public class Options
{
    public string DataPath { get; set; } = string.Empty;
    public string CnvAnalysisFilePath { get; set; } = string.Empty;
    public string CnvProfileAnalysisFilePath { get; set; } = string.Empty;
    public string CnvProfileFileName { get; set; } = string.Empty;
    public IDictionary<string, GenomeOptions> Genomes { get; set; }
}