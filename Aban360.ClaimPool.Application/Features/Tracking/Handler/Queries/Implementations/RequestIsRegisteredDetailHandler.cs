using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations;
using Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Tracking.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Implementations
{
    internal sealed class RequestIsRegisteredDetailHandler : IRequestIsRegisteredDetailHandler
    {
        private readonly ITrackingDetailQueryService _trackingDetailQueryService;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IValidator<TrackingDetailGetDto> _validator;
        public RequestIsRegisteredDetailHandler(
            ITrackingDetailQueryService trackingDetailQueryService,
            ITrackingQueryService trackingQueryService,
            IValidator<TrackingDetailGetDto> validator)
        {
            _trackingDetailQueryService = trackingDetailQueryService;
            _trackingDetailQueryService.NotNull(nameof(trackingDetailQueryService));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<RequestIsRegisterdOutputDto> Handle(TrackingDetailGetDto inputDto, CancellationToken cancellationToken)
        {
            await Validation(inputDto, cancellationToken);
            RequestIsRegisterdDto data = await _trackingDetailQueryService.GetRequestIsRegistered(inputDto);
            TrackingOutputDto latestTrackingInfo = await _trackingQueryService.GetLatest(data.TrackNumber);
            data.BillId = latestTrackingInfo.BillId ?? string.Empty;

            MoshtrakServiceDto sData = GetSDto(data);

            IEnumerable<NumericDictionary> s = MoshtrakService.GetServicesSelectedDto(sData);
            RequestIsRegisterdOutputDto result = GetOutput(data, s);


            return result;
        }
        private async Task Validation(TrackingDetailGetDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BaseException(message);
            }
        }
        private MoshtrakServiceDto GetSDto(RequestIsRegisterdDto input)
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
        private RequestIsRegisterdOutputDto GetOutput(RequestIsRegisterdDto result, IEnumerable<Common.BaseEntities.NumericDictionary> s)
        {
            return new RequestIsRegisterdOutputDto()
            {
                TrackNumber = result.TrackNumber,
                BillId = result.BillId,
                NeighbourBillId = result.NeighbourBillId,
                ZoneId = result.ZoneId,
                ZoneTitle = result.ZoneTitle,
                RegionId = result.RegionId,
                RegionTitle = result.RegionTitle,
                FirstName = result.FirstName,
                Surname = result.Surname,
                FatherName = result.FatherName,
                NationalCode = result.NationalCode,
                MobileNumber = result.MobileNumber,
                PhoneNumber = result.PhoneNumber,
                Caller = result.Caller,
                NotificationMobile = result.NotificationMobile,
                Address = result.Address,
                CompanyServiceSelected = s.ToList()
            };
        }
    }
}
