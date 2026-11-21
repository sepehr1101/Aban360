using Aban360.ClaimPool.Application.Features.Base.Services;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.Common.Timing;
using Aban360.ReportPool.Domain.Base;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace SqlAfterWorkflow10.Aban360.ClaimPool.Application.Features.Request.Services;
public interface IRequestRegistrationService
{
    Task<(MoshtrakCreateDto, SetAssessmentTimeDataOutputDto? , Guid)> CreateNewBranchAsync(RequestNewBranchInputDto inputDto, int userName, CancellationToken cancellationToken);
    Task<(MoshtrakCreateDto, SetAssessmentTimeDataOutputDto? , Guid)> CreateAfterSaleAsync(RequestAfterSaleInputDto input, int userName, CancellationToken cancellationToken);
    Task<TrackingDuplicateValidationOutputDto> ValidateDuplicateAsync(TrackingDuplicateValidationInputDto inputDto, CancellationToken cancellationToken);
}

internal sealed class RequestRegistrationService : global::WorkflowRecordingConnectionBase, IRequestRegistrationService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ICommonMemberQueryService _commonMemberQueryService;
    private readonly IMoshtrakQueryService _moshtrakQueryService;
    private readonly IExaminationScheduleQueryService _examinationScheduleQueryService;
    private readonly IExaminationQueryService _examinationQueryService;
    private readonly IMoshtrakService _moshtrakService;
    private readonly IValidator<RequestNewBranchInputDto> _validator;
    static string _insertWayTitle = ReportLiterals.RayabPersian;
    static int _setAssessmentTimeStatusId = (int)TrackingStatusEnum.SetAssessmentResult;
    static int _firstStepStatusId = (int)TrackingStatusEnum.RequestRegistered;
    static int saleRequestTypeId = ReportLiterals.SaleRequestTypeId;
    static int _requestOrigin = 12;
    private readonly IValidator<RequestAfterSaleInputDto> _createAfterSaleValidator;
    static string _createAfterSaleInsertWayTitle = ReportLiterals.RayabPersian;
    static int _afterSaleRequestServiceId = ReportLiterals.AfterSaleRequestTypeId;
    static int _createAfterSaleSetAssessmentTimeStatusId = (int)TrackingStatusEnum.SetAssessmentResult;
    static int _createAfterSaleFirstStepStatusId = (int)TrackingStatusEnum.RequestRegistered;
    static int _createAfterSaleRequestOrigin = 12;
    private readonly ITrackingQueryService _trackingQueryService;
    public RequestRegistrationService(IHttpContextAccessor contextAccessor, ICommonMemberQueryService commonMemberQueryService, IMoshtrakQueryService moshtrakQueryService, IExaminationScheduleQueryService examinationScheduleQueryService, IExaminationQueryService examinationQueryService, IMoshtrakService moshtrakService, IValidator<RequestNewBranchInputDto> validator, IConfiguration configuration, IValidator<RequestAfterSaleInputDto> createAfterSaleValidator, ITrackingQueryService trackingQueryService) : base(configuration)
    {
        _contextAccessor = contextAccessor;
        _contextAccessor.NotNull(nameof(contextAccessor));
        _commonMemberQueryService = commonMemberQueryService;
        _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));
        _moshtrakQueryService = moshtrakQueryService;
        _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));
        _examinationScheduleQueryService = examinationScheduleQueryService;
        _examinationScheduleQueryService.NotNull(nameof(examinationScheduleQueryService));
        _examinationQueryService = examinationQueryService;
        _examinationQueryService.NotNull(nameof(examinationQueryService));
        _moshtrakService = moshtrakService;
        _moshtrakService.NotNull(nameof(moshtrakService));
        _validator = validator;
        _validator.NotNull(nameof(validator));
        _createAfterSaleValidator = createAfterSaleValidator;
        _createAfterSaleValidator.NotNull(nameof(createAfterSaleValidator));
        _trackingQueryService = trackingQueryService;
        _trackingQueryService.NotNull(nameof(trackingQueryService));
    }

    public async Task<(MoshtrakCreateDto, SetAssessmentTimeDataOutputDto? , Guid)> CreateNewBranchAsync(RequestNewBranchInputDto inputDto, int userName, CancellationToken cancellationToken)
    {
        await CreateNewBranchValidate(inputDto, cancellationToken);
        MemberInfoGetDto? neighbourMemeberInfo = null;
        if (!string.IsNullOrWhiteSpace(inputDto.NeighbourBillId))
        {
            ZoneIdAndCustomerNumber neighbourCustomerInfo = await _commonMemberQueryService.Get(inputDto.NeighbourBillId);
            neighbourMemeberInfo = await _commonMemberQueryService.Get(neighbourCustomerInfo);
            if (!inputDto.IsSkipDuplicate)
            {
                await _moshtrakQueryService.CheckOpenRequest(inputDto.NationalCode, neighbourCustomerInfo.ZoneId);
            }
        }

        var(assessmentCode, assessmentDateJalali) = await CreateNewBranchGetAssessmentDateTime(neighbourMemeberInfo);
        var(moshtrakCreateDto, assessmentInsertDto, trackId) = await CreateNewBranchExecSql(inputDto, userName, assessmentDateJalali, assessmentCode);
        SetAssessmentTimeDataOutputDto? assessmentTimeDto = assessmentInsertDto is not null ? await CreateNewBranchGetAssessmentTimeOutputDto(moshtrakCreateDto, neighbourMemeberInfo, assessmentInsertDto, trackId) : null;
        return (moshtrakCreateDto, assessmentTimeDto, trackId);
    }

    private async Task CreateNewBranchValidate(RequestNewBranchInputDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    }

    private async Task<MoshtrakCreateDto> CreateNewBranchGetMoshtrackCreateDto(RequestNewBranchInputDto inputDto, int trackNumber, int zoneId)
    {
        MoshtrakServiceDto serviceSelected = _moshtrakService.GetServicesSelected(inputDto.SelectedServices);
        await CreateNewBranchSelectedServiceValidate(serviceSelected);
        return new MoshtrakCreateDto()
        {
            TrackNumber = trackNumber,
            ServiceGroupId = saleRequestTypeId,
            StringTrackNumber = trackNumber.ToString().PadLeft(11, '0'),
            BillId = string.Empty,
            CustomerNumber = 0,
            NeighbourBillId = inputDto.NeighbourBillId,
            ZoneId = zoneId,
            NotificationMobile = inputDto.MobileNumber ?? string.Empty,
            UsageId = 0,
            MeterDiameterId = 0,
            BranchTypeId = 0,
            DiscountTypeId = 0,
            DiscountCount = 0,
            PhoneNumber = inputDto.PhoneNumber ?? string.Empty,
            MobileNumber = inputDto.MobileNumber,
            NationalCode = inputDto.NationalCode,
            FirstName = inputDto.FirstName,
            Surname = inputDto.Surname,
            FatherName = inputDto.FatherName ?? string.Empty,
            Premises = 0,
            ImprovementCommertial = 0,
            ImprovementDomestic = 0,
            ImprovementOverall = 0,
            Siphon100 = 0,
            Siphon125 = 0,
            Siphon150 = 0,
            Siphon200 = 0,
            MainSiphon = 0,
            CommonSiphon = 0,
            ContractualCapacity = 0,
            HouseValue = 0,
            CommertialUnit = 0,
            DomesticUnit = 0,
            OtherUnit = 0,
            IsNonPermanent = false,
            Address = inputDto.Address,
            PreViewId = string.Empty,
            CounterType = 0,
            InstallAgentState = 0,
            BlockId = string.Empty,
            InsertWayTitle = _insertWayTitle,
            PostalCode = string.Empty,
            ReadingNumber = string.Empty,
            IsSpecial = false,
            CertificateNumber = inputDto.BirthCertificateNumber ?? string.Empty,
            BrokerId = 0, //todo8
            s0 = serviceSelected.s0,
            s1 = serviceSelected.s1,
            s2 = serviceSelected.s2,
            s3 = serviceSelected.s3,
            s4 = serviceSelected.s4,
            s5 = serviceSelected.s5,
            s8 = serviceSelected.s8,
            s9 = serviceSelected.s9,
            s10 = serviceSelected.s10,
            s11 = serviceSelected.s11,
            s12 = serviceSelected.s12,
            s13 = serviceSelected.s13,
            s14 = serviceSelected.s14,
            s15 = serviceSelected.s15,
            s16 = serviceSelected.s16,
            s17 = serviceSelected.s17,
            s18 = serviceSelected.s18,
            s19 = serviceSelected.s19,
            s20 = serviceSelected.s20,
            s21 = serviceSelected.s21,
            s22 = serviceSelected.s22,
            s23 = serviceSelected.s23,
            s24 = serviceSelected.s24,
            s25 = serviceSelected.s25,
            s26 = serviceSelected.s26,
            s27 = serviceSelected.s27,
            s28 = serviceSelected.s28,
            s29 = serviceSelected.s29,
            s30 = serviceSelected.s30,
            s31 = serviceSelected.s31,
            s32 = serviceSelected.s32,
            s33 = serviceSelected.s33,
            s34 = serviceSelected.s34,
            s35 = serviceSelected.s35,
            s36 = serviceSelected.s36,
            s37 = serviceSelected.s37,
            s38 = serviceSelected.s38,
            s39 = serviceSelected.s39,
            s40 = serviceSelected.s40,
            s41 = serviceSelected.s41,
            s42 = serviceSelected.s42,
            s43 = serviceSelected.s43,
            s44 = serviceSelected.s44,
            s45 = serviceSelected.s45,
            s46 = serviceSelected.s46,
            s47 = serviceSelected.s47,
            s48 = serviceSelected.s48,
        };
    }

    private async Task CreateNewBranchSelectedServiceValidate(MoshtrakServiceDto serviceSelected)
    {
        ICollection<int> selectedItems = await _moshtrakService.GetServicesSelected(serviceSelected, saleRequestTypeId);
        if (!selectedItems.Any())
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidZeroServiceSelected);
        }
    }

    private TrackingInsertDto CreateNewBranchGetTrackingCreateDto(RequestNewBranchInputDto inputDto, int userName, int trackNumber, int zoneId)
    {
        return new TrackingInsertDto()
        {
            TrackNumber = trackNumber,
            ZoneId = zoneId,
            BillId = null,
            ServiceGroupId = saleRequestTypeId,
            StatusId = _firstStepStatusId,
            InsertByUserId = userName,
            Description = inputDto.Description,
            NotificationMobile = inputDto.MobileNumber,
            NeighbourBillId = inputDto.NeighbourBillId,
            RequestOrigin = _requestOrigin,
        };
    }

    private async Task<(MoshtrakCreateDto, AssessmentInsertDto? , Guid)> CreateNewBranchExecSql(RequestNewBranchInputDto inputDto, int userName, string assessmentDateJalali, int assessmentCode)
    {
        MoshtrakCreateDto moshtrakInsertDto;
        AssessmentInsertDto? assessmentInsert = null;
        Guid trackId = new Guid();
        string dbName = GetDbName(inputDto.ZoneId);
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
            {
                MoshtrakCommandService moshtrakCommandService = new(connection, transaction);
                TrackingCommandService trackingCommandService = new(connection, transaction);
                T0CommandService t0CommandService = new(connection, transaction);
                ExaminationCommandService examinationCommandService = new(connection, transaction);
                int trackNumber = (int)(await t0CommandService.GetTrackNumber());
                moshtrakInsertDto = await CreateNewBranchGetMoshtrackCreateDto(inputDto, trackNumber, inputDto.ZoneId);
                TrackingInsertDto trackingSetRequestInsertDto = CreateNewBranchGetTrackingCreateDto(inputDto, userName, trackNumber, inputDto.ZoneId);
                trackId = trackingSetRequestInsertDto.TrackId;
                await moshtrakCommandService.Insert(moshtrakInsertDto, dbName);
                await trackingCommandService.Insert(trackingSetRequestInsertDto);
                if (!string.IsNullOrWhiteSpace(assessmentDateJalali))
                {
                    TrackingInsertDuplicateDto trackingInsertSetTimeDto = new(trackNumber, _setAssessmentTimeStatusId, inputDto.Description, userName, _requestOrigin, true, false, 2);
                    assessmentInsert = await CreateNewBranchGetAssessmentInsertDto(trackingInsertSetTimeDto, assessmentCode, assessmentDateJalali, trackingSetRequestInsertDto.ZoneId);
                    await trackingCommandService.UpdateIsConsiderdLatest(trackingSetRequestInsertDto.TrackNumber, true);
                    await trackingCommandService.InsertDuplicate(trackingInsertSetTimeDto);
                    await examinationCommandService.Insert(assessmentInsert);
                }

                transaction.Commit();
            }
        }

        return (moshtrakInsertDto, assessmentInsert, trackId);
    }

    private async Task<(int, string)> CreateNewBranchGetAssessmentDateTime(MemberInfoGetDto? memberInfo)
    {
        if (memberInfo is not null)
        {
            IEnumerable<AssessmentScaduleGetDto> assessmentsScadule = await _examinationScheduleQueryService.Get(memberInfo.ZoneId, memberInfo.ReadingNumber);
            for (int offset = 1; offset <= 14; offset++)
            {
                DateTime date = DateTime.Now.Date.AddDays(offset);
                string dateJalali = date.ToShortPersianDateString();
                int dayIndex = date.DayOfWeek == DayOfWeek.Saturday ? 0 : date.DayOfWeek == DayOfWeek.Sunday ? 1 : date.DayOfWeek == DayOfWeek.Monday ? 2 : date.DayOfWeek == DayOfWeek.Tuesday ? 3 : date.DayOfWeek == DayOfWeek.Wednesday ? 4 : date.DayOfWeek == DayOfWeek.Thursday ? 5 : 6;
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
                    if (dayValue <= 0)
                        continue;
                    int assessmentTaskCount = await _examinationQueryService.GetWithoutResultInDate(dateJalali, eachAssessment.AssessmentCode);
                    if (assessmentTaskCount < dayValue)
                    {
                        return (eachAssessment.AssessmentCode, dateJalali);
                    }
                }
            }
        }

        return (0, string.Empty);
    }

    private async Task<AssessmentInsertDto> CreateNewBranchGetAssessmentInsertDto(TrackingInsertDuplicateDto trackingInsertSetTimeDto, int assessmentCode, string assessmentDateJalali, int zoneId)
    {
        AssessmentGetDto assessmentData = await _examinationQueryService.Get(assessmentCode);
        string body = await new StreamReader(_contextAccessor.HttpContext.Request.Body).ReadToEndAsync();
        return new AssessmentInsertDto()
        {
            TrackNumber = trackingInsertSetTimeDto.TrackNumber,
            BillId = string.Empty,
            AssessmentCode = assessmentData.Code,
            AssessmentMobile = assessmentData.PhoneNumber,
            AssessmentName = assessmentData.FullName,
            ZoneId = zoneId,
            AssessmentDateJalali = assessmentDateJalali,
            AssessmentGregorianDateTime = ConvertDate.JalaliToDateTime(assessmentDateJalali, true),
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
            HasMap = false, //
            ReadingNumber = string.Empty,
            Premises = 0,
            HouseValue = 0,
            UsageId = 0,
            AllInJson = body
        };
    }

    private async Task<SetAssessmentTimeDataOutputDto> CreateNewBranchGetAssessmentTimeOutputDto(MoshtrakCreateDto moshtrakInsertDto, MemberInfoGetDto neighbourMemeberInfo, AssessmentInsertDto assessmentInsertDto, Guid trackId)
    {
        IEnumerable<NumericDictionary> moshtrakServiceSelected = await _moshtrakService.GetServicesSelectedDto(_moshtrakService.GetMoshtrakServiceDto(moshtrakInsertDto), moshtrakInsertDto.ServiceGroupId);
        string serviceSelected = string.Join(",", moshtrakServiceSelected.Select(m => m.Title));
        return new SetAssessmentTimeDataOutputDto()
        {
            TrackId = trackId,
            ServiceGroupId = moshtrakInsertDto.ServiceGroupId,
            TrackNumber = moshtrakInsertDto.TrackNumber,
            Address = moshtrakInsertDto.Address,
            FullName = $"{moshtrakInsertDto.FirstName} {moshtrakInsertDto.Surname}",
            MobileNumber = moshtrakInsertDto.NotificationMobile,
            BillId = moshtrakInsertDto.BillId,
            ServiceSelectedList = serviceSelected,
            NeighbourBillId = moshtrakInsertDto.NeighbourBillId,
            NeighbourAddress = neighbourMemeberInfo.Address,
            AssessmentName = assessmentInsertDto.AssessmentName,
            AssessmentCode = assessmentInsertDto.AssessmentCode,
            AssessmentMobileNumber = assessmentInsertDto.AssessmentMobile,
            AssessmentDateJalai = assessmentInsertDto.AssessmentDateJalali,
        };
    }

    public async Task<(MoshtrakCreateDto, SetAssessmentTimeDataOutputDto? , Guid)> CreateAfterSaleAsync(RequestAfterSaleInputDto input, int userName, CancellationToken cancellationToken)
    {
        await CreateAfterSaleInputValidate(input, cancellationToken);
        MemberInfoGetDto memberInfo = await CreateAfterSaleOpenRequestValidate(input);
        var(assessmentCode, assessmentDateJalali) = await CreateAfterSaleGetAssessmentDateTime(memberInfo);
        var(moshtrakCreateDto, assessmentTimeInsertDto, trackId) = await CreateAfterSaleExecSql(input, memberInfo, userName, assessmentCode, assessmentDateJalali);
        SetAssessmentTimeDataOutputDto? assessmentTimeDto = assessmentTimeInsertDto is not null ? await CreateAfterSaleGetAssessmentTimeOutputDto(moshtrakCreateDto, assessmentTimeInsertDto, trackId) : null;
        return (moshtrakCreateDto, assessmentTimeDto, trackId);
    }

    private async Task CreateAfterSaleInputValidate(RequestAfterSaleInputDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createAfterSaleValidator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    }

    private async Task<MemberInfoGetDto> CreateAfterSaleOpenRequestValidate(RequestAfterSaleInputDto input)
    {
        ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(input.BillId);
        MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumber);
        await _moshtrakQueryService.CheckOpenRequest(memberInfo.CustomerNumber, memberInfo.ZoneId);
        return memberInfo;
    }

    private async Task<(MoshtrakCreateDto, AssessmentInsertDto? , Guid)> CreateAfterSaleExecSql(RequestAfterSaleInputDto input, MemberInfoGetDto memberInfo, int userName, int assessmentCode, string assessmentDateJalali)
    {
        MoshtrakCreateDto moshtrakInsertDto;
        AssessmentInsertDto? assessmentInsert = null;
        Guid trackId = new Guid();
        string dbName = GetDbName(memberInfo.ZoneId);
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
            {
                MoshtrakCommandService moshtrakCommandService = new(connection, transaction);
                TrackingCommandService trackingCommandService = new(connection, transaction);
                T0CommandService t0CommandService = new(connection, transaction);
                ExaminationCommandService examinationCommandService = new(connection, transaction);
                int trackNumber = (int)(await t0CommandService.GetTrackNumber());
                moshtrakInsertDto = await CreateAfterSaleGetMoshtrackCreateDto(input, memberInfo, trackNumber);
                TrackingInsertDto trackingSetRequestInsertDto = CreateAfterSaleGetTrackingCreateDto(input, memberInfo, userName, trackNumber);
                trackId = trackingSetRequestInsertDto.TrackId;
                await moshtrakCommandService.Insert(moshtrakInsertDto, dbName);
                await trackingCommandService.Insert(trackingSetRequestInsertDto);
                if (!string.IsNullOrWhiteSpace(assessmentDateJalali))
                {
                    TrackingInsertDuplicateDto trackingInsertSetTimeDto = new(trackNumber, _createAfterSaleSetAssessmentTimeStatusId, input.Description, userName, _createAfterSaleRequestOrigin, true, false, 1);
                    assessmentInsert = await CreateAfterSaleGetAssessmentInsertDto(trackingInsertSetTimeDto, trackingSetRequestInsertDto, memberInfo, assessmentCode, assessmentDateJalali);
                    await trackingCommandService.UpdateIsConsiderdLatest(trackingSetRequestInsertDto.TrackNumber, true);
                    await trackingCommandService.InsertDuplicate(trackingInsertSetTimeDto);
                    await examinationCommandService.Insert(assessmentInsert);
                }

                transaction.Commit();
            }
        }

        return (moshtrakInsertDto, assessmentInsert, trackId);
    }

    private async Task<(int, string)> CreateAfterSaleGetAssessmentDateTime(MemberInfoGetDto memberInfo)
    {
        IEnumerable<AssessmentScaduleGetDto> assessmentsScadule = await _examinationScheduleQueryService.Get(memberInfo.ZoneId, memberInfo.ReadingNumber);
        for (int offset = 1; offset <= 14; offset++)
        {
            DateTime date = DateTime.Now.Date.AddDays(offset);
            string dateJalali = date.ToShortPersianDateString();
            int dayIndex = date.DayOfWeek == DayOfWeek.Saturday ? 0 : date.DayOfWeek == DayOfWeek.Sunday ? 1 : date.DayOfWeek == DayOfWeek.Monday ? 2 : date.DayOfWeek == DayOfWeek.Tuesday ? 3 : date.DayOfWeek == DayOfWeek.Wednesday ? 4 : date.DayOfWeek == DayOfWeek.Thursday ? 5 : 6;
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
                if (dayValue <= 0)
                    continue;
                int assessmentTaskCount = await _examinationQueryService.GetWithoutResultInDate(dateJalali, eachAssessment.AssessmentCode);
                if (assessmentTaskCount < dayValue)
                {
                    return (eachAssessment.AssessmentCode, dateJalali);
                }
            }
        }

        return (0, string.Empty);
    }

    private async Task<MoshtrakCreateDto> CreateAfterSaleGetMoshtrackCreateDto(RequestAfterSaleInputDto inputDto, MemberInfoGetDto memberInfo, int trackNumber) //todo
    {
        MoshtrakServiceDto serviceSelected = _moshtrakService.GetServicesSelected(inputDto.SelectedServices);
        await CreateAfterSaleSelectedServiceValidate(serviceSelected);
        return new MoshtrakCreateDto()
        {
            TrackNumber = trackNumber,
            ServiceGroupId = _afterSaleRequestServiceId,
            StringTrackNumber = trackNumber.ToString().PadLeft(11, '0'),
            BillId = memberInfo.BillId,
            CustomerNumber = memberInfo.CustomerNumber,
            NeighbourBillId = null,
            ZoneId = memberInfo.ZoneId,
            NotificationMobile = inputDto.NotificationNumber ?? string.Empty,
            UsageId = memberInfo.UsageId,
            MeterDiameterId = memberInfo.MeterDiameterId,
            BranchTypeId = memberInfo.UseStateId,
            DiscountTypeId = memberInfo.DiscountId,
            DiscountCount = memberInfo.DiscountCount,
            PhoneNumber = memberInfo.PhoneNumber,
            MobileNumber = memberInfo.MobileNumber,
            NationalCode = memberInfo.NationalCode,
            FirstName = memberInfo.FirstName,
            Surname = memberInfo.Surname,
            FatherName = memberInfo.FatherName,
            Premises = memberInfo.Premises,
            ImprovementCommertial = memberInfo.CommercialImprovement,
            ImprovementDomestic = memberInfo.DomesticImprovement,
            ImprovementOverall = memberInfo.OverallImprovement,
            Siphon100 = memberInfo.Siphon100,
            Siphon125 = memberInfo.Siphon125,
            Siphon150 = memberInfo.Siphon150,
            Siphon200 = memberInfo.Siphon200,
            MainSiphon = int.Parse(memberInfo.MainSiphon),
            CommonSiphon = memberInfo.CommonSiphon1,
            ContractualCapacity = memberInfo.ContractualCapacity,
            HouseValue = 0, //todo
            CommertialUnit = memberInfo.CommercialUnit,
            DomesticUnit = memberInfo.DomesticUnit,
            OtherUnit = memberInfo.OtherUnit,
            IsNonPermanent = false,
            Address = inputDto.Address,
            PreViewId = string.Empty, //todo
            CounterType = memberInfo.DeletionStateId,
            InstallAgentState = 0, //todo
            BlockId = string.IsNullOrWhiteSpace(memberInfo.BlockCode) ? string.Empty : memberInfo.BlockCode,
            InsertWayTitle = _createAfterSaleInsertWayTitle,
            PostalCode = memberInfo.PostalCode,
            IsSpecial = memberInfo.IsSpecial,
            ReadingNumber = memberInfo.ReadingNumber,
            CertificateNumber = string.Empty,
            BrokerId = 0, //todo
            s0 = serviceSelected.s0,
            s1 = serviceSelected.s1,
            s2 = serviceSelected.s2,
            s3 = serviceSelected.s3,
            s4 = serviceSelected.s4,
            s5 = serviceSelected.s5,
            s8 = serviceSelected.s8,
            s9 = serviceSelected.s9,
            s10 = serviceSelected.s10,
            s11 = serviceSelected.s11,
            s12 = serviceSelected.s12,
            s13 = serviceSelected.s13,
            s14 = serviceSelected.s14,
            s15 = serviceSelected.s15,
            s16 = serviceSelected.s16,
            s17 = serviceSelected.s17,
            s18 = serviceSelected.s18,
            s19 = serviceSelected.s19,
            s20 = serviceSelected.s20,
            s21 = serviceSelected.s21,
            s22 = serviceSelected.s22,
            s23 = serviceSelected.s23,
            s24 = serviceSelected.s24,
            s25 = serviceSelected.s25,
            s26 = serviceSelected.s26,
            s27 = serviceSelected.s27,
            s28 = serviceSelected.s28,
            s29 = serviceSelected.s29,
            s30 = serviceSelected.s30,
            s31 = serviceSelected.s31,
            s32 = serviceSelected.s32,
            s33 = serviceSelected.s33,
            s34 = serviceSelected.s34,
            s35 = serviceSelected.s35,
            s36 = serviceSelected.s36,
            s37 = serviceSelected.s37,
            s38 = serviceSelected.s38,
            s39 = serviceSelected.s39,
            s40 = serviceSelected.s40,
            s41 = serviceSelected.s41,
            s42 = serviceSelected.s42,
            s43 = serviceSelected.s43,
            s44 = serviceSelected.s44,
            s45 = serviceSelected.s45,
            s46 = serviceSelected.s46,
            s47 = serviceSelected.s47,
            s48 = serviceSelected.s48,
        };
    }

    private async Task CreateAfterSaleSelectedServiceValidate(MoshtrakServiceDto serviceSelected)
    {
        ICollection<int> selectedItems = await _moshtrakService.GetServicesSelected(serviceSelected, _afterSaleRequestServiceId);
        if (!selectedItems.Any())
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidZeroServiceSelected);
        }
    }

    private TrackingInsertDto CreateAfterSaleGetTrackingCreateDto(RequestAfterSaleInputDto inputDto, MemberInfoGetDto memberInfo, int userName, int trackNumber)
    {
        return new TrackingInsertDto()
        {
            TrackNumber = trackNumber,
            ZoneId = memberInfo.ZoneId,
            BillId = memberInfo.BillId,
            ServiceGroupId = _afterSaleRequestServiceId,
            StatusId = _createAfterSaleFirstStepStatusId,
            InsertByUserId = userName,
            Description = inputDto.Description,
            NotificationMobile = inputDto.MobileNumber,
            NeighbourBillId = null,
            RequestOrigin = _createAfterSaleRequestOrigin
        };
    }

    private async Task<AssessmentInsertDto> CreateAfterSaleGetAssessmentInsertDto(TrackingInsertDuplicateDto trackingInsertSetTimeDto, TrackingInsertDto previousTrackingInfo, MemberInfoGetDto memberInfo, int assessmentCode, string assessmentDateJalali)
    {
        AssessmentGetDto assessmentData = await _examinationQueryService.Get(assessmentCode);
        string body = await new StreamReader(_contextAccessor.HttpContext.Request.Body).ReadToEndAsync();
        return new AssessmentInsertDto()
        {
            TrackNumber = trackingInsertSetTimeDto.TrackNumber,
            BillId = previousTrackingInfo.BillId ?? string.Empty,
            AssessmentCode = assessmentData.Code,
            AssessmentMobile = assessmentData.PhoneNumber,
            AssessmentName = assessmentData.FullName,
            ZoneId = previousTrackingInfo.ZoneId,
            AssessmentDateJalali = assessmentDateJalali,
            AssessmentGregorianDateTime = ConvertDate.JalaliToDateTime(assessmentDateJalali, true).Value,
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
            HasMap = false, //
            ReadingNumber = memberInfo.ReadingNumber ?? string.Empty,
            Premises = 0,
            HouseValue = 0,
            UsageId = memberInfo.UsageId,
            AllInJson = body
        };
    }

    private async Task<SetAssessmentTimeDataOutputDto> CreateAfterSaleGetAssessmentTimeOutputDto(MoshtrakCreateDto moshtrakInsertDto, AssessmentInsertDto assessmentInsertDto, Guid trackId)
    {
        IEnumerable<NumericDictionary> moshtrakServiceSelected = await _moshtrakService.GetServicesSelectedDto(_moshtrakService.GetMoshtrakServiceDto(moshtrakInsertDto), moshtrakInsertDto.ServiceGroupId);
        string serviceSelected = string.Join(",", moshtrakServiceSelected.Select(m => m.Title));
        return new SetAssessmentTimeDataOutputDto()
        {
            TrackId = trackId,
            ServiceGroupId = moshtrakInsertDto.ServiceGroupId,
            TrackNumber = moshtrakInsertDto.TrackNumber,
            Address = moshtrakInsertDto.Address,
            FullName = $"{moshtrakInsertDto.FirstName} {moshtrakInsertDto.Surname}",
            MobileNumber = moshtrakInsertDto.NotificationMobile,
            BillId = moshtrakInsertDto.BillId,
            ServiceSelectedList = serviceSelected,
            NeighbourBillId = moshtrakInsertDto.NeighbourBillId,
            NeighbourAddress = string.Empty,
            AssessmentName = assessmentInsertDto.AssessmentName,
            AssessmentCode = assessmentInsertDto.AssessmentCode,
            AssessmentMobileNumber = assessmentInsertDto.AssessmentMobile,
            AssessmentDateJalai = assessmentInsertDto.AssessmentDateJalali,
        };
    }

    public async Task<TrackingDuplicateValidationOutputDto> ValidateDuplicateAsync(TrackingDuplicateValidationInputDto inputDto, CancellationToken cancellationToken)
    {
        var(moshtrakSearch, moshtrakSearchType, regionAndZoneInfo) = await ValidateDuplicateGetMoshtrakDto(inputDto);
        MoshtrakOutputDto? moshtrakInfo = moshtrakSearch is not null ? (await _moshtrakQueryService.GetValid(moshtrakSearch, moshtrakSearchType, false)).OrderBy(m => m.IsRegistered).ThenByDescending(m => m.RequestDateJalali).FirstOrDefault() : null;
        TrackingOutputDto? latestTrackingInfo = await _trackingQueryService.GetLatest(moshtrakInfo?.TrackNumber ?? 0, false);
        TrackingOutputDto? firstTrackingInfo = await _trackingQueryService.GetFirstStep(moshtrakInfo?.TrackNumber ?? 0, false);
        NumericDictionary? requestOrigin = RequestOrigin.GetRequestOrigin(firstTrackingInfo?.RequestOriginId ?? 0);
        IEnumerable<NumericDictionary> serviceSelected = new List<NumericDictionary>();
        if (moshtrakInfo is not null)
        {
            MoshtrakServiceDto moshtrakServiceDto = ValidateDuplicateGetMoshtrakService(moshtrakInfo);
            serviceSelected = latestTrackingInfo is null ? new List<NumericDictionary>() : await _moshtrakService.GetServicesSelectedDto(moshtrakServiceDto, latestTrackingInfo.ServiceGroupId);
        }

        return ValidateDuplicateGetResultDto(inputDto, serviceSelected, moshtrakInfo, latestTrackingInfo, firstTrackingInfo, requestOrigin, regionAndZoneInfo);
    }

    private async Task<(MoshtrakGetDto? , MoshtrakSearchTypeEnum, RegionAndZoneGetDto? )> ValidateDuplicateGetMoshtrakDto(TrackingDuplicateValidationInputDto inputDto)
    {
        RegionAndZoneGetDto? regionAndZoneInfo = null;
        MoshtrakGetDto? moshtrakSearch = null;
        MoshtrakSearchTypeEnum moshtrakSearchType = 0;
        if (inputDto.ValidationType == TrackingDuplicateValidationTypeEnum.ByNationalCode)
        {
            if (!string.IsNullOrWhiteSpace(inputDto.NeighbourBillId))
            {
                ZoneIdAndCustomerNumber neighbourZoneInfo = await _commonMemberQueryService.Get(inputDto.NeighbourBillId);
                MemberInfoGetDto neighbourInfo = await _commonMemberQueryService.Get(neighbourZoneInfo);
                regionAndZoneInfo = new(neighbourInfo.RegionId, neighbourInfo.RegionTitle, neighbourInfo.ZoneId, neighbourInfo.ZoneTitle);
                moshtrakSearch = new(neighbourZoneInfo.ZoneId, null, inputDto.NationalCode, null);
            }

            moshtrakSearchType = MoshtrakSearchTypeEnum.ByNationalCode;
        }

        if (inputDto.ValidationType == TrackingDuplicateValidationTypeEnum.ByBillId)
        {
            ZoneIdAndCustomerNumber customerZoneInfo = await _commonMemberQueryService.Get(inputDto.BillId);
            MemberInfoGetDto customerInfo = await _commonMemberQueryService.Get(customerZoneInfo);
            regionAndZoneInfo = new(customerInfo.RegionId, customerInfo.RegionTitle, customerInfo.ZoneId, customerInfo.ZoneTitle);
            moshtrakSearch = new(customerZoneInfo.ZoneId, customerZoneInfo.CustomerNumber, null, null);
            moshtrakSearchType = MoshtrakSearchTypeEnum.ByCustomerNumber;
        }

        return (moshtrakSearch, moshtrakSearchType, regionAndZoneInfo);
    }

    private MoshtrakServiceDto ValidateDuplicateGetMoshtrakService(MoshtrakOutputDto serviceSelected)
    {
        return new MoshtrakServiceDto()
        {
            s0 = serviceSelected.s0,
            s1 = serviceSelected.s1,
            s2 = serviceSelected.s2,
            s3 = serviceSelected.s3,
            s4 = serviceSelected.s4,
            s5 = serviceSelected.s5,
            s8 = serviceSelected.s8,
            s9 = serviceSelected.s9,
            s10 = serviceSelected.s10,
            s11 = serviceSelected.s11,
            s12 = serviceSelected.s12,
            s13 = serviceSelected.s13,
            s14 = serviceSelected.s14,
            s15 = serviceSelected.s15,
            s16 = serviceSelected.s16,
            s17 = serviceSelected.s17,
            s18 = serviceSelected.s18,
            s19 = serviceSelected.s19,
            s20 = serviceSelected.s20,
            s21 = serviceSelected.s21,
            s22 = serviceSelected.s22,
            s23 = serviceSelected.s23,
            s24 = serviceSelected.s24,
            s25 = serviceSelected.s25,
            s26 = serviceSelected.s26,
            s27 = serviceSelected.s27,
            s28 = serviceSelected.s28,
            s29 = serviceSelected.s29,
            s30 = serviceSelected.s30,
            s31 = serviceSelected.s31,
            s32 = serviceSelected.s32,
            s33 = serviceSelected.s33,
            s34 = serviceSelected.s34,
            s35 = serviceSelected.s35,
            s36 = serviceSelected.s36,
            s37 = serviceSelected.s37,
            s38 = serviceSelected.s38,
            s39 = serviceSelected.s39,
            s40 = serviceSelected.s40,
            s41 = serviceSelected.s41,
            s42 = serviceSelected.s42,
            s43 = serviceSelected.s43,
            s44 = serviceSelected.s44,
            s45 = serviceSelected.s45,
            s46 = serviceSelected.s46,
            s47 = serviceSelected.s47,
            s48 = serviceSelected.s48,
        };
    }

    private TrackingDuplicateValidationOutputDto ValidateDuplicateGetResultDto(TrackingDuplicateValidationInputDto inputDto, IEnumerable<NumericDictionary>? serviceSelected, MoshtrakOutputDto? moshtrakInfo, TrackingOutputDto? latestTrackingInfo, TrackingOutputDto? firstTrackingInfo, NumericDictionary? requestOrigin, RegionAndZoneGetDto? regionAndZoneInfo)
    {
        return new TrackingDuplicateValidationOutputDto()
        {
            Id = moshtrakInfo?.Id ?? 0,
            ZoneId = regionAndZoneInfo?.ZoneId ?? 0,
            ZoneTitle = regionAndZoneInfo?.ZoneTitle ?? string.Empty,
            RegionId = regionAndZoneInfo?.RegionId ?? 0,
            RegionTitle = regionAndZoneInfo?.RegionTitle ?? string.Empty,
            BillId = latestTrackingInfo?.BillId ?? inputDto.BillId ?? string.Empty,
            CustomerNumber = moshtrakInfo?.CustomerNumber ?? 0,
            NationalCode = moshtrakInfo?.NationalCode ?? inputDto.NationalCode ?? string.Empty,
            TrackNumber = moshtrakInfo?.TrackNumber ?? 0,
            FirstName = moshtrakInfo?.FirstName ?? string.Empty,
            Surname = moshtrakInfo?.Surname ?? string.Empty,
            FatherName = moshtrakInfo?.FatherName ?? string.Empty,
            PhoneNumber = moshtrakInfo?.PhoneNumber ?? string.Empty,
            MobileNumber = moshtrakInfo?.MobileNumber ?? string.Empty,
            NotificationNumber = latestTrackingInfo?.NotificationMobile ?? string.Empty,
            Address = moshtrakInfo?.Address ?? string.Empty,
            CertificateNumber = moshtrakInfo?.CertificateNumber ?? string.Empty,
            Description = moshtrakInfo?.Description ?? string.Empty,
            NeighbourBillId = moshtrakInfo?.NeighbourBillId ?? inputDto.NeighbourBillId ?? string.Empty,
            PostalCode = moshtrakInfo?.PostalCode ?? string.Empty,
            RequestDateJalali = moshtrakInfo?.RequestDateJalali ?? string.Empty,
            IsDuplicate = (moshtrakInfo?.IsRegistered ?? true) ? false : true,
            LatestStatusId = latestTrackingInfo?.StatusId ?? 0,
            LatestStatusTitle = latestTrackingInfo?.StatusTitle ?? string.Empty,
            RequestOriginId = firstTrackingInfo?.RequestOriginId ?? 0,
            RequestOrigin = requestOrigin?.Title ?? string.Empty,
            ServiceSelected = serviceSelected
        };
    }
}
