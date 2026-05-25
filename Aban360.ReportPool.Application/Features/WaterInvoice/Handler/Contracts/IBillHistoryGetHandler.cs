using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;

namespace Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Contracts
{
    public interface IBillHistoryGetHandler
    {
        Task<ReportOutput<BillHistoryHeaderOutputDto, BillHistoryDataOutputDto>> Handle(BillHistoryInputDto input, IAppUser appUser, CancellationToken cancellationToken);
    }
}
