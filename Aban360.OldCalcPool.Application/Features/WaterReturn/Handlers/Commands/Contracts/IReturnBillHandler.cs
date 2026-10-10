using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts
{
    public interface IReturnBillHandler
    {
        Task<FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto>> Handle(ReturnBillInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
