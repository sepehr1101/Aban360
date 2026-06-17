using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Implementations
{
    internal sealed class ServiceLinkReturnRemoveHandler : AbstractBaseConnection, IServiceLinkReturnRemoveHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IRequestBillDetailQueryService _requestBillDetailQueryService;
        private readonly IVariabService _variabService;
        private readonly IModifyTypeQueryService _modifyTypeQueryService;

        public ServiceLinkReturnRemoveHandler(
            IHttpContextAccessor contextAccessor,
            ICommonMemberQueryService commonMemberQueryService,
            IRequestBillDetailQueryService requestBillDetailQueryService,
            IVariabService variabService,
            IModifyTypeQueryService modifyTypeQueryService,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _requestBillDetailQueryService = requestBillDetailQueryService;
            _requestBillDetailQueryService.NotNull(nameof(requestBillDetailQueryService));

            _variabService = variabService;
            _variabService.NotNull(nameof(variabService));

            _modifyTypeQueryService = modifyTypeQueryService;
            _modifyTypeQueryService.NotNull(nameof(modifyTypeQueryService));
        }

        public async Task Handle(ServiceLinkReturnRemoveInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            RequestBillDetailGetDto requestBillDetailInfo = await _requestBillDetailQueryService.Get(inputDto.Id, inputDto.BillId);
            await DateValidate(requestBillDetailInfo);

            string opLogText = string.Format(OpLogLiterals.ServiceLinkReturnRemoved, requestBillDetailInfo.BillId, requestBillDetailInfo.RegisterDateJalali, requestBillDetailInfo.Amount, requestBillDetailInfo.TypeTitle, requestBillDetailInfo.ItemTitle);
            RequestBillDetailsRemoveDto requestBillDetailRemoveDto = new(inputDto.Id, inputDto.BillId);
            ModifyTypeGetDto modifyType = await _modifyTypeQueryService.GetByRequestBillDetails(requestBillDetailInfo.TypeCode);
            KartRemoveByConditionDto kartRemoveDto = new(requestBillDetailInfo.ZoneId, requestBillDetailInfo.CustomerNumber, requestBillDetailInfo.Amount, requestBillDetailInfo.RegisterDateJalali, modifyType.Karten75Id, requestBillDetailInfo.ItemId);

            await SqlCommands(requestBillDetailRemoveDto, kartRemoveDto, appUser, opLogText);
        }
        private async Task SqlCommands(RequestBillDetailsRemoveDto requestBillDetailRemoveDto, KartRemoveByConditionDto kartRemoveDto, IAppUser appUser, string opLogText)
        {
            string dbName = GetDbName(kartRemoveDto.ZoneId);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    KartCommandService kartCommandService = new(connection, transaction);
                    RequestBillDetailsCommandService requestBillDetailCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await kartCommandService.Remove(kartRemoveDto, dbName);
                    await requestBillDetailCommandService.Remove(requestBillDetailRemoveDto);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task DateValidate(RequestBillDetailGetDto requestBillDetailInfo)
        {
            string dateCheck = await _variabService.GetDateCheck(requestBillDetailInfo.ZoneId);
            if (requestBillDetailInfo.RegisterDateJalali.CompareTo(dateCheck) < 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidServiceLinkReturnRemoveBeforDateCheck);
            }
            if (DateTime.Now.AddDays(-7).ToShortPersianDateString().CompareTo(requestBillDetailInfo.RegisterDateJalali) > 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidServiceLinkReturnRemoveLessThan7Days);
            }
        }
    }
}
