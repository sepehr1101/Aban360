using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Reading.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Aban360.MeterPool.Persistence.Features.Reading.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;

namespace Aban360.MeterPool.Application.Features.Reading.Handlers.Queries.Implementataions
{
    public sealed class ReadingDetailGetHandler : IReadingDetailGetHandler
    {
        private readonly IDetailQueryService _detailQueryService;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private static string _title = ReportLiterals.ReadingDetail;
        public ReadingDetailGetHandler(
            IDetailQueryService detailQueryService,
            ITrackingQueryService trackingQueryService,
            ICommonZoneService commonZoneService)
        {
            _detailQueryService = detailQueryService;
            _detailQueryService.NotNull(nameof(detailQueryService));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
        }

        public async Task<ReportOutput<DetailHeaderDto, DetailDataDto>> Handle(Guid trackingId, IAppUser appUser, CancellationToken cancellationToken)
        {
            TrackingGetDto trackingInfo = await _trackingQueryService.Get(trackingId);
            IEnumerable<DetailGetDto> detailInfo = await _detailQueryService.GetByTrackId(trackingId);
            await _commonZoneService.IsUserInZone(appUser.UserId, trackingInfo.ZoneId);

            IEnumerable<DetailDataDto> data = GetDetailData(detailInfo);
            DetailHeaderDto header = new()
            {
                RecordCount=data?.Count()??0,
                Title=_title
            };

            ReportOutput<DetailHeaderDto, DetailDataDto> result = new(_title, header, data);
            return result;
        }
        public IEnumerable<DetailDataDto> GetDetailData(IEnumerable<DetailGetDto> data)
        {
            return data.Select(d => new DetailDataDto()
            {
                Id = d.Id,
                TrackingId = d.TrackingId,
                TrackNumber = d.TrackNumber,
                BillId = d.BillId,
                CustomerNumber = d.CustomerNumber,
                ReadingNumber = d.ReadingNumber,
                FirstName = d.FirstName,
                SurName = d.SurName,
                Address = d.Address,
                Plaque = d.Plaque,
                UsageId = d.UsageId,
                UsageTitle = d.UsageTitle,
                DomesticUnit = d.DomesticUnit,
                CommercialUnit = d.CommercialUnit,
                OtherUnit = d.OtherUnit,
                EmptyUnit = d.EmptyUnit,
                MeterDiameterId = d.MeterDiameterId,
                MeterDiameterTitle = d.MeterDiameterTitle,
                SiphonDiameterId = d.SiphonDiameterId,
                SiphonDiameterTitle = d.SiphonDiameterTitle,
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
                Traverse = d.Traverse,
                ConsumptionStateId = d.ConsumptionStateId,
                Description = d.Description,
                IsCalculated = d.IsCalculated,
                CalculationErrorDescription = d.CalculationErrorDescription,
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
                HighLowStateId = d.HighLowStateId,
                ExcludeBy = d.ExcludeBy,
                ExcludeDateTime = d.ExcludeDateTime,
                ExcludeCauseId = d.ExcludeCauseId,
                ExcludeCauseTitle = d.ExcludeCauseTitle,

            }).ToList();
        }
    }
}
