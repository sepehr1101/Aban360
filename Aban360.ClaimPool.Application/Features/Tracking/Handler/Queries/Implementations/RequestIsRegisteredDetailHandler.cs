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
    internal sealed class RequestIsRegisteredDetailHandler : IRequestIsRegisteredDetailHandler
    {
        private readonly ITrackingDetailQueryService _trackingDetailQueryService;
        private readonly IValidator<TrackingDetailGetDto> _validator;
        public RequestIsRegisteredDetailHandler(
            ITrackingDetailQueryService trackingDetailQueryService,
            IValidator<TrackingDetailGetDto> validator)
        {
            _trackingDetailQueryService = trackingDetailQueryService;
            _trackingDetailQueryService.NotNull(nameof(trackingDetailQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<RequestIsRegisterdOutputDto> Handle(TrackingDetailGetDto inputDto, CancellationToken cancellationToken)
        {
            await Validation(inputDto, cancellationToken);
            RequestIsRegisterdDto data = await _trackingDetailQueryService.GetRequestIsRegistered(inputDto);
            IEnumerable<NumericDictionary> s = GetCompanyService(data);
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
        private IEnumerable<Common.BaseEntities.NumericDictionary> GetCompanyService(RequestIsRegisterdDto input)
        {
            ICollection<Common.BaseEntities.NumericDictionary> companyServiceSelected = new List<Common.BaseEntities.NumericDictionary>();

            #region s
            if (input.s0 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.IsEnsheabAb, CompanySeviceLiterals.IsEnsheabAb));

            if (input.s1 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.IsEnsheabFazelab, CompanySeviceLiterals.IsEnsheabFazelab));

            if (input.s2 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.IsTaqirVahed, CompanySeviceLiterals.IsTaqirVahed));

            //if (input.s3 > 0)
            //companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.IsTaqirVahed, CompanySeviceLiterals.IsTaqirVahed));

            if (input.s4 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.IsTaqirNam, CompanySeviceLiterals.IsTaqirNam));

            if (input.s5 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.IsTaqirQotrEnsheab, CompanySeviceLiterals.IsTaqirQotrEnsheab));

            //if (input.s8 > 0)
            //companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.SifoonEzafe, CompanySeviceLiterals.SifoonEzafe));

            //if (input.s9 > 0)
            //companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.JabejaiiKontor, CompanySeviceLiterals.JabejaiiKontor));

            if (input.s10 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.EstelamMahzar, CompanySeviceLiterals.EstelamMahzar));

            if (input.s11 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.TafkikArseAb, CompanySeviceLiterals.TafkikArseAb));

            if (input.s12 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.IsTafkikArseFazelab, CompanySeviceLiterals.IsTafkikArseFazelab));

            if (input.s13 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.TaqirSathCounter, CompanySeviceLiterals.TaqirSathCounter));

            //if (input.s14 > 0)
            //companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.JabejaiiSifoon, CompanySeviceLiterals.JabejaiiSifoon));

            //if (input.s15 > 0)
            //companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.NezamMohandesi, CompanySeviceLiterals.NezamMohandesi));

            if (input.s16 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.IsTaqirKarbari, CompanySeviceLiterals.IsTaqirKarbari));

            //if (input.s17 > 0)
            //companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.IsZarfiatQarardadi, CompanySeviceLiterals.IsZarfiatQarardadi));

            //if (input.s18 > 0)
            //companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.IsAmadeSaziAb, CompanySeviceLiterals.IsAmadeSaziAb));

            //if (input.s19 > 0)
            //companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.IsAmadeSaziFazelab, CompanySeviceLiterals.IsAmadeSaziFazelab));

            if (input.s20 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.JabejaiiKontor, CompanySeviceLiterals.JabejaiiKontor));

            if (input.s21 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.KhatEnteqhalAb, CompanySeviceLiterals.KhatEnteqhalAb));

            if (input.s22 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.KhatEnteqhalFazelab, CompanySeviceLiterals.KhatEnteqhalFazelab));

            if (input.s23 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.SahmManbaAb, CompanySeviceLiterals.SahmManbaAb));

            if (input.s24 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.TaqirQotrSifoon, CompanySeviceLiterals.TaqirQotrSifoon));

            //if (input.s25 > 0)
            //    companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum., CompanySeviceLiterals.));

            if (input.s26 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.IsAmadeSaziAb, CompanySeviceLiterals.IsAmadeSaziAb));

            if (input.s27 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.IsAmadeSaziFazelab, CompanySeviceLiterals.IsAmadeSaziFazelab));

            //if (input.s28 > 0)
            //companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum., CompanySeviceLiterals.));

            //if (input.s29 > 0)
            //companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum., CompanySeviceLiterals.));

            //if (input.s30 > 0)
            //companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum., CompanySeviceLiterals.));

            //if (input.s31 > 0)
            //companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum., CompanySeviceLiterals.));

            if (input.s32 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.QatVaslEnsheab, CompanySeviceLiterals.QatVaslEnsheab));

            if (input.s33 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.SifoonEzafe, CompanySeviceLiterals.SifoonEzafe));

            if (input.s34 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.AdamTakhfifAb, CompanySeviceLiterals.AdamTakhfifAb));

            if (input.s35 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.AdamTakhfifFazelab, CompanySeviceLiterals.AdamTakhfifFazelab));

            if (input.s36 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.JabejaiiSifoon, CompanySeviceLiterals.JabejaiiSifoon));

            if (input.s37 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.NezamMohandesi, CompanySeviceLiterals.NezamMohandesi));

            if (input.s38 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.TavizSifoon, CompanySeviceLiterals.TavizSifoon));

            if (input.s39 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.KhanevarShomari, CompanySeviceLiterals.KhanevarShomari));

            if (input.s40 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.TafkikEdqam, CompanySeviceLiterals.TafkikEdqam));

            if (input.s41 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.TavizKontor, CompanySeviceLiterals.TavizKontor));

            if (input.s42 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.LooleGozariAb, CompanySeviceLiterals.LooleGozariAb));

            if (input.s43 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.LooleGozareAbFazelab, CompanySeviceLiterals.LooleGozareAbFazelab));

            if (input.s44 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.IsZarfiatQarardadi, CompanySeviceLiterals.IsZarfiatQarardadi));

            if (input.s45 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.KontorMojaza, CompanySeviceLiterals.KontorMojaza));

            if (input.s46 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.TaqirTarefe, CompanySeviceLiterals.TaqirTarefe));

            if (input.s47 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.Peymayesh, CompanySeviceLiterals.Peymayesh));

            if (input.s48 > 0)
                companyServiceSelected.Add(new Common.BaseEntities.NumericDictionary((int)CompanyServiceEnum.Saier, CompanySeviceLiterals.Saier));
            #endregion

            return companyServiceSelected;
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
