namespace CnvProfileCalculation.Domain.Model;

public class Options
{
    public string DataPath { get; set; } = string.Empty;
    public string GenomeBuild { get; set; } = "GRCh37";
    public string CnvAnalysisFilePath { get; set; } = string.Empty;
    public string CnvProfileAnalysisFilePath { get; set; } = string.Empty;
}