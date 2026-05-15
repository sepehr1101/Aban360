using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts
{
    public interface IReturnBillSetConfirmHandler
    {
        Task<FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto>> Handle(ReturnBillSetConfirmInputDto input, IAppUser appUser, CancellationToken cancellationToken);
    }
}
