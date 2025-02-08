using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts
{
    public interface ISubModuleGetAllHandler
    {
        Task<ICollection<SubModuleGetDto>> Handle(CancellationToken cancellationToken);
    }


}