using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Commands.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Implementations
{
    internal sealed class MeterSmsStateTemplateCreateHandler : AbstractBaseConnection, IMeterSmsStateTemplateCreateHandler
    {
        private readonly IValidator<MeterSmsStateTemplateInsertInputDto> _validator;
        public MeterSmsStateTemplateCreateHandler(
            IValidator<MeterSmsStateTemplateInsertInputDto> validator,
            IConfiguration configuration)
            : base(configuration)
        {
            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(MeterSmsStateTemplateInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            MeterSmsStateTemplateInsertDto insertDto = GetInsertDto(inputDto, appUser);
            await ExecSql(insertDto);
        }
        private async Task ExecSql(MeterSmsStateTemplateInsertDto inputDto)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MeterSmsStateTemplateCommandService meterSmsStateCommandService = new(connection, transaction);
                    await meterSmsStateCommandService.Insert(inputDto);

                    transaction.Commit();
                }
            }
        }
        private async Task InputValidate(MeterSmsStateTemplateInsertInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private MeterSmsStateTemplateInsertDto GetInsertDto(MeterSmsStateTemplateInsertInputDto inputDto, IAppUser appUser)
        {
            return new MeterSmsStateTemplateInsertDto()
            {
                SmsTypeId = inputDto.SmsTypeId,
                StepOrder = inputDto.StepOrder,
                SmsText = inputDto.SmsText,
                DueDay = inputDto.DueDay,
                Description = inputDto.Description,
                InsertBy = appUser.UserId,
            };
        }
    }
}
