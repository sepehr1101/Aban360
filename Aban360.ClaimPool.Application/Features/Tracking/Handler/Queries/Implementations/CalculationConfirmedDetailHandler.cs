using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations;
using Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.ClaimPool.Persistence.Features.Tracking.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Implementations
{
    internal sealed class CalculationConfirmedDetailHandler : ICalculationConfirmedDetailHandler
    {
        private readonly ITrackingDetailQueryService _trackingDetailQueryService;
        private readonly IValidator<TrackingDetailGetDto> _validator;
        public CalculationConfirmedDetailHandler(
            ITrackingDetailQueryService trackingDetailQueryService,
            IValidator<TrackingDetailGetDto> validator)
        {
            _trackingDetailQueryService = trackingDetailQueryService;
            _trackingDetailQueryService.NotNull(nameof(trackingDetailQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<CalculationConfirmedOutputDto> Handle(TrackingDetailGetDto inputDto, CancellationToken cancellationToken)
        {
            await Validation(inputDto, cancellationToken);
            CalculationConfirmedDto data = await _trackingDetailQueryService.GetCalculationConfirmed(inputDto);
            IEnumerable<NumericDictionary> s = MoshtrakService.GetServicesSelectedDto(GetMoshtrakServiceDto(data), data.ServiceGroupId);
            CalculationConfirmedOutputDto result = GetOutput(data, s);

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
        private CalculationConfirmedOutputDto GetOutput(CalculationConfirmedDto result, IEnumerable<Common.BaseEntities.NumericDictionary> s)
        {
            return new CalculationConfirmedOutputDto()
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
                NotificationNumber = result.NotificationMobile,
                Address = result.Address,
                UsageId = result.UsageId,
                UsageTitle = result.UsageTitle,
                Siphon100 = result.Siphon100,
                Siphon125 = result.Siphon125,
                Siphon150 = result.Siphon150,
                Siphon200 = result.Siphon200,
                Premises = result.Premises,
                ImprovementOverall = result.ImprovementOverall,
                ImprovementDomestic = result.ImprovementDomestic,
                ImprovementCommertial = result.ImprovementCommertial,
                CommertialUnit = result.CommertialUnit,
                DomesticUnit = result.DomesticUnit,
                OtherUnit = result.OtherUnit,
                FamilyCount = result.FamilyCount,
                HouseholdNumber = result.HouseholdNumber,
                DiscountTypeId = result.DiscountTypeId,
                DiscountTypeTitle = result.DiscountTypeTitle,
                DiscountCount = result.DiscountCount,
                HasBroker = result.HasBroker,
                ContractualCapacity = result.ContractualCapacity,
                BranchTypeId = result.BranchTypeId,
                BranchTypeTitle = result.BranchTypeTitle,
                RegionMultiplier = result.RegionMultiplier,
                MeterTypeId = result.MeterTypeId,
                MeterTypeTitle = result.MeterTypeTitle,
                MeterDiamterId = result.MeterDiamterId,
                MeterDiamterTitle = result.MeterDiamterTitle,
                PostalCode = result.PostalCode,
                Description = result.Description,
                CompanyServiceSelected = s.ToList(),

            };
        }
        private MoshtrakServiceDto GetMoshtrakServiceDto(CalculationConfirmedDto input)
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
    }
}
