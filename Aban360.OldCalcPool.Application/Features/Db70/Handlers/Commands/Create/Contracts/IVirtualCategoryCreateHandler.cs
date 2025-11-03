using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Create.Contracts
{
    public interface IVirtualCategoryCreateHandler
    {
        Task Handle(VirtualCategoryCreateDto createDto, CancellationToken cancellationToken);
    }
}