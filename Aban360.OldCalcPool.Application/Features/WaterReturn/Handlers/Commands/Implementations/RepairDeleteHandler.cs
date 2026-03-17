using Aban360.OldCalcPools.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Command.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Command.Implementations;
using System.Data;
using Aban360.Common.Db.Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPools.Application.Features.WaterReturn.Handlers.Commands.Implementations
{
    internal sealed class RepairDeleteHandler : AbstractBaseConnection, IRepairDeleteHandler
    {
        private readonly IValidator<RepairDeleteDto> _validator;
        public RepairDeleteHandler(
            IValidator<RepairDeleteDto> validator,
            IConfiguration configuration)
            : base(configuration)
        {
            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(RepairDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(deleteDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    RepairCommandService repairCommandService = new(connection, transaction);
                    await repairCommandService.Delete(deleteDto);
                }
            }
        }
    }
}
