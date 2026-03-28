using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts
{
    public interface ICalculationRequestDisplayHandler
    {
        Task<ReportOutput<CalculationRequestDisplayHeaderOutputDto, CalculationRequestDisplayDataOutputDto>> Handle(int trackNumber, CancellationToken cancellationToken);
    }
}
