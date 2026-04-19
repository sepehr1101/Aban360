using Aban360.ClaimPool.Application.Features.Base.Services;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class RequestDuplicateValidationHandler : IRequestDuplicateValidationHandler
    {
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        public RequestDuplicateValidationHandler(
            IMoshtrakQueryService moshtrakQueryService,
            ITrackingQueryService trackingQueryService,
            ICommonMemberQueryService commonMemberQueryService)
        {
            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));
        }

        public async Task<TrackingDuplicateValidationOutputDto> Handle(TrackingDuplicateValidationInputDto inputDto, CancellationToken cancellationToken)
        {
            var (moshtrakSearch, moshtrakSearchType) = await GetMoshtrakDto(inputDto);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(moshtrakSearch, moshtrakSearchType)).FirstOrDefault();
            TrackingOutputDto latestTrackingInfo = await _trackingQueryService.GetLatest(moshtrakInfo.TrackNumber);
            TrackingOutputDto firstTrackingInfo = await _trackingQueryService.GetFirstStep(moshtrakInfo.TrackNumber);
            NumericDictionary? requestOrigin = RequestOrigin.GetRequestOrigin(firstTrackingInfo.RequestOriginId);

            MoshtrakServiceDto moshtrakServiceDto = GetMoshtrakService(moshtrakInfo);
            IEnumerable<NumericDictionary> serviceSelected = MoshtrakService.GetServicesSelectedDto(moshtrakServiceDto);

            return GetResultDto(serviceSelected, moshtrakInfo, latestTrackingInfo, firstTrackingInfo, requestOrigin);
        }
        private async Task<(MoshtrakGetDto, MoshtrakSearchTypeEnum)> GetMoshtrakDto(TrackingDuplicateValidationInputDto inputDto)
        {
            MoshtrakGetDto moshtrakSearch = new();
            MoshtrakSearchTypeEnum moshtrakSearchType = 0;
            if (inputDto.ValidationType == TrackingDuplicateValidationTypeEnum.ByNationalCode)
            {
                ZoneIdAndCustomerNumber neighbourInfo = await _commonMemberQueryService.Get(inputDto.NeighbourBillId);
                moshtrakSearch = new(neighbourInfo.ZoneId, null, inputDto.NationalCode, null);
                moshtrakSearchType = MoshtrakSearchTypeEnum.ByNationalCode;
            }
            if (inputDto.ValidationType == TrackingDuplicateValidationTypeEnum.ByBillId)
            {
                ZoneIdAndCustomerNumber customerInfo = await _commonMemberQueryService.Get(inputDto.BillId);
                moshtrakSearch = new(customerInfo.ZoneId, customerInfo.CustomerNumber, null, null);
                moshtrakSearchType = MoshtrakSearchTypeEnum.ByCustomerNumber;
            }

            return (moshtrakSearch, moshtrakSearchType);
        }
        private MoshtrakServiceDto GetMoshtrakService(MoshtrakOutputDto serviceSelected)
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
        private TrackingDuplicateValidationOutputDto GetResultDto(IEnumerable<NumericDictionary> serviceSelected,MoshtrakOutputDto moshtrakInfo, TrackingOutputDto latestTrackingInfo, TrackingOutputDto firstTrackingInfo, NumericDictionary? requestOrigin)
        {
            return new TrackingDuplicateValidationOutputDto()
            {
                ZoneId = moshtrakInfo.ZoneId,
                CustomerNumber = moshtrakInfo.CustomerNumber,
                NationalCode = moshtrakInfo.NationalCode,
                TrackNumber = moshtrakInfo.TrackNumber,
                FirstName = moshtrakInfo.FirstName,
                Surname = moshtrakInfo.Surname,
                FatherName = moshtrakInfo.FatherName,
                MobileNumber = moshtrakInfo.MobileNumber,
                RequestDateJalali = moshtrakInfo.RequestDateJalali,
                IsDuplicate = moshtrakInfo.IsRegistered ? false : true,
                LatestStatusId = latestTrackingInfo.StatusId,
                LatestStatusTitle = latestTrackingInfo.StatusTitle,
                RequestOriginId = firstTrackingInfo.RequestOriginId,
                RequestOrigin = requestOrigin?.Title ?? string.Empty,
                ServiceSelected=serviceSelected
            };
        }
    }
}
