using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.CalculationPool.Persistence.Features.ServiceLink.Commands.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
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
            await InputValidate(input, cancellationToken);
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQuery.Get(input.BillId);
            await _commonZoneService.IsUserInZone(appUser, zoneIdAndCustomerNumber.ZoneId);
            MemberInfoGetDto memberInfo = await _commonMemberQuery.Get(zoneIdAndCustomerNumber);
            IEnumerable<VosoEnInsertDto> vosolEnsInsertDto = await GetVosolEnsInsertDto(input, memberInfo);
            string opLogText = string.Format(Literals.ServiceLinkRegisterManualOpLog, input.BillId, input.PayItems?.Count() ?? 0, input.PayItems?.Sum(s => s.Cod1 + s.Cod2 + s.Cod3) ?? 0);

            await SqlCommands(vosolEnsInsertDto, appUser, opLogText);
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
        private async Task<IEnumerable<VosoEnInsertDto>> GetVosolEnsInsertDto(ServiceLinkRegisterManualInputDto input, MemberInfoGetDto s)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            decimal barge = await _variabService.GetAndRenew(s.ZoneId);

            return input.PayItems.Select(i => new VosoEnInsertDto()
            {
                Town = s.ZoneId,
                Radif = s.CustomerNumber,
                ParNo = "0",
                PayDate = i.PayDateJalali,
                DateBes = currentDateJalali,
                DateBank = i.BankDateJalali,
                DateSabt = currentDateJalali,
                CodBank = string.IsNullOrWhiteSpace(i.BankBranchCode) ? string.Empty : i.BankBranchCode,
                Serial = i.BankCode,
                Ser = _ser,
                Cod1 = i.Cod1,
                Cod2 = i.Cod2,
                Cod3 = i.Cod3,
                Pard = ((i.Cod1 + i.Cod2 + i.Cod3) / 1000) * 1000,
                Jam = 0,
                Elat = 0,
                Barge = (int)barge,
                Enshab = s.MeterDiameterId,
                CodEnshab = s.UsageId,
                Operator = _operator,
                TypePay = "0",
                ShPard = string.Empty,
                ShGhabs = s.BillId,
                Type = _type,
                NoeBed = _noeBed,
                Mohlat = string.Empty,
                TedadMas = s.DomesticUnit,
                TedadTej = s.CommercialUnit,
                TedadVahd = s.OtherUnit,
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

            }).ToList();
        }
        private async Task SqlCommands(IEnumerable<VosoEnInsertDto> vosolEnsInsertDto, IAppUser appUser, string opLogText)
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
                    OpLogCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await vosolEnCommandService.Insert(vosolEnsInsertDto, dbName);
                    //todo: insert In PaymentEn
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
    }
}
