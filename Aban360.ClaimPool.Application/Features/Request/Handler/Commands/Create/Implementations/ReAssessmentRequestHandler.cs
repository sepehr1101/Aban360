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

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class ReAssessmentRequestHandler : AbstractBaseConnection, IReAssessmentRequestHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly IExaminationQueryService _assessmentQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        static int _reAssessmentStatusId = 15;
        static int _deletedSatatus = 90000;
        static int _requestOrigin = 12;
        static int _afterSaleRequestType = 2;
        public ReAssessmentRequestHandler(
            ITrackingQueryService trackingQueryService,
            IMoshtrakQueryService moshtrakQueryService,
            IExaminationQueryService assessmentQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            IConfiguration configuration)
            : base(configuration)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _assessmentQueryService = assessmentQueryService;
            _assessmentQueryService.NotNull(nameof(assessmentQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));
        }

        public async Task Handle(TrackNumberWithDescriptionInputDto inputDto, int userCode, CancellationToken cancellationToken)
        {
            TrackingOutputDto latestTrackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(latestTrackingInfo.ZoneId, null, null, inputDto.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            Validation(inputDto.TrackNumber, latestTrackingInfo.StatusId, moshtrakInfo.IsRegistered);

            TrackingInsertDuplicateDto trackingInsertDto = GetTrackingInsertDto(inputDto, userCode);

            await SqlCommands(latestTrackingInfo, inputDto, trackingInsertDto, moshtrakInfo);
        }
        private void Validation(int trackNumber, int statusId, bool isRegistered)//todo: need Or not?
        {
            if (isRegistered == true || statusId == _deletedSatatus)//and not Deleted
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
            }
        }
        private TrackingInsertDuplicateDto GetTrackingInsertDto(TrackNumberWithDescriptionInputDto inputDto, int userCode)
        {
            return new TrackingInsertDuplicateDto(inputDto.TrackNumber, _reAssessmentStatusId, inputDto.Description, userCode, _requestOrigin, true, false);
        }
        private async Task<MoshtrakUpdateCustomerInfoDto> GetMoshtrakUpdateDto(MoshtrakOutputDto moshtrakInfo)
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
                Siphon100 = memberInfo.Siphon1,
                Siphon125 = memberInfo.Siphon2,
                Siphon150 = memberInfo.Siphon3,
                Siphon200 = memberInfo.Siphon4,
                MainSiphon = int.Parse(memberInfo.MainSiphon),
                CommonSiphon = memberInfo.CommonSiphon1,
                MeterDiameterId = memberInfo.MeterDiameterId,
                DiscountTypeId = memberInfo.DiscountId,
                DiscountCount = memberInfo.DiscountCount,
            };
        }

        private async Task SqlCommands(TrackingOutputDto latestTrackingInfo, TrackNumberWithDescriptionInputDto inputDto, TrackingInsertDuplicateDto trackingInsertDto, MoshtrakOutputDto moshtrakInfo)
        {
            string dbName = GetDbName(latestTrackingInfo.ZoneId);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    TrackingCommandService trackingCommandService = new(connection, transaction);
                    MoshtrakCommandService moshtrakCommandService = new(connection, transaction);

                    await trackingCommandService.UpdateIsConsiderdLatest(inputDto.TrackNumber, true);
                    await trackingCommandService.InsertDuplicate(trackingInsertDto);

                    if (latestTrackingInfo.ServiceGroupId == _afterSaleRequestType)
                    {
                        MoshtrakUpdateCustomerInfoDto moshtrackUpdateDto = await GetMoshtrakUpdateDto(moshtrakInfo);
                        await moshtrakCommandService.Update(moshtrackUpdateDto, dbName);
                    }

                    transaction.Commit();
                }
            }
        }
    }
}
