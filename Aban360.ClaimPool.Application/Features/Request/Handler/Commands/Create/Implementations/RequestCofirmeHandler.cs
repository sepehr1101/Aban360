using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class RequestCofirmeHandler : AbstractBaseConnection, IRequestCofirmeHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly IExaminationQueryService _examinationQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IValidator<RequestConfirmInputDto> _validator;
        private int _requestConfirmStatusId = 90;
        private int _validCounterStateCode = 0;
        private int _saleGroupServiceId = 1;
        private int _operator = 666;
        public RequestCofirmeHandler(
            IHttpContextAccessor contextAccessor,
            ITrackingQueryService trackingQueryService,
            IMoshtrakQueryService moshtrakQueryService,
            IExaminationQueryService examinationQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            ICommonZoneService commonZoneService,
            IValidator<RequestConfirmInputDto> validator,
            IConfiguration configuration)
            : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _examinationQueryService = examinationQueryService;
            _examinationQueryService.NotNull(nameof(examinationQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(RequestConfirmInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            AssessmentDataOutputDto assessmentInfo = await _examinationQueryService.GetLatestByTrackNumber(inputDto.TrackNumber);
            MoshtrakOutputDto? moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, inputDto.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            if (moshtrakInfo is null)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidTrackNumber);
            }

            bool isSale = trackingInfo.ServiceGroupId == _saleGroupServiceId;
            Validate(moshtrakInfo, trackingInfo);
            MoshtrakSabtUpdateDto moshtrakUpdateDto = new(moshtrakInfo.Id, true, inputDto.Description);
            CustomerInsertDto customerInsertDto = GetCustomerInsertDto(moshtrakInfo, trackingInfo, assessmentInfo);
            CustomerUpdateDto? customerUpdateDto = new();
            if (!isSale)
            {
                MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(new ZoneIdAndCustomerNumber(trackingInfo.ZoneId, moshtrakInfo.CustomerNumber));
                customerUpdateDto = GetCustomerUpdateDto(moshtrakInfo, trackingInfo, assessmentInfo);
                //switch on requests , each request most edit someProp 
            }

            await ExecSql(moshtrakUpdateDto, customerInsertDto, customerUpdateDto, appUser, inputDto.TrackNumber, isSale);
        }
        private async Task ExecSql(MoshtrakSabtUpdateDto moshtrakUpdateDto, CustomerInsertDto customerInsertDto, CustomerUpdateDto? customerUpdateDto, IAppUser appUser, int trackNumber, bool isSale)
        {
            string dbName = GetDbName(customerInsertDto.ZoneId);
            int archMemRecordId = 0;
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
                    MembersCommandService membersCommandService = new(connection, transaction);
                    ArchMemCommandService archMemCommandService = new(connection, transaction);
                    ClientsCommandService clientsCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await moshtrakCommandService.UpdateSabt(moshtrakUpdateDto, dbName);
                    await trackingCommandService.UpdateIsConsiderdLatest(trackNumber, true);
                    if (isSale)
                    {
                        await membersCommandService.Update(customerUpdateDto, dbName);
                        archMemRecordId = await archMemCommandService.Insert(customerUpdateDto, dbName, dbName);
                    }
                    else
                    {
                        await membersCommandService.Insert(customerInsertDto, dbName);
                        archMemRecordId = await archMemCommandService.Insert(customerInsertDto, dbName);
                    }
                    string opLogText = string.Format(OpLogLiterals.RequestConfirmTracking, trackNumber, archMemRecordId);
                    await clientsCommandService.InsertByArchMemId(archMemRecordId, dbName);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private CustomerInsertDto GetCustomerInsertDto(MoshtrakOutputDto moshtrakInfo, TrackingOutputDto trackingInfo, AssessmentDataOutputDto assessmentInfo)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            return new CustomerInsertDto()
            {
                ZoneId = moshtrakInfo.ZoneId,
                CustomerNumber = moshtrakInfo.CustomerNumber,
                BillId = trackingInfo.BillId,
                X = assessmentInfo.X1 ?? string.Empty,
                Y = assessmentInfo.Y1 ?? string.Empty,
                ReadingNumber = moshtrakInfo.ReadingNumber,//todo
                FirstName = moshtrakInfo.FirstName ?? string.Empty,
                Surname = moshtrakInfo.Surname ?? string.Empty,
                Address = moshtrakInfo.Address ?? string.Empty,
                PostalCode = moshtrakInfo.PostalCode ?? string.Empty,
                Plaque = string.Empty,//
                NationalCode = moshtrakInfo.NationalCode,
                PhoneNumber = moshtrakInfo.PhoneNumber ?? string.Empty,
                MobileNumber = moshtrakInfo.MobileNumber,
                FatherName = moshtrakInfo.FatherName,
                BranchTypeId = moshtrakInfo.BranchTypeId,
                UsageSellId = moshtrakInfo.UsageId,
                UsageConsumptionId = moshtrakInfo.UsageId,
                EmptyUnit = 0,//
                CommertialUnit = moshtrakInfo.CommercialUnit,
                DomesticUnit = moshtrakInfo.DomesticUnit,
                OtherUnit = moshtrakInfo.OtherUnit,
                ImprovementCommertial = moshtrakInfo.ImprovementCommercial,
                ImprovementDomestic = moshtrakInfo.ImprovementDomestic,
                ImprovementOverall = moshtrakInfo.ImprovementOverall,
                Premises = moshtrakInfo.Premises,
                HouseholdNumber = 0,//
                HouseholdDateJalali = string.Empty,//
                MeterDiamterId = moshtrakInfo.MeterDiameterId,
                IsSpecial = moshtrakInfo.IsSpecial,
                ContractualCapacity = moshtrakInfo.ContractualCapacity,
                WaterDebt = 0,
                C20 = 0,
                NAb = 0,
                NFaz = 0,

                MeterInstallationDateJalali = string.Empty,
                MeterRequestDateJalali = currentDateJalali,
                SewageInstallationDateJalali = string.Empty,
                SewageRequestDateJalali = currentDateJalali,
                Siphon100 = moshtrakInfo.Siphon100,
                Siphon125 = moshtrakInfo.Siphon125,
                Siphon150 = moshtrakInfo.Siphon150,
                Siphon200 = moshtrakInfo.Siphon200,
                Siphon5 = 0,//
                Siphon6 = 0,//
                Siphon7 = 0,//
                Siphon8 = 0,//
                MainSiphon = moshtrakInfo.MainSiphon,
                CommonSiphon = moshtrakInfo.CommonSiphon,
                Operator = _operator,
                DeletionStateId = _validCounterStateCode,
                BodySerial = string.Empty,
                MeterRegisterDateJalali = string.Empty,
                SewageRegisterDateJalali = string.Empty,
                GuildId = 0,
            };
        }
        private CustomerUpdateDto GetCustomerUpdateDto(MoshtrakOutputDto moshtrakInfo, TrackingOutputDto trackingInfo, AssessmentDataOutputDto assessmentInfo)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            return new CustomerUpdateDto()
            {
                ZoneId = moshtrakInfo.ZoneId,
                CustomerNumber = moshtrakInfo.CustomerNumber,
                BillId = trackingInfo.BillId,
                X = assessmentInfo.X1 ?? string.Empty,
                Y = assessmentInfo.Y1 ?? string.Empty,
                ReadingNumber = moshtrakInfo.ReadingNumber,//todo
                FirstName = moshtrakInfo.FirstName ?? string.Empty,
                Surname = moshtrakInfo.Surname ?? string.Empty,
                Address = moshtrakInfo.Address ?? string.Empty,
                PostalCode = moshtrakInfo.PostalCode ?? string.Empty,
                Plaque = string.Empty,//
                NationalCode = moshtrakInfo.NationalCode,
                PhoneNumber = moshtrakInfo.PhoneNumber ?? string.Empty,
                MobileNumber = moshtrakInfo.MobileNumber,
                FatherName = moshtrakInfo.FatherName,
                BranchTypeId = moshtrakInfo.BranchTypeId,
                UsageSellId = moshtrakInfo.UsageId,
                UsageConsumptionId = moshtrakInfo.UsageId,
                EmptyUnit = 0,//
                CommertialUnit = moshtrakInfo.CommercialUnit,
                DomesticUnit = moshtrakInfo.DomesticUnit,
                OtherUnit = moshtrakInfo.OtherUnit,
                ImprovementCommertial = moshtrakInfo.ImprovementCommercial,
                ImprovementDomestic = moshtrakInfo.ImprovementDomestic,
                ImprovementOverall = moshtrakInfo.ImprovementOverall,
                Premises = moshtrakInfo.Premises,
                HouseholdNumber = 0,//
                HouseholdDateJalali = string.Empty,//
                MeterDiamterId = moshtrakInfo.MeterDiameterId,
                IsSpecial = moshtrakInfo.IsSpecial,
                ContractualCapacity = moshtrakInfo.ContractualCapacity,

                MeterInstallationDateJalali = string.Empty,
                MeterRequestDateJalali = currentDateJalali,
                SewageInstallationDateJalali = string.Empty,
                SewageRequestDateJalali = currentDateJalali,
                Siphon100 = moshtrakInfo.Siphon100,
                Siphon125 = moshtrakInfo.Siphon125,
                Siphon150 = moshtrakInfo.Siphon150,
                Siphon200 = moshtrakInfo.Siphon200,
                Siphon5 = 0,//
                Siphon6 = 0,//
                Siphon7 = 0,//
                Siphon8 = 0,//
                MainSiphon = moshtrakInfo.MainSiphon,
                CommonSiphon = moshtrakInfo.CommonSiphon,
                Operator = _operator,
                DeletionStateId = _validCounterStateCode,
                BodySerial = string.Empty,
                MeterRegisterDateJalali = string.Empty,
                SewageRegisterDateJalali = string.Empty,
                GuildId = 0,
            };
        }
        private async Task InputValidate(RequestConfirmInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private void Validate(MoshtrakOutputDto moshtrakInfo, TrackingOutputDto trackingInfo)
        {
            if (moshtrakInfo.IsRegistered)
            {
                throw new InvalidTrackingException(ExceptionLiterals.NotFoundAnyOpenTrack);
            }
            if (trackingInfo.StatusId != _requestConfirmStatusId)
            {
                //throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
            }
            if (moshtrakInfo.CustomerNumber <= 0 || string.IsNullOrWhiteSpace(trackingInfo.BillId))
            {
                throw new InvalidTrackingException(ExceptionLiterals.SetBillId);
            }
        }
    }
}