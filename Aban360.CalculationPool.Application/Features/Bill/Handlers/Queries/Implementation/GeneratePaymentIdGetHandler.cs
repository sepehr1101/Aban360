using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    internal sealed class GeneratePaymentIdGetHandler : IGeneratePaymentIdGetHandler
    {
        private readonly ICommonMemberQueryService _memberQueryService;
        private readonly ICommonZoneService _zoneService;
        public GeneratePaymentIdGetHandler(
            ICommonMemberQueryService memberQueryService,
            ICommonZoneService zoneService)
        {
            _memberQueryService = memberQueryService;
            _memberQueryService.NotNull(nameof(memberQueryService));

            _zoneService = zoneService;
            _zoneService.NotNull(nameof(zoneService));
        }

        public async Task<string> Handle(GeneratePaymentIdInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _memberQueryService.Get(inputDto.BillId);
            await _zoneService.IsUserInZone(appUser, zoneIdAndCustomerNumber.ZoneId);

            string paymentTypeOption = inputDto.IsWater ? CommonLiterals.WaterPayIdUniqueCode : CommonLiterals.BranchPayIdUniqueCode;
            string paymentId = TransactionIdGenerator.GeneratePaymentId(inputDto.Amount, inputDto.BillId, $"{paymentTypeOption}00");

            return paymentId;
        }
    }
}
