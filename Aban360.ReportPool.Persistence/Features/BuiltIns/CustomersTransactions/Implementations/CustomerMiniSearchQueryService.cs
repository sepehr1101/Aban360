using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Constants;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class CustomerMiniSearchQueryService : ChangeHistoryBase, ICustomerMiniSearchQueryService
    {
        public CustomerMiniSearchQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<ReportOutput<CustomerMiniSearchHeaderOutputDto, CustomerMiniSearchDataOutputDto>> Get(CustomerMiniSearchInputDto input)
        {
            string fieldSearch = GetSqlCondition(input.SearchType);
            string query = GetQuery(fieldSearch);
            string normalizedInput = NormalizeText(input.Input);
            if (input.SearchType == CustomerMiniSearchInputEnum.Name)
            {
                normalizedInput = $"%{normalizedInput}%";
            }

            IEnumerable<CustomerMiniSearchDataOutputDto> data = await _sqlReportConnection.QueryAsync<CustomerMiniSearchDataOutputDto>(query, new { input = normalizedInput, zoneIds=input.UserZoneIds });
            CustomerMiniSearchHeaderOutputDto header = new CustomerMiniSearchHeaderOutputDto()
            {
                Input =input.Input,
                SearchType = input.SearchType,

                CustomerCount = data.Count(),
                RecordCount = data.Count(),
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = ReportLiterals.CustomerSearch
            };

            var result = new ReportOutput<CustomerMiniSearchHeaderOutputDto, CustomerMiniSearchDataOutputDto>(ReportLiterals.CustomerSearch, header, data);

            return result;
        }
        private string NormalizeText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            return text
                .Replace('ي', 'ی')
                .Replace('ك', 'ک')
                .Replace('أ', 'ا')
                .Replace('إ', 'ا')
                .Replace('آ', 'ا')
                .Replace('ٱ', 'ا')
                .Replace('ة', 'ه')
                .Replace('ۀ', 'ه')
                .Trim();
        }
        private string GetSqlCondition(CustomerMiniSearchInputEnum input)
        {
            return input switch
            {
                CustomerMiniSearchInputEnum.MobileNumber => "c.MobileNo=@input",
                CustomerMiniSearchInputEnum.NationalCode => "c.NationalId=@input",
                CustomerMiniSearchInputEnum.PostalCode => "c.PostalCode=@input",
                CustomerMiniSearchInputEnum.Name => "c.FullNameNormalized LIKE @input",
                CustomerMiniSearchInputEnum.PhoneNumber => "c.PhoneNo=@input",
                CustomerMiniSearchInputEnum.CustomerNumber => "(c.CustomerNumber=@input OR TRIM(c.ReadingNumber)=@input)",
				CustomerMiniSearchInputEnum.BillId=> "c.BillId=@input",
                _ => throw new InvalidDateException(ExceptionLiterals.MustEnum),
            };
        }
        private string GetQuery(string condition)
        {
            return $@"Select TOP 100
						TRIM(c.FirstName) FirstName,
						TRIM(c.SureName) Surname,
						(TRIM(c.FirstName)+' '+TRIM(c.SureName)) FullName,
						TRIM(c.FatherName) FatherName,                    	
						t46.C2 AS RegionTitle,
						c.ZoneTitle,
						c.ZoneId,
						c.CustomerNumber,
						c.BillId,
						c.ReadingNumber,
						c.UsageTitle UsageSellTitle,
						c.UsageTitle2 UsageConsumptionTitle,
						c.BranchType BranchTypeTitle,
						TRIM(c.Address) Address,
						TRIM(c.MobileNo) MobileNumber,
						TRIM(c.PhoneNo) PhoneNumber,
						TRIM(c.PostalCode) PostalCode,
						TRIM(c.NationalId) NationalCode,
						c.CommercialCount CommercialUnit,
						c.DomesticCount DomesticUnit,
						c.OtherCount OtherUnit,
					    IIF((c.DomesticCount+c.CommercialCount +c.OtherCount=0) ,1, (c.DomesticCount+c.CommercialCount +c.OtherCount)) AS TotalUnit,
						c.FamilyCount,
						c.DeletionStateTitle,
						c.DiscountTypeTitle,
						c.EmptyCount EmptyUnit,
						c.HouseholdDateJalali,
						c.HasCommonSiphon CommonSiphon,
						c.GuildTitle,
						c.HasSewage,
						c.MainSiphonTitle AS SiphonDiameterTitle,
						c.ContractCapacity AS ContractualCapacity,
						c.WaterDiameterTitle MeterDiameterTitle,
						c.IsSpecial SpecialCustomer
					From CustomerWarehouse.dbo.Clients c
					JOIN [Db70].dbo.T51 t51
					    On t51.C0=c.ZoneId
					JOIN [Db70].dbo.T46 t46
					    On t51.C1=t46.C0
					Where 
						c.ZoneId IN @zoneIds AND
						c.ToDayJalali IS NULL AND
						{condition}";
        }
    }
}
