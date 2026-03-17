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
    internal sealed class RepairUpdateHandler : AbstractBaseConnection, IRepairUpdateHandler
    {
        private readonly IValidator<RepairUpdateDto> _validator;
        public RepairUpdateHandler(
            IValidator<RepairUpdateDto> validator,
            IConfiguration configuration)
            :base(configuration)
        {
            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(RepairUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            decimal sum = updateDto.AbonFas + updateDto.FasBaha + updateDto.AbBaha + updateDto.Ztadil + updateDto.Shahrdari + updateDto.AbonAb + updateDto.ZaribFasl + updateDto.Ab10 + updateDto.Ab20 + updateDto.Zabresani + updateDto.TabAbnA + updateDto.TabAbnF + updateDto.TabsFa + updateDto.Bodjeh + updateDto.Avarez;
            updateDto.Jam = sum;
            updateDto.Pard = sum;
            updateDto.Baha = sum;

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    RepairCommandService repairCommandService = new(connection, transaction);
                    await repairCommandService.Update(updateDto);
                }
            }
        }
    }
}
