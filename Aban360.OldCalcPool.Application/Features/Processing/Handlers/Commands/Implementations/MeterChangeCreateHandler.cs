using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Timing;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Constants;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class MeterChangeCreateHandler : AbstractBaseConnection, IMeterChangeCreateHandler
    {
        private readonly ICommonMemberQueryService _memberQueryService;
        private readonly IChangeMeterCauseQueryService _changeMeterCauseQueryService;
        private readonly IValidator<MeterChangeInputDto> _validator;
        private int _meterChangeDateLimitMonth = -2;
        public MeterChangeCreateHandler(
            ICommonMemberQueryService memberQueryService,
            IChangeMeterCauseQueryService changeMeterCauseQueryService,
            IValidator<MeterChangeInputDto> validator,
            IConfiguration configuration)
            : base(configuration)
        {
            _memberQueryService = memberQueryService;
            _memberQueryService.NotNull(nameof(memberQueryService));

            _changeMeterCauseQueryService = changeMeterCauseQueryService;
            _changeMeterCauseQueryService.NotNull(nameof(changeMeterCauseQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(MeterChangeInputDto inputDto, CancellationToken cancellationToken)
        {
            await Validation(inputDto, cancellationToken);
            var (memberInfo, changeCause) = await GetInfos(inputDto);
            var (tavizInsertDto, meterChangeInsertDto, contorMeterChangeUpdateDto) = GetCommandDtos(inputDto, memberInfo, changeCause);
            string dbName = GetDbName(memberInfo.ZoneId);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    TavizCommandService tavizCommandService = new(connection, transaction);
                    MeterChangeCommandService meterChangeCommandService = new(connection, transaction);
                    ContorCommandService contorCommandService = new(connection, transaction);

                    await tavizCommandService.Insert(tavizInsertDto, dbName);
                    await meterChangeCommandService.Insert(meterChangeInsertDto);
                    await contorCommandService.UpdateMeterChange(contorMeterChangeUpdateDto, dbName);

                    transaction.Commit();
                }
            }
        }
        private (TavizInsertDto, MeterChangeInsertDto, ContorMeterChangeUpdateDto) GetCommandDtos(MeterChangeInputDto inputDto, MemberInfoGetDto memberInfo, NumericDictionary changeCause)
        {
            TavizInsertDto tavizInsertDto = new()
            {
                ZoneId = memberInfo.ZoneId,
                CustomerNumber = memberInfo.CustomerNumber,
                Operator = 0,
                MeterNumber = inputDto.MeterNumber,
                MeterChangeDateJalali = inputDto.MeterChangeDateJalali,
                MeterDiameterId = memberInfo.MeterDiameterId,
                BodySerial = inputDto.BodySerial,
                ChangeCauseId = inputDto.ChangeCauseId,
                UsageId = memberInfo.UsageId,
            };
            MeterChangeInsertDto meterChangeInsertDto = new()
            {
                ZoneId = memberInfo.ZoneId,
                CustomerNumber = memberInfo.CustomerNumber,
                MeterNumber = inputDto.MeterNumber,
                MeterChangeDateJalali = inputDto.MeterChangeDateJalali,
                BodySerial = inputDto.BodySerial,
                ChangeCauseId = changeCause.Id,
                ChangeCauseTitle = changeCause.Title,
            };
            ContorMeterChangeUpdateDto contorMeterChangeUpdateDto = new()
            {
                ZoneId = memberInfo.ZoneId,
                CustomerNumber = memberInfo.CustomerNumber,
                MeterChangeDateJalali = inputDto.MeterChangeDateJalali,
                MeterChangeNumber = inputDto.MeterNumber
            };

            return (tavizInsertDto, meterChangeInsertDto, contorMeterChangeUpdateDto);
        }
        private async Task<(MemberInfoGetDto, NumericDictionary)> GetInfos(MeterChangeInputDto inputDto)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _memberQueryService.Get(inputDto.BillId);
            MemberInfoGetDto memberInfo = await _memberQueryService.Get(zoneIdAndCustomerNumber);
            NumericDictionary changeCause = await _changeMeterCauseQueryService.Get(inputDto.ChangeCauseId);

            return (memberInfo, changeCause);
        }
        private async Task Validation(MeterChangeInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                string message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
            string meterChangeGregorian = ConvertDate.JalaliToGregorian(inputDto.MeterChangeDateJalali);

            if (meterChangeGregorian.Length != 10)
                throw new InvalidDateException(meterChangeGregorian);

            if (meterChangeGregorian.CompareTo(DateTime.Now.AddMonths(_meterChangeDateLimitMonth).Date.ToString("yyyy-MM-dd")) < 0)
                throw new InvalidDateException(Exceptionliterals.InvalidDateLessThan2Month);
        }
    }
}
