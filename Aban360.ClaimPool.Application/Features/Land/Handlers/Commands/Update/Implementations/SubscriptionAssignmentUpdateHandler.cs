using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class SubscriptionAssignmentUpdateHandler : AbstractBaseConnection, ISubscriptionAssignmentUpdateHandler
    {
        private readonly ISubscriptionQueryService _subscriptionAssignmentQueryService;

        public SubscriptionAssignmentUpdateHandler(
            IConfiguration configuration,
            ISubscriptionQueryService subscriptionAssignmentQueryService)
            : base(configuration)
        {
            _subscriptionAssignmentQueryService = subscriptionAssignmentQueryService;
            _subscriptionAssignmentQueryService.NotNull(nameof(subscriptionAssignmentQueryService));
        }

        public async Task Handle(SubscriptionAssignmentUpdateDto updateDto, CancellationToken cancellationToken)
        {
            SubscriptionGetDto previousSubscription = await _subscriptionAssignmentQueryService.GetInfo(updateDto.BillId);
            if (previousSubscription == null)
            {
                throw new BaseException("شناسه قبض یافت نشد");
            }
            CustomerUpdateDto subscriptionUpdate = GetCustomerUpdateDto(updateDto, previousSubscription);

            using (IDbConnection connection = _sqlReportConnection)
            {                
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    ArchMemCommandService _archMemCommandService = new (_sqlReportConnection, transaction);
                    MembersCommandService _membersCommandService = new (_sqlReportConnection, transaction);

                    await _archMemCommandService.Insert(subscriptionUpdate);
                    await _membersCommandService.Update(subscriptionUpdate);

                    transaction.Commit();
                }
            }
        }
        private CustomerUpdateDto GetCustomerUpdateDto(SubscriptionAssignmentUpdateDto inputDto, SubscriptionGetDto previousSubscription)
        {
            return new CustomerUpdateDto()
            {
                Id = inputDto.Id,
                CustomerNumber = previousSubscription.CustomerNumber,
                ZoneId = previousSubscription.ZoneId,
                BillId = previousSubscription.BillId,
                X = inputDto.X,
                Y = inputDto.Y,
                ReadingNumber = inputDto.ReadingNumber,
                FirstName = previousSubscription.FirstName,
                Surname = previousSubscription.Surname,
                Address = inputDto.Address,
                PostalCode = inputDto.PostalCode,
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
                MeterDiamterId = previousSubscription.MeterDiameterId,
                IsSpecial = previousSubscription.IsSpecial,
                ContractualCapacity = previousSubscription.ContractualCapacity,
                ImprovementCommertial = previousSubscription.ImprovementCommertial,
                ImprovementDomestic = previousSubscription.ImprovementDomestic,
                ImprovementOverall = previousSubscription.ImprovementOverall,
                Premises = previousSubscription.Premises,
                Operator = previousSubscription.Operator,
                SewageInstallationDateJalali = previousSubscription.SewageInstallationDateJalali,
                SewageRequestDateJalali = previousSubscription.SewageRequestDateJalali,
                WaterInstallationDateJalali = previousSubscription.MeterInstallationDateJalali,
                WaterRequestDateJalali = previousSubscription.MeterRequestDateJalali,
                Siphon100 = previousSubscription.Siphon100,
                Siphon125 = previousSubscription.Siphon125,
                Siphon150 = previousSubscription.Siphon150,
                Siphon200 = previousSubscription.Siphon200,
                Siphon5 = previousSubscription.Siphon5,
                Siphon6 = previousSubscription.Siphon6,
                Siphon7 = previousSubscription.Siphon7,
                Siphon8 = previousSubscription.Siphon8,
                MainSiphon = previousSubscription.MainSiphon,

            };
        }
    }
}
