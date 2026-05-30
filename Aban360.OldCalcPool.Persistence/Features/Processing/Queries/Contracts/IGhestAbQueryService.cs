using Aban360.Common.BaseEntities;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts
{
    public interface IGhestAbQueryService
    {
        Task<IEnumerable<BillInstallmentOutputDto>> Get(ZoneIdAndCustomerNumber inputDto);
        Task<IEnumerable<BillInstallmentOutputDto>> Get(ZoneIdAndCustomerNumber inputDto, string dateJalali);
        Task<IEnumerable<BillInstallmentOutputDto>> GetLatestBatch(ZoneIdAndCustomerNumber inputDto);
        Task<BillInstallmentOutputDto> Get(ZoneIdAndCustomerNumber inputDto, int id);
    }
}
