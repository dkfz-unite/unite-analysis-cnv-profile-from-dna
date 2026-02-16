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
        //TODO: select all cnv-variants from the DB
        throw new NotImplementedException();
    }
}