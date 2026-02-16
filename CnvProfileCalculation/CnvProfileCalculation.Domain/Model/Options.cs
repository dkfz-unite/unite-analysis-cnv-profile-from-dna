namespace CnvProfileCalculation.Domain.Model;

public class Options
{
    public string OutputDataPath { get; set; } = string.Empty;
    public SqlOptions SqlOptions { get; set; } = new();
    public string GenomeBuild { get; set; } = "GRCh37";
}