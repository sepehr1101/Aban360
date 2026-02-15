using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Microsoft.Extensions.Configuration;
using System.Data;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.Common.Literals;
using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class CustomerUpdateHandler : AbstractBaseConnection, ICustomerUpdateHandler
    {
        private readonly ISubscriptionQueryService _customerQueryService;
        public CustomerUpdateHandler(
            ISubscriptionQueryService customerQueryService,
            IConfiguration configuration)
            : base(configuration)
        {
            _customerQueryService = customerQueryService;
            _customerQueryService.NotNull(nameof(customerQueryService));
        }

        public async Task Handle(SubscriptionGetDto inputDto, CancellationToken cancellationToken)
        {
            SubscriptionGetDto previousSubscription = await GetConsumptionPreviousInfo(inputDto.BillId);
            CustomerUpdateDto customerUpdate = GetCustomerUpdate(inputDto, previousSubscription);

            await UpdateCustomer(customerUpdate);
        }
        public async Task Handle(CustomerUpdate1Dto inputDto, CancellationToken cancellationToken)
        {
            SubscriptionGetDto previousSubscription = await GetConsumptionPreviousInfo(inputDto.BillId);
            CustomerUpdateDto customerUpdate = GetCustomerUpdate(inputDto, previousSubscription);

            await UpdateCustomer(customerUpdate);
        }
        public async Task Handle(CustomerUpdate2Dto inputDto, CancellationToken cancellationToken)
        {
            SubscriptionGetDto previousSubscription = await GetConsumptionPreviousInfo(inputDto.BillId);
            CustomerUpdateDto customerUpdate = GetCustomerUpdate(inputDto, previousSubscription);

            await UpdateCustomer(customerUpdate);
        }
        public async Task Handle(CustomerUpdate3Dto inputDto, CancellationToken cancellationToken)
        {
            SubscriptionGetDto previousSubscription = await GetConsumptionPreviousInfo(inputDto.BillId);
            CustomerUpdateDto customerUpdate = GetCustomerUpdate(inputDto, previousSubscription);

            await UpdateCustomer(customerUpdate);
        }
        public async Task Handle(CustomerUpdate5Dto inputDto, CancellationToken cancellationToken)
        {
            SubscriptionGetDto previousSubscription = await GetConsumptionPreviousInfo(inputDto.BillId);
            CustomerUpdateDto customerUpdate = GetCustomerUpdate(inputDto, previousSubscription);

            await UpdateCustomer(customerUpdate);
        }
        public async Task Handle(ServiceLinkConnectionInput inputDto, int deletionStateId, CancellationToken cancellationToken)
        {
            
            //Todo: جلوگیری از ثبت دوباره وضعیت جاری
            SubscriptionGetDto previousSubscription = await GetConsumptionPreviousInfo(inputDto.BillId);
            CustomerUpdateDto customerUpdate = GetCustomerUpdate(inputDto, deletionStateId, previousSubscription);

            await UpdateCustomerAndClient(customerUpdate);
        }
        private async Task UpdateCustomer(CustomerUpdateDto updateDto)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    ArchMemCommandService _archMemCommandService = new(_sqlReportConnection, transaction);
                    MembersCommandService _membersCommandService = new(_sqlReportConnection, transaction);
                    //string dbName = GetDbName(updateDto.ZoneId);
                    string dbName = "Atlas";

                    int rowId = await _archMemCommandService.Insert(updateDto, dbName);
                    await _membersCommandService.Update(updateDto, dbName);

                    transaction.Commit();
                }
            }
        }
        private async Task UpdateCustomerAndClient(CustomerUpdateDto updateDto)
        {
            ZoneIdCustomerNumber zoneIdAndCustomer = new(updateDto.ZoneId,updateDto.CustomerNumber.ToString());
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    ArchMemCommandService _archMemCommandService = new(_sqlReportConnection, transaction);
                    MembersCommandService _membersCommandService = new(_sqlReportConnection, transaction);
                    ClientsCommandService _clientCommandService = new(_sqlReportConnection, transaction);
                    string dbName = GetDbName(updateDto.ZoneId);
                    //string dbName = "Atlas";

                    int rowId = await _archMemCommandService.Insert(updateDto, dbName);
                    await _membersCommandService.Update(updateDto, dbName);
                    await _clientCommandService.UpdateToDayJalali(zoneIdAndCustomer, updateDto.ToDayDateJalali);
                    await _clientCommandService.InsertByArchMemId(rowId, dbName);

                    transaction.Commit();
                }
            }
        }

        private CustomerUpdateDto GetCustomerUpdate(SubscriptionGetDto inputDto, SubscriptionGetDto previousSubscription)
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
                Surname = inputDto.Surname,
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
                HouseholdDateJalali = DateValidation(inputDto.HouseholdDateJalali, true),
                HouseholdNumber = inputDto.HouseholdNumber,
                MeterDiamterId = inputDto.MeterDiameterId,
                IsSpecial = inputDto.IsSpecial,
                ContractualCapacity = inputDto.ContractualCapacity,
                ImprovementCommertial = inputDto.ImprovementCommertial,
                ImprovementDomestic = inputDto.ImprovementDomestic,
                ImprovementOverall = inputDto.ImprovementOverall,
                Premises = inputDto.Premises,
                Operator = inputDto.Operator,
                SewageInstallationDateJalali = DateValidation(inputDto.SewageInstallationDateJalali, false),
                SewageRequestDateJalali = DateValidation(inputDto.SewageRequestDateJalali, false),
                MeterInstallationDateJalali = DateValidation(inputDto.MeterInstallationDateJalali, true),
                MeterRequestDateJalali = DateValidation(inputDto.MeterRequestDateJalali, true),
                Siphon100 = inputDto.Siphon100,
                Siphon125 = inputDto.Siphon125,
                Siphon150 = inputDto.Siphon150,
                Siphon200 = inputDto.Siphon200,
                Siphon5 = inputDto.Siphon5,
                Siphon6 = inputDto.Siphon6,
                Siphon7 = inputDto.Siphon7,
                Siphon8 = inputDto.Siphon8,
                MainSiphon = inputDto.MainSiphon,
                DeletionStateId = inputDto.DeletionStateId,
                BodySerial = inputDto.BodySerial ?? string.Empty,
                CommonSiphon = inputDto.CommonSiphon,
                MeterRegisterDateJalali = DateValidation(inputDto.MeterRegisterDateJalali, true),
                SewageRegisterDateJalali = DateValidation(inputDto.SewageRegisterDateJalali, false),
                GuildId = inputDto.GuildId
            };
        }
        private CustomerUpdateDto GetCustomerUpdate(CustomerUpdate1Dto inputDto, SubscriptionGetDto previousSubscription)
        {
            return new CustomerUpdateDto()
            {
                Id = previousSubscription.Id,
                CustomerNumber = previousSubscription.CustomerNumber,
                ZoneId = previousSubscription.ZoneId,
                BillId = inputDto.BillId,
                X = previousSubscription.X,
                Y = previousSubscription.Y,
                ReadingNumber = previousSubscription.ReadingNumber,
                FirstName = previousSubscription.FirstName,
                Surname = previousSubscription.Surname,
                Address = previousSubscription.Address,
                PostalCode = previousSubscription.PostalCode,
                Plaque = previousSubscription.Plaque,
                NationalCode = previousSubscription.NationalCode,
                PhoneNumber = previousSubscription.PhoneNumber,
                MobileNumber = previousSubscription.MobileNumber,
                FatherName = previousSubscription.FatherName,
                BranchTypeId = previousSubscription.BranchTypeId,
                UsageSellId = previousSubscription.UsageSellId,
                UsageConsumptionId = inputDto.UsageConsumptionId,
                EmptyUnit = inputDto.EmptyUnit,
                CommertialUnit = inputDto.CommertialUnit,
                DomesticUnit = inputDto.DomesticUnit,
                OtherUnit = inputDto.OtherUnit,
                HouseholdDateJalali = DateValidation(previousSubscription.HouseholdDateJalali, true),
                HouseholdNumber = previousSubscription.HouseholdNumber,
                MeterDiamterId = previousSubscription.MeterDiameterId,
                IsSpecial = previousSubscription.IsSpecial,
                ContractualCapacity = inputDto.ContractualCapacity,
                ImprovementCommertial = inputDto.ImprovementCommertial,
                ImprovementDomestic = inputDto.ImprovementDomestic,
                ImprovementOverall = inputDto.ImprovementOverall,
                Premises = previousSubscription.Premises,
                Operator = previousSubscription.Operator,
                SewageInstallationDateJalali = DateValidation(previousSubscription.SewageInstallationDateJalali, false),
                SewageRequestDateJalali = DateValidation(previousSubscription.SewageRequestDateJalali, false),
                MeterInstallationDateJalali = DateValidation(previousSubscription.MeterInstallationDateJalali, true),
                MeterRequestDateJalali = DateValidation(previousSubscription.MeterRequestDateJalali, true),
                Siphon100 = previousSubscription.Siphon100,
                Siphon125 = previousSubscription.Siphon125,
                Siphon150 = previousSubscription.Siphon150,
                Siphon200 = previousSubscription.Siphon200,
                Siphon5 = previousSubscription.Siphon5,
                Siphon6 = previousSubscription.Siphon6,
                Siphon7 = previousSubscription.Siphon7,
                Siphon8 = previousSubscription.Siphon8,
                MainSiphon = previousSubscription.MainSiphon,
                DeletionStateId = previousSubscription.DeletionStateId,
                BodySerial = previousSubscription.BodySerial ?? string.Empty,
                CommonSiphon = previousSubscription.CommonSiphon,
                MeterRegisterDateJalali = DateValidation(previousSubscription.MeterRegisterDateJalali, true),
                SewageRegisterDateJalali = DateValidation(previousSubscription.SewageRegisterDateJalali, false),
                GuildId = inputDto.GuildId
            };
        }
        private CustomerUpdateDto GetCustomerUpdate(CustomerUpdate2Dto inputDto, SubscriptionGetDto previousSubscription)
        {
            return new CustomerUpdateDto()
            {
                Id = previousSubscription.Id,
                CustomerNumber = previousSubscription.CustomerNumber,
                ZoneId = previousSubscription.ZoneId,
                BillId = inputDto.BillId,
                X = previousSubscription.X,
                Y = previousSubscription.Y,
                ReadingNumber = previousSubscription.ReadingNumber,
                FirstName = previousSubscription.FirstName,
                Surname = previousSubscription.Surname,
                Address = previousSubscription.Address,
                PostalCode = previousSubscription.PostalCode,
                Plaque = previousSubscription.Plaque,
                NationalCode = previousSubscription.NationalCode,
                PhoneNumber = previousSubscription.PhoneNumber,
                MobileNumber = previousSubscription.MobileNumber,
                FatherName = previousSubscription.FatherName,
                BranchTypeId = inputDto.BranchTypeId,
                UsageSellId = previousSubscription.UsageSellId,
                UsageConsumptionId = previousSubscription.UsageConsumptionId,
                EmptyUnit = inputDto.EmptyUnit,
                CommertialUnit = previousSubscription.CommertialUnit,
                DomesticUnit = previousSubscription.DomesticUnit,
                OtherUnit = previousSubscription.OtherUnit,
                HouseholdDateJalali = DateValidation(previousSubscription.HouseholdDateJalali, false),
                HouseholdNumber = inputDto.HouseholdNumber,
                MeterDiamterId = previousSubscription.MeterDiameterId,
                IsSpecial = inputDto.IsSpecial,
                ContractualCapacity = previousSubscription.ContractualCapacity,
                ImprovementCommertial = previousSubscription.ImprovementCommertial,
                ImprovementDomestic = previousSubscription.ImprovementDomestic,
                ImprovementOverall = previousSubscription.ImprovementOverall,
                Premises = previousSubscription.Premises,
                Operator = previousSubscription.Operator,
                SewageInstallationDateJalali = DateValidation(previousSubscription.SewageInstallationDateJalali, false),
                SewageRequestDateJalali = DateValidation(previousSubscription.SewageRequestDateJalali, false),
                MeterInstallationDateJalali = DateValidation(previousSubscription.MeterInstallationDateJalali, true),
                MeterRequestDateJalali = DateValidation(previousSubscription.MeterRequestDateJalali, true),
                Siphon100 = previousSubscription.Siphon100,
                Siphon125 = previousSubscription.Siphon125,
                Siphon150 = previousSubscription.Siphon150,
                Siphon200 = previousSubscription.Siphon200,
                Siphon5 = previousSubscription.Siphon5,
                Siphon6 = previousSubscription.Siphon6,
                Siphon7 = previousSubscription.Siphon7,
                Siphon8 = previousSubscription.Siphon8,
                MainSiphon = previousSubscription.MainSiphon,
                DeletionStateId = inputDto.DeletionStateId,
                BodySerial = previousSubscription.BodySerial ?? string.Empty,
                CommonSiphon = previousSubscription.CommonSiphon,
                MeterRegisterDateJalali = DateValidation(previousSubscription.MeterRegisterDateJalali, true),
                SewageRegisterDateJalali = DateValidation(previousSubscription.SewageRegisterDateJalali, false),
                GuildId = previousSubscription.GuildId
            };
        }
        private CustomerUpdateDto GetCustomerUpdate(CustomerUpdate3Dto inputDto, SubscriptionGetDto previousSubscription)
        {
            return new CustomerUpdateDto()
            {
                Id = previousSubscription.Id,
                CustomerNumber = previousSubscription.CustomerNumber,
                ZoneId = previousSubscription.ZoneId,
                BillId = inputDto.BillId,
                X = previousSubscription.X,
                Y = previousSubscription.Y,
                ReadingNumber = previousSubscription.ReadingNumber,
                FirstName = previousSubscription.FirstName,
                Surname = previousSubscription.Surname,
                Address = previousSubscription.Address,
                PostalCode = previousSubscription.PostalCode,
                Plaque = previousSubscription.Plaque,
                NationalCode = previousSubscription.NationalCode,
                PhoneNumber = previousSubscription.PhoneNumber,
                MobileNumber = previousSubscription.MobileNumber,
                FatherName = previousSubscription.FatherName,
                BranchTypeId = previousSubscription.BranchTypeId,
                UsageSellId = inputDto.UsageSellId,
                UsageConsumptionId = inputDto.UsageConsumptionId,
                EmptyUnit = previousSubscription.EmptyUnit,
                CommertialUnit = inputDto.CommertialUnit,
                DomesticUnit = inputDto.DomesticUnit,
                OtherUnit = inputDto.OtherUnit,
                HouseholdDateJalali = DateValidation(previousSubscription.HouseholdDateJalali, false),
                HouseholdNumber = previousSubscription.HouseholdNumber,
                MeterDiamterId = previousSubscription.MeterDiameterId,
                IsSpecial = previousSubscription.IsSpecial,
                ContractualCapacity = inputDto.ContractualCapacity,
                ImprovementCommertial = inputDto.ImprovementCommertial,
                ImprovementDomestic = inputDto.ImprovementDomestic,
                ImprovementOverall = inputDto.ImprovementOverall,
                Premises = inputDto.Premises,
                Operator = previousSubscription.Operator,
                SewageInstallationDateJalali = DateValidation(previousSubscription.SewageInstallationDateJalali, false),
                SewageRequestDateJalali = DateValidation(previousSubscription.SewageRequestDateJalali, false),
                MeterInstallationDateJalali = DateValidation(previousSubscription.MeterInstallationDateJalali, true),
                MeterRequestDateJalali = DateValidation(previousSubscription.MeterRequestDateJalali, true),
                Siphon100 = previousSubscription.Siphon100,
                Siphon125 = previousSubscription.Siphon125,
                Siphon150 = previousSubscription.Siphon150,
                Siphon200 = previousSubscription.Siphon200,
                Siphon5 = previousSubscription.Siphon5,
                Siphon6 = previousSubscription.Siphon6,
                Siphon7 = previousSubscription.Siphon7,
                Siphon8 = previousSubscription.Siphon8,
                MainSiphon = previousSubscription.MainSiphon,
                DeletionStateId = previousSubscription.DeletionStateId,
                BodySerial = previousSubscription.BodySerial ?? string.Empty,
                CommonSiphon = previousSubscription.CommonSiphon,
                MeterRegisterDateJalali = DateValidation(previousSubscription.MeterRegisterDateJalali, true),
                SewageRegisterDateJalali = DateValidation(previousSubscription.SewageRegisterDateJalali, false),
                GuildId = previousSubscription.GuildId
            };
        }
        private CustomerUpdateDto GetCustomerUpdate(CustomerUpdate5Dto inputDto, SubscriptionGetDto previousSubscription)
        {
            return new CustomerUpdateDto()
            {
                Id = previousSubscription.Id,
                CustomerNumber = previousSubscription.CustomerNumber,
                ZoneId = previousSubscription.ZoneId,
                BillId = inputDto.BillId,
                X = previousSubscription.X,
                Y = previousSubscription.Y,
                ReadingNumber = previousSubscription.ReadingNumber,
                FirstName = previousSubscription.FirstName,
                Surname = previousSubscription.Surname,
                Address = previousSubscription.Address,
                PostalCode = previousSubscription.PostalCode,
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
                HouseholdDateJalali = DateValidation(inputDto.HouseholdDateJalali, false),
                HouseholdNumber = previousSubscription.HouseholdNumber,
                MeterDiamterId = inputDto.MeterDiameterId,
                IsSpecial = previousSubscription.IsSpecial,
                ContractualCapacity = previousSubscription.ContractualCapacity,
                ImprovementCommertial = previousSubscription.ImprovementCommertial,
                ImprovementDomestic = previousSubscription.ImprovementDomestic,
                ImprovementOverall = previousSubscription.ImprovementOverall,
                Premises = previousSubscription.Premises,
                Operator = previousSubscription.Operator,
                SewageInstallationDateJalali = DateValidation(inputDto.SewageInstallationDateJalali, false),
                SewageRequestDateJalali = DateValidation(inputDto.SewageRequestDateJalali, false),
                MeterInstallationDateJalali = DateValidation(inputDto.MeterInstallationDateJalali, true),
                MeterRequestDateJalali = DateValidation(inputDto.MeterRequestDateJalali, true),
                Siphon100 = inputDto.Siphon100,
                Siphon125 = inputDto.Siphon125,
                Siphon150 = inputDto.Siphon150,
                Siphon200 = inputDto.Siphon200,
                Siphon5 = inputDto.Siphon5,
                Siphon6 = inputDto.Siphon6,
                Siphon7 = inputDto.Siphon7,
                Siphon8 = inputDto.Siphon8,
                MainSiphon = previousSubscription.MainSiphon,
                DeletionStateId = previousSubscription.DeletionStateId,
                BodySerial = inputDto.BodySerial ?? string.Empty,
                CommonSiphon = inputDto.CommonSiphon,
                MeterRegisterDateJalali = DateValidation(inputDto.MeterRegisterDateJalali, true),
                SewageRegisterDateJalali = DateValidation(inputDto.SewageRegisterDateJalali, false),
                GuildId = previousSubscription.GuildId
            };
        }
        private CustomerUpdateDto GetCustomerUpdate(ServiceLinkConnectionInput inputDto, int deletionStateId, SubscriptionGetDto previousSubscription)
        {
            return new CustomerUpdateDto()
            {
                Id = previousSubscription.Id,
                CustomerNumber = previousSubscription.CustomerNumber,
                ZoneId = previousSubscription.ZoneId,
                BillId = previousSubscription.BillId,
                X = previousSubscription.X,
                Y = previousSubscription.Y,
                ReadingNumber = previousSubscription.ReadingNumber,
                FirstName = previousSubscription.FirstName,
                Surname = previousSubscription.Surname,
                Address = previousSubscription.Address,
                PostalCode = previousSubscription.PostalCode,
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
                HouseholdDateJalali = DateValidation(previousSubscription.HouseholdDateJalali, false),
                HouseholdNumber = previousSubscription.HouseholdNumber,
                MeterDiamterId = previousSubscription.MeterDiameterId,
                IsSpecial = previousSubscription.IsSpecial,
                ContractualCapacity = previousSubscription.ContractualCapacity,
                ImprovementCommertial = previousSubscription.ImprovementCommertial,
                ImprovementDomestic = previousSubscription.ImprovementDomestic,
                ImprovementOverall = previousSubscription.ImprovementOverall,
                Premises = previousSubscription.Premises,
                Operator = 0,//todo
                SewageInstallationDateJalali = DateValidation(previousSubscription.SewageInstallationDateJalali, false),
                SewageRequestDateJalali = DateValidation(previousSubscription.SewageRequestDateJalali, false),
                MeterInstallationDateJalali = DateValidation(previousSubscription.MeterInstallationDateJalali, false),
                MeterRequestDateJalali = DateValidation(previousSubscription.MeterRequestDateJalali, false),
                Siphon100 = previousSubscription.Siphon100,
                Siphon125 = previousSubscription.Siphon125,
                Siphon150 = previousSubscription.Siphon150,
                Siphon200 = previousSubscription.Siphon200,
                Siphon5 = previousSubscription.Siphon5,
                Siphon6 = previousSubscription.Siphon6,
                Siphon7 = previousSubscription.Siphon7,
                Siphon8 = previousSubscription.Siphon8,
                MainSiphon = previousSubscription.MainSiphon,
                DeletionStateId = deletionStateId,
                BodySerial = previousSubscription.BodySerial ?? string.Empty,
                CommonSiphon = previousSubscription.CommonSiphon,
                MeterRegisterDateJalali = DateValidation(previousSubscription.MeterRegisterDateJalali, false),
                SewageRegisterDateJalali = DateValidation(previousSubscription.SewageRegisterDateJalali, false),
                GuildId = previousSubscription.GuildId,
                ToDayDateJalali = DateTime.Now.ToShortPersianDateString(),
                ToDayDateJalaliWithFragmentYear = DateTime.Now.ToShortPersianDateString().Substring(2, 8),
            };
        }

        private async Task<SubscriptionGetDto> GetConsumptionPreviousInfo(string billId)
        {
            SubscriptionGetDto previousSubscription = await _customerQueryService.GetInfo(billId);
            if (previousSubscription == null)
            {
                throw new BaseException("شناسه قبض یافت نشد");
            }

            return previousSubscription;
        }
        private string DateValidation(string? inputDate, bool hasException)
        {
            if (hasException)
            {
                return string.IsNullOrWhiteSpace(inputDate) || inputDate.Trim().Length != 10 ?
                    throw new InvalidDateException(ExceptionLiterals.InvalidDate) :
                    inputDate.Trim();
            }
            return string.IsNullOrWhiteSpace(inputDate) || inputDate.Trim().Length != 10 ? string.Empty : inputDate.Trim();
        }
    }
}
