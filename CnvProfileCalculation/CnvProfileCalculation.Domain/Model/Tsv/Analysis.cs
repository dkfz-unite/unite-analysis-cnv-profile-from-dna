using Unite.Essentials.Tsv.Attributes;

namespace CnvProfileCalculation.Domain.Model.Tsv;

public class Analysis
{
    [Column("donor_id")]
    public string Donor { get; set; }
    
    [Column("specimen_id")]
    public string Specimen { get; set; }
    
    [Column("specimen_type")]
    public string SpecimenType { get; set; }
    
    [Column("matched_specimen_id")]
    public string MatchedSpecimen { get; set; }
    
    [Column("matched_specimen_type")]
    public string MatchedSpecimenType { get; set; }
    
    [Column("analysis_type")]
    public string AnalysisType { get; set; }
    
    [Column("genome")]
    public string Genome { get; set; }
    
    [Column("purity")]
    public float Purity { get; set; }
    
    [Column("ploidy")]
    public float Ploidy { get; set; }
    
    [Column("reader")]
    public string Reader { get; set; }
    
    [Column("path")]
    public string Path { get; set; }
}