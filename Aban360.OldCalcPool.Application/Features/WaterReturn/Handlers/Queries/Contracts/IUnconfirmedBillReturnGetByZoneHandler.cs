using Aban360.Common.BaseEntities;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Queries.Contracts
{
    public interface IUnconfirmedBillReturnGetByZoneHandler
    {
        Task<ReportOutput<UnconfirmedBillReturnHeaderOutputDto, UnconfirmedBillReturnDataOutputDto>> Handle(int zoneId, CancellationToken cancellationToken);
    }
}
