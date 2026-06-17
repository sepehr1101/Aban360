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

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Implementations
{
    internal sealed class ServiceLinkDeleteHandler : AbstractBaseConnection, IServiceLinkDeleteHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IVosolEnQueryService _vosolEnQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IVariabService _variabService;
        public ServiceLinkDeleteHandler(
            IHttpContextAccessor contextAccessor,
            IVosolEnQueryService vosolEnQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            ICommonZoneService commonZoneService,
            IVariabService variabService,
            IConfiguration configuration)
                : base(configuration)
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
        }

        public async Task Handle(ServiceLinkPaymentRemoveInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await _commonZoneService.IsUserInZone(appUser, inputDto.ZoneId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(new ZoneIdAndCustomerNumber(inputDto.ZoneId, inputDto.CustomerNumber));
            ServiceLinkPaidDataOutputDto paidInfo = await _vosolEnQueryService.Get(inputDto);//todo:in GetMethod:ReName DbName
            await ValidateDate(inputDto.ZoneId, paidInfo.RegisterDateJalali);
            string opLogText = string.Format(OpLogLiterals.ServiceLinkDeleteManualOpLog, memberInfo.BillId, paidInfo.Amount);

            await ExecSql(inputDto, appUser, opLogText);
        }
        private async Task ValidateDate(int zoneId, string payDateJalali)
        {
            string checkDateJalali = await _variabService.GetDateCheck(zoneId);
            if (payDateJalali.CompareTo(checkDateJalali) < 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidIPaymentDeleteAfterDateCheck);
            }
        }
        private async Task ExecSql(ServiceLinkPaymentRemoveInputDto inputDto, IAppUser appUser, string opLogText)
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


    }
}
