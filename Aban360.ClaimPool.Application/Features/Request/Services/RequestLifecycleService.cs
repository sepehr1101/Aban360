using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Microsoft.Extensions.Configuration;
using System.Data;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.Common.BaseEntities;
using FluentValidation;
using Aban360.Common.ApplicationUser;
using Aban360.ReportPool.Domain.Base;
using DNTPersianUtils.Core;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations;

namespace Aban360.ClaimPool.Application.Features.Request.Services
{
    public interface IRequestLifecycleService
    {
        Task<RequestCloseOuputDto> CloseRequestAsync(CloseRequestInputDto inputDto, int userName, CancellationToken cancellationToken);
        Task PreviousStatusRequestAsync(TrackNumberWithDescriptionInputDto inputDto, int userCode, CancellationToken cancellationToken);
        Task ReferredToIndividualRequestAsync(TrackNumberWithDescriptionInputDto inputDto, int userCode, CancellationToken cancellationToken);
        Task MoshtrakRequestUpdateAsync(MoshtrakUpdateInputDto inputDto, CancellationToken cancellationToken);
        Task<SwapRequestTypeOutputDto> SwapRequestTypeAsync(SwapRequestTypeInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
        Task ArchiveRequestAsync(ArchiveRequestInputDto inputDto, int userCode, CancellationToken cancellationToken);
        Task<ReportOutput<TrackingKartableHeaderOutputDto, TrackingKartableDataOutputDto>> ArchivedRequestAsync(CancellationToken cancellationToken);
    }

    internal sealed class RequestLifecycleService : AbstractBaseConnection, IRequestLifecycleService
    {
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly ITrackingQueryService _trackingQueryService;
        static int _CloseRequestremoveRequestStatusId = (int)TrackingStatusEnum.Deleted;
        static int _CloseRequestrequestOrigin = 12;
        static int _PreviousStatusRequestrequestOrigin = 12;
        static int _ReferredToIndividualRequestreferredToIndividualStatusId = (int)TrackingStatusEnum.ReferredToIndividual;
        static int _ReferredToIndividualRequestremoveRequestStatusId = (int)TrackingStatusEnum.Deleted;
        static int _ReferredToIndividualRequestrequestOrigin = 12;
        private readonly IMoshtrakService _moshtrakService;
        private readonly IValidator<MoshtrakUpdateInputDto> _validator;
        private static int _SwapRequestTyperequestRegisteredStatusId = (int)TrackingStatusEnum.RequestRegistered;
        private static int _SwapRequestTypesetAssessmentTimeStatusId = (int)TrackingStatusEnum.SetAssessmentTime;
        private static int _SwapRequestTypeassessmentSetResultStatusId = (int)TrackingStatusEnum.ExamineResultSet;
        private static int _SwapRequestTypereCalculationStatusId = (int)TrackingStatusEnum.ReCalculation;
        private static int[] _SwapRequestTypeallowedStatusId =
        {
            _SwapRequestTyperequestRegisteredStatusId,
            _SwapRequestTypesetAssessmentTimeStatusId,
            _SwapRequestTypeassessmentSetResultStatusId,
            _SwapRequestTypereCalculationStatusId
        };
        private static int _SwapRequestTypesaleRequestTypeId = ReportLiterals.SaleRequestTypeId;
        private static int _SwapRequestTypeafterSaleRequestTypeId = ReportLiterals.AfterSaleRequestTypeId;
        private static string _SwapRequestTypenewRequest = "انشعاب جدید";
        private static string _SwapRequestTypeafterSaleRequest = "پس از فروش";
        static int _ArchiveRequestarchiveStatusId = (int)TrackingStatusEnum.Archived;
        static int _ArchiveRequestdeletedSatatus = (int)TrackingStatusEnum.Deleted;
        static int _ArchiveRequestreAssessmentRequest = (int)TrackingStatusEnum.ReAssessment;
        static int _ArchiveRequestreCalculationRequest = (int)TrackingStatusEnum.ReCalculation;
        static int _ArchiveRequestrequestOrigin = 12;
        static int[] _ArchiveRequestallowedRemoveArchivedStatusId =
        {
            _ArchiveRequestreAssessmentRequest,
            _ArchiveRequestreCalculationRequest
        };
        static string _ArchivedRequesttitle = "کارتابل درخواست‌های آرشیو شده";
        public RequestLifecycleService(IMoshtrakQueryService moshtrakQueryService, ITrackingQueryService trackingQueryService, IConfiguration configuration, IValidator<MoshtrakUpdateInputDto> validator, IMoshtrakService moshtrakService) : base(configuration)
        {
            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));
            _moshtrakService = moshtrakService;
            _moshtrakService.NotNull(nameof(moshtrakService));
            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<RequestCloseOuputDto> CloseRequestAsync(CloseRequestInputDto inputDto, int userName, CancellationToken cancellationToken)
        {
            TrackingOutputDto? trackingInfo = await _trackingQueryService.GetFirstStep(inputDto.TrackNumber, false);
            string deleteDescription = $"حذف توسط {userName}";
            var(moshtrakSabtUpdate, moshtrakInfo) = await CloseRequestGetMoshtrakUpdateDto(trackingInfo, inputDto, userName, deleteDescription);
            TrackingInsertDuplicateDto trackingInsertDto = new(trackingInfo?.TrackNumber ?? 0, _CloseRequestremoveRequestStatusId, deleteDescription, userName, _CloseRequestrequestOrigin, true, false);
            string dbName = GetDbName(moshtrakInfo.ZoneId);
            await CloseRequestExecSql(trackingInfo, moshtrakSabtUpdate, trackingInsertDto, dbName);
            return new RequestCloseOuputDto(moshtrakInfo.ZoneId, moshtrakInfo.ZoneTitle, inputDto.TrackNumber);
        }

