using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class SetAssessmentResultHandler : ISetAssessmentResultHandler
    {
        private readonly IMoshtrakCommandService _moshtrackCommandService;
        private readonly ITrackingCommandService _trackingCommandService;
        private static int _status = 110;
        public SetAssessmentResultHandler(
            IMoshtrakCommandService moshtrackCommandService,
            ITrackingCommandService trackingCommandService)
        {
            _moshtrackCommandService = moshtrackCommandService;
            _moshtrackCommandService.NotNull(nameof(moshtrackCommandService));

            _trackingCommandService = trackingCommandService;
            _trackingCommandService.NotNull(nameof(trackingCommandService));
        }

        public async Task Handle(AssessmentResultInputDto inputDto, CancellationToken cancellationToken)
        {
            TrackingInsertDto trackingInsertDto = new(inputDto.TrackNumber, _status, inputDto.Description);

            await _trackingCommandService.UpdateIsConsiderdLatest(inputDto.TrackNumber, true);
            await _trackingCommandService.Insert(trackingInsertDto);
            await _moshtrackCommandService.Update(GetMoshtrackUpdateDto(inputDto));
            //Examination

        }
        private MoshtrkUpdateDto GetMoshtrackUpdateDto(AssessmentResultInputDto inputDto)
        {
            return new MoshtrkUpdateDto()
            {
                TrackingId = inputDto.TrackingId,
                TrackNumber = inputDto.TrackNumber,
                ServiceGroupId = inputDto.ServiceGroupId,
                StringTrackNumber = inputDto.StringTrackNumber,
                BillId = inputDto.BillId,
                CustomerNumber = inputDto.CustomerNumber,
                NeighbourBillId = inputDto.NeighbourBillId,
                ZoneId = inputDto.ZoneId,
                NotificationMobile = inputDto.NotificationMobile,
                UsageId = inputDto.UsageId,
                MeterDiameterId = inputDto.MeterDiameterId,
                BranchTypeId = inputDto.BranchTypeId,
                DiscountCount = inputDto.DiscountCount,
                TrackingResultId = inputDto.TrackingResultId,
                PhoneNumber = inputDto.PhoneNumber,
                MobileNumber = inputDto.MobileNumber,
                FirstName = inputDto.FirstName,
                Surname = inputDto.Surname,
                Premises = inputDto.Premises,
                ImprovementCommertial = inputDto.ImprovementCommertial,
                ImprovementDomestic = inputDto.ImprovementDomestic,
                ImprovementOverall = inputDto.ImprovementOverall,
                Siphon100 = inputDto.Siphon100,
                Siphon125 = inputDto.Siphon125,
                Siphon150 = inputDto.Siphon150,
                Siphon200 = inputDto.Siphon200,
                ContractualCapacity = inputDto.ContractualCapacity,
                HouseValue = inputDto.HouseValue,
                CommertialUnit = inputDto.CommertialUnit,
                DomesticUnit = inputDto.DomesticUnit,
                OtherUnit = inputDto.OtherUnit,
                DiscountTypeId = inputDto.DiscountTypeId,
                NationalCode = inputDto.NationalCode,
                FatherName = inputDto.FatherName,
                PostalCode = inputDto.PostalCode,
                IsNonPermanent = inputDto.IsNonPermanent,
                Address = inputDto.Address,
                Description = inputDto.Description,
                PreViewId = inputDto.PreViewId,
                CounterType = inputDto.CounterType,
                InstallAgentState = inputDto.InstallAgentState,
                BlockId = inputDto.BlockId,

                s0 = inputDto.SelectedServices.Contains((int)CompanyServiceEnum.IsEnsheabAb) ? 1 : 0,
                s1 = inputDto.SelectedServices.Contains((int)CompanyServiceEnum.IsEnsheabFazelab) ? 1 : 0,
                s2 = inputDto.SelectedServices.Contains((int)CompanyServiceEnum.IsEnsheabFazelab) ? 1 : 0,
                //s3=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                s4 = inputDto.SelectedServices.Contains((int)CompanyServiceEnum.IsTaqirNam) ? 1 : 0,
                s5 = inputDto.SelectedServices.Contains((int)CompanyServiceEnum.IsTaqirQotrEnsheab) ? 1 : 0,
                //s8=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                //s9= s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                s10 = inputDto.SelectedServices.Contains((int)CompanyServiceEnum.EstelamMahzar) ? 1 : 0,
                s11 = inputDto.SelectedServices.Contains(0) ? 1 : 0,//تفکیک عرصه اب
                s12 = inputDto.SelectedServices.Contains(0) ? 1 : 0,//تفکیک عرصه فاضلاب
                s13 = inputDto.SelectedServices.Contains((int)CompanyServiceEnum.TaqirSathCounter) ? 1 : 0,
                //s14=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                //s15=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                s16 = inputDto.SelectedServices.Contains((int)CompanyServiceEnum.IsTaqirKarbari) ? 1 : 0,
                //s17=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                //s18=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                //s19=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                s20 = inputDto.SelectedServices.Contains((int)CompanyServiceEnum.JabejaiiKontor) ? 1 : 0,
                s21 = inputDto.SelectedServices.Contains(0) ? 1 : 0,//خط انتقال اب
                s22 = inputDto.SelectedServices.Contains(0) ? 1 : 0,//خط انتقال فاضلاب
                s23 = inputDto.SelectedServices.Contains(0) ? 1 : 0,//سهم منبع اب
                s24 = inputDto.SelectedServices.Contains((int)CompanyServiceEnum.TaqirQotrSifoon) ? 1 : 0,
                //s25=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                s26 = inputDto.SelectedServices.Contains((int)CompanyServiceEnum.IsAmadeSaziAb) ? 1 : 0,
                s27 = inputDto.SelectedServices.Contains((int)CompanyServiceEnum.IsAmadeSaziFazelab) ? 1 : 0,
                //s28=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                //s29=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                //s30=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                //s31=s.SelectedServices.Contains((int)CompanyServiceEnum.) ? 1 : 0,
                s32 = inputDto.SelectedServices.Contains((int)CompanyServiceEnum.QatVaslEnsheab) ? 1 : 0,
                s33 = inputDto.SelectedServices.Contains((int)CompanyServiceEnum.SifoonEzafe) ? 1 : 0,
                s34 = inputDto.SelectedServices.Contains(0) ? 1 : 0,//عدم تخفیف آب
                s35 = inputDto.SelectedServices.Contains(0) ? 1 : 0,//عدم تخفیف فاضلاب
                s36 = inputDto.SelectedServices.Contains((int)CompanyServiceEnum.JabejaiiSifoon) ? 1 : 0,
                s37 = inputDto.SelectedServices.Contains((int)CompanyServiceEnum.NezamMohandesi) ? 1 : 0,
                s38 = inputDto.SelectedServices.Contains((int)CompanyServiceEnum.TavizSifoon) ? 1 : 0,
                s39 = inputDto.SelectedServices.Contains((int)CompanyServiceEnum.KhanevarShomari) ? 1 : 0,
                s40 = inputDto.SelectedServices.Contains((int)CompanyServiceEnum.TafkikEdqam) ? 1 : 0,
                s41 = inputDto.SelectedServices.Contains((int)CompanyServiceEnum.TavizKontor) ? 1 : 0,
                s42 = inputDto.SelectedServices.Contains(0) ? 1 : 0,//لوله گذاری آب
                s43 = inputDto.SelectedServices.Contains(0) ? 1 : 0,//لوله گذاری فاضلاب
                s44 = inputDto.SelectedServices.Contains((int)CompanyServiceEnum.IsZarfiatQarardadi) ? 1 : 0,
                s45 = inputDto.SelectedServices.Contains((int)CompanyServiceEnum.KontorMojaza) ? 1 : 0,
                s46 = inputDto.SelectedServices.Contains((int)CompanyServiceEnum.TaqirTarefe) ? 1 : 0,
                s47 = inputDto.SelectedServices.Contains((int)CompanyServiceEnum.Peymayesh) ? 1 : 0,
                s48 = inputDto.SelectedServices.Contains((int)CompanyServiceEnum.Saier) ? 1 : 0,
            };
        }

    }
}
