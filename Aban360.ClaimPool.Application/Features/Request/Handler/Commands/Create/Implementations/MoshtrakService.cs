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
                s34 = servicesSelected.Contains(0) ? 1 : 0,//عدم تخفیف آب
                s35 = servicesSelected.Contains(0) ? 1 : 0,//عدم تخفیف فاضلاب
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
    }
}
