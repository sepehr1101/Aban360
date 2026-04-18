using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations;
using Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Implementations
{
    internal sealed class GeneralInformationHandler : IGeneralInformationHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        public GeneralInformationHandler(
            ITrackingQueryService trackingDetailQueryService,
            IMoshtrakQueryService moshtrakQueryService)
        {
            _trackingQueryService = trackingDetailQueryService;
            _trackingQueryService.NotNull(nameof(trackingDetailQueryService));

            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));
        }

        public async Task<GeneralRequestDataOutputDto> Handle(Guid trackId, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.Get(trackId);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, trackingInfo.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            MoshtrakServiceDto sData = GetSDto(moshtrakInfo);

            IEnumerable<MoshtrakCompanyService> companyServices = MoshtrakService.GetMoshtrakCompanyServiceDto(sData);
            GeneralRequestDataOutputDto result = GetOutput(trackingInfo, moshtrakInfo, companyServices);


            return result;
        }
        private MoshtrakServiceDto GetSDto(MoshtrakOutputDto input)
        {
            return new MoshtrakServiceDto()
            {
                s0 = input.s0,
                s1 = input.s1,
                s2 = input.s2,
                s3 = input.s3,
                s4 = input.s4,
                s5 = input.s5,
                s8 = input.s8,
                s9 = input.s9,
                s10 = input.s10,
                s11 = input.s11,
                s12 = input.s12,
                s13 = input.s13,
                s14 = input.s14,
                s15 = input.s15,
                s16 = input.s16,
                s17 = input.s17,
                s18 = input.s18,
                s19 = input.s19,
                s20 = input.s20,
                s21 = input.s21,
                s22 = input.s22,
                s23 = input.s23,
                s24 = input.s24,
                s25 = input.s25,
                s26 = input.s26,
                s27 = input.s27,
                s28 = input.s28,
                s29 = input.s29,
                s30 = input.s30,
                s31 = input.s31,
                s32 = input.s32,
                s33 = input.s33,
                s34 = input.s34,
                s35 = input.s35,
                s36 = input.s36,
                s37 = input.s37,
                s38 = input.s38,
                s39 = input.s39,
                s40 = input.s40,
                s41 = input.s41,
                s42 = input.s42,
                s43 = input.s43,
                s44 = input.s44,
                s45 = input.s45,
                s46 = input.s46,
                s47 = input.s47,
                s48 = input.s48,
            };
        }
        private GeneralRequestDataOutputDto GetOutput(TrackingOutputDto trackingInfo, MoshtrakOutputDto moshtrakInfo, IEnumerable<MoshtrakCompanyService> companyServices)
        {
            return new GeneralRequestDataOutputDto()
            {
                TrackNumber = trackingInfo.TrackNumber,
                BillId = trackingInfo.BillId,
                NeighbourBillId = trackingInfo.NeighbourBillId,
                ZoneId = trackingInfo.ZoneId,
                ZoneTitle = trackingInfo.ZoneTitle,
                RegionId = trackingInfo.RegionId,
                RegionTitle = trackingInfo.RegionTitle,
                FirstName = moshtrakInfo.FirstName,
                Surname = moshtrakInfo.Surname,
                FatherName = moshtrakInfo.FatherName,
                NationalCode = moshtrakInfo.NationalCode,
                MobileNumber = moshtrakInfo.MobileNumber,
                PhoneNumber = moshtrakInfo.PhoneNumber,
                Caller = trackingInfo.Caller,
                NotificationMobile = moshtrakInfo.NotificationMobile,
                Address = moshtrakInfo.Address,
                CompanyServices = companyServices.ToList()
            };
        }
    }
}