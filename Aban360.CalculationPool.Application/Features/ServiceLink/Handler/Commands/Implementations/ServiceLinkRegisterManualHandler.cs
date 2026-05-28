using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.CalculationPool.Persistence.Features.ServiceLink.Commands.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Constant;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Implementations
{
    internal sealed class ServiceLinkRegisterManualHandler : AbstractBaseConnection, IServiceLinkRegisterManualHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IValidator<ServiceLinkRegisterManualInputDto> _validator;
        private readonly ICommonMemberQueryService _commonMemberQuery;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IVariabService _variabService;
        short _ser = 1;
        short _operator = 666;
        short _type = 2;
        short _noeBed = 2;

        public ServiceLinkRegisterManualHandler(
            IHttpContextAccessor contextAccessor,
            IValidator<ServiceLinkRegisterManualInputDto> validator,
            ICommonMemberQueryService commonMemberQuery,
            ICommonZoneService commonZoneService,
            IVariabService variabService,
            IConfiguration configuration)
            : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _validator = validator;
            _validator.NotNull(nameof(validator));

            _commonMemberQuery = commonMemberQuery;
            _commonMemberQuery.NotNull(nameof(commonMemberQuery));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _variabService = variabService;
            _variabService.NotNull(nameof(variabService));
        }

        public async Task Handle(ServiceLinkRegisterManualInputDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validate(input, cancellationToken);
            MemberInfoGetDto memberInfo = await _commonMemberQuery.Get(new ZoneIdAndCustomerNumber(input.ZoneId, input.CustomerNumber));
            await _commonZoneService.IsUserInZone(appUser, memberInfo.ZoneId);
            VosoEnInsertDto vosolEnInsertDto = GetVosolEnInsertDto(input, memberInfo);
            PaymentEnInsertDto paymentEnInsertDto = GetPaymentEnInsertDto(vosolEnInsertDto, memberInfo);

            string opLogText = string.Format(Literals.ServiceLinkRegisterManualOpLog, memberInfo.BillId, input.Amount);

            await SqlCommands(vosolEnInsertDto, paymentEnInsertDto, appUser, opLogText);
        }
        private async Task Validate(ServiceLinkRegisterManualInputDto input, CancellationToken cancellationToken)
        {
            await InputValidate(input, cancellationToken);
            await DateValidate(input);
        }
        private async Task InputValidate(ServiceLinkRegisterManualInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private async Task DateValidate(ServiceLinkRegisterManualInputDto input)
        {
            string checkDateJalali = await _variabService.GetDateCheck(input.ZoneId);
            if (DateTime.Now.ToShortPersianDateString().CompareTo(checkDateJalali) < 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidPaymentInsertAfterDateCheck);
            }
            if (input.PayDateJalali.CompareTo(DateTime.Now.ToShortPersianDateString()) > 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidMoreThanCurrentDate);
            }
        }
        private VosoEnInsertDto GetVosolEnInsertDto(ServiceLinkRegisterManualInputDto input, MemberInfoGetDto memberInfo)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            //decimal barge = await _variabService.GetAndRenew(memberInfo.ZoneId);

            return new VosoEnInsertDto()
            {
                Town = memberInfo.ZoneId,
                Radif = memberInfo.CustomerNumber,
                ParNo = "0",
                PayDate = input.PayDateJalali,
                DateBank = input.BankDateJalali,
                DateBes = currentDateJalali,
                DateSabt = currentDateJalali,
                CodBank = string.IsNullOrWhiteSpace(input.BankBranchCode) ? string.Empty : input.BankBranchCode,
                Serial = input.BankCode,
                Ser = _ser,
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
                Operator = _operator,
                TypePay = "0",
                ShPard = string.Empty,
                ShGhabs = memberInfo.BillId,
                Type = _type,
                NoeBed = _noeBed,
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
        private PaymentEnInsertDto GetPaymentEnInsertDto(VosoEnInsertDto input, MemberInfoGetDto memberInfo)
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
                BankBranchCode = int.Parse(input.CodBank),
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
        private async Task SqlCommands(VosoEnInsertDto vosolEnInsertDto, PaymentEnInsertDto paymentEnInsertDto, IAppUser appUser, string opLogText)
        {
            string dbName = "Atlas";
            //string dbName = GetDbName(vosolEnsInsertDto?.FirstOrDefault()?.Town ?? 0);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    VosolEnCommandService vosolEnCommandService = new(connection, transaction);
                    PaymentEnCommandService paymentEnCommandService = new(connection, transaction);
                    OpLogCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    int recordId = await vosolEnCommandService.Insert(vosolEnInsertDto, dbName);
                    paymentEnInsertDto.VosoolTableId = recordId;
                    await paymentEnCommandService.Insert(paymentEnInsertDto);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
    }
}
