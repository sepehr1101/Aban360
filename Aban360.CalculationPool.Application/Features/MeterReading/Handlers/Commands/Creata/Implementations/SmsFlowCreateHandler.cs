using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Commands.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Timing;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Implementations
{
    internal sealed class SmsFlowCreateHandler : AbstractBaseConnection, ISmsFlowCreateHandler
    {
        private readonly IValidator<SmsFlowInsertInputDto> _validator;
        public SmsFlowCreateHandler(
            IValidator<SmsFlowInsertInputDto> validator,
            IConfiguration configuration)
            : base(configuration)
        {
            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(SmsFlowInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            SmsFlowInsertDto insertDto = GetInsertDto(inputDto, appUser);
            await ExecSql(insertDto);
        }
        private async Task ExecSql(SmsFlowInsertDto inputDto)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    SmsFlowCommandService meterSmsFlowCommandService = new(connection, transaction);
                    await meterSmsFlowCommandService.Insert(inputDto);

                    transaction.Commit();
                }
            }
        }
        private async Task InputValidate(SmsFlowInsertInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private SmsFlowInsertDto GetInsertDto(SmsFlowInsertInputDto inputDto, IAppUser appUser)
        {
            return new SmsFlowInsertDto()
            {
                FirstFlowId = inputDto.FirstFlowId,
                SmsCount = inputDto.SmsCount,
                SmsTemplateId = inputDto.SmsTemplateId,
                InsertBy = appUser.UserId,
                DueDateTime = ConvertDate.JalaliToDateTime(inputDto.DueDateJalali),
            };
        }
    }
}
