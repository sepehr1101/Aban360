using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;

namespace Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Contracts
{
    public interface IBillLatestListGetHandler
    {
        Task<ReportOutput<BillLatestListHeaderOutputDto, BillLatestListDataOutputDto>> Handle(BillLatestListInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
        Task<ReportOutput<BillLatestListHeaderOutputDto, BillLatestListDataOutputDto>> HandleByBedBes(BillLatestListInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
