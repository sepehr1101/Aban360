using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Create.Contracts
{
    public interface ISubModuleCreateHandler
    {
        Task Handle(SubModuleCreateDto createDto, CancellationToken cancellationToken);
    }
}