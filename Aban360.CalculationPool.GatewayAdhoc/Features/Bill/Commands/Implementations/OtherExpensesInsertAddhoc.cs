using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.CalculationPool.GatewayAdhoc.Features.Bill.Commands.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.GatewayAdhoc.Features.Bill.Commands.Implementations
{
    internal sealed class OtherExpensesInsertAddhoc : IOtherExpensesInsertAddhoc
    {
        private readonly IOtherExpensesInsertHandler _otherExpensesInsertHandler;
        public OtherExpensesInsertAddhoc(IOtherExpensesInsertHandler otherExpensesInsertHandler)
        {
            _otherExpensesInsertHandler = otherExpensesInsertHandler;
            _otherExpensesInsertHandler.NotNull(nameof(otherExpensesInsertHandler));
        }

        public async Task<ReportOutput<OtherExpensesHeaderOutputDto, OtherExpensesDataOutputDto>> Handle(OtherExpensesInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            ReportOutput<OtherExpensesHeaderOutputDto, OtherExpensesDataOutputDto> result = await _otherExpensesInsertHandler.Handle(inputDto, appUser, cancellationToken);
            return result;
        }
    }
}
