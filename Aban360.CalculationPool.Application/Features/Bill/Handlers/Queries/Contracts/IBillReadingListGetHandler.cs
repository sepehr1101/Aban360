using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts
{
    public interface IBillReadingListGetHandler
    {
        Task<ReportOutput<BillReadingListHeaderOutputDto, BillReadingListDataOutputDto>> Handle(BillReadingListInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
