using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    public static class MoshtrakService
    {
        public static MoshtrakServiceDto GetServicesSelected(ICollection<int> servicesSelected)
        {
            return new MoshtrakServiceDto()//todo: s1,s2?
            {
                s0 = servicesSelected.Contains((int)CompanyServiceEnum.IsEnsheabAb) ? 1 : 0,
                s1 = servicesSelected.Contains((int)CompanyServiceEnum.IsEnsheabFazelab) ? 1 : 0,
                s2 = servicesSelected.Contains((int)CompanyServiceEnum.IsEnsheabFazelab) ? 1 : 0,
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
                s26 = servicesSelected.Contains((int)CompanyServiceEnum.IsAmadeSaziAb) ? 1 : 0,
                s27 = servicesSelected.Contains((int)CompanyServiceEnum.IsAmadeSaziFazelab) ? 1 : 0,
                //s28=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                //s29=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                //s30=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                //s31=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                s32 = servicesSelected.Contains((int)CompanyServiceEnum.QatVaslEnsheab) ? 1 : 0,
                s33 = servicesSelected.Contains((int)CompanyServiceEnum.SifoonEzafe) ? 1 : 0,
                s34 = servicesSelected.Contains(0) ? 1 : 0,//عدم تخفیف آب00000
                s35 = servicesSelected.Contains(0) ? 1 : 0,//عدم تخفیف فاضلاب000
                s36 = servicesSelected.Contains((int)CompanyServiceEnum.JabejaiiSifoon) ? 1 : 0,
                s37 = servicesSelected.Contains((int)CompanyServiceEnum.NezamMohandesi) ? 1 : 0,
                s38 = servicesSelected.Contains((int)CompanyServiceEnum.TavizSifoon) ? 1 : 0,
                s39 = servicesSelected.Contains((int)CompanyServiceEnum.KhanevarShomari) ? 1 : 0,
                s40 = servicesSelected.Contains((int)CompanyServiceEnum.TafkikEdqam) ? 1 : 0,
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
        public static ICollection<int> GetServicesSelected(MoshtrakServiceDto inputDto)
        {
            ICollection<int> servicesSelected = new List<int>();//todo: s1,s2

            if (inputDto.s0 == 1) servicesSelected.Add(101);// CompanyServiceEnum.IsEnsheabAb,101
            if (inputDto.s1 == 1) servicesSelected.Add(201);// CompanyServiceEnum.IsEnsheabFazelab, 201
            if (inputDto.s2 == 1) servicesSelected.Add(201);// CompanyServiceEnum.IsEnsheabFazelab, 201
            if (inputDto.s4 == 1) servicesSelected.Add(301);// CompanyServiceEnum.IsTaqirNam,301
            if (inputDto.s5 == 1) servicesSelected.Add(302);// CompanyServiceEnum.IsTaqirQotrEnsheab,302
            if (inputDto.s10 == 1) servicesSelected.Add(300);// CompanyServiceEnum.EstelamMahzar,300
            if (inputDto.s11 == 1) servicesSelected.Add(105);//تفکیک عرصه اب   105
            if (inputDto.s12 == 1) servicesSelected.Add(205);//تفکیک عرصه فاضلاب  205
            if (inputDto.s13 == 1) servicesSelected.Add(304);// CompanyServiceEnum.TaqirSathCounter,304
            if (inputDto.s16 == 1) servicesSelected.Add(331);// CompanyServiceEnum.IsTaqirKarbari,331
            if (inputDto.s20 == 1) servicesSelected.Add(308);// CompanyServiceEnum.JabejaiiKontor,308
            if (inputDto.s21 == 1) servicesSelected.Add(107);//خط انتقال اب   107
            if (inputDto.s22 == 1) servicesSelected.Add(203);//خط انتقال فاضلاب  203
            if (inputDto.s23 == 1) servicesSelected.Add(108);//سهم منبع اب   108
            if (inputDto.s24 == 1) servicesSelected.Add(309);// CompanyServiceEnum.TaqirQotrSifoon,309
            if (inputDto.s26 == 1) servicesSelected.Add(109);// CompanyServiceEnum.IsAmadeSaziAb,109
            if (inputDto.s27 == 1) servicesSelected.Add(209);// CompanyServiceEnum.IsAmadeSaziFazelab,209
            if (inputDto.s32 == 1) servicesSelected.Add(303);// CompanyServiceEnum.QatVaslEnsheab,303
            if (inputDto.s33 == 1) servicesSelected.Add(310);// CompanyServiceEnum.SifoonEzafe,310
            if (inputDto.s34 == 1) servicesSelected.Add(-1);//,//عدم تخفیف آب
            if (inputDto.s35 == 1) servicesSelected.Add(-2);//,//عدم تخفیف فاضلاب
            if (inputDto.s36 == 1) servicesSelected.Add(323);// CompanyServiceEnum.JabejaiiSifoon,323
            if (inputDto.s37 == 1) servicesSelected.Add(0);// CompanyServiceEnum.NezamMohandesi,؟
            if (inputDto.s38 == 1) servicesSelected.Add(0);// CompanyServiceEnum.TavizSifoon,؟
            if (inputDto.s39 == 1) servicesSelected.Add(0);// CompanyServiceEnum.KhanevarShomari,
            if (inputDto.s40 == 1) servicesSelected.Add(205);// CompanyServiceEnum.TafkikEdqam,205
            if (inputDto.s41 == 1) servicesSelected.Add(0);// CompanyServiceEnum.TavizKontor,
            if (inputDto.s42 == 1) servicesSelected.Add(375);// ,//لوله گذاری آب  375
            if (inputDto.s43 == 1) servicesSelected.Add(376);// ,//لوله گذاری فاضلاب  376
            if (inputDto.s44 == 1) servicesSelected.Add(0);// CompanyServiceEnum.IsZarfiatQarardadi,
            if (inputDto.s45 == 1) servicesSelected.Add(0);// CompanyServiceEnum.KontorMojaza,
            if (inputDto.s46 == 1) servicesSelected.Add(0);// CompanyServiceEnum.TaqirTarefe,
            if (inputDto.s47 == 1) servicesSelected.Add(0);// CompanyServiceEnum.Peymayesh,
            if (inputDto.s48 == 1) servicesSelected.Add(500);// CompanyServiceEnum.Saier,500

            return servicesSelected;
        }
    }
}
