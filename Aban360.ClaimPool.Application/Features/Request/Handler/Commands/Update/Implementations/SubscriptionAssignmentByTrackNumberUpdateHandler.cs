using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Constants;
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
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Implementations
{
    internal sealed class SubscriptionAssignmentByTrackNumberUpdateHandler : AbstractBaseConnection, ISubscriptionAssignmentByTrackNumberUpdateHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly ICommonZoneService _zoneService;
        private static int[] _invalidStatus = { (int)TrackingStatusEnum.Deleted, (int)TrackingStatusEnum.Archived, (int)TrackingStatusEnum.Registered };
        public SubscriptionAssignmentByTrackNumberUpdateHandler(
            IHttpContextAccessor contextAccessor,
            ITrackingQueryService trackingQueryService,
            IMoshtrakQueryService moshtrakQueryService,
            ICommonZoneService zoneService,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _zoneService = zoneService;
            _zoneService.NotNull(nameof(zoneService));
        }

        public async Task Handle(SubscriptionAssignmentByTrackNumberUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            MoshtrakOutputDto? mosthrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, trackingInfo.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            Validate(trackingInfo, mosthrakInfo);

            await _zoneService.IsUserInZone(appUser, trackingInfo.ZoneId);
            string opLogText = string.Format(OpLogLiterals.SubscriptionAssignmentByTrackNumberUpdateOpLog, inputDto.TrackNumber);
            await ExecSql(inputDto, appUser, trackingInfo.ZoneId, opLogText);
        }
        private async Task ExecSql(SubscriptionAssignmentByTrackNumberUpdateDto inputDto, IAppUser appUser, int zoneId, string opLogText)
        {
            string dbName = GetDbName(zoneId);
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MoshtrakCommandService moshtrakCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await moshtrakCommandService.Update(inputDto, dbName);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private void Validate(TrackingOutputDto trackingInfo, MoshtrakOutputDto? mosthrakInfo)
        {
            if (_invalidStatus.Contains(trackingInfo.StatusId))
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
            }
            if (mosthrakInfo is null)
            {
                throw new InvalidTrackingException(ExceptionLiterals.NotFoundMoshtrak);
            }
            if (mosthrakInfo.IsRegistered)
            {
                throw new InvalidTrackingException(ExceptionLiterals.TrackingRegistered);
            }
        }
    }
}
