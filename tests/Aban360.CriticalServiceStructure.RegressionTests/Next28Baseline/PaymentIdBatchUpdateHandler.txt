using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Implementation
{
    internal sealed class PaymentIdBatchUpdateHandler : AbstractBaseConnection, IPaymentIdBatchUpdateHandler
    {
        private readonly ICommonZoneService _commonZoneService;
        private readonly IValidator<PaymentIdBatchUpdateInputDto> _validator;

        public PaymentIdBatchUpdateHandler(
            ICommonZoneService commonZoneService,
            IValidator<PaymentIdBatchUpdateInputDto> validator,
            IConfiguration configuration)
            : base(configuration)
        {
            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<int> Handle(PaymentIdBatchUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            string dbName = GetDbName(inputDto.ZoneId);
            string selectQuery = $"select TRIM(sh_ghabs1) BillId, pard PayableAmount from [{dbName}].dbo.bed_bes where id=@id";
            string updateQuery = $"Update [{dbName}].dbo.bed_bes set sh_pard1=@paymentId where id=@id";

            using (var connection = _sqlReportConnection)
            {
                await connection.OpenAsync(cancellationToken);
                using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        int updatedCount = 0;
                        foreach (int id in inputDto.Ids.Distinct())
                        {
                            PaymentInfo paymentInfo = await connection.QuerySingleOrDefaultAsync<PaymentInfo>(
                                new CommandDefinition(selectQuery, new { id }, transaction, cancellationToken: cancellationToken));

                            if (paymentInfo is null)
                            {
                                throw new CustomValidationException($"رکوردی با شناسه {id} در ناحیه {inputDto.ZoneId} یافت نشد");
                            }

                            string paymentId = TransactionIdGenerator.GeneratePaymentId(paymentInfo.PayableAmount, paymentInfo.BillId, "106");
                            updatedCount += await connection.ExecuteAsync(
                                new CommandDefinition(updateQuery, new { paymentId, id }, transaction, cancellationToken: cancellationToken));
                        }

                        transaction.Commit();
                        return updatedCount;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private async Task InputValidate(PaymentIdBatchUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                string message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }

        private sealed record PaymentInfo
        {
            public string BillId { get; set; }
            public long PayableAmount { get; set; }
        }
    }
}
