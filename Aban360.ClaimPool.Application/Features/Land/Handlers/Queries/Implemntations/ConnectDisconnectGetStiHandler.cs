using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Geo.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Domain.Features.Geo;
using Aban360.ReportPool.Infrastructure.Features.Geo;
using DNTPersianUtils.Core;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    public class ConnectDisconnectGetStiHandler : IConnectDisconnectGetStiHandler
    {
        private readonly IConnectDisconnectQueryService _connectDisconnectQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly ILocationInfoGetHandler _locationInfoService;
        private readonly IMapService _mapService;
        private const int _connectTypeId = 1;
        public ConnectDisconnectGetStiHandler(
            IConnectDisconnectQueryService connectDisconnectQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            ICommonZoneService commonZoneService,
            ILocationInfoGetHandler locationInfoService,
            IMapService mapService)
        {
            _connectDisconnectQueryService = connectDisconnectQueryService;
            _connectDisconnectQueryService.NotNull(nameof(connectDisconnectQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(locationInfoService));

            _locationInfoService = locationInfoService;
            _locationInfoService.NotNull(nameof(locationInfoService));

            _mapService = mapService;
            _mapService.NotNull(nameof(mapService));
        }

        public async Task<ReportOutput<ConnectDisconnectPrintHeaderOutputDto, ConnectDisconnectPrintDataOutputDto>> Handle(long id, IAppUser appUser, CancellationToken cancellationToken)
        {
            ConnectDisconnectGetDto connectDisconnectInfo = await _connectDisconnectQueryService.Get(id, null);
            if (connectDisconnectInfo.RemovedDateTime is not null || connectDisconnectInfo.ResultId is not null)
                throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidGetSti);

            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(connectDisconnectInfo.BillId);
            await _commonZoneService.IsUserInZone(appUser, zoneIdAndCustomerNumber.ZoneId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumber);

            string title = connectDisconnectInfo.TypeId == _connectTypeId ? ReportLiterals.Connect : ReportLiterals.Disconnect;

            return await GetResult(memberInfo, connectDisconnectInfo, title, cancellationToken);
        }
        private async Task<ReportOutput<ConnectDisconnectPrintHeaderOutputDto, ConnectDisconnectPrintDataOutputDto>> GetResult(MemberInfoGetDto memberInfo, ConnectDisconnectGetDto connectDisconnectInfo, string title, CancellationToken cancellationToken)
        {
            ICollection<ConnectDisconnectPrintDataOutputDto> data = new List<ConnectDisconnectPrintDataOutputDto>();
            var locInfo = await GetBase64Location(connectDisconnectInfo.BillId, cancellationToken);
            data.Add(new ConnectDisconnectPrintDataOutputDto()
            {
                CustomerNumber = memberInfo.CustomerNumber,
                RegionTitle = memberInfo.RegionTitle,
                ZoneTitle = memberInfo.ZoneTitle,
                PostalCode = memberInfo.PostalCode,
                NationalCode = memberInfo.NationalCode,
                Address = memberInfo.Address,
                WaterDebtAmount = connectDisconnectInfo.WaterDebt,
                FirstName = memberInfo.FirstName,
                Surname = memberInfo.Surname,
                FatherName = memberInfo.FatherName,
                FullName = memberInfo.FullName,
                MobileNumber = memberInfo.MobileNumber,
                ReadingNumber = memberInfo.ReadingNumber,
                BillId = memberInfo.BillId,
                PaymentId = TransactionIdGenerator.GeneratePaymentId(connectDisconnectInfo.WaterDebt, "100"),
                UsageTitle = memberInfo.UsageTitle,
                MeterDiameterId = memberInfo.MeterDiameterId,
                MeterDiameterTitle = memberInfo.MeterDiameterTitle,
                BranchTypeTitle = memberInfo.UseStateTitle,
                CompanyTitle = connectDisconnectInfo.CompanyTitle ?? string.Empty,
                IsConnect = connectDisconnectInfo.TypeId == _connectTypeId ? true : false,
                CauseTitle = connectDisconnectInfo.CommandCauseTitle,
                Base64 = locInfo.Item2,
                X = locInfo.Item1.Easting.ToString(),
                Y = locInfo.Item1.Northing.ToString(),
            });
            ConnectDisconnectPrintHeaderOutputDto header = new()
            {
                CustomerCount = 1,
                RecordCount = 1,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = title,
                MessageText = string.Empty
            };
            return new ReportOutput<ConnectDisconnectPrintHeaderOutputDto, ConnectDisconnectPrintDataOutputDto>(title, header, data);
        }
        private async Task<(LocationInfoDto, string)> GetBase64Location(string billId, CancellationToken cancellationToken)
        {
            LocationInfoDto location = await _locationInfoService.Handle(billId, cancellationToken);
            if (location is null ||
                string.IsNullOrWhiteSpace(location.X) || location.X.Trim() == "0" ||
                string.IsNullOrWhiteSpace(location.Y) || location.Y.Trim() == "0")
            {
                return (location, await Base64Operation.GetNotFoundBase64(cancellationToken));
            }
            string base64 = await _mapService.GenerateMapBase64(location.X, location.Y);
            return (location, base64);
        }
    }
}
