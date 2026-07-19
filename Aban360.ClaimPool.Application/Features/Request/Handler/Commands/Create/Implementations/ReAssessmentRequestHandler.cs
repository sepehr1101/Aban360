using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Microsoft.Extensions.Configuration;
using System.Data;
using Aban360.Common.BaseEntities;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Contracts;
using DNTPersianUtils.Core;
using Aban360.Common.Timing;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class ReAssessmentRequestHandler : AbstractBaseConnection, IReAssessmentRequestHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IExaminationScheduleQueryService _examinationScheduleQueryService;
        private readonly IExaminationQueryService _examinationQueryService;
        private static int _requestOrigin = 12;
        private static int _afterSaleRequestType = 2;
        private static int _saleRequestType = 1;
        private static int _requestRegistered = 0;
        private static int _reAssessmentStatusId = 15;
        private static int _setAssessmentTimeStatusId = 10;
        private static int _deletedSatatus = 90000;
        private static int _registeredStatus = 90;
        private static int _amountConfirmedStatus = 75;
        private int[] _disallowedPreviousStatusIds = { _requestRegistered, _reAssessmentStatusId, _setAssessmentTimeStatusId, _deletedSatatus, _registeredStatus, _amountConfirmedStatus };
        public ReAssessmentRequestHandler(
            ITrackingQueryService trackingQueryService,
            IMoshtrakQueryService moshtrakQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            IExaminationScheduleQueryService examinationScheduleQueryService,
            IExaminationQueryService examinationQueryService,
            IConfiguration configuration)
            : base(configuration)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));


            _examinationScheduleQueryService = examinationScheduleQueryService;
            _examinationScheduleQueryService.NotNull(nameof(examinationScheduleQueryService));

            _examinationQueryService = examinationQueryService;
            _examinationQueryService.NotNull(nameof(examinationQueryService));
        }

        public async Task Handle(TrackNumberWithDescriptionInputDto inputDto, int userCode, CancellationToken cancellationToken)
        {
            TrackingOutputDto latestTrackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(latestTrackingInfo.ZoneId, null, null, inputDto.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            Validate(inputDto.TrackNumber, latestTrackingInfo.StatusId, moshtrakInfo.IsRegistered);

            TrackingInsertDuplicateDto trackingInsertDto = new(inputDto.TrackNumber, _reAssessmentStatusId, inputDto.Description, userCode, _requestOrigin, true, false);
            var (assessmentCode, assessmentDateJalali) = await GetAssessmentTaskInfo(latestTrackingInfo, moshtrakInfo);

            await ExecSql(latestTrackingInfo, inputDto, trackingInsertDto, moshtrakInfo, userCode, assessmentCode, assessmentDateJalali);
        }
        private void Validate(int trackNumber, int statusId, bool isRegistered)
        {
            if (isRegistered == true || _disallowedPreviousStatusIds.Contains(statusId))
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
            }
        }
        private async Task<MoshtrakUpdateCustomerInfoDto> GetAfterSaleMoshtrakUpdateDto(MoshtrakOutputDto moshtrakInfo)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = new(moshtrakInfo.ZoneId, moshtrakInfo.CustomerNumber);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumber);
            return new MoshtrakUpdateCustomerInfoDto()
            {
                ZoneId = moshtrakInfo.ZoneId,
                TrackNumber = moshtrakInfo.TrackNumber,
                FirstName = memberInfo.FirstName,
                Surname = memberInfo.Surname,
                FatherName = memberInfo.FatherName,
                NationalCode = memberInfo.NationalCode,
                PhoneNumber = memberInfo.PhoneNumber,
                MobileNumber = memberInfo.MobileNumber,
                Address = memberInfo.Address,
                PostalCode = memberInfo.PostalCode,
                UsageId = memberInfo.UsageId,
                BranchTypeId = memberInfo.UseStateId,
                Premises = memberInfo.Premises,
                ImprovementOverall = memberInfo.OverallImprovement,
                ImprovementDomestic = memberInfo.DomesticImprovement,
                ImprovementCommercial = memberInfo.CommercialImprovement,
                OtherUnit = memberInfo.OtherUnit,
                DomesticUnit = memberInfo.DomesticUnit,
                CommercialUnit = memberInfo.CommercialUnit,
                ContractualCapacity = memberInfo.ContractualCapacity,
                Siphon100 = memberInfo.Siphon100,
                Siphon125 = memberInfo.Siphon125,
                Siphon150 = memberInfo.Siphon150,
                Siphon200 = memberInfo.Siphon200,
                MainSiphon = int.Parse(memberInfo.MainSiphon),
                CommonSiphon = memberInfo.CommonSiphon1,
                MeterDiameterId = memberInfo.MeterDiameterId,
                DiscountTypeId = memberInfo.DiscountId,
                DiscountCount = memberInfo.DiscountCount,
            };
        }
        private async Task<(int, string?)> GetAssessmentTaskInfo(TrackingOutputDto latestTrackingInfo, MoshtrakOutputDto moshtrakInfo)
        {
            int assessmentCode = 0;
            string? assessmentDateJalali;
            if (latestTrackingInfo.ServiceGroupId == _afterSaleRequestType)
            {
                MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(new ZoneIdAndCustomerNumber(latestTrackingInfo.ZoneId, moshtrakInfo.CustomerNumber));
                (assessmentCode, assessmentDateJalali) = await GetAssessmentDateTime(memberInfo);
            }
            else
            {
                ZoneIdAndCustomerNumber neighbourCustomerNumber = await _commonMemberQueryService.Get(moshtrakInfo.NeighbourBillId);
                MemberInfoGetDto neighbourMemberInfo = await _commonMemberQueryService.Get(neighbourCustomerNumber);
                (assessmentCode, assessmentDateJalali) = await GetAssessmentDateTime(neighbourMemberInfo);
            }

            return (assessmentCode, assessmentDateJalali);
        }
        private async Task<MoshtrakUpdateCustomerInfoDto> GetSaleMoshtrakUpdateDto(MoshtrakOutputDto moshtrakInfo)
        {
            moshtrakInfo = await _moshtrakQueryService.Get(moshtrakInfo.Id, moshtrakInfo.ZoneId);
            return new MoshtrakUpdateCustomerInfoDto()
            {
                ZoneId = moshtrakInfo.ZoneId,
                TrackNumber = moshtrakInfo.TrackNumber,
                FirstName = moshtrakInfo.FirstName ?? string.Empty,
                Surname = moshtrakInfo.Surname ?? string.Empty,
                FatherName = moshtrakInfo.FatherName,
                NationalCode = moshtrakInfo.NationalCode,
                PhoneNumber = moshtrakInfo.PhoneNumber ?? string.Empty,
                MobileNumber = moshtrakInfo.MobileNumber,
                Address = moshtrakInfo.Address,
                PostalCode = moshtrakInfo.PostalCode ?? string.Empty,
                UsageId = 0,
                BranchTypeId = 0,
                Premises = 0,
                ImprovementOverall = 0,
                ImprovementDomestic = 0,
                ImprovementCommercial = 0,
                OtherUnit = 0,
                DomesticUnit = 0,
                CommercialUnit = 0,
                ContractualCapacity = 0,
                Siphon100 = 0,
                Siphon125 = 0,
                Siphon150 = 0,
                Siphon200 = 0,
                MainSiphon = 0,
                CommonSiphon = 0,
                MeterDiameterId = 0,
                DiscountTypeId = 0,
                DiscountCount = 0,
            };
        }
        private async Task ExecSql(TrackingOutputDto latestTrackingInfo, TrackNumberWithDescriptionInputDto inputDto, TrackingInsertDuplicateDto trackingInsertDto, MoshtrakOutputDto moshtrakInfo, int userCode, int assessmentCode, string? assessmentDateJalali)
        {
            string dbName = GetDbName(latestTrackingInfo.ZoneId);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    TrackingCommandService trackingCommandService = new(connection, transaction);
                    MoshtrakCommandService moshtrakCommandService = new(connection, transaction);
                    ExaminationCommandService examinationCommandService = new(connection, transaction);

                    await trackingCommandService.UpdateIsConsiderdLatest(inputDto.TrackNumber, true);
                    await trackingCommandService.InsertDuplicate(trackingInsertDto);

                    if (latestTrackingInfo.ServiceGroupId == _afterSaleRequestType)
                    {
                        MoshtrakUpdateCustomerInfoDto moshtrackUpdateDto = await GetAfterSaleMoshtrakUpdateDto(moshtrakInfo);
                        await moshtrakCommandService.Update(moshtrackUpdateDto, dbName);
                    }
                    else if (latestTrackingInfo.ServiceGroupId == _saleRequestType)
                    {
                        MoshtrakUpdateCustomerInfoDto moshtrackUpdateDto = await GetSaleMoshtrakUpdateDto(moshtrakInfo);
                        await moshtrakCommandService.Update(moshtrackUpdateDto, dbName);
                    }

                    if (!string.IsNullOrWhiteSpace(assessmentDateJalali))
                    {
                        TrackingInsertDuplicateDto trackingInsertSetTimeDto = new(latestTrackingInfo.TrackNumber, _setAssessmentTimeStatusId, inputDto.Description, userCode, _requestOrigin, true, false, 1);
                        AssessmentInsertDto assessmentInsert = await GetAssessmentInsertDto(trackingInsertSetTimeDto, latestTrackingInfo, assessmentCode, assessmentDateJalali);
                        await trackingCommandService.UpdateIsConsiderdLatest(latestTrackingInfo.TrackNumber, true);
                        await trackingCommandService.InsertDuplicate(trackingInsertSetTimeDto);
                        await examinationCommandService.Insert(assessmentInsert);
                    }

                    transaction.Commit();
                }
            }
        }
        private async Task<(int, string)> GetAssessmentDateTime(MemberInfoGetDto memberInfo)
        {
            IEnumerable<AssessmentScaduleGetDto> assessmentsScadule = await _examinationScheduleQueryService.Get(memberInfo.ZoneId, memberInfo.ReadingNumber);

            for (int offset = 1; offset <= 14; offset++)
            {
                DateTime date = DateTime.Now.Date.AddDays(offset);
                string dateJalali = date.ToShortPersianDateString();

                int dayIndex =
                    date.DayOfWeek == DayOfWeek.Saturday ? 0 :
                    date.DayOfWeek == DayOfWeek.Sunday ? 1 :
                    date.DayOfWeek == DayOfWeek.Monday ? 2 :
                    date.DayOfWeek == DayOfWeek.Tuesday ? 3 :
                    date.DayOfWeek == DayOfWeek.Wednesday ? 4 :
                    date.DayOfWeek == DayOfWeek.Thursday ? 5 :
                                                          6;

                foreach (var eachAssessment in assessmentsScadule)
                {
                    int dayValue = dayIndex switch
                    {
                        0 => eachAssessment.Day0,
                        1 => eachAssessment.Day1,
                        2 => eachAssessment.Day2,
                        3 => eachAssessment.Day3,
                        4 => eachAssessment.Day4,
                        5 => eachAssessment.Day5,
                        6 => eachAssessment.Day6,
                        _ => 0
                    };

                    if (dayValue <= 0) continue;

                    int assessmentTaskCount =
                        await _examinationQueryService.GetWithoutResultInDate(dateJalali, eachAssessment.AssessmentCode);

                    if (assessmentTaskCount < dayValue)
                    {
                        return (eachAssessment.AssessmentCode, dateJalali);
                    }
                }
            }
            return (0, string.Empty);
        }
        private async Task<AssessmentInsertDto> GetAssessmentInsertDto(TrackingInsertDuplicateDto trackingInsertSetTimeDto, TrackingOutputDto latestTrackingInfo, int assessmentCode, string assessmentDateJalali)
        {
            AssessmentGetDto assessmentData = await _examinationQueryService.Get(assessmentCode);

            return new AssessmentInsertDto()
            {
                TrackNumber = trackingInsertSetTimeDto.TrackNumber,
                BillId = latestTrackingInfo.BillId ?? string.Empty,
                AssessmentCode = assessmentData.Code,
                AssessmentMobile = assessmentData.PhoneNumber,
                AssessmentName = assessmentData.FullName,
                ZoneId = latestTrackingInfo.ZoneId,
                AssessmentDateJalali = assessmentDateJalali,
                AssessmentGregorianDateTime = ConvertDate.JalaliToDateTime(assessmentDateJalali),
                ResultId = null,
                Description = null,
                TrackId = trackingInsertSetTimeDto.TrackId,
                TrackIdResult = null,
                X1 = "0",
                Y1 = "0",
                X2 = "0",
                Y2 = "0",
                TrenchLenS = 0,
                TrenchLenW = 0,
                AsphaltLenW = 0,
                AsphaltLenS = 0,
                RockyLenS = 0,
                RockyLenW = 0,
                OtherLenS = 0,
                OtherLenW = 0,
                BasementDepth = 0,
                HasMap = false,//
                ReadingNumber = string.Empty,
                Premises = 0,
                HouseValue = 0,
                UsageId = 0,
                AllInJson = string.Empty,
            };
        }
    }
}
