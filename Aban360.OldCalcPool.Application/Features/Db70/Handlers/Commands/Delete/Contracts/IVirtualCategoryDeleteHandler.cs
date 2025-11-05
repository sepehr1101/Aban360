using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Delete.Contracts
{
    public interface IVirtualCategoryDeleteHandler
    {
        Task Handle(SearchShortInputDto deleteDto, CancellationToken cancellationToken);
    }
}
