using CnvProfileCalculation.Domain.Model;

namespace CnvProfileCalculation.Domain.Repositories;

public interface ICnvVariantRepository
{
    Task<IEnumerable<Analysis<CnvVariant>>> GetCnvVariants();
}