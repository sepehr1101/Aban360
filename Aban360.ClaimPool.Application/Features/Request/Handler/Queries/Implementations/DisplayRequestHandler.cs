using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class DisplayRequestHandler : IDisplayRequestHandler
    {
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly IValidator<ZoneIdAndTrackNumber> _validator;
        public DisplayRequestHandler(
            IMoshtrakQueryService moshtrakQueryService,
            IValidator<ZoneIdAndTrackNumber> validator)
        {
            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<MoshtrakDataOutputDto> Handle(ZoneIdAndTrackNumber inputDto, CancellationToken cancellationToken)
        {
            await Validation(inputDto, cancellationToken);

            MoshtrakGetDto moshtrackSearch = new(inputDto.ZoneId, null, null, inputDto.TrackNumber);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(moshtrackSearch, MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();

            MoshtrakServiceDto sData = GetSDto(moshtrakInfo);
            IEnumerable<MoshtrakCompanyService> companyServices = MoshtrakService.GetMoshtrakCompanyServiceDto(sData);

            MoshtrakDataOutputDto moshtrakData = GetMoshtrakData(moshtrakInfo, companyServices);
            return moshtrakData;
        }
        private async Task Validation(ZoneIdAndTrackNumber inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
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
        private MoshtrakDataOutputDto GetMoshtrakData(MoshtrakOutputDto input, IEnumerable<MoshtrakCompanyService> companyServices)
        {
            return new MoshtrakDataOutputDto()
            {
                Id=input.Id,
                ZoneId = input.ZoneId,
                ZoneTitle = input.ZoneTitle,
                CustomerNumber = input.CustomerNumber,
                ReadingNumber = input.ReadingNumber,
                FirstName = input.FirstName,
                Surname = input.Surname,
                FatherName = input.FatherName,
                NationalCode = input.NationalCode,
                PhoneNumber = input.PhoneNumber,
                MobileNumber = input.MobileNumber,
                RequestDateJalali = input.RequestDateJalali,
                Address = input.Address,
                PostalCode = input.PostalCode,
                NeighbourBillId = input.NeighbourBillId,
                TrackNumber = input.TrackNumber,
                UsageId = input.UsageId,
                UsageTitle = input.UsageTitle,
                IsRegistered = input.IsRegistered,
                BranchTypeId = input.BranchTypeId,
                BranchTypeTitle = input.BranchTypeTitle,
                Premises = input.Premises,
                ImprovementOverall = input.ImprovementOverall,
                ImprovementDomestic = input.ImprovementDomestic,
                ImprovementCommercial = input.ImprovementCommercial,
                OtherUnit = input.OtherUnit,
                DomesticUnit = input.DomesticUnit,
                CommercialUnit = input.CommercialUnit,
                ContractualCapacity = input.ContractualCapacity,
                Siphon100 = input.Siphon100,
                Siphon125 = input.Siphon125,
                Siphon150 = input.Siphon150,
                Siphon200 = input.Siphon200,
                MainSiphon = input.MainSiphon,
                CommonSiphon = input.CommonSiphon,
                MeterDiameterTitle = input.MeterDiameterTitle,
                MeterDiameterId = input.MeterDiameterId,
                DiscountTypeId = input.DiscountTypeId,
                DiscountTypeTitle = input.DiscountTypeTitle,
                DiscountCount = input.DiscountCount,
                IsSpecial = input.IsSpecial,
                CounterType = input.CounterType,
                NotificationNumber = input.NotificationMobile,
                Description = input.Description,
                HouseValue = input.HouseValue,
                IsNonPermanent = input.IsNonPermanent,
                BlockId = input.BlockId,
                BrokerId = input.BrokerId,
                CompanyServiceItems = companyServices

            };
        }

    }
}
