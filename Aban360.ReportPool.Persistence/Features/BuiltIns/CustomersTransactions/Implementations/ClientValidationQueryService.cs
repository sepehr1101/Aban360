using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class ClientValidationQueryService : AbstractBaseConnection, IClientValidationQueryService
    {
        public ClientValidationQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<ReportOutput<ClientValidationHeaderOutputDto, ClientValidationDataOutputDto>> GetInfo(ClientValidationInputDto input)
        {
            Dictionary<short, (string, string)> validationState = GetValidationState();
            var (condition, persianText) = validationState.First(v => v.Key == (short)input.ValidationEstate).Value;
            string clientValidationQuery = GetClientValidationQuery(condition);

            IEnumerable<ClientValidationDataOutputDto> ClientValidationData = await _sqlReportConnection.QueryAsync<ClientValidationDataOutputDto>(clientValidationQuery, input);
            ClientValidationHeaderOutputDto ClientValidationHeader = new ClientValidationHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                RecordCount = ClientValidationData is not null && ClientValidationData.Any() ? ClientValidationData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                CustomerCount = ClientValidationData is not null && ClientValidationData.Any() ? ClientValidationData.Count() : 0,
                Title = ReportLiterals.ClientValidation + "-" + persianText
            };


            var result = new ReportOutput<ClientValidationHeaderOutputDto, ClientValidationDataOutputDto>(ReportLiterals.ClientValidation, ClientValidationHeader, ClientValidationData);

            return result;
        }
        private string GetClientValidationQuery(string condition)
        {
            return @$"Select 
						c.ZoneTitle,
						c.CustomerNumber,
						c.ReadingNumber,
						c.BillId,
						TRIM(c.FirstName) AS FirstName,
						TRIM(c.SureName) AS Surname,
						TRIM(c.Address) AS Address,
						TRIM(c.PhoneNo) AS PhoneNumber,
						TRIM(c.MobileNo) AS MobileNumber,
						TRIM(c.PostalCode) AS PostalCode,
						TRIM(c.NationalId) AS NationalCode,
						c.UsageTitle,
						c.ContractCapacity AS ContractualCapacity,
						c.HasSewage,
						TRIM(c.SewageInstallDate) AS SewageInstallationDateJalali,
						c.CommercialCount AS CommercialUnit,
						c.DomesticCount AS DomesticUnit,
						c.OtherCount AS OtherUnit,
						c.CommercialArea,
						c.DomesticArea,
						c.FieldArea,
						c.ConstructedArea
					From [CustomerWarehouse].dbo.Clients c
					Where
						(
							{condition}
						)AND
						(@fromReadingNumber IS NULL OR
						@toReadingNumber IS NULL OR
						c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
						c.ZoneId IN @zoneIds AND
						c.ToDayJalali IS NULL AND
                        c.DeletionStateId NOT IN (1,2)
					Order By
						c.ZoneTitle,
						c.CustomerNumber";
        }

        private Dictionary<short, (string, string)> GetValidationState()
        {
            return new Dictionary<short, (string, string)>()
            {
               { 0  , (@" LEN(TRIM(c.NationalId)) != 10 ", "کد ملی نامعتبر") },
               { 1  , (@" LEN(TRIM(c.PostalCode)) != 10 ", "کد پستی نامعتبر") },
               { 2  , (@" (LEN(TRIM(c.FirstName)) = 0 OR c.FirstName IS NULL) ", "نام خالی") },
               { 3  , (@" (LEN(TRIM(c.SureName)) = 0 OR c.SureName IS NULL) ", "نام خانوادگی خالی") },
               { 4  , (@" (c.CommercialCount = 0 AND c.DomesticCount = 0 AND c.OtherCount = 0) ", "تعداد واحدها صفر") },
               { 5  , (@" (c.CommercialArea = 0 AND c.DomesticArea = 0 AND c.FieldArea = 0) ", "عرصه‌ها خالی") },
               { 6  , (@" (c.ConstructedArea = 0) ", "اعیان خالی") },
               { 7  , (@" LEN(TRIM(c.MobileNo)) != 11 ", "شماره موبایل نامعتبر") },
               { 8  , (@" LEN(TRIM(c.PhoneNo)) != 8 ", "تلفن ثابت نامعتبر") },
               { 9  , (@" LEN(TRIM(c.Address)) < 10 ", "آدرس کوتاه‌تر از 10 کاراکتر") },
               { 10 , (@" (c.UsageId NOT IN (1, 3, 5, 15, 19, 25, 34, 39) AND c.ContractCapacity = 0) ", "غیرمسکونی فاقد ظرفیت") },
               { 11 , (@" (LEN(c.SewageInstallDate) != 0 AND c.MainSiphonTitle = '0') ", "دارای تاریخ نصب فاضلاب بدون سیفون") },
               { 12 , (@" (c.UsageId = 0) ", "کد کاربری صفر") },
               { 13 , (@" (c.FamilyCount IS NOT NULL AND c.FamilyCount > 0 AND LEN(TRIM(c.HouseholdDateJalali)) < 8) ", "تاریخ خالی از سکنه نامعتبر") },
               { 14 , (@" (c.EmptyCount IS NOT NULL AND c.EmptyCount > 0 AND c.UsageId NOT IN (1, 3)) ", "مغایرت خالی از سکنه با کاربری ") },
               { 15 , (@" (LEN(TRIM(c.ReadingNumber))<5) ", " بدون اشتراک ") },
               { 16 , (@" (c.UsageStateId IN (6,7) AND c.UsageId NOT IN (1)) ", " مخفف غیرمسکونی ") },
               { 17 , (@" (c.UsageStateId IN (6,7) AND (c.DomesticCount+c.CommercialCount+c.OtherCount)>1) ", " مخفف بیش از یک واحد ") },
            };
        }
    }
}