        private async Task<(MoshtrakSabtUpdateDto, MoshtrakOutputDto)> CloseRequestGetMoshtrakUpdateDto(TrackingOutputDto? trackingInfo, CloseRequestInputDto inputDto, int userName, string deleteDescription)
        {
            MoshtrakOutputDto moshtrakInfo;
            if (trackingInfo == null)
            {
                moshtrakInfo = await _moshtrakQueryService.Get(inputDto.Id, inputDto.ZoneId);
                if (moshtrakInfo.IsRegistered == true)
                {
                    throw new InvalidTrackingException(ExceptionLiterals.NotFountOpenRequest);
                }
            }
            else
            {
                IEnumerable<MoshtrakOutputDto> moshtrakListInfo = await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, trackingInfo.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber);
                if (!moshtrakListInfo.Where(m => m.IsRegistered == false).Any())
                {
                    throw new InvalidTrackingException(ExceptionLiterals.NotFountOpenRequest);
                }

                moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, inputDto.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            }

            MoshtrakSabtUpdateDto moshtrakSabtUpdate = new(moshtrakInfo.Id, true, $"{moshtrakInfo.Description} - {deleteDescription}");
            return (moshtrakSabtUpdate, moshtrakInfo);
        }

        private async Task CloseRequestExecSql(TrackingOutputDto? trackingInfo, MoshtrakSabtUpdateDto moshtrakSabtUpdate, TrackingInsertDuplicateDto trackingInsertDto, string dbName)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    if (trackingInfo != null)
                    {
                        TrackingCommandService _trackingCommandService = new(connection, transaction);
                        await _trackingCommandService.UpdateIsConsiderdLatest(trackingInfo.TrackNumber, true);
                        await _trackingCommandService.InsertDuplicate(trackingInsertDto);
                    }

