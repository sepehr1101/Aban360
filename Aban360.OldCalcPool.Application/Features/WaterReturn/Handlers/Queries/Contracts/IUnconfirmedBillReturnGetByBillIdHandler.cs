using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Queries.Contracts
{
    public interface IUnconfirmedBillReturnGetByBillIdHandler
    {
        Task<ReportOutput<UnconfirmedBillReturnHeaderOutputDto, UnconfirmedBillReturnDataOutputDto>> Handle(string billId, IAppUser appUser, CancellationToken cancellationToken);
    }
}
