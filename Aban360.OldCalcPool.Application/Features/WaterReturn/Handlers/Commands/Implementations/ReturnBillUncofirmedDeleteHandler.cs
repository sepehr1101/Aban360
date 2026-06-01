using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.WaterReturn.Command.Implementations;
using Aban360.OldCalcPool.Persistence.Features.WaterReturn.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;
using Microsoft.Extensions.Configuration;
using System.Data;
using Aban360.OldCalcPool.Application.Constant;
using Microsoft.AspNetCore.Http;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Implementations
{
    internal sealed class ReturnBillUncofirmedDeleteHandler : AbstractBaseConnection, IReturnBillUncofirmedDeleteHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICommonZoneService _commonZoneService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IAutoBackQueryService _autoBackQueryService;
        public ReturnBillUncofirmedDeleteHandler(
            IHttpContextAccessor contextAccessor,
            ICommonZoneService commonZoneService,
            ICommonMemberQueryService commonMemberQueryService,
            IAutoBackQueryService autoBackQueryService,
            IConfiguration configuration)
              : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _autoBackQueryService = autoBackQueryService;
            _autoBackQueryService.NotNull(nameof(autoBackQueryService));
        }

        public async Task Handle(int confirmedNumber, IAppUser appUser, CancellationToken cancellationToken)
        {
            IEnumerable<AutoBackGetByBargeDto> autoBacksInfo = await _autoBackQueryService.GetByConfirmNumber(confirmedNumber);
            if (autoBacksInfo.Where(a => a.IsConfirmed).Any())
                throw new ReturnedBillException(ExceptionLiterals.InvalidDeleteConfirmedReturn);

            await _commonZoneService.IsUserInZone(appUser, (int)(autoBacksInfo.FirstOrDefault()?.Town ?? 0));
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(new ZoneIdAndCustomerNumber((int)(autoBacksInfo.FirstOrDefault()?.Town ?? 0), (int)(autoBacksInfo.FirstOrDefault()?.Radif ?? 0)));
            string logText = string.Format(Literals.BillReturnDeletedOpLog, memberInfo.BillId, autoBacksInfo.FirstOrDefault().PriDate, autoBacksInfo.FirstOrDefault().TodayDate, confirmedNumber);

            await SqlCommand(confirmedNumber, appUser, logText);
        }
        private async Task SqlCommand(int confirmedNumber, IAppUser appUser, string logText)
        {
            string atlasDbName = ReportLiterals.Atlas;

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    AutoBackCommandService autoBackCommandService = new(connection, transaction);
                    OpLogCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await autoBackCommandService.UpdateIsDeleted(confirmedNumber, atlasDbName);
                    await opLogCommandService.Insert(logText, appUser);

                    transaction.Commit();
                }
            }
        }
    }
}
