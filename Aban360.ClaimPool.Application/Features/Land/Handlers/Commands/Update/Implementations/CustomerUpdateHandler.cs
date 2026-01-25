using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using System.Transactions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class CustomerUpdateHandler : ICustomerUpdateHandler
    {
        private readonly ISubscriptionQueryService _customerQueryService;
        private readonly IArchMemCommandService _archMemCommandService;
        private readonly IMembersCommandService _membersCommandService;
        public CustomerUpdateHandler(
            ISubscriptionQueryService customerQueryService,
            IArchMemCommandService archMemCommandService,
            IMembersCommandService membersCommandService)
        {
            _customerQueryService = customerQueryService;
            _customerQueryService.NotNull(nameof(customerQueryService));

            _archMemCommandService = archMemCommandService;
            _archMemCommandService.NotNull(nameof(archMemCommandService));

            _membersCommandService = membersCommandService;
            _membersCommandService.NotNull(nameof(membersCommandService));
        }

        public async Task Handle(SubscriptionGetDto inputDto, CancellationToken cancellationToken)
        {
            SubscriptionGetDto previousSubscription = await _customerQueryService.GetInfo(inputDto.BillId);
            if (previousSubscription == null)
            {
                throw new BaseException("شناسه قبض یافت نشد");
            }
            CustomerUpdateDto subscriptionUpdate = GetCustomerUpdateDto(inputDto, previousSubscription);

            //System.Transactions.TransactionManager.ImplicitDistributedTransactions = true;
            //using (TransactionScope transaction = TransactionBuilder.Create(0, 3, IsolationLevel.ReadCommitted))
            //{
            await _archMemCommandService.Insert(subscriptionUpdate);
            await _membersCommandService.Update(subscriptionUpdate);

            //    transaction.Complete();
            //}
        }
        private CustomerUpdateDto GetCustomerUpdateDto(SubscriptionGetDto inputDto, SubscriptionGetDto previousSubscription)
        {
            return new CustomerUpdateDto()
            {
                Id = previousSubscription.Id,
                CustomerNumber = previousSubscription.CustomerNumber,
                ZoneId = previousSubscription.ZoneId,
                BillId = inputDto.BillId,
                X = inputDto.X,
                Y = inputDto.Y,
                ReadingNumber = inputDto.ReadingNumber,
                FirstName = inputDto.FirstName,
                SurName = inputDto.SurName,
                Address = inputDto.Address,
                PostalCode = inputDto.PostalCode,
                Plaque = inputDto.Plaque,
                NationalCode = inputDto.NationalCode,
                PhoneNumber = inputDto.PhoneNumber,
                MobileNumber = inputDto.MobileNumber,
                FatherName = inputDto.FatherName,
                BranchTypeId = inputDto.BranchTypeId,
                UsageSellId = inputDto.UsageSellId,
                UsageConsumptionId = inputDto.UsageConsumptionId,
                EmptyUnit = inputDto.EmptyUnit,
                CommertialUnit = inputDto.CommertialUnit,
                DomesticUnit = inputDto.DomesticUnit,
                OtherUnit = inputDto.OtherUnit,
                HouseholdDateJalali = inputDto.HouseholdDateJalali ?? string.Empty,
                HouseholdNumber = inputDto.HouseholdNumber,
                MeterDiamterId = inputDto.MeterDiamterId,
                IsSpecial = inputDto.IsSpecial,
                ContractualCapacity = inputDto.ContractualCapacity,
                ImprovementCommertial = inputDto.ImprovementCommertial,
                ImprovementDomestic = inputDto.ImprovementDomestic,
                ImprovementOverall = inputDto.ImprovementOverall,
                Premises = inputDto.Premises,
                Operator = inputDto.Operator,
                SewageInstallationDateJalali = inputDto.SewageInstallationDateJalali ?? string.Empty,
                SewageRequestDateJalali = inputDto.SewageRequestDateJalali ?? string.Empty,
                WaterInstallationDateJalali = inputDto.WaterInstallationDateJalali,
                WaterRequestDateJalali = inputDto.WaterRequestDateJalali,
                Siphon100 = inputDto.Siphon100,
                Siphon125 = inputDto.Siphon125,
                Siphon150 = inputDto.Siphon150,
                Siphon200 = inputDto.Siphon200,
                Siphon5 = inputDto.Siphon5,
                Siphon6 = inputDto.Siphon6,
                Siphon7 = inputDto.Siphon7,
                Siphon8 = inputDto.Siphon8,
                MainSiphon = inputDto.MainSiphon,
            };
        }
    }
}
