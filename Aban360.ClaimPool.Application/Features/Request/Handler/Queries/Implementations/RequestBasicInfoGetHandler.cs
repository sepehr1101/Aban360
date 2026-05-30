using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class RequestBasicInfoGetHandler : IRequestBasicInfoGetHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly ICommonZoneService _commonZoneService;
        public RequestBasicInfoGetHandler(
            ITrackingQueryService trackingQueryService,
            IMoshtrakQueryService moshtrakQueryService,
            ICommonZoneService commonZoneService)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
        }

        public async Task<RequestBasicInfoDataOutputDto> Handle(int trackNumber, IAppUser appUser, CancellationToken cancellationToken)
        {
            TrackingOutputDto latestTrackingInfo = await _trackingQueryService.GetLatest(trackNumber);
            await _commonZoneService.IsUserInZone(appUser, latestTrackingInfo.ZoneId);

            RequestBasicInfoGetDto basicInfo = await _moshtrakQueryService.GetBasicInfo(trackNumber, latestTrackingInfo.ZoneId);
            MoshtrakServiceDto moshtrakServiceDto = GetMoshtrakService(basicInfo);
            IEnumerable<SelectionDto> ServiceSelected = MoshtrakService.GetMoshtrakCompanyServiceDto(moshtrakServiceDto, latestTrackingInfo.ServiceGroupId);
            return GetResult(basicInfo, ServiceSelected);

        }
        private RequestBasicInfoDataOutputDto GetResult(RequestBasicInfoGetDto inputDto, IEnumerable<SelectionDto> ServiceSelected)
        {
            return new RequestBasicInfoDataOutputDto()
            {
                RegionId = inputDto.RegionId,
                RegionTitle = inputDto.RegionTitle,
                ZoneId = inputDto.ZoneId,
                ZoneTitle = inputDto.ZoneTitle,
                FirstName = inputDto.FirstName,
                Surname = inputDto.Surname,
                FullName = inputDto.FullName,
                FatherName = inputDto.FatherName,
                BranchTypeId = inputDto.BranchTypeId,
                BranchTypeTitle = inputDto.BranchTypeTitle,
                CustomerNumber = inputDto.CustomerNumber,
                ReadingNumber = inputDto.ReadingNumber,
                TrackNumber = inputDto.TrackNumber,
                Address = inputDto.Address,
                PostalCode = inputDto.PostalCode,
                NationalCode = inputDto.NationalCode,
                Description = inputDto.Description,
                RequestDateJalali = inputDto.RequestDateJalali,
                RegisterDateJalali = inputDto.RegisterDateJalali,
                DiscountCount = inputDto.DiscountCount,
                DiscountTypeId = inputDto.DiscountTypeId,
                DiscountTypeTitle = inputDto.DiscountTypeTitle,
                PhoneNumber = inputDto.PhoneNumber,
                NeighbourBillId = inputDto.NeighbourBillId,
                ServiceGroupId = inputDto.ServiceGroupId,
                ServiceGroupTitle = inputDto.ServiceGroupTitle,
                PreviousStatusId = inputDto.PreviousStatusId,
                PreviousStatusTitle = inputDto.PreviousStatusTitle,
                PreviousStepDateJalali = inputDto.PreviousStepDateJalali,
                BillId = inputDto.BillId,
                PreviousTrackId = inputDto.PreviousTrackId,
                ServiceSelected = ServiceSelected
            };
        }
        private MoshtrakServiceDto GetMoshtrakService(RequestBasicInfoGetDto serviceSelected)
        {
            return new MoshtrakServiceDto()
            {
                s0 = serviceSelected.s0,
                s1 = serviceSelected.s1,
                s2 = serviceSelected.s2,
                s3 = serviceSelected.s3,
                s4 = serviceSelected.s4,
                s5 = serviceSelected.s5,
                s8 = serviceSelected.s8,
                s9 = serviceSelected.s9,
                s10 = serviceSelected.s10,
                s11 = serviceSelected.s11,
                s12 = serviceSelected.s12,
                s13 = serviceSelected.s13,
                s14 = serviceSelected.s14,
                s15 = serviceSelected.s15,
                s16 = serviceSelected.s16,
                s17 = serviceSelected.s17,
                s18 = serviceSelected.s18,
                s19 = serviceSelected.s19,
                s20 = serviceSelected.s20,
                s21 = serviceSelected.s21,
                s22 = serviceSelected.s22,
                s23 = serviceSelected.s23,
                s24 = serviceSelected.s24,
                s25 = serviceSelected.s25,
                s26 = serviceSelected.s26,
                s27 = serviceSelected.s27,
                s28 = serviceSelected.s28,
                s29 = serviceSelected.s29,
                s30 = serviceSelected.s30,
                s31 = serviceSelected.s31,
                s32 = serviceSelected.s32,
                s33 = serviceSelected.s33,
                s34 = serviceSelected.s34,
                s35 = serviceSelected.s35,
                s36 = serviceSelected.s36,
                s37 = serviceSelected.s37,
                s38 = serviceSelected.s38,
                s39 = serviceSelected.s39,
                s40 = serviceSelected.s40,
                s41 = serviceSelected.s41,
                s42 = serviceSelected.s42,
                s43 = serviceSelected.s43,
                s44 = serviceSelected.s44,
                s45 = serviceSelected.s45,
                s46 = serviceSelected.s46,
                s47 = serviceSelected.s47,
                s48 = serviceSelected.s48,
            };
        }
    }
}