                    MoshtrakCommandService _moshtrackCommandService = new(connection, transaction);
                    await _moshtrackCommandService.UpdateSabt(moshtrakSabtUpdate, dbName);
                    transaction.Commit();
                }
            }
        }

        public async Task PreviousStatusRequestAsync(TrackNumberWithDescriptionInputDto inputDto, int userCode, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, inputDto.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            PreviousStatusRequestValidate(inputDto.TrackNumber, moshtrakInfo.IsRegistered);
            TrackingOutputDto previousTrackingInfo = await _trackingQueryService.GetSecondToLatest(inputDto.TrackNumber); //todo : check
            TrackingInsertDuplicateDto trackingInsertDto = new TrackingInsertDuplicateDto(inputDto.TrackNumber, previousTrackingInfo.StatusId, inputDto.Description, userCode, _PreviousStatusRequestrequestOrigin, true, false);
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    TrackingCommandService trackingCommandService = new(connection, transaction);
                    await trackingCommandService.UpdateIsConsiderdLatest(inputDto.TrackNumber, true);
                    await trackingCommandService.InsertDuplicate(trackingInsertDto);
                    transaction.Commit();
                }
            }
        }

        private void PreviousStatusRequestValidate(int trackNumber, bool isRegistered) //todo: need Or not?
        {
            if (isRegistered == true) //and not Deleted
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
            }
        }

        public async Task ReferredToIndividualRequestAsync(TrackNumberWithDescriptionInputDto inputDto, int userCode, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, inputDto.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            ReferredToIndividualRequestValidate(inputDto.TrackNumber, moshtrakInfo.IsRegistered, trackingInfo.StatusId);
            TrackingInsertDuplicateDto trackingInsertDto = new(inputDto.TrackNumber, _ReferredToIndividualRequestreferredToIndividualStatusId, inputDto.Description, userCode, _ReferredToIndividualRequestrequestOrigin, true, false);
            await ReferredToIndividualRequestSqlCommands(trackingInsertDto);
        }

        private async Task ReferredToIndividualRequestSqlCommands(TrackingInsertDuplicateDto trackingInsertDto)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    TrackingCommandService trackingCommandService = new(connection, transaction);
                    await trackingCommandService.UpdateIsConsiderdLatest(trackingInsertDto.TrackNumber, true);
                    await trackingCommandService.InsertDuplicate(trackingInsertDto);
                    transaction.Commit();
                }
            }
        }

        private void ReferredToIndividualRequestValidate(int trackNumber, bool isRegistered, int previousStatusId)
        {
            if (isRegistered == true || previousStatusId == _ReferredToIndividualRequestremoveRequestStatusId)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
            }
        }

        public async Task MoshtrakRequestUpdateAsync(MoshtrakUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await MoshtrakRequestUpdateValidate(inputDto, cancellationToken);
            MoshtrakUpdateInfoDto moshtrakUpdateDto = MoshtrakRequestUpdateGetUpdateDto(inputDto);
            await MoshtrakRequestUpdateExecSql(moshtrakUpdateDto);
        }

        private async Task MoshtrakRequestUpdateExecSql(MoshtrakUpdateInfoDto moshtrakUpdateDto)
        {
            string dbName = GetDbName(moshtrakUpdateDto.ZoneId);
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MoshtrakCommandService _moshtrackCommandService = new(connection, transaction);
                    await _moshtrackCommandService.Update(moshtrakUpdateDto, dbName);
                    transaction.Commit();
                }
            }
        }

        private async Task MoshtrakRequestUpdateValidate(MoshtrakUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await MoshtrakRequestUpdateInputValidate(inputDto, cancellationToken);
            if (inputDto.IsRegistered)
            {
                throw new InvalidTrackingException(ExceptionLiterals.CantUpdate);
            }
        }

        private async Task MoshtrakRequestUpdateInputValidate(MoshtrakUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }

        private MoshtrakUpdateInfoDto MoshtrakRequestUpdateGetUpdateDto(MoshtrakUpdateInputDto inputDto)
        {
            string blockId = string.Empty;
            MoshtrakServiceDto serviceSelected = _moshtrakService.GetServicesSelected(inputDto.SelectedServices);
            if (string.IsNullOrWhiteSpace(inputDto.BlockId) || inputDto.BlockId == "ندارد")
            {
                blockId = string.Empty;
            }
            else
            {
                blockId = inputDto.BlockId;
            }

            return new MoshtrakUpdateInfoDto()
            {
                TrackNumber = inputDto.TrackNumber,
                CustomerNumber = inputDto.CustomerNumber,
                ZoneId = inputDto.ZoneId,
                NotificationMobile = inputDto.NotificationMobile,
                UsageId = inputDto.UsageId,
                MeterDiameterId = inputDto.MeterDiameterId,
                BranchTypeId = inputDto.BranchTypeId,
                DiscountCount = inputDto.DiscountCount,
                PhoneNumber = inputDto.PhoneNumber,
                MobileNumber = inputDto.MobileNumber,
                FirstName = inputDto.FirstName,
                Surname = inputDto.Surname,
                Premises = inputDto.Premises,
                ImprovementCommercial = inputDto.ImprovementCommercial,
                ImprovementDomestic = inputDto.ImprovementDomestic,
                ImprovementOverall = inputDto.ImprovementOverall,
                Siphon100 = inputDto.Siphon100,
                Siphon125 = inputDto.Siphon125,
                Siphon150 = inputDto.Siphon150,
                Siphon200 = inputDto.Siphon200,
                ContractualCapacity = inputDto.ContractualCapacity,
                HouseValue = inputDto.HouseValue,
                CommercialUnit = inputDto.CommercialUnit,
                DomesticUnit = inputDto.DomesticUnit,
                OtherUnit = inputDto.OtherUnit,
                DiscountTypeId = inputDto.DiscountTypeId,
                NationalCode = inputDto.NationalCode,
                FatherName = inputDto.FatherName,
                PostalCode = inputDto.PostalCode,
                IsNonPermanent = inputDto.IsNonPermanent,
                Address = inputDto.Address,
                CounterType = inputDto.CounterType,
                MainSiphon = inputDto.MainSiphon,
                CommonSiphon = inputDto.CommonSiphon,
                Description = inputDto.Description,
                IsSpecial = inputDto.IsSpecial,
                ReadingNumber = inputDto.ReadingNumber,
                BlockId = blockId,
                BrokerId = inputDto.HasBroker ? 1 : 0,
                KitId = inputDto.KitId ?? 0,
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

        public async Task<SwapRequestTypeOutputDto> SwapRequestTypeAsync(SwapRequestTypeInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            TrackingOutputDto latestTrackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            SwapRequestTypeValidate(latestTrackingInfo.StatusId);
            var(serviceGroupId, serviceGroupTitle, description) = SwapRequestTypeGetServiceGroup(latestTrackingInfo.ServiceGroupId, appUser.Username);
            TrackingRequestTypeUpdateDto swapRequestTypeDto = new(inputDto.TrackNumber, serviceGroupId, description);
            MoshtrakServicesUpdateDto moshtrakServicesUpdateDto = SwapRequestTypeGetMoshtrakServicesDto(inputDto);
            string dbName = GetDbName(latestTrackingInfo.ZoneId);
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    TrackingCommandService trackingCommandService = new(connection, transaction);
                    MoshtrakCommandService moshtrakCommandService = new(connection, transaction);
                    await trackingCommandService.SwapRequestType(swapRequestTypeDto);
                    await moshtrakCommandService.Update(moshtrakServicesUpdateDto, dbName);
                    transaction.Commit();
                }
            }

            return new SwapRequestTypeOutputDto(inputDto.TrackNumber, serviceGroupId, serviceGroupTitle, inputDto.SelectedServices);
        }

        private void SwapRequestTypeValidate(int latestStatusId)
        {
            if (!_SwapRequestTypeallowedStatusId.Contains(latestStatusId))
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
            }
        }

        private (int, string, string) SwapRequestTypeGetServiceGroup(int previousServiceTypeId, string userName)
        {
            int requestTypeId = previousServiceTypeId == _SwapRequestTypesaleRequestTypeId ? _SwapRequestTypeafterSaleRequestTypeId : _SwapRequestTypesaleRequestTypeId;
            string requestTypeTitle = requestTypeId == _SwapRequestTypesaleRequestTypeId ? _SwapRequestTypenewRequest : _SwapRequestTypeafterSaleRequest;
            string description = $@"-نوع درخواست توسط{userName} به '{requestTypeTitle}' تغیر یافت.";
            return new(requestTypeId, requestTypeTitle, description);
        }

        private MoshtrakServicesUpdateDto SwapRequestTypeGetMoshtrakServicesDto(SwapRequestTypeInputDto inputDto)
        {
            MoshtrakServiceDto moshtrakServices = _moshtrakService.GetServicesSelected(inputDto.SelectedServices);
            return new MoshtrakServicesUpdateDto()
            {
                TrackNumber = inputDto.TrackNumber,
                s0 = moshtrakServices.s0,
                s1 = moshtrakServices.s1,
                s2 = moshtrakServices.s2,
                s3 = moshtrakServices.s3,
                s4 = moshtrakServices.s4,
                s5 = moshtrakServices.s5,
                s8 = moshtrakServices.s8,
                s9 = moshtrakServices.s9,
                s10 = moshtrakServices.s10,
                s11 = moshtrakServices.s11,
                s12 = moshtrakServices.s12,
                s13 = moshtrakServices.s13,
                s14 = moshtrakServices.s14,
                s15 = moshtrakServices.s15,
                s16 = moshtrakServices.s16,
                s17 = moshtrakServices.s17,
                s18 = moshtrakServices.s18,
                s19 = moshtrakServices.s19,
                s20 = moshtrakServices.s20,
                s21 = moshtrakServices.s21,
                s22 = moshtrakServices.s22,
                s23 = moshtrakServices.s23,
                s24 = moshtrakServices.s24,
                s25 = moshtrakServices.s25,
                s26 = moshtrakServices.s26,
                s27 = moshtrakServices.s27,
                s28 = moshtrakServices.s28,
                s29 = moshtrakServices.s29,
                s30 = moshtrakServices.s30,
                s31 = moshtrakServices.s31,
                s32 = moshtrakServices.s32,
                s33 = moshtrakServices.s33,
                s34 = moshtrakServices.s34,
                s35 = moshtrakServices.s35,
                s36 = moshtrakServices.s36,
                s37 = moshtrakServices.s37,
                s38 = moshtrakServices.s38,
                s39 = moshtrakServices.s39,
                s40 = moshtrakServices.s40,
                s41 = moshtrakServices.s41,
                s42 = moshtrakServices.s42,
                s43 = moshtrakServices.s43,
                s44 = moshtrakServices.s44,
                s45 = moshtrakServices.s45,
                s46 = moshtrakServices.s46,
                s47 = moshtrakServices.s47,
                s48 = moshtrakServices.s48,
            };
        }

        public async Task ArchiveRequestAsync(ArchiveRequestInputDto inputDto, int userCode, CancellationToken cancellationToken)
        {
            TrackingOutputDto latestTrackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.GetValid(new MoshtrakGetDto(latestTrackingInfo.ZoneId, null, null, inputDto.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            ArchiveRequestValidation(inputDto, latestTrackingInfo.StatusId, moshtrakInfo.IsRegistered);
            TrackingInsertDuplicateDto trackingInsertDto = ArchiveRequestGetTrackingInsertDto(inputDto, userCode);
            string dbName = GetDbName(latestTrackingInfo.ZoneId);
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    TrackingCommandService trackingCommandService = new(connection, transaction);
                    await trackingCommandService.UpdateIsConsiderdLatest(inputDto.TrackNumber, true);
                    await trackingCommandService.InsertDuplicate(trackingInsertDto);
                    transaction.Commit();
                }
            }
        }

        private void ArchiveRequestValidation(ArchiveRequestInputDto inputDto, int previousStatusId, bool isRegistered) //todo: need Or not?
        {
            if (isRegistered == true || previousStatusId == _ArchiveRequestdeletedSatatus || (previousStatusId == _ArchiveRequestarchiveStatusId)) //and not Deleted
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
            }
        }

        private TrackingInsertDuplicateDto ArchiveRequestGetTrackingInsertDto(ArchiveRequestInputDto inputDto, int userCode)
        {
            return new TrackingInsertDuplicateDto(inputDto.TrackNumber, _ArchiveRequestarchiveStatusId, inputDto.Description, userCode, _ArchiveRequestrequestOrigin, true, false);
        }

        public async Task<ReportOutput<TrackingKartableHeaderOutputDto, TrackingKartableDataOutputDto>> ArchivedRequestAsync(CancellationToken cancellationToken)
        {
            IEnumerable<TrackingKartableDataOutputDto> data = await _trackingQueryService.GetAllArchivedRequest();
            TrackingKartableHeaderOutputDto header = new()
            {
                ZoneCount = data?.DistinctBy(d => d.ZoneId)?.Count() ?? 0,
                RequestCount = data?.Count() ?? 0,
                CurrentDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = _ArchivedRequesttitle,
            };
            return new ReportOutput<TrackingKartableHeaderOutputDto, TrackingKartableDataOutputDto>(_ArchivedRequesttitle, header, data);
        }
    }
}
