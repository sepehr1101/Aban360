using Aban360.ClaimPool.Application.Features.Base;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class RequestDuplicateValidation : IRequestDuplicateValidation
    {
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        public RequestDuplicateValidation(
            IMoshtrakQueryService moshtrakQueryService,
            ITrackingQueryService trackingQueryService,
            ICommonMemberQueryService commonMemberQueryService)
        {
            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));
        }

        public async Task<TrackingDuplicateValidationOutputDto> Handle(TrackingDuplicateValidationInputDto inputDto, CancellationToken cancellationToken)
        {
            MoshtrakGetDto moshtrakSearch = new();
            MoshtrakSearchTypeEnum moshtrakSearchType = 0;
            if (inputDto.ValidationType == TrackingDuplicateValidationTypeEnum.ByNationalCode)
            {
                ZoneIdAndCustomerNumber neighbourInfo = await _commonMemberQueryService.Get(inputDto.NeighbourBillId);
                moshtrakSearch = new(neighbourInfo.ZoneId, null, inputDto.NationalCode, null);
                moshtrakSearchType = MoshtrakSearchTypeEnum.ByNationalCode;
            }
            if (inputDto.ValidationType == TrackingDuplicateValidationTypeEnum.ByBillId)
            {
                ZoneIdAndCustomerNumber customerInfo = await _commonMemberQueryService.Get(inputDto.BillId);
                moshtrakSearch = new(customerInfo.ZoneId, customerInfo.CustomerNumber, null, null);
                moshtrakSearchType = MoshtrakSearchTypeEnum.ByCustomerNumber;
            }
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(moshtrakSearch, moshtrakSearchType)).FirstOrDefault();
            TrackingOutputDto latestTrackingInfo = await _trackingQueryService.GetLatest(moshtrakInfo.TrackNumber);
            TrackingOutputDto firstTrackingInfo = await _trackingQueryService.GetFirstStep(moshtrakInfo.TrackNumber);
            NumericDictionary? requestOrigin = RequestOrigin.GetRequestOrigin(firstTrackingInfo.RequestOriginId);

            return new TrackingDuplicateValidationOutputDto()
            {
                ZoneId = moshtrakInfo.ZoneId,
                CustomerNumber = moshtrakInfo.CustomerNumber,
                NationalCode = moshtrakInfo.NationalCode,
                TrackNumber = moshtrakInfo.TrackNumber,
                FirstName = moshtrakInfo.FirstName,
                Surname = moshtrakInfo.Surname,
                FatherName = moshtrakInfo.FatherName,
                MobileNumber = moshtrakInfo.MobileNumber,
                RequestDateJalali = moshtrakInfo.RequestDateJalali,
                IsDuplicate = moshtrakInfo.IsRegistered ? false : true,
                LatestStatusId = latestTrackingInfo.StatusId,
                LatestStatusTitle = latestTrackingInfo.StatusTitle,
                RequestOriginId = firstTrackingInfo.RequestOriginId,
                RequestOrigin = requestOrigin?.Title ?? string.Empty,
            };
        }
    }
}
