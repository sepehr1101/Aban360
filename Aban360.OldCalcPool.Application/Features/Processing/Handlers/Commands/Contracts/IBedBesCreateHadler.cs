using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts
{
    public interface IBedBesCreateHadler
    {
        Task Handle(AbBahaCalculationDetails inputDto, decimal codVas, CancellationToken cancellationToken);
    }
}
