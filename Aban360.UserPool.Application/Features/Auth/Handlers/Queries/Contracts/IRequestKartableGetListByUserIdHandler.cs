using Aban360.Common.BaseEntities;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts
{
    public interface IRequestKartableGetListByUserIdHandler
    {
        Task<IEnumerable<SelectionDto>> Handle(Guid userId, CancellationToken cancellationToken);
    }
}
