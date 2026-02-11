using Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.ClaimPool.Persistence.Constants.Literals;
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
            IEnumerable<NumericDictionary> s = GetCompanyService(data);
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
        private IEnumerable<Common.BaseEntities.NumericDictionary> GetCompanyService(CalculationConfirmedDto input)
        {
            ICollection<Common.BaseEntities.NumericDictionary> companyServiceSelected = new List<Common.BaseEntities.NumericDictionary>();

            #region s
            if (input.s0 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.IsEnsheabAb, CompanySeviceLiterals.IsEnsheabAb));

            if (input.s1 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.IsEnsheabFazelab, CompanySeviceLiterals.IsEnsheabFazelab));

            if (input.s2 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.IsTaqirVahed, CompanySeviceLiterals.IsTaqirVahed));

            //if (input.s3 > 0)
            //companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.IsTaqirVahed, CompanySeviceLiterals.IsTaqirVahed));

            if (input.s4 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.IsTaqirNam, CompanySeviceLiterals.IsTaqirNam));

            if (input.s5 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.IsTaqirQotrEnsheab, CompanySeviceLiterals.IsTaqirQotrEnsheab));

            //if (input.s8 > 0)
            //companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.SifoonEzafe, CompanySeviceLiterals.SifoonEzafe));

            //if (input.s9 > 0)
            //companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.JabejaiiKontor, CompanySeviceLiterals.JabejaiiKontor));

            if (input.s10 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.EstelamMahzar, CompanySeviceLiterals.EstelamMahzar));

            if (input.s11 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.TafkikArseAb, CompanySeviceLiterals.TafkikArseAb));

            if (input.s12 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.IsTafkikArseFazelab, CompanySeviceLiterals.IsTafkikArseFazelab));

            if (input.s13 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.TaqirSathCounter, CompanySeviceLiterals.TaqirSathCounter));

            //if (input.s14 > 0)
            //companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.JabejaiiSifoon, CompanySeviceLiterals.JabejaiiSifoon));

            //if (input.s15 > 0)
            //companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.NezamMohandesi, CompanySeviceLiterals.NezamMohandesi));

            if (input.s16 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.IsTaqirKarbari, CompanySeviceLiterals.IsTaqirKarbari));

            //if (input.s17 > 0)
            //companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.IsZarfiatQarardadi, CompanySeviceLiterals.IsZarfiatQarardadi));

            //if (input.s18 > 0)
            //companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.IsAmadeSaziAb, CompanySeviceLiterals.IsAmadeSaziAb));

            //if (input.s19 > 0)
            //companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.IsAmadeSaziFazelab, CompanySeviceLiterals.IsAmadeSaziFazelab));

            if (input.s20 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.JabejaiiKontor, CompanySeviceLiterals.JabejaiiKontor));

            if (input.s21 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.KhatEnteqhalAb, CompanySeviceLiterals.KhatEnteqhalAb));

            if (input.s22 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.KhatEnteqhalFazelab, CompanySeviceLiterals.KhatEnteqhalFazelab));

            if (input.s23 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.SahmManbaAb, CompanySeviceLiterals.SahmManbaAb));

            if (input.s24 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.TaqirQotrSifoon, CompanySeviceLiterals.TaqirQotrSifoon));

            //if (input.s25 > 0)
            //    companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum., CompanySeviceLiterals.));

            if (input.s26 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.IsAmadeSaziAb, CompanySeviceLiterals.IsAmadeSaziAb));

            if (input.s27 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.IsAmadeSaziFazelab, CompanySeviceLiterals.IsAmadeSaziFazelab));

            //if (input.s28 > 0)
            //companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum., CompanySeviceLiterals.));

            //if (input.s29 > 0)
            //companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum., CompanySeviceLiterals.));

            //if (input.s30 > 0)
            //companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum., CompanySeviceLiterals.));

            //if (input.s31 > 0)
            //companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum., CompanySeviceLiterals.));

            if (input.s32 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.QatVaslEnsheab, CompanySeviceLiterals.QatVaslEnsheab));

            if (input.s33 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.SifoonEzafe, CompanySeviceLiterals.SifoonEzafe));

            if (input.s34 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.AdamTakhfifAb, CompanySeviceLiterals.AdamTakhfifAb));

            if (input.s35 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.AdamTakhfifFazelab, CompanySeviceLiterals.AdamTakhfifFazelab));

            if (input.s36 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.JabejaiiSifoon, CompanySeviceLiterals.JabejaiiSifoon));

            if (input.s37 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.NezamMohandesi, CompanySeviceLiterals.NezamMohandesi));

            if (input.s38 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.TavizSifoon, CompanySeviceLiterals.TavizSifoon));

            if (input.s39 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.KhanevarShomari, CompanySeviceLiterals.KhanevarShomari));

            if (input.s40 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.TafkikEdqam, CompanySeviceLiterals.TafkikEdqam));

            if (input.s41 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.TavizKontor, CompanySeviceLiterals.TavizKontor));

            if (input.s42 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.LooleGozariAb, CompanySeviceLiterals.LooleGozariAb));

            if (input.s43 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.LooleGozareAbFazelab, CompanySeviceLiterals.LooleGozareAbFazelab));

            if (input.s44 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.IsZarfiatQarardadi, CompanySeviceLiterals.IsZarfiatQarardadi));

            if (input.s45 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.KontorMojaza, CompanySeviceLiterals.KontorMojaza));

            if (input.s46 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.TaqirTarefe, CompanySeviceLiterals.TaqirTarefe));

            if (input.s47 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.Peymayesh, CompanySeviceLiterals.Peymayesh));

            if (input.s48 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)Domain.Constants.CompanyServiceEnum.Saier, CompanySeviceLiterals.Saier));
            #endregion

            return companyServiceSelected;
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
                NotificationMobile = result.NotificationMobile,
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
                Description=result.Description,
                CompanyServiceSelected = s.ToList()
            };
        }
    }
}
