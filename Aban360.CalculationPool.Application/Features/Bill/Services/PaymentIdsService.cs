using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.Bill.Services;
public interface IPaymentIdsService
{
    Task<int> PaymentIdBatchUpdateAsync(PaymentIdBatchUpdateInputDto inputDto, CancellationToken cancellationToken);
    Task<string> GeneratePaymentIdGetAsync(GeneratePaymentIdInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task<ReportOutput<InvalidPaymentIdHeaderOutputDto, InvalidPaymentIdDataOutputDto>> InvalidPaymentIdGetAsync(InvalidPaymentIdInputDto input, IAppUser appUser, CancellationToken cancellationToken);
}

internal sealed class PaymentIdsService : AbstractBaseConnection, IPaymentIdsService
{
    public PaymentIdsService(ICommonZoneService commonZoneService, IValidator<PaymentIdBatchUpdateInputDto> validator, IConfiguration configuration, ICommonMemberQueryService memberQueryService, IBedBesQueryService bedBesQueryService) : base(configuration)
    {
        _commonZoneService = commonZoneService;
        _commonZoneService.NotNull(nameof(commonZoneService));
        _validator = validator;
        _validator.NotNull(nameof(validator));
        _memberQueryService = memberQueryService;
        _memberQueryService.NotNull(nameof(memberQueryService));
        _zoneService = commonZoneService;
        _zoneService.NotNull(nameof(commonZoneService));
        _bedBesQueryService = bedBesQueryService;
        _bedBesQueryService.NotNull(nameof(bedBesQueryService));
    }

    private readonly ICommonZoneService _commonZoneService;
    private readonly IValidator<PaymentIdBatchUpdateInputDto> _validator;
    public async Task<int> PaymentIdBatchUpdateAsync(PaymentIdBatchUpdateInputDto inputDto, CancellationToken cancellationToken)
    {
        await PaymentIdBatchUpdateInputValidate(inputDto, cancellationToken);
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
                        PaymentInfo paymentInfo = await connection.QuerySingleOrDefaultAsync<PaymentInfo>(new CommandDefinition(selectQuery, new { id }, transaction, cancellationToken: cancellationToken));
                        if (paymentInfo is null)
                        {
                            throw new CustomValidationException($"رکوردی با شناسه {id} در ناحیه {inputDto.ZoneId} یافت نشد");
                        }

                        string paymentId = TransactionIdGenerator.GeneratePaymentId(paymentInfo.PayableAmount, paymentInfo.BillId, "106");
                        updatedCount += await connection.ExecuteAsync(new CommandDefinition(updateQuery, new { paymentId, id }, transaction, cancellationToken: cancellationToken));
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

    private async Task PaymentIdBatchUpdateInputValidate(PaymentIdBatchUpdateInputDto inputDto, CancellationToken cancellationToken)
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

    private readonly ICommonMemberQueryService _memberQueryService;
    private readonly ICommonZoneService _zoneService;
    public async Task<string> GeneratePaymentIdGetAsync(GeneratePaymentIdInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _memberQueryService.Get(inputDto.BillId);
        await _zoneService.IsUserInZone(appUser, zoneIdAndCustomerNumber.ZoneId);
        string paymentTypeOption = inputDto.IsWater ? CommonLiterals.WaterPayIdUniqueCode : CommonLiterals.BranchPayIdUniqueCode;
        string paymentId = TransactionIdGenerator.GeneratePaymentId(inputDto.Amount, inputDto.BillId, $"{paymentTypeOption}00");
        return paymentId;
    }

    private readonly IBedBesQueryService _bedBesQueryService;
    private static string _title = ReportLiterals.InvalidPaymentId;
    public async Task<ReportOutput<InvalidPaymentIdHeaderOutputDto, InvalidPaymentIdDataOutputDto>> InvalidPaymentIdGetAsync(InvalidPaymentIdInputDto input, IAppUser appUser, CancellationToken cancellationToken)
    {
        IEnumerable<InvalidPaymentIdDataOutputDto> data = await _bedBesQueryService.Get(input);
        InvalidPaymentIdHeaderOutputDto header = new()
        {
            ZoneId = input.ZoneId,
            ZoneTitle = data?.FirstOrDefault()?.ZoneTitle ?? string.Empty,
            BillCount = data?.Count() ?? 0,
            CustomerCount = data?.DistinctBy(d => d.CustomerNumber)?.Count() ?? 0,
            RecordCount = data?.Count() ?? 0,
            Title = _title
        };
        ReportOutput<InvalidPaymentIdHeaderOutputDto, InvalidPaymentIdDataOutputDto> result = new(_title, header, data);
        return result;
    }
}
