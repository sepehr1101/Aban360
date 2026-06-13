using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    public interface IConnectDisconnectUpdateHandler
    {
        Task Handle(ServiceLinkConnectionInput inputDto, int deletionStateId, IAppUser appUser, CancellationToken cancellationToken);
        ICollection<NumericDictionary> GetDisconnectResults();
    }
    internal sealed class ConnectDisconnectUpdateHandler : AbstractBaseConnection, IConnectDisconnectUpdateHandler
    {
        private readonly ISubscriptionQueryService _customerQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IConnectDisconnectQueryService _connectDisconnectQueryService;
        public ConnectDisconnectUpdateHandler(
            ISubscriptionQueryService customerQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            ICommonZoneService commonZoneService,
            IConnectDisconnectQueryService connectDisconnectQueryService,
            IConfiguration configuratio)
            : base(configuratio)
        {
            _customerQueryService = customerQueryService;
            _customerQueryService.NotNull(nameof(customerQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _connectDisconnectQueryService = connectDisconnectQueryService;
            _connectDisconnectQueryService.NotNull(nameof(connectDisconnectQueryService));
        }

        public async Task Handle(ServiceLinkConnectionInput inputDto, int deletionStateId, IAppUser appUser, CancellationToken cancellationToken)
        {
            ConnectDisconnectGetDto connectDisconnectResult = await _connectDisconnectQueryService.Get(inputDto.Id);
            await Validate(connectDisconnectResult, connectDisconnectResult.BillId, deletionStateId);

            SubscriptionGetDto previousSubscription = await GetCustomerPreviousInfo(appUser, connectDisconnectResult.BillId);
            CustomerUpdateDto customerUpdate = GetCustomerUpdate(inputDto, deletionStateId, previousSubscription);


            //ICollection<NumericDictionary> GetDisconnectResults
            //ConnectDisconnectUpdateDto connectDiscnnectUpdateDto = new(inputDto.Id,);

            await SqlCommands(customerUpdate);
        }
        private async Task SqlCommands(CustomerUpdateDto updateDto)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomer = new(updateDto.ZoneId, updateDto.CustomerNumber);
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    ArchMemCommandService _archMemCommandService = new(connection, transaction);
                    MembersCommandService _membersCommandService = new(connection, transaction);
                    ClientsCommandService _clientCommandService = new(connection, transaction);
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
        private async Task<SubscriptionGetDto> GetCustomerPreviousInfo(IAppUser appUser, string billId)
        {
            SubscriptionGetDto previousSubscription = await _customerQueryService.GetInfo(billId);
            if (previousSubscription == null)
            {
                throw new BaseException("شناسه قبض یافت نشد");
            }
            await _commonZoneService.IsUserInZone(appUser, previousSubscription.ZoneId);

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
        private async Task Validate(ConnectDisconnectGetDto connectDisconnectResult, string billId, int deletionStateId)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(billId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumber);
            if (memberInfo.DeletionStateId == deletionStateId)
            {
                throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidDuplicateDeletionState);
            }

            if (connectDisconnectResult.ResultId is not null)
            {
                throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidConnectDisconnectDuplicateResult);
            }
        }
        public ICollection<NumericDictionary> GetDisconnectResults()
        {
            ICollection<NumericDictionary> results = new List<NumericDictionary>()
            {
                new NumericDictionary(1,"انشعاب از طریق قطع و وصل، قطع گردید."),
                new NumericDictionary(2,"انشعاب از طریق حفاری قطع گردید."),
                new NumericDictionary(3,"محل انشعاب پیدا نشد."),
                new NumericDictionary(4,"مشترک از قطع جلوگیری نمود."),
                new NumericDictionary(5,"مشترک تسویه حساب نمود."),
                new NumericDictionary(6,"قرائت میسر شد."),
            };
            return results;
        }
    }
}
