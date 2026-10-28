using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.GatewayAdhoc.Features.Bill.Commands.Contracts
{
    public interface IOtherExpensesInsertAddhoc
    {
        Task<ReportOutput<OtherExpensesHeaderOutputDto, OtherExpensesDataOutputDto>> Handle(OtherExpensesInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
