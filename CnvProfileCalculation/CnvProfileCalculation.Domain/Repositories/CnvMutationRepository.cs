using Unite.Data.Context;

namespace CnvProfileCalculation.Domain.Repositories;

public class CnvMutationRepository(DomainDbContext context)
{
    public IEnumerable<Unite.Data.Entities.Omics.Analysis.Dna.Cnv.Variant> GetCnvMutations(IEnumerable<int> sampleIds)
    {
        return context.CnvEntries.Where(entry => sampleIds.Contains(entry.SampleId))
            .Join(context.Cnvs, entry => entry.EntityId, variant => variant.Id,
            (entry, cnv) => cnv)
            .ToArray();
    }

    public IEnumerable<Unite.Data.Entities.Omics.Analysis.Dna.Cnv.Variant> GetCnvMutations()
    {
        context.CnvEntries
            .Join(context.Cnvs, entry => entry.EntityId, variant => variant.Id,
                (entry, cnv) => new { entry, cnv })
            .Join(context.OmicsSamples, x => x.entry.SampleId, s => s.Id,
                (x, s) =>
                new {
                    
                });
        
        throw new NotImplementedException();
    }
}