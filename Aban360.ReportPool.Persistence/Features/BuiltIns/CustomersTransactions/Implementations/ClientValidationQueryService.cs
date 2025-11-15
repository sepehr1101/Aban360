using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

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
            string clientValidationQuery = GetClientValidationQuery();
            var @params = new
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ZoneIds = input.ZoneIds,
                ValidationEstate = input.ValidationEstate.ToString(),
            };

            IEnumerable<ClientValidationDataOutputDto> ClientValidationData = await _sqlReportConnection.QueryAsync<ClientValidationDataOutputDto>(clientValidationQuery, input);
            ClientValidationHeaderOutputDto ClientValidationHeader = new ClientValidationHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                RecordCount = ClientValidationData is not null && ClientValidationData.Any() ? ClientValidationData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                CustomerCount = ClientValidationData is not null && ClientValidationData.Any() ? ClientValidationData.Count() : 0,
                Title = ReportLiterals.ClientValidation
            };


            var result = new ReportOutput<ClientValidationHeaderOutputDto, ClientValidationDataOutputDto>(ReportLiterals.ClientValidation, ClientValidationHeader, ClientValidationData);

            return result;
        }
        private string GetClientValidationQuery()
        {
            return @"Select  
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
					c.ConstructedArea,
					Case
					  When @ValidationEstate = 0 AND LEN(TRIM(c.NationalId))!=10 Then N'کد ملی نامعتبر'
					  When @ValidationEstate = 1 AND  LEN(TRIM(c.PostalCode))!=10 Then N'کد پستی نامعتبر'
					  When @ValidationEstate = 2 AND (LEN(TRIM(c.FirstName))=0 OR  c.FirstName IS NULL)  Then N'نام خالی '
					  When @ValidationEstate = 3 AND (LEN(TRIM(c.SureName))=0 OR  c.SureName IS NULL) Then N'نام خانوادگی خالی '
					  When @ValidationEstate = 4 AND c.CommercialCount=0 Then N'تعداد واحد تجاری صفر '
					  When @ValidationEstate = 5 AND c.DomesticCount=0  Then N'تعداد واحد مسکونی صفر '
					  When @ValidationEstate = 6 AND c.OtherCount=0 Then N'تعداد واحد سایر صفر '
					  When @ValidationEstate = 7 AND c.CommercialArea=0 Then N'عرصه تجاری خالی'
					  When @ValidationEstate = 8 AND c.DomesticArea=0 Then N'عرصه مسکونی خالی'
					  When @ValidationEstate = 9 AND c.FieldArea=0 Then N'اعیان کل خالی'
					  When @ValidationEstate = 10 AND (c.ConstructedArea=0) Then N'اعیان سایر خالی'
					  When @ValidationEstate = 11 AND LEN(TRIM(c.MobileNo))!=11 Then N'شماره موبایل نامعتبر'
					  When @ValidationEstate = 12 AND LEN(TRIM(c.PhoneNo))!=8 Then N' تلفن ثابت نامعتبر'
					  When @ValidationEstate = 13 AND LEN(TRIM(c.Address))<10 Then N'آدرس کوتاه تر از 10 کاراکتر '
					  When @ValidationEstate = 14 AND (c.UsageId NOT IN (1,3) AND c.ContractCapacity=0) Then N'غیرمسکونی فاقد ظرفیت'
					  When @ValidationEstate = 15 AND (LEN(c.SewageInstallDate)!=0 AND c.MainSiphonTitle='0') Then N'دارای تاریخ نصب فاضلاب بدون سیفون'
					  When @ValidationEstate = 16 AND (c.UsageId=0) Then N'کد کاربری صفر '
					  Else N'هیچ مشکلی ندارد'
					End as Description
				From [CustomerWarehouse].dbo.Clients c
				Where
					(
						(@ValidationEstate = 0 AND LEN(TRIM(c.NationalId))!=10) OR
						(@ValidationEstate = 1 AND LEN(TRIM(c.PostalCode))!=10 ) OR
						(@ValidationEstate = 2 AND (LEN(TRIM(c.FirstName)) = 0 OR c.FirstName IS NULL)) OR
						(@ValidationEstate = 3 AND (LEN(TRIM(c.SureName)) = 0 OR c.SureName IS NULL))OR
						(@ValidationEstate = 4 AND c.CommercialCount = 0)OR
						(@ValidationEstate = 5 AND c.DomesticCount = 0)OR
						(@ValidationEstate = 6 AND c.OtherCount = 0)OR
						(@ValidationEstate = 7 AND c.CommercialArea = 0)OR
						(@ValidationEstate = 8 AND c.DomesticArea = 0)OR
						(@ValidationEstate = 9 AND c.FieldArea = 0)OR
						(@ValidationEstate = 10 AND (c.ConstructedArea = 0)) OR
						(@ValidationEstate = 11 AND (LEN(TRIM(c.MobileNo)) != 11)) OR
						(@ValidationEstate = 12 AND (LEN(TRIM(c.PhoneNo)) != 8)) OR
						(@ValidationEstate = 13 AND (LEN(TRIM(c.Address)) < 10))OR
						(@ValidationEstate = 14 AND (c.UsageId NOT IN (1, 3) AND c.ContractCapacity = 0))OR
						(@ValidationEstate = 15 AND (LEN(c.SewageInstallDate) != 0 AND c.MainSiphonTitle = '0')) OR
						(@ValidationEstate = 16 AND (c.UsageId = 0))
					)AND
					(@fromReadingNumber IS NULL OR
					@toReadingNumber IS NULL OR
					c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
					c.ZoneId IN @zoneIds AND
					c.ToDayJalali IS NULL 
				Order By
					c.ZoneTitle,
					c.CustomerNumber";
        }
    }
}
