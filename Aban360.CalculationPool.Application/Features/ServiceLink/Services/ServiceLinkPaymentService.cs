using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.CalculationPool.Persistence.Features.ServiceLink.Commands.Implementations;
using Aban360.CalculationPool.Persistence.Features.ServiceLink.Qeuries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Implementations;
using DNTPersianUtils.Core;
using FluentValidation;
using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Implementations;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Services
{
    public interface IServiceLinkPaymentService
    {
        Task ServiceLinkDeleteAsync(ServiceLinkPaymentRemoveInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
        Task ServiceLinkRegisterManualAsync(ServiceLinkRegisterManualInputDto input, IAppUser appUser, CancellationToken cancellationToken);
        Task<ReportOutput<ServiceLinkPaidHeaderOutputDto, ServiceLinkPaidDataOutputDto>> ServiceLinkPaidGetAsync(ServiceLinkPaidInputDto input, IAppUser appUser, CancellationToken cancellationToken);
    }

    internal sealed class ServiceLinkPaymentService : AbstractBaseConnection, IServiceLinkPaymentService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IVosolEnQueryService _vosolEnQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IVariabService _variabService;
        private readonly IValidator<ServiceLinkRegisterManualInputDto> _validator;
        string _ServiceLinkRegisterManualtodayJalali = DateTime.Now.ToShortPersianDateString();
        string _ServiceLinkRegisterManual60DayAgoDateJalali = DateTime.Now.AddDays(-60).ToShortPersianDateString();
        short _ServiceLinkRegisterManualser = 1;
        short _ServiceLinkRegisterManualoperator = 666;
        short _ServiceLinkRegisterManualtype = 2;
        short _ServiceLinkRegisterManualnoeBed = 2;
        private string _ServiceLinkPaidGettitle = ReportLiterals.ServiceLinkPaid;
        string _ServiceLinkPaidGettodayJalali = DateTime.Now.ToShortPersianDateString();
        string _ServiceLinkPaidGet30DayAgoDateJalali = DateTime.Now.AddDays(-30).ToShortPersianDateString();
        public ServiceLinkPaymentService(IHttpContextAccessor contextAccessor, IVosolEnQueryService vosolEnQueryService, ICommonMemberQueryService commonMemberQueryService, ICommonZoneService commonZoneService, IVariabService variabService, IConfiguration configuration, IValidator<ServiceLinkRegisterManualInputDto> validator) : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));
            _vosolEnQueryService = vosolEnQueryService;
            _vosolEnQueryService.NotNull(nameof(vosolEnQueryService));
            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));
            _commonZoneService = commonZoneService;
            _commonMemberQueryService.NotNull(nameof(commonZoneService));
            _variabService = variabService;
            _variabService.NotNull(nameof(variabService));
            _validator = validator;
            _validator.NotNull(nameof(validator));
            _commonZoneService.NotNull(nameof(commonZoneService));
        }

        public async Task ServiceLinkDeleteAsync(ServiceLinkPaymentRemoveInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await _commonZoneService.IsUserInZone(appUser, inputDto.ZoneId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(new ZoneIdAndCustomerNumber(inputDto.ZoneId, inputDto.CustomerNumber));
            ServiceLinkPaidDataOutputDto paidInfo = await _vosolEnQueryService.Get(inputDto); //todo:in GetMethod:ReName DbName
            await ServiceLinkDeleteValidateDate(inputDto.ZoneId, paidInfo.RegisterDateJalali);
            string opLogText = string.Format(OpLogLiterals.ServiceLinkDeleteManualOpLog, memberInfo.BillId, paidInfo.Amount);
            await ServiceLinkDeleteExecSql(inputDto, appUser, opLogText);
        }

        private async Task ServiceLinkDeleteValidateDate(int zoneId, string payDateJalali)
        {
            string checkDateJalali = await _variabService.GetDateCheck(zoneId);
            if (payDateJalali.CompareTo(checkDateJalali) <= 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidIPaymentDeleteAfterDateCheck);
            }
        }

        private async Task ServiceLinkDeleteExecSql(ServiceLinkPaymentRemoveInputDto inputDto, IAppUser appUser, string opLogText)
        {
            //string dbName = "Atlas";
            string dbName = GetDbName(inputDto.ZoneId);
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    VosolEnCommandService vosolEnCommandService = new(connection, transaction);
                    PaymentEnCommandService paymentEnCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                    await vosolEnCommandService.Remove(inputDto, dbName);
                    await paymentEnCommandService.Remove(inputDto);
                    await opLogCommandService.Insert(opLogText, appUser);
                    transaction.Commit();
                }
            }
        }

        public async Task ServiceLinkRegisterManualAsync(ServiceLinkRegisterManualInputDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            await ServiceLinkRegisterManualValidate(input, cancellationToken);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(new ZoneIdAndCustomerNumber(input.ZoneId, input.CustomerNumber));
            await _commonZoneService.IsUserInZone(appUser, memberInfo.ZoneId);
            VosoEnInsertDto vosolEnInsertDto = ServiceLinkRegisterManualGetVosolEnInsertDto(input, memberInfo);
            PaymentEnInsertDto paymentEnInsertDto = ServiceLinkRegisterManualGetPaymentEnInsertDto(vosolEnInsertDto, memberInfo);
            string opLogText = string.Format(OpLogLiterals.ServiceLinkRegisterManualOpLog, memberInfo.BillId, input.Amount);
            await ServiceLinkRegisterManualExecSql(vosolEnInsertDto, paymentEnInsertDto, appUser, opLogText);
        }

        private async Task ServiceLinkRegisterManualValidate(ServiceLinkRegisterManualInputDto input, CancellationToken cancellationToken)
        {
            await ServiceLinkRegisterManualValidateInput(input, cancellationToken);
            await ServiceLinkRegisterManualValidateDates(input);
        }

        private async Task ServiceLinkRegisterManualValidateInput(ServiceLinkRegisterManualInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }

        private async Task ServiceLinkRegisterManualValidateDates(ServiceLinkRegisterManualInputDto input)
        {
            string checkDateJalali = await _variabService.GetDateCheck(input.ZoneId);
            DateOnly? dateOnlyBank = input.BankDateJalali.ToGregorianDateOnly();
            DateOnly? dateOnlyPay = input.PayDateJalali.ToGregorianDateOnly();
            if (!dateOnlyBank.HasValue || !dateOnlyPay.HasValue)
            {
                throw new InvalidDateException(ExceptionLiterals.InvalidDate);
            }

            if (_ServiceLinkRegisterManualtodayJalali.CompareTo(checkDateJalali) <= 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidPaymentInsertAfterDateCheck);
            }

            if (input.PayDateJalali.CompareTo(_ServiceLinkRegisterManualtodayJalali) > 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidMoreThanCurrentDate);
            }

            if (input.PayDateJalali.CompareTo(checkDateJalali) <= 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidPaymentInsertAfterDateCheck);
            }

            if (input.BankDateJalali.CompareTo(_ServiceLinkRegisterManualtodayJalali) > 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidMoreThanCurrentDate);
            }

            //if (input.BankDateJalali.CompareTo(checkDateJalali) <= 0)
            //{
            //    throw new InvalidBillCommandException(ExceptionLiterals.InvalidPaymentInsertAfterDateCheck);
            //}
            if (input.BankDateJalali.CompareTo(_ServiceLinkRegisterManual60DayAgoDateJalali) < 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidBankDateBefor60DaysAgo);
            }
        }

        private VosoEnInsertDto ServiceLinkRegisterManualGetVosolEnInsertDto(ServiceLinkRegisterManualInputDto input, MemberInfoGetDto memberInfo)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            //decimal barge = await _variabService.GetAndRenew(memberInfo.ZoneId);
            return new VosoEnInsertDto()
            {
                Town = memberInfo.ZoneId,
                Radif = memberInfo.CustomerNumber,
                ParNo = "0",
                PayDate = string.Empty,
                DateBank = input.BankDateJalali,
                DateBes = currentDateJalali,
                DateSabt = input.PayDateJalali, //تاریخ عملکرد
                CodBank = string.IsNullOrWhiteSpace(input.BankBranchCode) ? string.Empty : input.BankBranchCode,
                Serial = input.BankCode,
                Ser = _ServiceLinkRegisterManualser,
                Cod1 = 0,
                Cod2 = 0,
                Cod3 = input.Amount,
                Pard = (input.Amount / 1000) * 1000,
                Jam = 0,
                Elat = 0,
                //Barge = (int)barge,
                Barge = 0,
                Enshab = memberInfo.MeterDiameterId,
                CodEnshab = memberInfo.UsageId,
                Operator = _ServiceLinkRegisterManualoperator,
                TypePay = "0",
                ShPard = string.Empty,
                ShGhabs = memberInfo.BillId,
                Type = _ServiceLinkRegisterManualtype,
                NoeBed = _ServiceLinkRegisterManualnoeBed,
                Mohlat = string.Empty,
                TedadMas = memberInfo.DomesticUnit,
                TedadTej = memberInfo.CommercialUnit,
                TedadVahd = memberInfo.OtherUnit,
                CheckNo = string.Empty,
                CodReport = string.Empty,
                ChkKarbari = 0,
                PassCheck = 0,
                C120 = 0,
                C220 = 0,
                TmpDateBes = string.Empty,
                TmpDateSabt = string.Empty,
                TmpPayDate = string.Empty,
                TmpDateBank = string.Empty,
            };
        }

        private PaymentEnInsertDto ServiceLinkRegisterManualGetPaymentEnInsertDto(VosoEnInsertDto input, MemberInfoGetDto memberInfo)
        {
            return new PaymentEnInsertDto()
            {
                ZoneId = memberInfo.ZoneId,
                ZoneTitle = memberInfo.ZoneTitle,
                CustomerNumber = memberInfo.CustomerNumber,
                BillId = memberInfo.BillId,
                Amount = input.Pard,
                RegisterDay = DateTime.Now.ToShortPersianDateString(),
                RegisterDayGregorian = DateTime.Now,
                BankName = input.Serial.ToString(),
                BankBranchCode = string.IsNullOrWhiteSpace(input.CodBank) ? 0 : int.Parse(input.CodBank),
                PaymentGateway = input.TypePay,
                BillTableId = 0,
                VillageId = string.Empty,
                VillageName = string.Empty,
                IsVillage = memberInfo.ZoneId > 140000 ? 1 : 0,
                PayId = input.ShPard,
                BankCode = input.CodBank,
                PayDateJalali = input.PayDate,
                TempId = 0,
                VosoolTableId = 0,
            };
        }

        private async Task ServiceLinkRegisterManualExecSql(VosoEnInsertDto vosolEnInsertDto, PaymentEnInsertDto paymentEnInsertDto, IAppUser appUser, string opLogText)
        {
            //string dbName = "Atlas";
            string dbName = GetDbName(vosolEnInsertDto.Town);
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    VosolEnCommandService vosolEnCommandService = new(connection, transaction);
                    PaymentEnCommandService paymentEnCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                    int recordId = await vosolEnCommandService.Insert(vosolEnInsertDto, dbName);
                    paymentEnInsertDto.VosoolTableId = recordId;
                    await paymentEnCommandService.Insert(paymentEnInsertDto);
                    await opLogCommandService.Insert(opLogText, appUser);
                    transaction.Commit();
                }
            }
        }

        public async Task<ReportOutput<ServiceLinkPaidHeaderOutputDto, ServiceLinkPaidDataOutputDto>> ServiceLinkPaidGetAsync(ServiceLinkPaidInputDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            await _commonZoneService.IsUserInZone(appUser, input.ZoneId);
            IEnumerable<ServiceLinkPaidDataOutputDto> data = await _vosolEnQueryService.Get(input);
            await ServiceLinkPaidGetValidateDates(data);
            ServiceLinkPaidHeaderOutputDto header = new()
            {
                ZoneId = input.ZoneId,
                ZoneTitle = data?.FirstOrDefault()?.ZoneTitle ?? string.Empty,
                Amount = data?.Sum(a => a.Amount) ?? 0,
                RecordCount = data?.Count() ?? 0,
                Title = _ServiceLinkPaidGettitle
            };
            return new ReportOutput<ServiceLinkPaidHeaderOutputDto, ServiceLinkPaidDataOutputDto>(_ServiceLinkPaidGettitle, header, data);
        }

        private async Task ServiceLinkPaidGetValidateDates(IEnumerable<ServiceLinkPaidDataOutputDto> input)
        {
            foreach (var item in input)
            {
                string checkDateJalali = await _variabService.GetDateCheck(item.ZoneId);
                DateOnly? dateOnlyBank = item.BankDateJalali.ToGregorianDateOnly();
                DateOnly? dateOnlyRegister = item.RegisterDateJalali.ToGregorianDateOnly();
                if (!dateOnlyBank.HasValue || !dateOnlyRegister.HasValue)
                {
                    throw new InvalidDateException(ExceptionLiterals.InvalidDate);
                }

                if (_ServiceLinkPaidGettodayJalali.CompareTo(checkDateJalali) < 0)
                {
                    throw new InvalidBillCommandException(ExceptionLiterals.InvalidPaymentInsertAfterDateCheck);
                }

                if (item.PayDateJalali.CompareTo(_ServiceLinkPaidGettodayJalali) > 0)
                {
                    throw new InvalidBillCommandException(ExceptionLiterals.InvalidMoreThanCurrentDate);
                }

                if (item.PayDateJalali.CompareTo(checkDateJalali) <= 0)
                {
                    throw new InvalidBillCommandException(ExceptionLiterals.InvalidPaymentInsertAfterDateCheck);
                }

                if (item.BankDateJalali.CompareTo(_ServiceLinkPaidGettodayJalali) > 0)
                {
                    throw new InvalidBillCommandException(ExceptionLiterals.InvalidMoreThanCurrentDate);
                }

                if (item.BankDateJalali.CompareTo(checkDateJalali) <= 0)
                {
                    throw new InvalidBillCommandException(ExceptionLiterals.InvalidPaymentInsertAfterDateCheck);
                }

                if (item.BankDateJalali.CompareTo(_ServiceLinkPaidGet30DayAgoDateJalali) < 0)
                {
                    throw new InvalidBillCommandException(ExceptionLiterals.InvalidBankDateBefor60DaysAgo);
                }
            }
        }
    }
}
