using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class SetAssessmentResultHandler : AbstractBaseConnection, ISetAssessmentResultHandler
    {
        private readonly IAssessmentQueryService _assessmentQueryService;
        private static int _status = 110;
        public SetAssessmentResultHandler(
            IConfiguration configuration,
            IAssessmentQueryService assessmentQueryService)
            : base(configuration)
        {
            _assessmentQueryService = assessmentQueryService;
            _assessmentQueryService.NotNull(nameof(assessmentQueryService));
        }

        public async Task Handle(AssessmentResultInputDto inputDto, int assessmentCode, CancellationToken cancellationToken)
        {
            TrackingInsertDto trackingInsertDto = new(inputDto.TrackNumber, _status, inputDto.Description, assessmentCode);
            AssessmentInsertDto assessmentInsertDto = await GetAssessmentInsertDto(inputDto, assessmentCode);

            using (IDbConnection connection = _sqlReportConnection)//why didnt use SqlConnection?
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MoshtrakCommandService _moshtrackCommandService = new(_sqlReportConnection, transaction);//wht didnt use connection
                    TrackingCommandService _trackingCommandService = new(_sqlReportConnection, transaction);
                    ExaminationCommandService _assessmentCommandService = new(_sqlReportConnection, transaction);
                    string dbName = GetDbName(inputDto.ZoneId);


                    await _trackingCommandService.UpdateIsConsiderdLatest(inputDto.TrackNumber, true);
                    await _trackingCommandService.Insert(trackingInsertDto);
                    await _moshtrackCommandService.Update(GetMoshtrackUpdateDto(inputDto), dbName);
                    await _assessmentCommandService.Insert(assessmentInsertDto);

                    transaction.Commit();
                }
            }
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
                PreViewId = inputDto.PreViewId,
                CounterType = inputDto.CounterType,
                InstallAgentState = inputDto.InstallAgentState,
                BlockId = inputDto.BlockId,
                MainSiphon = inputDto.MainSiphon,
                CommonSiphon = inputDto.CommonSiphon,

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
        private async Task<AssessmentInsertDto> GetAssessmentInsertDto(AssessmentResultInputDto inputDto, int assessmentCode)
        {
            AssessmentGetDto assessmentData = await _assessmentQueryService.Get(assessmentCode);
            return new AssessmentInsertDto()
            {
                TrackNumber = inputDto.TrackNumber,
                BillId = inputDto.BillId,
                AssessmentCode = assessmentData.Code,
                AssessmentMobile = assessmentData.PhoneNumber,
                AssessmentName = assessmentData.FullName,
                ZoneId = inputDto.ZoneId,
                ResultId = inputDto.TrackingResultId,
                Description = inputDto.Description,//todo: need or not?
                TrackId = inputDto.TrackingId,
                TrackIdResult = Guid.Empty,//todo
                X1 = inputDto.X1,
                Y1 = inputDto.Y1,
                X2 = inputDto.X2,
                Y2 = inputDto.Y2,
                TrenchLenS = inputDto.TrenchLenS,
                TrenchLenW = inputDto.TrenchLenW,
                AsphaltLenW = inputDto.AsphaltLenW,
                AsphaltLenS = inputDto.AsphaltLenS,
                RockyLenS = inputDto.RockyLenS,
                RockyLenW = inputDto.RockyLenW,
                OtherLenS = inputDto.OtherLenS,
                OtherLenW = inputDto.OtherLenW,
                BasementDepth = inputDto.BasementDepth,
                HasMap = inputDto.HasMap,
                ReadingNumber = inputDto.ReadingNumber,
                Premises = inputDto.Premises,
                HouseValue = inputDto.HouseValue,
                UsageId = inputDto.UsageId,
            };
        }
    }
}
