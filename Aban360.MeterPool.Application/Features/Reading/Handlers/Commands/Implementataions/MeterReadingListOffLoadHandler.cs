using Aban360.ClaimPool.Domain.Constants;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.MeterPool.Application.Features.Reading.Handlers.Commands.Contracts;
using Aban360.MeterPool.Domain.Constants;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Aban360.MeterPool.Persistence.Features.Reading.Commands;
using Aban360.MeterPool.Persistence.Features.Reading.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.MeterPool.Application.Features.Reading.Handlers.Commands.Implementataions
{
    internal sealed class MeterReadingListOffLoadHandler : AbstractBaseConnection, IMeterReadingListOffLoadHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IDetailQueryService _detailQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IUserQueryService _userQueryService;
        private readonly ICounterStateQueryService _counterStateQueryService;
        private static int[] _validStepFlowIds = { (int)ReadingFlowStepEnum.DownloadByReader, (int)ReadingFlowStepEnum.StartReading };
        private static DateTime _currentDateTime = DateTime.Now;
        private string _currentDateJalali = _currentDateTime.ToShortPersianDateString();
        public MeterReadingListOffLoadHandler(
            ITrackingQueryService trackingQueryService,
            IDetailQueryService detailQueryService,
            ICommonZoneService commonZoneService,
            IUserQueryService userQueryService,
            ICounterStateQueryService counterStateQueryService,
            IConfiguration configuration)
                : base(configuration)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _detailQueryService = detailQueryService;
            _detailQueryService.NotNull(nameof(detailQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _userQueryService = userQueryService;
            _userQueryService.NotNull(nameof(userQueryService));

            _counterStateQueryService = counterStateQueryService;
            _counterStateQueryService.NotNull(nameof(counterStateQueryService));
        }

        public async Task Handle(IEnumerable<MeterReadingListOffLoadInputDto> inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            //appUser Validate

            IEnumerable<TrackingGetDto> trackingsInfo = await TrackingValidate(inputDto, appUser);
            await DetailValidate(inputDto);

            var (trackingDuplicateDto, trackingUpdateSeenDto) = GetTrackingDuplicateList(trackingsInfo, appUser);
            IEnumerable<DetailUpdateDto> detailsUpdateList = GetDetailUpdateList(inputDto, appUser);
            await ExecSql(trackingDuplicateDto, trackingUpdateSeenDto, detailsUpdateList);
        }
        private async Task ExecSql(TrackingDuplicateInsertDto trackingDuplicateDto, TrackingSeenUpdateDto trackingUpdateSeenDto, IEnumerable<DetailUpdateDto> detailUpdateList)
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
                    DetailCommandService detailCommandService = new(connection, transaction);

                    await trackingCommandService.UpdateSeen(trackingUpdateSeenDto);
                    await trackingCommandService.InsertDuplicate(trackingDuplicateDto);
                    await detailCommandService.Update(detailUpdateList);

                    transaction.Commit();
                }
            }
        }
        private async Task<IEnumerable<TrackingGetDto>> TrackingValidate(IEnumerable<MeterReadingListOffLoadInputDto> inputDto, IAppUser appUser)
        {
            IEnumerable<TrackingGetDto> trackingsInfo = await _trackingQueryService.Get(inputDto.Select(details => details.TrackNumber).Distinct().ToList());
            foreach (var tracking in trackingsInfo)
            {
                if (!_validStepFlowIds.Contains(tracking.FlowStepId))
                {
                    throw new ReadingException(ExceptionLiterals.InvalidDetailByCurrentTrackingFlowId(tracking.TrackNumber));
                }
                if (tracking.CounterReaderId != appUser.UserId)
                {
                    throw new ReadingException(string.Format(ExceptionLiterals.InvalidTrackingByCurrentUser, tracking.TrackNumber));
                }
            }

            return trackingsInfo;
        }
        private async Task DetailValidate(IEnumerable<MeterReadingListOffLoadInputDto> inputDto)
        {
            IEnumerable<DetailGetDto> previousDetailsInfo = await _detailQueryService.Get(inputDto.Select(details => details.Id).ToList());
            IEnumerable<CounterStateCodeDto> validCounterStates = await _counterStateQueryService.GetValids();
            foreach (var inputDetail in inputDto)
            {
                DetailGetDto? detailInfo = previousDetailsInfo?.Where(preDetail => preDetail.Id == inputDetail.Id)?.FirstOrDefault();
                if (detailInfo is not null && detailInfo.ExcludeBy is not null)
                {
                    throw new ReadingException(string.Format(ExceptionLiterals.InvalidExcludedMeterReadingOffLoad, inputDetail.TrackNumber, inputDetail.BillId));
                }
                if (detailInfo is null)
                {
                    throw new ReadingException(string.Format(ExceptionLiterals.NotFoundOffLoadInput, inputDetail.TrackNumber, inputDetail.BillId));
                }
                if (inputDetail.CurrentCounterStateCode is null)
                {
                    throw new ReadingException(string.Format(ExceptionLiterals.InvalidNullCounterStateCode, inputDetail.TrackNumber, inputDetail.BillId));
                }
                if ((inputDetail.CurrentCounterStateCode == (int)CounterStateCodeEnum.Common) && detailInfo.PreNumber > inputDetail.CounterNumber)
                {
                    throw new ReadingException(string.Format(ExceptionLiterals.InvalidCommonCounterStateWithPreviousGTCurrent, inputDetail.TrackNumber, inputDetail.BillId));
                }
                if (!validCounterStates.Where(counter => counter.Id == inputDetail.CurrentCounterStateCode).Any())
                {
                    throw new ReadingException(string.Format(ExceptionLiterals.NotFoundCounterStateCode, inputDetail.TrackNumber, inputDetail.BillId));
                }
            }
        }
        private (TrackingDuplicateInsertDto, TrackingSeenUpdateDto) GetTrackingDuplicateList(IEnumerable<TrackingGetDto> trackingsInfo, IAppUser appUser)
        {
            IEnumerable<TrackingGetDto> trackingToUpdate = trackingsInfo.Where(tracking => tracking.FlowStepId == (int)ReadingFlowStepEnum.DownloadByReader).Distinct().ToList();
            IEnumerable<Guid> trackingIds = trackingToUpdate.Select(tracking => tracking.Id).ToList();
            IEnumerable<Guid> blockIds=trackingToUpdate.Select(tracking => tracking.BlockId).Distinct().ToList();

            TrackingSeenUpdateDto trackingUpdateSeenDto = new(blockIds, true);
            TrackingDuplicateInsertDto trackingDuplicateDto = new(trackingIds, (int)ReadingFlowStepEnum.StartReading, appUser.UserId);

            return (trackingDuplicateDto, trackingUpdateSeenDto);
        }
        private IEnumerable<DetailUpdateDto> GetDetailUpdateList(IEnumerable<MeterReadingListOffLoadInputDto> inputDto, IAppUser appUser)
        {
            return inputDto.Select(input => new DetailUpdateDto()
            {
                Id = input.Id,
                CounterNumber = input.CounterNumber,
                LocationDateTime = input.LocationDateTime,
                PhoneDateTime = input.PhoneDateTime,
                CurrentCounterStateCode = input.CurrentCounterStateCode.Value,
                Y = input.Y,
                X = input.X,
                GisAccuracy = input.GisAccuracy,
                CounterNumberShown = input.CounterNumberShown,
                Description = input.Description,
                AttemptCount = input.AttemptCount,
                IsLocked = input.IsLocked,
                HighLowStateId = input.HighLowStateId,
                OffLoadDateTime = _currentDateTime,
                OffloadDateJalali = _currentDateJalali,
                OffLoadTime =$"{_currentDateTime.TimeOfDay.Hours.ToString()}:{_currentDateTime.TimeOfDay.Minutes.ToString()}" ,
                OffLoadBy = appUser.UserId,
            }).ToList();
        }
    }
}
