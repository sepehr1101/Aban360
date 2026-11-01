using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Implementations
{
    internal sealed class MeterReadingCalculationConfirmThenSendSmsHandler : IMeterReadingCalculationConfirmThenSendSmsHandler
    {
        private readonly ICalculationConfirmationHandler _confirmationHandler;
        private readonly IMeterReadingDetailSendSmsHandler _detailSendSmsHandler;
        public MeterReadingCalculationConfirmThenSendSmsHandler(
            ICalculationConfirmationHandler confirmationHandler,
            IMeterReadingDetailSendSmsHandler detailSendSmsHandler)
        {
            _confirmationHandler = confirmationHandler;
            _confirmationHandler.NotNull(nameof(confirmationHandler));

            _detailSendSmsHandler = detailSendSmsHandler;
            _detailSendSmsHandler.NotNull(nameof(detailSendSmsHandler));
        }

        public async Task Handle(int latestFlowId, Guid userId, CancellationToken cancellationToken)
        {
            MeterReadingCheckedOutputDto calculationConfirmResult = await _confirmationHandler.Handle(latestFlowId, userId, cancellationToken);
            await _detailSendSmsHandler.Handle(latestFlowId, userId, cancellationToken);
        }
    }
}
