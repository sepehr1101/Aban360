using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Transactions;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts
{
    public interface ICustomerGeneralInfoQueryService
    {
        Task<ReportOutput<CustomerGeneralInfoHeaderDto, CustomerGeneralInfoDataDto>> Get(ZoneIdAndCustomerNumberOutputDto input);
    }
}
