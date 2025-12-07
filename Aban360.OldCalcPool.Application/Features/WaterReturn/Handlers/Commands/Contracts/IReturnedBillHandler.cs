using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts
{
    public interface IReturnedBillHandler
    {
        Task Handle(ReturnedBillInputDto inputDto, CancellationToken cancellationToken);
    }
}
