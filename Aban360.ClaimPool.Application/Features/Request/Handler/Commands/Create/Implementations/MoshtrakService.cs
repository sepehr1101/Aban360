using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    public static class MoshtrakService
    {
        public static MoshtrakServiceDto GetServicesSelected(ICollection<int> servicesSelected)
        {
            return new MoshtrakServiceDto()//todo: s1,s2?
            {
                s0 = servicesSelected.Contains((int)CompanyServiceEnum.IsEnsheabAb) ? 1 : 0,
                s1 = servicesSelected.Contains((int)CompanyServiceEnum.IsSaleEnsheabFazelab) || servicesSelected.Contains((int)CompanyServiceEnum.IsAfterSaleEnsheabFazelab) ? 1 : 0,
                s2 = servicesSelected.Contains((int)CompanyServiceEnum.IsTaqirVahed) ? 1 : 0,
                //s3=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                s4 = servicesSelected.Contains((int)CompanyServiceEnum.IsTaqirNam) ? 1 : 0,
                s5 = servicesSelected.Contains((int)CompanyServiceEnum.IsTaqirQotrEnsheab) ? 1 : 0,
                //s8=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                //s9= s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                s10 = servicesSelected.Contains((int)CompanyServiceEnum.EstelamMahzar) ? 1 : 0,
                s11 = servicesSelected.Contains(0) ? 1 : 0,//تفکیک عرصه اب
                s12 = servicesSelected.Contains(0) ? 1 : 0,//تفکیک عرصه فاضلاب
                s13 = servicesSelected.Contains((int)CompanyServiceEnum.TaqirSathCounter) ? 1 : 0,
                //s14=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                //s15=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                s16 = servicesSelected.Contains((int)CompanyServiceEnum.IsTaqirKarbari) ? 1 : 0,
                //s17=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                //s18=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                //s19=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                s20 = servicesSelected.Contains((int)CompanyServiceEnum.JabejaiiKontor) ? 1 : 0,
                s21 = servicesSelected.Contains(0) ? 1 : 0,//خط انتقال اب
                s22 = servicesSelected.Contains(0) ? 1 : 0,//خط انتقال فاضلاب
                s23 = servicesSelected.Contains(0) ? 1 : 0,//سهم منبع اب
                s24 = servicesSelected.Contains((int)CompanyServiceEnum.TaqirQotrSifoon) ? 1 : 0,
                //s25=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                s26 = servicesSelected.Contains((int)CompanyServiceEnum.SaleIsAmadeSaziAb) || servicesSelected.Contains((int)CompanyServiceEnum.AfterSaleIsAmadeSaziAb) ? 1 : 0,
                s27 = servicesSelected.Contains((int)CompanyServiceEnum.SaleIsAmadeSaziFazelab) || servicesSelected.Contains((int)CompanyServiceEnum.AfterSaleIsAmadeSaziFazelab) ? 1 : 0,
                //s28=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                //s29=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                //s30=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                //s31=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                s32 = servicesSelected.Contains((int)CompanyServiceEnum.QatVaslEnsheab) ? 1 : 0,
                s33 = servicesSelected.Contains((int)CompanyServiceEnum.SifoonEzafe) ? 1 : 0,
                s34 = servicesSelected.Contains(0) ? 1 : 0,//عدم تخفیف آب00000
                s35 = servicesSelected.Contains(0) ? 1 : 0,//عدم تخفیف فاضلاب000
                s36 = servicesSelected.Contains((int)CompanyServiceEnum.JabejaiiSifoon) ? 1 : 0,
                s37 = servicesSelected.Contains((int)CompanyServiceEnum.SaleNezamMohandesi) || servicesSelected.Contains((int)CompanyServiceEnum.SaleNezamMohandesi) ? 1 : 0,
                s38 = servicesSelected.Contains((int)CompanyServiceEnum.TavizSifoon) ? 1 : 0,
                s39 = servicesSelected.Contains((int)CompanyServiceEnum.KhanevarShomari) ? 1 : 0,
                s40 = servicesSelected.Contains((int)CompanyServiceEnum.TajmiEdqam) ? 1 : 0,
                s41 = servicesSelected.Contains((int)CompanyServiceEnum.TavizKontor) ? 1 : 0,
                s42 = servicesSelected.Contains(0) ? 1 : 0,//لوله گذاری آب
                s43 = servicesSelected.Contains(0) ? 1 : 0,//لوله گذاری فاضلاب
                s44 = servicesSelected.Contains((int)CompanyServiceEnum.IsZarfiatQarardadi) ? 1 : 0,
                s45 = servicesSelected.Contains((int)CompanyServiceEnum.KontorMojaza) ? 1 : 0,
                s46 = servicesSelected.Contains((int)CompanyServiceEnum.TaqirTarefe) ? 1 : 0,
                s47 = servicesSelected.Contains((int)CompanyServiceEnum.Peymayesh) ? 1 : 0,
                s48 = servicesSelected.Contains((int)CompanyServiceEnum.Saier) ? 1 : 0,
            };
        }
        public static ICollection<int> GetServicesSelected(MoshtrakServiceDto inputDto, int serviceGroupId)
        {
            bool isSaleRequest = serviceGroupId == 1;
            ICollection<int> servicesSelected = new List<int>();//todo: s1,s2
            int fazelabId = isSaleRequest ? (int)CompanyServiceEnum.IsSaleEnsheabFazelab : (int)CompanyServiceEnum.IsAfterSaleEnsheabFazelab;
            int nezamMohandesiId = isSaleRequest ? (int)CompanyServiceEnum.SaleNezamMohandesi : (int)CompanyServiceEnum.AfterSaleNezamMohandesi;
            int amadesaziAbId = isSaleRequest ? (int)CompanyServiceEnum.SaleIsAmadeSaziAb : (int)CompanyServiceEnum.AfterSaleIsAmadeSaziAb;
            int amadesaziFazelabId = isSaleRequest ? (int)CompanyServiceEnum.SaleIsAmadeSaziFazelab : (int)CompanyServiceEnum.AfterSaleIsAmadeSaziFazelab;

            if (inputDto.s0 == 1) servicesSelected.Add((int)CompanyServiceEnum.IsEnsheabAb);
            if (inputDto.s1 == 1) servicesSelected.Add(fazelabId);
            if (inputDto.s2 == 1) servicesSelected.Add((int)CompanyServiceEnum.IsTaqirVahed);
            if (inputDto.s4 == 1) servicesSelected.Add((int)CompanyServiceEnum.IsTaqirNam);
            if (inputDto.s5 == 1) servicesSelected.Add((int)CompanyServiceEnum.IsTaqirQotrEnsheab);
            if (inputDto.s10 == 1) servicesSelected.Add((int)CompanyServiceEnum.EstelamMahzar);
            if (inputDto.s11 == 1) servicesSelected.Add((int)CompanyServiceEnum.TafkikArseAb);
            if (inputDto.s12 == 1) servicesSelected.Add((int)CompanyServiceEnum.IsTafkikArseFazelab);
            if (inputDto.s13 == 1) servicesSelected.Add((int)CompanyServiceEnum.TaqirSathCounter);
            if (inputDto.s16 == 1) servicesSelected.Add((int)CompanyServiceEnum.IsTaqirKarbari);
            if (inputDto.s20 == 1) servicesSelected.Add((int)CompanyServiceEnum.JabejaiiKontor);
            if (inputDto.s21 == 1) servicesSelected.Add((int)CompanyServiceEnum.KhatEnteqhalAb);
            if (inputDto.s22 == 1) servicesSelected.Add((int)CompanyServiceEnum.KhatEnteqhalFazelab);
            if (inputDto.s23 == 1) servicesSelected.Add((int)CompanyServiceEnum.SahmManbaAb);
            if (inputDto.s24 == 1) servicesSelected.Add((int)CompanyServiceEnum.TaqirQotrSifoon);
            if (inputDto.s26 == 1) servicesSelected.Add(amadesaziAbId);
            if (inputDto.s27 == 1) servicesSelected.Add(amadesaziFazelabId);
            if (inputDto.s32 == 1) servicesSelected.Add((int)CompanyServiceEnum.QatVaslEnsheab);
            if (inputDto.s33 == 1) servicesSelected.Add((int)CompanyServiceEnum.SifoonEzafe);
            if (inputDto.s34 == 1) servicesSelected.Add((int)CompanyServiceEnum.AdamTakhfifAb);
            if (inputDto.s35 == 1) servicesSelected.Add((int)CompanyServiceEnum.AdamTakhfifFazelab);
            if (inputDto.s36 == 1) servicesSelected.Add((int)CompanyServiceEnum.JabejaiiSifoon);
            if (inputDto.s37 == 1) servicesSelected.Add(nezamMohandesiId);
            if (inputDto.s38 == 1) servicesSelected.Add((int)CompanyServiceEnum.TavizSifoon);
            if (inputDto.s39 == 1) servicesSelected.Add((int)CompanyServiceEnum.KhanevarShomari);
            if (inputDto.s40 == 1) servicesSelected.Add((int)CompanyServiceEnum.TajmiEdqam);
            if (inputDto.s41 == 1) servicesSelected.Add((int)CompanyServiceEnum.TavizKontor);
            if (inputDto.s42 == 1) servicesSelected.Add((int)CompanyServiceEnum.LooleGozariAb);
            if (inputDto.s43 == 1) servicesSelected.Add((int)CompanyServiceEnum.LooleGozareAbFazelab);
            if (inputDto.s44 == 1) servicesSelected.Add((int)CompanyServiceEnum.IsZarfiatQarardadi);
            if (inputDto.s45 == 1) servicesSelected.Add((int)CompanyServiceEnum.KontorMojaza);
            if (inputDto.s46 == 1) servicesSelected.Add((int)CompanyServiceEnum.TaqirTarefe);
            if (inputDto.s47 == 1) servicesSelected.Add((int)CompanyServiceEnum.Peymayesh);
            if (inputDto.s48 == 1) servicesSelected.Add((int)CompanyServiceEnum.Saier);

            return servicesSelected;
        }
        public static IEnumerable<SelectionDto> GetMoshtrakCompanyServiceDto(MoshtrakServiceDto input, int serviceGroupId)
        {
            bool isSaleRequest = serviceGroupId == 1;
            ICollection<SelectionDto> companyService = new List<SelectionDto>();
            int fazelabId = isSaleRequest ? (int)CompanyServiceEnum.IsSaleEnsheabFazelab : (int)CompanyServiceEnum.IsAfterSaleEnsheabFazelab;
            int nezamMohandesiId = isSaleRequest ? (int)CompanyServiceEnum.SaleNezamMohandesi : (int)CompanyServiceEnum.AfterSaleNezamMohandesi;
            int amadesaziAbId = isSaleRequest ? (int)CompanyServiceEnum.SaleIsAmadeSaziAb : (int)CompanyServiceEnum.AfterSaleIsAmadeSaziAb;
            int amadesaziFazelabId = isSaleRequest ? (int)CompanyServiceEnum.SaleIsAmadeSaziFazelab : (int)CompanyServiceEnum.AfterSaleIsAmadeSaziFazelab;

            companyService.Add(new SelectionDto((int)CompanyServiceEnum.IsEnsheabAb, CompanySeviceLiterals.IsEnsheabAb, input.HasEnsheabAb));
            companyService.Add(new SelectionDto(fazelabId, CompanySeviceLiterals.IsEnsheabFazelab, input.HasEnsheabFazelab));
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.IsTaqirVahed, CompanySeviceLiterals.IsTaqirVahed, input.HasTaqirVahed));
            //s3
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.IsTaqirNam, CompanySeviceLiterals.IsTaqirNam, input.HasTaqirName));
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.IsTaqirQotrEnsheab, CompanySeviceLiterals.IsTaqirQotrEnsheab, input.HasTaqirQotrEnsheab));
            //s8 , s9
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.EstelamMahzar, CompanySeviceLiterals.EstelamMahzar, input.HasEstelamMahzar));
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.TafkikArseAb, CompanySeviceLiterals.TafkikArseAb, input.HasTafkikArseAb));
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.IsTafkikArseFazelab, CompanySeviceLiterals.IsTafkikArseFazelab, input.HasTafkikArseFazelab));
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.TaqirSathCounter, CompanySeviceLiterals.TaqirSathCounter, input.HasTaqirSathCounter));
            //s14 , s15
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.IsTaqirKarbari, CompanySeviceLiterals.IsTaqirKarbari, input.HasTaqirKarbari));
            //s17 , s18 , s19
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.JabejaiiKontor, CompanySeviceLiterals.JabejaiiKontor, input.HasJabejaiiKontor));
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.KhatEnteqhalAb, CompanySeviceLiterals.KhatEnteqhalAb, input.HasKhatEnteghalAb));
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.KhatEnteqhalFazelab, CompanySeviceLiterals.KhatEnteqhalFazelab, input.HasKhatEnteghalFazelab));
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.SahmManbaAb, CompanySeviceLiterals.SahmManbaAb, input.HasSahmManbaAb));
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.TaqirQotrSifoon, CompanySeviceLiterals.TaqirQotrSifoon, input.HasTaqirQotrSifoon));
            //s25
            companyService.Add(new SelectionDto(amadesaziAbId, CompanySeviceLiterals.IsAmadeSaziAb, input.HasAmadeSaziAb));
            companyService.Add(new SelectionDto(amadesaziFazelabId, CompanySeviceLiterals.IsAmadeSaziFazelab, input.HazAmadeSaziFazelab));
            //s28 , s29 , s30 , s31
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.QatVaslEnsheab, CompanySeviceLiterals.QatVaslEnsheab, input.HasQatVaslEnsheab));
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.SifoonEzafe, CompanySeviceLiterals.SifoonEzafe, input.HasSifoonEzafe));
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.AdamTakhfifAb, CompanySeviceLiterals.AdamTakhfifAb, input.HasAdamTakhfifAb));
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.AdamTakhfifFazelab, CompanySeviceLiterals.AdamTakhfifFazelab, input.HasAdamTakhfifFazelab));
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.JabejaiiSifoon, CompanySeviceLiterals.JabejaiiSifoon, input.HasJabejaiiSifoon));
            companyService.Add(new SelectionDto(nezamMohandesiId, CompanySeviceLiterals.NezamMohandesi, input.HasNezamMohandesi));
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.TavizSifoon, CompanySeviceLiterals.TavizSifoon, input.HasTavizSifoon));
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.KhanevarShomari, CompanySeviceLiterals.KhanevarShomari, input.HasKhanevarShomari));
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.TajmiEdqam, CompanySeviceLiterals.TajmiEdqam, input.HasTafkikEdqam));
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.TavizKontor, CompanySeviceLiterals.TavizKontor, input.HasTavizKontor));
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.LooleGozariAb, CompanySeviceLiterals.LooleGozariAb, input.HasLooleGozareAb));
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.IsZarfiatQarardadi, CompanySeviceLiterals.IsZarfiatQarardadi, input.HasZarfiatQarardadi));
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.KontorMojaza, CompanySeviceLiterals.KontorMojaza, input.HasKontorMojaza));
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.TaqirTarefe, CompanySeviceLiterals.TaqirTarefe, input.HasTaqirTarefe));
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.Peymayesh, CompanySeviceLiterals.Peymayesh, input.HasPeymayesh));
            companyService.Add(new SelectionDto((int)CompanyServiceEnum.Saier, CompanySeviceLiterals.Saier, input.HasSaier));

            return companyService;
        }
        public static IEnumerable<NumericDictionary> GetServicesSelectedDto(MoshtrakServiceDto input, int serviceGroupId)
        {
            bool isSaleRequest = serviceGroupId == 1;
            ICollection<NumericDictionary> companyServiceSelected = new List<NumericDictionary>();
            int fazelabId = isSaleRequest ? (int)CompanyServiceEnum.IsSaleEnsheabFazelab : (int)CompanyServiceEnum.IsAfterSaleEnsheabFazelab;
            int nezamMohandesiId = isSaleRequest ? (int)CompanyServiceEnum.SaleNezamMohandesi : (int)CompanyServiceEnum.AfterSaleNezamMohandesi;
            int amadesaziAbId = isSaleRequest ? (int)CompanyServiceEnum.SaleIsAmadeSaziAb : (int)CompanyServiceEnum.AfterSaleIsAmadeSaziAb;
            int amadesaziFazelabId = isSaleRequest ? (int)CompanyServiceEnum.SaleIsAmadeSaziFazelab : (int)CompanyServiceEnum.AfterSaleIsAmadeSaziFazelab;

            if (input.HasEnsheabAb)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.IsEnsheabAb, CompanySeviceLiterals.IsEnsheabAb));

            if (input.HasEnsheabFazelab)
                companyServiceSelected.Add(new NumericDictionary(fazelabId, CompanySeviceLiterals.IsEnsheabFazelab));

            if (input.HasTaqirVahed)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.IsTaqirVahed, CompanySeviceLiterals.IsTaqirVahed));

            //s3

            if (input.HasTaqirName)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.IsTaqirNam, CompanySeviceLiterals.IsTaqirNam));

            if (input.HasTaqirQotrEnsheab)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.IsTaqirQotrEnsheab, CompanySeviceLiterals.IsTaqirQotrEnsheab));

            //s8 , s9

            if (input.HasEstelamMahzar)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.EstelamMahzar, CompanySeviceLiterals.EstelamMahzar));

            if (input.HasTafkikArseAb)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.TafkikArseAb, CompanySeviceLiterals.TafkikArseAb));

            if (input.HasTafkikArseFazelab)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.IsTafkikArseFazelab, CompanySeviceLiterals.IsTafkikArseFazelab));

            if (input.HasTaqirSathCounter)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.TaqirSathCounter, CompanySeviceLiterals.TaqirSathCounter));

            //s14 , s15

            if (input.HasTaqirKarbari)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.IsTaqirKarbari, CompanySeviceLiterals.IsTaqirKarbari));

            //s17 , s18 , s19

            if (input.HasJabejaiiKontor)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.JabejaiiKontor, CompanySeviceLiterals.JabejaiiKontor));

            if (input.HasKhatEnteghalAb)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.KhatEnteqhalAb, CompanySeviceLiterals.KhatEnteqhalAb));

            if (input.HasKhatEnteghalFazelab)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.KhatEnteqhalFazelab, CompanySeviceLiterals.KhatEnteqhalFazelab));

            if (input.HasSahmManbaAb)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.SahmManbaAb, CompanySeviceLiterals.SahmManbaAb));

            if (input.HasTaqirQotrSifoon)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.TaqirQotrSifoon, CompanySeviceLiterals.TaqirQotrSifoon));

            //s25

            if (input.HasAmadeSaziAb)
                companyServiceSelected.Add(new NumericDictionary(amadesaziAbId, CompanySeviceLiterals.IsAmadeSaziAb));

            if (input.HazAmadeSaziFazelab)
                companyServiceSelected.Add(new NumericDictionary(amadesaziFazelabId, CompanySeviceLiterals.IsAmadeSaziFazelab));

            //s28 , s29 , s30 , s31

            if (input.HasQatVaslEnsheab)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.QatVaslEnsheab, CompanySeviceLiterals.QatVaslEnsheab));

            if (input.HasSifoonEzafe)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.SifoonEzafe, CompanySeviceLiterals.SifoonEzafe));

            if (input.HasAdamTakhfifAb)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.AdamTakhfifAb, CompanySeviceLiterals.AdamTakhfifAb));

            if (input.HasAdamTakhfifFazelab)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.AdamTakhfifFazelab, CompanySeviceLiterals.AdamTakhfifFazelab));

            if (input.HasJabejaiiSifoon)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.JabejaiiSifoon, CompanySeviceLiterals.JabejaiiSifoon));

            if (input.HasNezamMohandesi)
                companyServiceSelected.Add(new NumericDictionary(nezamMohandesiId, CompanySeviceLiterals.NezamMohandesi));

            if (input.HasTavizSifoon)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.TavizSifoon, CompanySeviceLiterals.TavizSifoon));

            if (input.HasKhanevarShomari)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.KhanevarShomari, CompanySeviceLiterals.KhanevarShomari));

            if (input.HasTafkikEdqam)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.TajmiEdqam, CompanySeviceLiterals.TajmiEdqam));

            if (input.HasTavizKontor)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.TavizKontor, CompanySeviceLiterals.TavizKontor));

            if (input.HasLooleGozareAb)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.LooleGozariAb, CompanySeviceLiterals.LooleGozariAb));

            if (input.HasLooleGozareAbFazelab)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.LooleGozareAbFazelab, CompanySeviceLiterals.LooleGozareAbFazelab));

            if (input.HasZarfiatQarardadi)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.IsZarfiatQarardadi, CompanySeviceLiterals.IsZarfiatQarardadi));

            if (input.HasKontorMojaza)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.KontorMojaza, CompanySeviceLiterals.KontorMojaza));

            if (input.HasTaqirTarefe)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.TaqirTarefe, CompanySeviceLiterals.TaqirTarefe));

            if (input.HasPeymayesh)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.Peymayesh, CompanySeviceLiterals.Peymayesh));

            if (input.HasSaier)
                companyServiceSelected.Add(new NumericDictionary((int)CompanyServiceEnum.Saier, CompanySeviceLiterals.Saier));

            return companyServiceSelected;
        }

    }
}
