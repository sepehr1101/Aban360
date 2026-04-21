using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Implementations
{
    internal sealed class SwapRequestTypeHandler : AbstractBaseConnection, ISwapRequestTypeHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        static int[] _allowedStatusId = { 0, 10, 20, 65 };
        public SwapRequestTypeHandler(
            ITrackingQueryService trackingQueryService,
            IConfiguration configuration)
            : base(configuration)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));
        }

        public async Task Handle(SwapRequestTypeInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            TrackingOutputDto latestTrackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            Validation(latestTrackingInfo.StatusId);

            TrackingRequestTypeUpdateDto swapRequestTypeDto = GetTrackingRequestTypeUpdateDto(inputDto.TrackNumber, latestTrackingInfo.ServiceGroupId, appUser.Username);
            MoshtrakServicesUpdateDto moshtrakServicesUpdateDto = GetMoshtrakServicesDto(inputDto);
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
        }
        private void Validation(int latestStatusId)
        {
            if (!_allowedStatusId.Contains(latestStatusId))
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
            }
        }
        private TrackingRequestTypeUpdateDto GetTrackingRequestTypeUpdateDto(int trackNumber, int previousServiceTypeId, string userName)
        {
            int requestTypeId = previousServiceTypeId == 1 ? 2 : 1;
            string requestTypeTitle = requestTypeId == 1 ? "انشعاب جدید" : "پس از فروش";
            string description = $@"-نوع درخواست توسط{userName} به '{requestTypeTitle}' تغیر یافت.";

            return new(trackNumber, requestTypeId, description);
        }
        private MoshtrakServicesUpdateDto GetMoshtrakServicesDto(SwapRequestTypeInputDto inputDto)
        {
            MoshtrakServiceDto moshtrakServices = MoshtrakService.GetServicesSelected(inputDto.SelectedServices);

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
    }
}
