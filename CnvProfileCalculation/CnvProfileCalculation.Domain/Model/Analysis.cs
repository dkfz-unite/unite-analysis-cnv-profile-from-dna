namespace CnvProfileCalculation.Domain.Model;

public class Analysis<TEntry>
{
    public string DonorKey { get; set; }
    public string SpecimenKey { get; set; }
    public string SpecimenType { get; set; }
    public string MatchedSpecimen { get; set; }
    public string MatchedSpecimenType { get; set; }
    public string AnalysisType { get; set; }
    public string Genome { get; set; }
    public string Reader { get; set; }
    public string Path { get; set; }
    public IEnumerable<TEntry> Entries { get; set; }
}