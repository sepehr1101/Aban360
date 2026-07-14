using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Commands.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Implementations
{
    internal sealed class MeterSmsFlowUpdateHandler : AbstractBaseConnection, IMeterSmsFlowUpdateHandler
    {
        private readonly IValidator<MeterSmsFlowUpdateDto> _validator;
        public MeterSmsFlowUpdateHandler(
            IValidator<MeterSmsFlowUpdateDto> validator,
            IConfiguration configuration)
            : base(configuration)
        {
            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(MeterSmsFlowUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            await ExecSql(inputDto);
        }
        private async Task ExecSql(MeterSmsFlowUpdateDto inputDto)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MeterSmsFlowCommandService meterSmsFlowCommandService = new(connection, transaction);
                    await meterSmsFlowCommandService.Update(inputDto);

                    transaction.Commit();
                }
            }
        }
        private async Task InputValidate(MeterSmsFlowUpdateDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
    }
}
