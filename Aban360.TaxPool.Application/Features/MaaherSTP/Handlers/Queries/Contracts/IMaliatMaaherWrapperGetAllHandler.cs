using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;

namespace Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Queries.Contracts
{
    public interface IMaliatMaaherWrapperGetAllHandler
    {
        Task<IEnumerable<MaliatMaaherWrapperGetDto>> Handle(CancellationToken cancellationToken);
    }
}
