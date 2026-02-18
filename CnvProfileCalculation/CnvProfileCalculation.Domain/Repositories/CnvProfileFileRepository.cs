using CnvProfileCalculation.Domain.Model;
using CnvProfileCalculation.Domain.Model.Tsv;
using Microsoft.Extensions.Options;
using Unite.Essentials.Tsv;
using CnvProfile = CnvProfileCalculation.Domain.Model.CnvProfile;

namespace CnvProfileCalculation.Domain.Repositories;

public class CnvProfileFileRepository(IOptions<CnvProfileCalculation.Domain.ConfigModel.Options> options): ICnvProfileRepository
{
    public void StoreCnvProfileAnalyses(IEnumerable<Analysis<CnvProfile>> cnvProfileAnalyses)
    {
        var analysesPath = Path.Combine(options.Value.DataPath, options.Value.CnvProfileAnalysisFilePath);

        if (File.Exists(analysesPath))
        {
            File.Delete(analysesPath);
        }

        List<CnvProfileAnalysis> analyses = new List<CnvProfileAnalysis>();
        
        foreach (var cnvProfileAnalysis in cnvProfileAnalyses)
        {
            var cnvPath = cnvProfileAnalysis.Path;
            
            //TODO: alert if directory name is invalid
            var directoryName = Path.GetDirectoryName(cnvPath);
            directoryName = string.IsNullOrEmpty(directoryName) ? "." : directoryName;
            
            var cnvProfilePath = Path.Combine(options.Value.DataPath, directoryName, options.Value.CnvProfileFileName);
            if (File.Exists(cnvProfilePath))
            {
                File.Delete(cnvProfilePath);
            }
            
            var analysis = new CnvProfileAnalysis
            {
                AnalysisType =  cnvProfileAnalysis.AnalysisType,
                Donor =  cnvProfileAnalysis.DonorKey,
                Genome = cnvProfileAnalysis.Genome,
                MatchedSpecimen =  cnvProfileAnalysis.MatchedSpecimen,
                SpecimenType =  cnvProfileAnalysis.SpecimenType,
                MatchedSpecimenType =   cnvProfileAnalysis.MatchedSpecimenType,
                Reader =  cnvProfileAnalysis.Reader,
                Specimen =  cnvProfileAnalysis.SpecimenKey,
                Path =  cnvProfilePath
            };
            
            analyses.Add(analysis);

            List<Model.Tsv.CnvProfile> entries = new List<Model.Tsv.CnvProfile>();
            
            foreach (var cnvProfileEntry in cnvProfileAnalysis.Entries)
            {
                var entry = new Model.Tsv.CnvProfile
                {
                    Chromosome =  cnvProfileEntry.Chromosome,
                    ChromosomeArm =  cnvProfileEntry.ChromosomeArm,
                    Gain =  cnvProfileEntry.Gain,
                    Loss =  cnvProfileEntry.Loss,
                    Neutral =  cnvProfileEntry.Neutral
                };
                
                entries.Add(entry);
            }

            var cnvProfileFileContent = TsvWriter.Write(entries);
            File.WriteAllText(cnvProfilePath, cnvProfileFileContent);
        }
        
        var cnvProfileAnalysisFileContent = TsvWriter.Write(analyses);
        File.WriteAllText(analysesPath, cnvProfileAnalysisFileContent);
    }
}