using TrackingCommandService = SqlCommandsWorkflow10.Aban360.MeterPool.Persistence.Features.Reading.Commands.TrackingCommandService;
using DetailCommandService = SqlCommandsWorkflow10.Aban360.MeterPool.Persistence.Features.Reading.Commands.DetailCommandService;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Aban360.MeterPool.Domain.Constants;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Aban360.MeterPool.Persistence.Features.Manegement.Queries.Contracts;
using Aban360.MeterPool.Persistence.Features.Reading.Commands;
using Aban360.MeterPool.Persistence.Features.Reading.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;
using Guild = Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text.Json;
using System.Threading;

namespace SqlAfterWorkflow10.Aban360.MeterPool.Application.Features.Reading.Services;
using BlockCommandService = SqlCommandsWorkflow10.BlockCommandService;

public interface IReadingListService
{
    Task<ReaderCartableOutputDto> GetReaderCartableAsync(IAppUser appUser, CancellationToken cancellationToken);
    Task OffloadAsync(IEnumerable<MeterReadingListOffLoadInputDto> inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task FinishAsync(long trackNumber, IAppUser appUser, CancellationToken cancellationToken);
    Task<GenerateReadingListOutputDto> GenerateAsync(GenerateReadingListInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
}

internal sealed class ReadingListService : global::WorkflowRecordingConnectionBase, IReadingListService
{
    private readonly IBlockQueryService _blockQueryService;
    private readonly ITrackingQueryService _trackingQueryService;
    private readonly IDetailQueryService _detailQueryService;
    private readonly IUserQueryService _userQueryService;
    private readonly IT41QueryService _usageQueryService;
    private readonly IT7QueryService _branchTypeQueryService;
    private readonly IDeletionStateQueryService _deletionStateQueryService;
    private readonly IReadingCounterStateQueryService _counterStateQueryService;
    private readonly IMapDictionaryGetHandler _mapDictionaryGetHandler;
    private readonly Guild.IGuildQueryService _guildQueryService;
    private static int _createPathFlowStepId = (int)ReadingFlowStepEnum.CreatePath;
    private static int _downloadByReadigFlowStepId = (int)ReadingFlowStepEnum.DownloadByReader;
    private static int _readerRoleId = ReportLiterals.ReaderRoleId;
    private readonly ICommonZoneService _commonZoneService;
    private readonly ICounterStateQueryService _offloadCounterStateQueryService;
    private static int[] _validStepFlowIds =
    {
        (int)ReadingFlowStepEnum.DownloadByReader,
        (int)ReadingFlowStepEnum.StartReading
    };
    private static DateTime _currentDateTime = DateTime.Now;
    private string _currentDateJalali = _currentDateTime.ToShortPersianDateString();
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IReadingBoundQueryService _readingBoundQueryService;
    private readonly IReadingBlockQueryService _readingBlockQeryService;
    private readonly IT51QueryService _zoneQueryService;
    private readonly IReadingCustomerDetailInfoQueryService _readingCustomerDetailInfoQueryService;
    private readonly IUserRoleQueryService _userRoleQueryService;
    private readonly ITrackNumberService _trackNumberService;
    private readonly IReadingPeriodTypeQueryService _readingPeriodTypeQueryService;
    private readonly IReadingPeriodQueryService _readingPeriodQueryService;
    private readonly IValidator<GenerateReadingListInputDto> _validator;
    private static DateTime _generateCurrentDateTime = DateTime.Now;
    private static string _generateCurrentDateJalali = _generateCurrentDateTime.ToShortPersianDateString();
    private static int _createPathStepId = (int)ReadingFlowStepEnum.CreatePath;
    public ReadingListService(IBlockQueryService blockQueryService, ITrackingQueryService trackingQueryService, IDetailQueryService detailQueryService, IUserQueryService userQueryService, IT41QueryService usageQueryService, IT7QueryService branchTypeQueryService, Guild.IGuildQueryService guildQueryService, IDeletionStateQueryService deletionStateQueryService, IReadingCounterStateQueryService counterStateQueryService, IMapDictionaryGetHandler mapDictionaryGetHandler, IConfiguration configuration, ICommonZoneService commonZoneService, ICounterStateQueryService offloadCounterStateQueryService, IHttpContextAccessor contextAccessor, IReadingBoundQueryService readingBoundQueryService, IReadingBlockQueryService readingBlockQeryService, IT51QueryService zoneQueryService, IReadingCustomerDetailInfoQueryService readingCustomerDetailInfoQueryService, IUserRoleQueryService userRoleQueryService, ITrackNumberService trackNumberService, IReadingPeriodTypeQueryService readingPeriodTypeQueryService, IReadingPeriodQueryService readingPeriodQueryService, IValidator<GenerateReadingListInputDto> validator) : base(configuration)
    {
        _blockQueryService = blockQueryService;
        _blockQueryService.NotNull(nameof(blockQueryService));
        _trackingQueryService = trackingQueryService;
        _trackingQueryService.NotNull(nameof(trackingQueryService));
        _detailQueryService = detailQueryService;
        _detailQueryService.NotNull(nameof(detailQueryService));
        _userQueryService = userQueryService;
        _userQueryService.NotNull(nameof(userQueryService));
        _usageQueryService = usageQueryService;
        _usageQueryService.NotNull(nameof(usageQueryService));
        _branchTypeQueryService = branchTypeQueryService;
        _branchTypeQueryService.NotNull(nameof(branchTypeQueryService));
        _guildQueryService = guildQueryService;
        _guildQueryService.NotNull(nameof(guildQueryService));
        _deletionStateQueryService = deletionStateQueryService;
        _deletionStateQueryService.NotNull(nameof(deletionStateQueryService));
        _mapDictionaryGetHandler = mapDictionaryGetHandler;
        _mapDictionaryGetHandler.NotNull(nameof(mapDictionaryGetHandler));
        _counterStateQueryService = counterStateQueryService;
        _counterStateQueryService.NotNull(nameof(counterStateQueryService));
        _commonZoneService = commonZoneService;
        _commonZoneService.NotNull(nameof(commonZoneService));
        _offloadCounterStateQueryService = offloadCounterStateQueryService;
        _offloadCounterStateQueryService.NotNull(nameof(offloadCounterStateQueryService));
        _contextAccessor = contextAccessor;
        _contextAccessor.NotNull(nameof(contextAccessor));
        _readingBoundQueryService = readingBoundQueryService;
        _readingBoundQueryService.NotNull(nameof(readingBoundQueryService));
        _readingBlockQeryService = readingBlockQeryService;
        _readingBlockQeryService.NotNull(nameof(readingBlockQeryService));
        _zoneQueryService = zoneQueryService;
        _zoneQueryService.NotNull(nameof(zoneQueryService));
        _readingCustomerDetailInfoQueryService = readingCustomerDetailInfoQueryService;
        _readingCustomerDetailInfoQueryService.NotNull(nameof(readingCustomerDetailInfoQueryService));
        _userRoleQueryService = userRoleQueryService;
        _userRoleQueryService.NotNull(nameof(userRoleQueryService));
        _trackNumberService = trackNumberService;
        _trackNumberService.NotNull(nameof(trackNumberService));
        _readingPeriodQueryService = readingPeriodQueryService;
        _readingPeriodQueryService.NotNull(nameof(readingPeriodQueryService));
        _readingPeriodTypeQueryService = readingPeriodTypeQueryService;
        _readingPeriodTypeQueryService.NotNull(nameof(readingPeriodTypeQueryService));
        _validator = validator;
        _validator.NotNull(nameof(validator));
    }

    public async Task<ReaderCartableOutputDto> GetReaderCartableAsync(IAppUser appUser, CancellationToken cancellationToken)
    {
        User readerInfo = await GetReaderCartableValidate(appUser, cancellationToken);
        IEnumerable<TrackingGetDto> trackingInfo = await _trackingQueryService.Get(appUser.UserId, _createPathFlowStepId);
        IEnumerable<Guid> trackingIds = trackingInfo.Select(t => t.Id).ToList();
        IEnumerable<DetailGetDto> detailInfo = await _detailQueryService.GetByTrackIds(trackingIds);
        TrackingSeenUpdateDto trackingSeenUpdateDto = new(trackingInfo.Select(t => t.BlockId).Distinct().ToList(), true);
        TrackingDuplicateInsertDto trackingDuplicateInsertDto = new(trackingIds, _downloadByReadigFlowStepId, appUser.UserId);
        await GetReaderCartableExecSql(trackingDuplicateInsertDto, trackingSeenUpdateDto);
        return await GetReaderCartableGetResult(trackingInfo, detailInfo, cancellationToken);
    }

    private async Task GetReaderCartableExecSql(TrackingDuplicateInsertDto trackingDuplicateInsertDto, TrackingSeenUpdateDto trackingSeenUpdateDto)
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
                //await trackingCommandService.UpdateSeen(trackingSeenUpdateDto);
                //await trackingCommandService.InsertDuplicate(trackingDuplicateInsertDto);
                transaction.Commit();
            }
        }
    }

    private async Task<ReaderCartableOutputDto> GetReaderCartableGetResult(IEnumerable<TrackingGetDto> trackingInfo, IEnumerable<DetailGetDto> detailInfo, CancellationToken cancellationToken)
    {
        IEnumerable<TrackingDataOutputDto> trackingData = trackingInfo.Select(t => new TrackingDataOutputDto() { Id = t.Id, BlockId = t.BlockId, TrackNumber = t.TrackNumber, TbReadingProgramId = t.TbReadingProgramId, ZoneId = t.ZoneId, ZoneTitle = t.ZoneTitle, FlowStepId = t.FlowStepId, FromReadingNumber = t.FromReadingNumber, ToReadingNumber = t.ToReadingNumber, ListNumber = t.ListNumber, InsertBy = t.InsertBy, InsertDateJalali = t.InsertDateJalali, InsertDateTime = t.InsertDateTime, Seen = t.Seen, CounterReaderCode = t.CounterReaderCode, CounterReaderId = t.CounterReaderId, CounterReaderName = t.CounterReaderName, CounterReaderMobile = t.CounterReaderMobile, IsBazdid = t.IsBazdid, IsRoosta = t.IsRoosta, FromDate = t.FromDate, ToDate = t.ToDate, OverallCustomerCount = t.OverallCustomerCount, ItemCustomerCount = t.ItemCustomerCount, ProvisionallyPercent = t.ProvisionallyPercent, ImagePercent = t.ImagePercent, HasPreNumber = t.HasPreNumber, DisplayBillId = t.DisplayBillId, DisplayCustomerNumber = t.DisplayCustomerNumber, Description = t.Description, X = t.X, Y = t.Y, DisplayPreDate = t.DisplayPreDate, DisplayMobile = t.DisplayMobile, HasImage = t.HasImage, DisplayDebt = t.DisplayDebt, DisplayIcons = t.DisplayIcons, }).ToList();
        IEnumerable<DetailDataOutputDto> detailData = detailInfo.Select(d => new DetailDataOutputDto() { Id = d.Id, TrackingId = d.TrackingId, TrackNumber = d.TrackNumber, BillId = d.BillId, CustomerNumber = d.CustomerNumber, ReadingNumber = d.ReadingNumber, FirstName = d.FirstName, SureName = d.SureName, Address = d.Address, Plaque = d.Plaque, UsageId = d.UsageId, UsageTitle = d.UsageTitle, DomesticUnit = d.DomesticUnit, CommercialUnit = d.CommercialUnit, OtherUnit = d.OtherUnit, MeterDiameterId = d.MeterDiameterId, MeterDiameterTitle = d.MeterDiameterTitle, SiphonDiameterId = d.SiphonDiameterId, SiphonDiameterTitle = d.SiphonDiameterTitle ?? string.Empty, PostalCode = d.PostalCode, PreNumber = d.PreNumber, PreDateJalali = d.PreDateJalali, PreDateTime = d.PreDateTime, PreConsumptionAverage = d.PreConsumptionAverage, PreCounterStateCode = d.PreCounterStateCode, BodySerial = d.BodySerial, MeterInstallationDateJalali = d.MeterInstallationDateJalali, ChangeDateJalali = d.ChangeDateJalali, ChangeNumber = d.ChangeNumber, ContractualCapacity = d.ContractualCapacity, MobileNumber = d.MobileNumber, DeletionStateId = d.DeletionStateId, BranchTypeId = d.BranchTypeId, CounterNumber = d.CounterNumber, CurrentCounterStateCode = d.CurrentCounterStateCode, PossibleAddress = d.PossibleAddress, PossibleBodySerial = d.PossibleBodySerial, PossibleReadingNumber = d.PossibleReadingNumber, PossibleMobileNumber = d.PossibleMobileNumber, PossiblePhoneNumber = d.PossiblePhoneNumber, PossibleDomesticUnit = d.PossibleDomesticUnit, PossibleCommercialUnit = d.PossibleCommercialUnit, PossibleOtherUnit = d.PossibleOtherUnit, PossibleUsageId = d.PossibleUsageId, PossibleEmptyUnit = d.PossibleEmptyUnit, OffLoadDateTime = d.OffLoadDateTime, OffloadDateJalali = d.OffloadDateJalali, OffLoadTime = d.OffLoadTime, OffLoadBy = d.OffLoadBy, Y = d.Y, X = d.X, GisAccuracy = d.GisAccuracy, ImageCount = d.ImageCount, CurrentConsumption = d.CurrentConsumption, CurrentConsumptionAverage = d.CurrentConsumptionAverage, DateDifference = d.DateDifference, CounterNumberShown = d.CounterNumberShown, ExcludedForBazdid = d.ExcludedForBazdid, ExcludedForEslah = d.ExcludedForEslah, JsonItems = d.JsonItems, ConsumptionStateId = d.ConsumptionStateId, Description = d.Description, IsCalculated = d.IsCalculated, CalculationErrorDescription = d.CalculationErrorDescription, D1 = d.D1, D2 = d.D2, CounterStatePosition = d.CounterStatePosition, AttemptCount = d.AttemptCount, IsLocked = d.IsLocked, LocationDateTime = d.LocationDateTime, PhoneDateTime = d.PhoneDateTime, FatherName = d.FatherName, OldReadingNumber = d.OldReadingNumber, OldCustomerNumber = d.OldCustomerNumber, MobilesNumber = d.MobilesNumber, WaterDebt = d.WaterDebt, ModifierUserId = d.ModifierUserId, GuildId = d.GuildId, SiphonInstallationDateJalali = d.SiphonInstallationDateJalali, PreGuildId = d.PreGuildId, HighLowStateId = d.HighLowStateId, ExcludeBy = d.ExcludeBy, ExcludeDateTime = d.ExcludeDateTime, ExcludeCauseId = d.ExcludeCauseId, ExcludeCauseTitle = d.ExcludeCauseTitle, }).ToList();
        IEnumerable<NumericDictionary> usageList = await _usageQueryService.Get();
        IEnumerable<NumericDictionary> branchTypeList = await _branchTypeQueryService.Get();
        IEnumerable<NumericDictionary> deletionStateList = await _deletionStateQueryService.Get();
        IEnumerable<ReadingCounterStateGetDto> counterStateList = await _counterStateQueryService.Get();
        IEnumerable<NumericDictionary> guildDictionary = await _guildQueryService.Get();
        IEnumerable<StringDictionary> mapDictionary = _mapDictionaryGetHandler.Handle(cancellationToken);
        ReaderCartableOutputDto result = new()
        {
            TrackingInfo = trackingData,
            DetailInfo = detailData,
            Usages = usageList,
            BranchTypes = branchTypeList,
            DeletionStates = deletionStateList,
            CounterStates = counterStateList,
            Guilds = guildDictionary,
            MapDictionary = mapDictionary,
            Traverse = GetReaderCartableGetTraverse()
        };
        return result;
    }

    private async Task<User> GetReaderCartableValidate(IAppUser appUser, CancellationToken cancellationToken)
    {
        User userInfo = await _userQueryService.GetByRoleInfo(appUser.UserId);
        if (!userInfo.UserRoles.Where(role => role.RoleId == _readerRoleId).Any())
        {
        //throw new ReadingException(ExceptionLiterals.NotFoundCurrentUserByReaderRole);
        }

        return userInfo;
    }

    private JsonElement GetReaderCartableGetTraverse()
    {
        string traverseString = @"[
    {
      ""type"": ""numeric"",
      ""values"": """",
      ""label"": ""شماره موبایل"",
      ""placeholder"": ""موبایل را وارد کنید"",
      ""isRequired"": true,
      ""regex"": ""^09\\d{9}$"",
      ""maxLen"": 11,
      ""minLen"": 11,
      ""selecteOptions"": [],
      ""name"": ""mobileNumber""
    },
    {
      ""type"": ""string"",
      ""values"": ""رضا"",
      ""label"": ""نام"",
      ""placeholder"": ""نام را وارد کنید"",
      ""isRequired"": true,
      ""regex"": """",
      ""maxLen"": 20,
      ""minLen"": 3,
      ""selecteOptions"": [],
      ""name"": ""name""
    },
    {
      ""type"": ""comboSingle"",
      ""values"": ""2"",
      ""label"": ""کاربری"",
      ""placeholder"": """",
      ""isRequired"": false,
      ""regex"": """",
      ""maxLen"": null,
      ""minLen"": null,
      ""selecteOptions"": [
        {
            ""key"": 1,
            ""value"": ""مسکونی""
        },
        {
            ""key"": 2,
            ""value"": ""تجاری""},
        {
            ""key"": 3,
            ""value"": ""سایر""
        }
    ],
      ""name"": ""کاربری""
    },
        {
      ""type"": ""comboMulti"",
      ""values"": ""[1,2]"",
      ""label"": ""خدمات"",
      ""placeholder"": """",
      ""isRequired"": false,
      ""regex"": """",
      ""maxLen"": null,
      ""minLen"": null,
      ""selecteOptions"": [
        {
            ""key"": 1,
            ""value"": ""قطع""
        },
        {
            ""key"": 2,
            ""value"": ""وصل""},
        {
            ""key"": 3,
            ""value"": ""شماره 3""
        }
    ],
      ""name"": ""خدمات""
    }
  ]
