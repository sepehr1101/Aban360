using Aban360.Common.BaseEntities;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;

namespace Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Queries.Contracts
{
    public interface IMaliatMaaherDetailGetByWrapperIdHandler
    {
        Task<ReportOutput<MaliatMaaherWrapperGetDto, MaliatMaaherDetailGetDto>> Handle(SearchInput inputDto,CancellationToken cancellationToken);  
    }
}
