using CnvProfileCalculation.Domain.Model;
using Microsoft.Extensions.Options;
using Options = CnvProfileCalculation.Domain.Model.Options;

namespace CnvProfileCalculation.Domain.Services;

public class CnvProfileCalculationService(IOptions<Options> options)
{
    public Analysis<CnvProfile> CalculateCnvProfile(Analysis<CnvVariant> cnvAnalysis)
    {
        throw new NotImplementedException();
    }
}