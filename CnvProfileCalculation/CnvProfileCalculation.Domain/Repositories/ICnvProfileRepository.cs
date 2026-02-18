using CnvProfileCalculation.Domain.Model;

namespace CnvProfileCalculation.Domain.Repositories;

public interface ICnvProfileRepository
{
    void StoreCnvProfileAnalyses(IEnumerable<Analysis<CnvProfile>> cnvProfileAnalyses);
}