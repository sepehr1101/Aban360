using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts
{
    public interface IMeterReadingDetailLatestInfoByReadingNumberGetHandler
    {
        Task<ReportOutput<BillTransactionDetailWithLastReadingDataHeaderOutputDto, BillTransactionDetailDataOutputDto>> Handle(string readingNumber, IAppUser appUser, CancellationToken cancellationToken);
    }
}
