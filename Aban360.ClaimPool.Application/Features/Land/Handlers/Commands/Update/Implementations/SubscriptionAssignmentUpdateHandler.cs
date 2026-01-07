using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class SubscriptionAssignmentUpdateHandler : ISubscriptionAssignmentUpdateHandler
    {
        private readonly ISubscriptionAssignmentQueryService _subscriptionAssignmentQueryService;
        private readonly ISubscriptionAssignmentCommandService _subscriptionAssignmentCommandService;
        public SubscriptionAssignmentUpdateHandler(
            ISubscriptionAssignmentQueryService subscriptionAssignmentQueryService,
            ISubscriptionAssignmentCommandService subscriptionAssignmentCommandService)
        {
            _subscriptionAssignmentQueryService = subscriptionAssignmentQueryService;
            _subscriptionAssignmentQueryService.NotNull(nameof(subscriptionAssignmentQueryService));

            _subscriptionAssignmentCommandService = subscriptionAssignmentCommandService;
            _subscriptionAssignmentCommandService.NotNull(nameof(subscriptionAssignmentCommandService));
        }

        public async Task Handle(SubscriptionAssignmentUpdateDto updateDto, CancellationToken cancellationToken)
        {
            SubscriptionGetDto previousSubscription = await _subscriptionAssignmentQueryService.GetInfo(updateDto.BillId);
            if (previousSubscription == null)
            {
                throw new BaseException("شناسه قبض یافت نشد");
            }

            SubscriptionUpdateDto subscriptionUpdate = new()
            {
                Id = updateDto.Id,
                CustomerNumber=previousSubscription.CustomerNumber,
                ZoneId=previousSubscription.ZoneId,
                BillId = updateDto.BillId,
                X = updateDto.X,
                Y = updateDto.Y,
                ReadingNumber = updateDto.ReadingNumber,
                FirstName = updateDto.FirstName,
                SurName = updateDto.SurName,
                Address = updateDto.Address,
                PostalCode = updateDto.PostalCode,
                Plaque = previousSubscription.Plaque,
                NationalCode = previousSubscription.NationalCode,
                PhoneNumber = previousSubscription.PhoneNumber,
                MobileNumber = previousSubscription.MobileNumber,
                FatherName = previousSubscription.FatherName,
                BranchTypeId = previousSubscription.BranchTypeId,
                UsageSellId = previousSubscription.UsageSellId,
                UsageConsumptionId = previousSubscription.UsageConsumptionId,
                EmptyUnit = previousSubscription.EmptyUnit,
                CommertialUnit = previousSubscription.CommertialUnit,
                DomesticUnit = previousSubscription.DomesticUnit,
                OtherUnit = previousSubscription.OtherUnit,
                HouseholdDateJalali = previousSubscription.HouseholdDateJalali,
                HouseholdNumber = previousSubscription.HouseholdNumber,
                MeterDiamterId = previousSubscription.MeterDiamterId,
                IsSpecial = previousSubscription.IsSpecial,
                ContractualCapacity = previousSubscription.ContractualCapacity,
            };

            //update
            await _subscriptionAssignmentCommandService.Update(subscriptionUpdate);
        }
    }
}
