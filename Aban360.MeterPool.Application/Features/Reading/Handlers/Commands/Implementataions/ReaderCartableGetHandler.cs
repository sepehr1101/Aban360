using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.MeterPool.Application.Features.Reading.Handlers.Commands.Contracts;
using Aban360.MeterPool.Domain.Constants;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Aban360.MeterPool.Persistence.Features.Reading.Commands;
using Aban360.MeterPool.Persistence.Features.Reading.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.MeterPool.Application.Features.Reading.Handlers.Commands.Implementataions
{
    internal sealed class ReaderCartableGetHandler : AbstractBaseConnection, IReaderCartableGetHandler
    {
        private readonly IBlockQueryService _blockQueryService;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IDetailQueryService _detailQueryService;
        private readonly IUserGetSingleQueryHandler _userGetSingleQueryHandler;
        private readonly IT41QueryService _usageQueryService;
        private readonly IT7QueryService _branchTypeQueryService;
        private readonly IDeletionStateQueryService _deletionStateQueryService;
        private readonly ICounterStateQueryService _counterStateQueryService;
        private static int _createPathFlowStepId = (int)ReadingFlowStepEnum.CreatePath;
        private static int _downloadByReadigFlowStepId = (int)ReadingFlowStepEnum.DownloadByReader;
        private static int _readerRoleId = ReportLiterals.ReaderRoleId;
        public ReaderCartableGetHandler(
            IBlockQueryService blockQueryService,
            ITrackingQueryService trackingQueryService,
            IDetailQueryService detailQueryService,
            IUserGetSingleQueryHandler userGetSingleQueryHandler,
            IT41QueryService usageQueryService,
            IT7QueryService branchTypeQueryService,
            IDeletionStateQueryService deletionStateQueryService,
            ICounterStateQueryService counterStateQueryService,
            IConfiguration configuration)
                : base(configuration)
        {
            _blockQueryService = blockQueryService;
            _blockQueryService.NotNull(nameof(blockQueryService));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _detailQueryService = detailQueryService;
            _detailQueryService.NotNull(nameof(detailQueryService));

            _userGetSingleQueryHandler = userGetSingleQueryHandler;
            _userGetSingleQueryHandler.NotNull(nameof(userGetSingleQueryHandler));

            _usageQueryService = usageQueryService;
            _usageQueryService.NotNull(nameof(usageQueryService));

            _branchTypeQueryService = branchTypeQueryService;
            _branchTypeQueryService.NotNull(nameof(branchTypeQueryService));

            _deletionStateQueryService = deletionStateQueryService;
            _deletionStateQueryService.NotNull(nameof(deletionStateQueryService));

            _counterStateQueryService = counterStateQueryService;
            _counterStateQueryService.NotNull(nameof(counterStateQueryService));
        }

        public async Task<ReaderCartableOutputDto> Handle(IAppUser appUser, CancellationToken cancellationToken)
        {
            UserDisplayDto readerInfo = await Validate(appUser, cancellationToken);

            IEnumerable<TrackingGetDto> trackingInfo = await _trackingQueryService.Get(appUser.UserId, _createPathFlowStepId);
            IEnumerable<Guid> trackingIds = trackingInfo.Select(t => t.Id).ToList();
            IEnumerable<DetailGetDto> detailInfo = await _detailQueryService.GetByTrackIds(trackingIds);

            TrackingDuplicateInsertDto trackingDuplicateInsertDto = new(trackingIds, _downloadByReadigFlowStepId,appUser.UserId);
            await ExecSql(trackingDuplicateInsertDto);
            return await GetResult(trackingInfo, detailInfo, readerInfo);
        }
        private async Task ExecSql(TrackingDuplicateInsertDto trackingDuplicateInsertDto)
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

                    await trackingCommandService.InsertDuplicate(trackingDuplicateInsertDto);

                    transaction.Commit();
                }
            }
        }
        private async Task<ReaderCartableOutputDto> GetResult(IEnumerable<TrackingGetDto> trackingInfo, IEnumerable<DetailGetDto> detailInfo, UserDisplayDto readerInfo)
        {
            IEnumerable<TrackingDataOutputDto> trackingData = trackingInfo
                .Select(t => new TrackingDataOutputDto()
                {
                    Id = t.Id,
                    BlockId = t.BlockId,
                    TrackNumber = t.TrackNumber,
                    TbReadingProgramId = t.TbReadingProgramId,
                    ZoneId = t.ZoneId,
                    ZoneTitle = t.ZoneTitle,
                    FlowStepId = t.FlowStepId,
                    FromReadingNumber = t.FromReadingNumber,
                    ToReadingNumber = t.ToReadingNumber,
                    ListNumber = t.ListNumber,
                    InsertBy = t.InsertBy,
                    InsertDateJalali = t.InsertDateJalali,
                    InsertDateTime = t.InsertDateTime,
                    Seen = t.Seen,
                    CounterReaderCode = t.CounterReaderCode,
                    CounterReaderId = t.CounterReaderId,
                    CounterReaderName = t.CounterReaderName,
                    CounterReaderMobile = t.CounterReaderMobile,
                    IsBazdid = t.IsBazdid,
                    IsRoosta = t.IsRoosta,
                    FromDate = t.FromDate,
                    ToDate = t.ToDate,
                    OverallCustomerCount = t.OverallCustomerCount,
                    ItemCustomerCount = t.ItemCustomerCount,
                    ProvisionallyPercent = t.ProvisionallyPercent,
                    ImagePercent = t.ImagePercent,
                    HasPreNumber = t.HasPreNumber,
                    DisplayBillId = t.DisplayBillId,
                    DisplayCustomerNumber = t.DisplayCustomerNumber,
                    Description = t.Description,
                    X = t.X,
                    Y = t.Y,
                    DisplayPreDate = t.DisplayPreDate,
                    DisplayMobile = t.DisplayMobile,
                    HasImage = t.HasImage,
                    DisplayDebt = t.DisplayDebt,
                    DisplayIcons = t.DisplayIcons,
                })
                .ToList();

            IEnumerable<DetailDataOutputDto> detailData = detailInfo
                .Select(d => new DetailDataOutputDto()
                {
                    Id = d.Id,
                    TrackingId = d.TrackingId,
                    TrackNumber = d.TrackNumber,
                    BillId = d.BillId,
                    CustomerNumber = d.CustomerNumber,
                    ReadingNumber = d.ReadingNumber,
                    FirstName = d.FirstName,
                    SureName = d.SureName,
                    Address = d.Address,
                    Plaque = d.Plaque,
                    UsageId = d.UsageId,
                    DomesticUnit = d.DomesticUnit,
                    CommercialUnit = d.CommercialUnit,
                    OtherUnit = d.OtherUnit,
                    MeterDiameterId = d.MeterDiameterId,
                    SiphonDiameterId = d.SiphonDiameterId,
                    PostalCode = d.PostalCode,
                    PreNumber = d.PreNumber,
                    PreDateJalali = d.PreDateJalali,
                    PreDateTime = d.PreDateTime,
                    PreConsumptionAverage = d.PreConsumptionAverage,
                    PreCounterStateCode = d.PreCounterStateCode,
                    BodySerial = d.BodySerial,
                    MeterInstallationDateJalali = d.MeterInstallationDateJalali,
                    ChangeDateJalali = d.ChangeDateJalali,
                    ChangeNumber = d.ChangeNumber,
                    ContractualCapacity = d.ContractualCapacity,
                    MobileNumber = d.MobileNumber,
                    DeletionStateId = d.DeletionStateId,
                    BranchTypeId = d.BranchTypeId,
                    CounterNumber = d.CounterNumber,
                    CurrentCounterStateCode = d.CurrentCounterStateCode,
                    PossibleAddress = d.PossibleAddress,
                    PossibleBodySerial = d.PossibleBodySerial,
                    PossibleReadingNumber = d.PossibleReadingNumber,
                    PossibleMobileNumber = d.PossibleMobileNumber,
                    PossiblePhoneNumber = d.PossiblePhoneNumber,
                    PossibleDomesticUnit = d.PossibleDomesticUnit,
                    PossibleCommercialUnit = d.PossibleCommercialUnit,
                    PossibleOtherUnit = d.PossibleOtherUnit,
                    PossibleUsageId = d.PossibleUsageId,
                    PossibleEmptyUnit = d.PossibleEmptyUnit,
                    OffLoadDateTime = d.OffLoadDateTime,
                    OffloadDateJalali = d.OffloadDateJalali,
                    OffLoadTime = d.OffLoadTime,
                    OffLoadBy = d.OffLoadBy,
                    Y = d.Y,
                    X = d.X,
                    GisAccuracy = d.GisAccuracy,
                    ImageCount = d.ImageCount,
                    CurrentConsumption = d.CurrentConsumption,
                    CurrentConsumptionAverage = d.CurrentConsumptionAverage,
                    DateDifference = d.DateDifference,
                    CounterNumberShown = d.CounterNumberShown,
                    ExcludedForBazdid = d.ExcludedForBazdid,
                    ExcludedForEslah = d.ExcludedForEslah,
                    JsonItems = d.JsonItems,
                    ConsumptionStateId = d.ConsumptionStateId,
                    Description = d.Description,
                    IsCalculated = d.IsCalculated,
                    CalculationErrorDescription = d.CalculationErrorDescription,
                    D1 = d.D1,
                    D2 = d.D2,
                    CounterStatePosition = d.CounterStatePosition,
                    AttemptCount = d.AttemptCount,
                    IsLocked = d.IsLocked,
                    LocationDateTime = d.LocationDateTime,
                    PhoneDateTime = d.PhoneDateTime,
                    FatherName = d.FatherName,
                    OldReadingNumber = d.OldReadingNumber,
                    OldCustomerNumber = d.OldCustomerNumber,
                    MobilesNumber = d.MobilesNumber,
                    WaterDebt = d.WaterDebt,
                    ModifierUserId = d.ModifierUserId,
                    GuildId = d.GuildId,
                    SiphonInstallationDateJalali = d.SiphonInstallationDateJalali,
                    PreGuildId = d.PreGuildId,
                    HighLowStateId = d.HighLowStateId,
                    ExcludeBy = d.ExcludeBy,
                    ExcludeDateTime = d.ExcludeDateTime,
                    ExcludeCauseId = d.ExcludeCauseId,
                    ExcludeCauseTitle = d.ExcludeCauseTitle,
                })
                .ToList();

            IEnumerable<NumericDictionary> usageList = await _usageQueryService.Get();
            IEnumerable<NumericDictionary> branchTypeList = await _branchTypeQueryService.Get();
            IEnumerable<NumericDictionary> deletionStateList = await _deletionStateQueryService.Get();
            IEnumerable<CounterStateCodeDto> counterStateList = await _counterStateQueryService.Get();

            ReaderCartableOutputDto result = new()
            {
                TrackingInfo = trackingData,
                DetailInfo = detailData,
                Usages = usageList,
                BranchTypes = branchTypeList,
                DeletionStates = deletionStateList,
                CounterStates = counterStateList.Select(c => new NumericDictionary(c.Id, c.Title)).ToList()
            };
            return result;
        }
        private async Task<UserDisplayDto> Validate(IAppUser appUser, CancellationToken cancellationToken)
        {//ToDo: user another handler to find user
            UserDisplayDto userInfo = await _userGetSingleQueryHandler.Handle(appUser.UserId, cancellationToken);
            if (!(userInfo?.RoleInfo?.RoleInfo?.Select(role => role.Id)?.ToList()?.Contains(_readerRoleId) ?? false))
            {
                throw new ReadingException(ExceptionLiterals.NotFoundCurrentUserByReaderRole);
            }

            return userInfo;
        }
    }
}
