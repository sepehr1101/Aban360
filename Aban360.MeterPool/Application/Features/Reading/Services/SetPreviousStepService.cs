using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.MeterPool.Domain.Constants;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Aban360.MeterPool.Persistence.Features.Reading.Commands;
using Aban360.MeterPool.Persistence.Features.Reading.Queries.Contracts;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.MeterPool.Application.Features.Reading.Services
{
    public interface ISetPreviousStepService
    {
        Task SetReGenerateListAsync(int trackNumber, IAppUser appUser, CancellationToken cancellationToken);
        Task SetReStartReadingAsync(int trackNumber, IAppUser appUser, CancellationToken cancellationToken);
        Task SetReFinishByReaderAsync(int trackNumber, IAppUser appUser, CancellationToken cancellationToken);
    }
    internal sealed class SetPreviousStepService : AbstractBaseConnection, ISetPreviousStepService
    {
        private readonly ICommonZoneService _zoneService;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IDetailQueryService _detailQueryService;
        public SetPreviousStepService(
            ICommonZoneService zoneService,
            ITrackingQueryService trackingQueryService,
            IDetailQueryService detailQueryService,
            IConfiguration configuration)
             : base(configuration)
        {
            _zoneService = zoneService;
            _zoneService.NotNull(nameof(zoneService));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _detailQueryService = detailQueryService;
            _detailQueryService.NotNull(nameof(detailQueryService));
        }

        public async Task SetReGenerateListAsync(int trackNumber, IAppUser appUser, CancellationToken cancellationToken)
        {
            TrackingGetDto trackingInfo = await _trackingQueryService.GetLatest(trackNumber);
            await _zoneService.IsUserInZone(appUser.UserId, trackingInfo.ZoneId);
            TrackingSeenUpdateDto trackingUpdateSeenDto = new(trackingInfo.Id, true);
            TrackingDuplicateInsertDto trackingDuplicateInsertDto = new(new List<Guid> { trackingInfo.Id }.ToList(), (int)ReadingFlowStepEnum.CreatePath, appUser.UserId);

            await SetNewStepExecSql(trackingUpdateSeenDto, trackingDuplicateInsertDto);
        }
        public async Task SetReStartReadingAsync(int trackNumber, IAppUser appUser, CancellationToken cancellationToken)
        {
            TrackingGetDto trackingInfo = await _trackingQueryService.GetLatest(trackNumber);
            if (trackingInfo.FlowStepId != (int)ReadingFlowStepEnum.FinishByReader)
            {
                throw new ReadingException(ExceptionLiterals.InvalidPreviousStepId);
            }
            await _zoneService.IsUserInZone(appUser.UserId, trackingInfo.ZoneId);
            TrackingSeenUpdateDto trackingUpdateSeenDto = new(trackingInfo.Id, true);
            TrackingDuplicateInsertDto trackingDuplicateInsertDto = new(new List<Guid> { trackingInfo.Id }.ToList(), (int)ReadingFlowStepEnum.StartReading, appUser.UserId);

            await SetNewStepExecSql(trackingUpdateSeenDto, trackingDuplicateInsertDto);
        }
        public async Task SetReFinishByReaderAsync(int trackNumber, IAppUser appUser, CancellationToken cancellationToken)
        {
            TrackingGetDto trackingInfo = await _trackingQueryService.GetLatest(trackNumber);
            if (trackingInfo.FlowStepId != (int)ReadingFlowStepEnum.StartReading)
            {
                throw new ReadingException(ExceptionLiterals.InvalidPreviousStepId);
            }
            await _zoneService.IsUserInZone(appUser.UserId, trackingInfo.ZoneId);
            TrackingSeenUpdateDto trackingUpdateSeenDto = new(trackingInfo.Id, true);
            TrackingDuplicateInsertDto trackingDuplicateInsertDto = new(new List<Guid> { trackingInfo.Id }.ToList(), (int)ReadingFlowStepEnum.FinishByReader, appUser.UserId);

            await SetNewStepExecSql(trackingUpdateSeenDto, trackingDuplicateInsertDto);
        }
        private async Task SetNewStepExecSql(TrackingSeenUpdateDto trackingUpdateSeenDto, TrackingDuplicateInsertDto trackingDuplicateInsertDto)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    TrackingCommandService trackingCommandService = new(connection, transaction);
                    await trackingCommandService.UpdateSeen(trackingUpdateSeenDto);
                    await trackingCommandService.InsertDuplicate(trackingDuplicateInsertDto);
                    transaction.Commit();
                }
            }
        }
    }
}