";
        JToken traverseJson = JToken.Parse(traverseString);
        JsonElement result = JsonDocument.Parse(traverseJson.ToString()).RootElement;
        return result;
    }

    public async Task OffloadAsync(IEnumerable<MeterReadingListOffLoadInputDto> inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        //appUser Validate
        IEnumerable<TrackingGetDto> trackingsInfo = await OffloadTrackingValidate(inputDto, appUser);
        await OffloadDetailValidate(inputDto);
        var(trackingDuplicateDto, trackingUpdateSeenDto) = OffloadGetTrackingDuplicateList(trackingsInfo, appUser);
        IEnumerable<DetailUpdateDto> detailsUpdateList = OffloadGetDetailUpdateList(inputDto, appUser);
        await OffloadExecSql(trackingDuplicateDto, trackingUpdateSeenDto, detailsUpdateList);
    }

    private async Task OffloadExecSql(TrackingDuplicateInsertDto trackingDuplicateDto, TrackingSeenUpdateDto trackingUpdateSeenDto, IEnumerable<DetailUpdateDto> detailUpdateList)
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
                //await detailCommandService.Update(detailUpdateList);
                transaction.Commit();
            }
        }
    }

    private async Task<IEnumerable<TrackingGetDto>> OffloadTrackingValidate(IEnumerable<MeterReadingListOffLoadInputDto> inputDto, IAppUser appUser)
    {
        IEnumerable<TrackingGetDto> trackingsInfo = await _trackingQueryService.Get(inputDto.Select(details => details.TrackNumber).Distinct().ToList());
        foreach (var tracking in trackingsInfo)
        {
            if (!_validStepFlowIds.Contains(tracking.FlowStepId))
            {
            //throw new ReadingException(ExceptionLiterals.InvalidDetailByCurrentTrackingFlowId(tracking.TrackNumber));
            }

            if (tracking.CounterReaderId != appUser.UserId)
            {
                throw new ReadingException(string.Format(ExceptionLiterals.InvalidTrackingByCurrentUser, tracking.TrackNumber));
            }
        }

        return trackingsInfo;
    }

    private async Task OffloadDetailValidate(IEnumerable<MeterReadingListOffLoadInputDto> inputDto)
    {
        IEnumerable<DetailGetDto> previousDetailsInfo = await _detailQueryService.Get(inputDto.Select(details => details.Id).ToList());
        IEnumerable<CounterStateCodeDto> validCounterStates = await _offloadCounterStateQueryService.GetValids();
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

    private (TrackingDuplicateInsertDto, TrackingSeenUpdateDto) OffloadGetTrackingDuplicateList(IEnumerable<TrackingGetDto> trackingsInfo, IAppUser appUser)
    {
        IEnumerable<TrackingGetDto> trackingToUpdate = trackingsInfo.Where(tracking => tracking.FlowStepId == (int)ReadingFlowStepEnum.DownloadByReader).Distinct().ToList();
        IEnumerable<Guid> trackingIds = trackingToUpdate.Select(tracking => tracking.Id).ToList();
        IEnumerable<Guid> blockIds = trackingToUpdate.Select(tracking => tracking.BlockId).Distinct().ToList();
        TrackingSeenUpdateDto trackingUpdateSeenDto = new(blockIds, true);
        TrackingDuplicateInsertDto trackingDuplicateDto = new(trackingIds, (int)ReadingFlowStepEnum.StartReading, appUser.UserId);
        return (trackingDuplicateDto, trackingUpdateSeenDto);
    }

    private IEnumerable<DetailUpdateDto> OffloadGetDetailUpdateList(IEnumerable<MeterReadingListOffLoadInputDto> inputDto, IAppUser appUser)
    {
        return inputDto.Select(input => new DetailUpdateDto() { Id = input.Id, CounterNumber = input.CounterNumber, LocationDateTime = input.LocationDateTime, PhoneDateTime = input.PhoneDateTime, CurrentCounterStateCode = input.CurrentCounterStateCode.Value, Y = input.Y, X = input.X, GisAccuracy = input.GisAccuracy, CounterNumberShown = input.CounterNumberShown, Description = input.Description, AttemptCount = input.AttemptCount, IsLocked = input.IsLocked, HighLowStateId = input.HighLowStateId, OffLoadDateTime = _currentDateTime, OffloadDateJalali = _currentDateJalali, OffLoadTime = $"{_currentDateTime.TimeOfDay.Hours.ToString()}:{_currentDateTime.TimeOfDay.Minutes.ToString()}", OffLoadBy = appUser.UserId, }).ToList();
    }

    public async Task FinishAsync(long trackNumber, IAppUser appUser, CancellationToken cancellationToken)
    {
        TrackingGetDto trackingsInfo = await _trackingQueryService.Get(trackNumber);
        if (trackingsInfo.FlowStepId != (int)ReadingFlowStepEnum.StartReading)
        {
            throw new ReadingException(string.Format(ExceptionLiterals.InvalidTrackingStepFlowToFinish, trackingsInfo.FlowStepTitle));
        }

        IEnumerable<DetailGetDto> uncompleteDetailList = await _detailQueryService.GetUnComplete(trackNumber);
        if (uncompleteDetailList.Any())
        {
            throw new ReadingException(string.Format(ExceptionLiterals.InvalidFinishTrackingByUncompleteDetail, uncompleteDetailList?.Count() ?? 0));
        }

        TrackingDuplicateInsertDto newTrackingDto = new(new List<Guid>() { trackingsInfo.Id }.ToList(), (int)ReadingFlowStepEnum.Upload, appUser.UserId);
        await FinishExecSql(newTrackingDto);
    }

    private async Task FinishExecSql(TrackingDuplicateInsertDto newTrackingDto)
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
                await trackingCommandService.InsertDuplicate(newTrackingDto);
                transaction.Commit();
            }
        }
    }

    public async Task<GenerateReadingListOutputDto> GenerateAsync(GenerateReadingListInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await GenerateValidate(inputDto, appUser, cancellationToken);
        var(newBoundDto, newBlockList, newTrackingList, newDetailList) = await GenerateGetBlocksInsert(inputDto, appUser);
        string opLogText = string.Format(OpLogLiterals.MeterReadingInsertOpLog, newBoundDto.Id);
        await GenerateExecSql(newBoundDto, newBlockList, newTrackingList, newDetailList, opLogText, appUser);
        GenerateReadingListOutputDto result = await GenerateGetResult(inputDto, newBoundDto, newBlockList?.Count() ?? 0);
        return result;
    }

    private async Task GenerateExecSql(BoundInsertDto newBoundDto, IEnumerable<BlockInsertDto> newBlockList, IEnumerable<TrackingInsertDto> newTrackingList, IEnumerable<DetailInsertDto> newDetailList, string opLogText, IAppUser appUser)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                BoundCommandService boundCommandService = new(connection, transaction);
                BlockCommandService blockCommandService = new(connection, transaction);
                TrackingCommandService trackingCommandService = new(connection, transaction);
                DetailCommandService detailCommandService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await boundCommandService.Insert(newBoundDto);
                await blockCommandService.Insert(newBlockList);
                await trackingCommandService.Insert(newTrackingList);
                await detailCommandService.Insert(newDetailList);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private async Task<GenerateReadingListOutputDto> GenerateGetResult(GenerateReadingListInputDto inputDto, BoundInsertDto newBoundDto, int blockCount)
    {
        return new GenerateReadingListOutputDto()
        {
            ZoneTitle = newBoundDto.ZoneTitle,
            BlockCount = blockCount,
            Year = inputDto.Year,
            ReadingPeriodTypeTitle = (await _readingPeriodTypeQueryService.Get((short)inputDto.ReadingPeriodTypeId)).Title,
            ReadingPeriodTitle = (await _readingPeriodQueryService.Get((short)inputDto.ReadingPeriodId)).Title,
            CustomerCount = newBoundDto.CustomerCount,
        };
    }

    private async Task<(BoundInsertDto, IEnumerable<BlockInsertDto>, IEnumerable<TrackingInsertDto>, IEnumerable<DetailInsertDto>)> GenerateGetBlocksInsert(GenerateReadingListInputDto inputDto, IAppUser appUser) //, BoundInsertDto newBoundDto)
    {
        int stepOrder = 0;
        int overallCustomerCount = 0;
        BoundInsertDto newBoundDto = await GenerateGetBoundInsert(inputDto, Guid.NewGuid(), appUser);
        ICollection<ReadingBlock> blocksInfo = await _readingBlockQeryService.Get(inputDto.ReadingBlockInfo.Select(r => r.ReadingBlockId));
        ICollection<BlockInsertDto> newBlockList = new List<BlockInsertDto>();
        ICollection<TrackingInsertDto> newTrackingList = new List<TrackingInsertDto>();
        ICollection<DetailInsertDto> newDetailList = new List<DetailInsertDto>();
        foreach (var item in blocksInfo) //block List
        {
            IEnumerable<CustomerInfoByBillsAndChangeGetDto> customersInfo = await _readingCustomerDetailInfoQueryService.Get(newBoundDto.ZoneId, item.FromReadingNumber, item.ToReadingNumber);
            int customerCount = customersInfo?.Count() ?? 0;
            overallCustomerCount += customerCount;
            BlockInsertDto newBlockInfo = GenerateGetNewBlock(newBoundDto.Id, customerCount, ++stepOrder, item);
            ICollection<UserQueryDto> readersInfo = await GenerateReadersValidateAngGet(inputDto.ReadingBlockInfo, newBoundDto.ZoneId);
            PerReadingBlockInfoDto perReadingBlockInfo = inputDto.ReadingBlockInfo.Where(p => p.ReadingBlockId == item.Id).FirstOrDefault();
            UserQueryDto currentReaderInfo = readersInfo.Where(r => r.Username == perReadingBlockInfo.ReaderCode.ToString()).FirstOrDefault();
            int trackNumber = await _trackNumberService.GetNewTrackNumber();
            TrackingInsertDto newTrackingInfo = GenerateGetTrackingInsert(newBoundDto, newBlockInfo, item, currentReaderInfo, trackNumber, customerCount); //todo: completer overalCustomerCount
            foreach (var perCustomer in customersInfo) //Detail List
            {
                DetailInsertDto newDetailInfo = GenerateGetDetail(perCustomer, newTrackingInfo);
                newDetailList.Add(newDetailInfo);
            }

            newBlockList.Add(newBlockInfo);
            newTrackingList.Add(newTrackingInfo);
        }

        newBoundDto.CustomerCount = overallCustomerCount;
        foreach (var item in newTrackingList)
        {
            item.OverallCustomerCount = overallCustomerCount;
        }

        return (newBoundDto, newBlockList, newTrackingList, newDetailList);
    }

    private async Task<BoundInsertDto> GenerateGetBoundInsert(GenerateReadingListInputDto inputDto, Guid Id, IAppUser appUser)
    {
        ReadingBound readingBoundInfo = await _readingBoundQueryService.Get(inputDto.ReadingBoundId);
        if (!readingBoundInfo.IsValid)
        {
            throw new ReadingException(ExceptionLiterals.InvalidReadingBoundIsValid);
        }

        NumericDictionary zoneInfo = await _zoneQueryService.Get(readingBoundInfo.ZoneId, true);
        return new BoundInsertDto()
        {
            Id = Id,
            ReadingBoundId = readingBoundInfo.Id,
            ReadingBoundTitle = readingBoundInfo.Title,
            ZoneId = zoneInfo.Id,
            ZoneTitle = zoneInfo.Title,
            FromReadingNumber = readingBoundInfo.FromReadingNumber,
            ToReadingNumber = readingBoundInfo.ToReadingNumber,
            IsValidated = readingBoundInfo.IsValid,
            BlockCount = inputDto.ReadingBlockInfo?.Count() ?? 0,
            CustomerCount = 0, //compeleteAfter
            Year = inputDto.Year,
            ReadingPeriodTypeId = inputDto.ReadingPeriodTypeId,
            ReadingPeriodId = inputDto.ReadingPeriodId,
            InsertBy = appUser.UserId,
            InsertDateTime = _generateCurrentDateTime,
        };
    }

    private async Task GenerateValidate(GenerateReadingListInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await GenerateInputValidate(inputDto, cancellationToken);
        await GenerateZoneValidate(inputDto, appUser);
    }

    private TrackingInsertDto GenerateGetTrackingInsert(BoundInsertDto newBoundDto, BlockInsertDto newBlockInfo, ReadingBlock readingBlock, UserQueryDto currentReaderInfo, int trackNumber, int customerCount)
    {
        return new TrackingInsertDto()
        {
            BlockId = newBlockInfo.Id,
            TrackNumber = trackNumber,
            TbReadingProgramId = Guid.Empty,
            ZoneId = newBoundDto.ZoneId,
            ZoneTitle = newBoundDto.ZoneTitle,
            FlowStepId = _createPathStepId,
            FromReadingNumber = readingBlock.FromReadingNumber,
            ToReadingNumber = readingBlock.ToReadingNumber,
            InsertBy = newBoundDto.InsertBy,
            InsertDateJalali = _generateCurrentDateJalali,
            InsertDateTime = _generateCurrentDateTime,
            CounterReaderCode = Convert.ToInt32(currentReaderInfo.Username),
            CounterReaderId = currentReaderInfo.Id,
            CounterReaderName = currentReaderInfo.DisplayName,
            CounterReaderMobile = currentReaderInfo.Mobile,
            IsBazdid = false,
            OverallCustomerCount = 0,
            ItemCustomerCount = customerCount,
        };
    }

    private async Task<ICollection<UserQueryDto>> GenerateReadersValidateAngGet(IEnumerable<PerReadingBlockInfoDto> selectedBlocksInfo, int zoneId)
    {
        ICollection<UserQueryDto> readerUserList = await _userRoleQueryService.GetByZone(ReportLiterals.ReaderRoleId, zoneId);
        ICollection<UserQueryDto> currentReaderInfo = new List<UserQueryDto>();
        foreach (var reader in selectedBlocksInfo)
        {
            UserQueryDto? readerInfo = readerUserList.Where(user => user.Username == reader.ReaderCode.ToString()).FirstOrDefault();
            if (readerInfo is null)
            {
                throw new ReadingException(ExceptionLiterals.InvalidReadingBoundWithReaderZoneId);
            }

            currentReaderInfo.Add(readerInfo);
        }

        return currentReaderInfo;
    }

    private async Task GenerateInputValidate(GenerateReadingListInputDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    }

    private async Task GenerateZoneValidate(GenerateReadingListInputDto inputDto, IAppUser appUser)
    {
        ReadingBound readingBoundInfo = await _readingBoundQueryService.Get(inputDto.ReadingBoundId);
        ICollection<ReadingBlock> readingBlocksInfo = await _readingBlockQeryService.Get(inputDto.ReadingBlockInfo.Select(r => r.ReadingBlockId));
        if (readingBlocksInfo.Where(b => b.ReadingBoundId != readingBoundInfo.Id).Any() && (readingBlocksInfo?.Count() ?? 0) != (inputDto?.ReadingBlockInfo?.Count() ?? 0))
        {
            throw new ReadingException(ExceptionLiterals.InvalidReadingBoundWithReadingBlock);
        }
    }

    private BlockInsertDto GenerateGetNewBlock(Guid newBoundId, int customerCount, int stepOrder, ReadingBlock readingBlockInfo)
    {
        return new BlockInsertDto()
        {
            BoundId = newBoundId,
            ReadingBlockId = readingBlockInfo.Id,
            ReadingBlockTitle = readingBlockInfo.Title,
            FromReadingNumber = readingBlockInfo.FromReadingNumber,
            ToReadingNumber = readingBlockInfo.ToReadingNumber,
            CustomerCount = customerCount,
            OrderDigit = stepOrder,
            OrderPersian = $"{stepOrder}-{stepOrder.NumberToText(language: Language.Persian)}",
        };
    }

    private DetailInsertDto GenerateGetDetail(CustomerInfoByBillsAndChangeGetDto customerInfo, TrackingInsertDto newTrackingInfo)
    {
        return new DetailInsertDto()
        {
            TrackingId = newTrackingInfo.Id,
            TrackNumber = newTrackingInfo.TrackNumber,
            BillId = customerInfo.BillId,
            CustomerNumber = customerInfo.CustomerNumber,
            ReadingNumber = customerInfo.ReadingNumber,
            FirstName = customerInfo.FirstName,
            SureName = customerInfo.Surname,
            Address = customerInfo.Address,
            Plaque = customerInfo.Plaque,
            UsageId = customerInfo.UsageId,
            DomesticUnit = customerInfo.DomesticUnit,
            CommercialUnit = customerInfo.CommercialUnit,
            OtherUnit = customerInfo.OtherUnit,
            MeterDiameterId = customerInfo.MeterDiameterId,
            SiphonDiameterId = customerInfo.MainSiphon,
            PostalCode = customerInfo.PostalCode,
            PreNumber = customerInfo.BillNumber,
            PreDateJalali = customerInfo.BillDateJalali,
            PreDateTime = null,
            PreConsumptionAverage = customerInfo.BillConsumption,
            PreCounterStateCode = customerInfo.BillCounterStateCode,
            BodySerial = customerInfo.BodySerial,
            MeterInstallationDateJalali = customerInfo.MeterInstallationDateJalali,
            ChangeDateJalali = customerInfo.ChangeDateJalali,
            ChangeNumber = customerInfo.ChangeNumber,
            ContractualCapacity = customerInfo.ContractualCapacity,
            MobileNumber = customerInfo.MobileNumber,
            DeletionStateId = customerInfo.DeletionStateId,
            BranchTypeId = customerInfo.BranchTypeId,
        };
    }
}
