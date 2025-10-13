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
        { }
        public async Task<ReportOutput<ClientValidationHeaderOutputDto, ClientValidationDataOutputDto>> GetInfo(ClientValidationInputDto input)
        {
            string clientValidationQuery = GetClientValidationQuery();
            var @params = new
            {
                input.FromReadingNumber,
                input.ToReadingNumber,

                input.ZoneIds
            };

            IEnumerable<ClientValidationDataOutputDto> ClientValidationData = await _sqlReportConnection.QueryAsync<ClientValidationDataOutputDto>(clientValidationQuery, @params);
            ClientValidationHeaderOutputDto ClientValidationHeader = new ClientValidationHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                RecordCount = ClientValidationData is not null && ClientValidationData.Any() ? ClientValidationData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                CustomerCount = ClientValidationData is not null && ClientValidationData.Any() ? ClientValidationData.Count() : 0,
				Title= ReportLiterals.ClientValidation
            };


            var result = new ReportOutput<ClientValidationHeaderOutputDto, ClientValidationDataOutputDto>(ReportLiterals.ClientValidation, ClientValidationHeader, ClientValidationData);

            return result;
        }
        private string GetClientValidationQuery()
		{
			return @"Select  
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
					  When LEN(TRIM(c.NationalId))!=10 Then N'کد ملی نامعتبر'
					  When LEN(TRIM(c.PostalCode))!=10 Then N'کد پستی نامعتبر'
					  When (LEN(TRIM(c.FirstName))=0 OR  c.FirstName IS NULL) 
					       AND (LEN(TRIM(c.SureName))=0 OR  c.SureName IS NULL) Then N'نام و نام خانوادگی خالی '
					  When (c.CommercialCount=0 AND c.DomesticCount=0 AND c.OtherCount=0) Then N'تعداد واحدها صفر '
					  When (c.CommercialArea=0 AND c.DomesticArea=0 AND c.FieldArea=0 ) Then N'عرصه ها خالی'
					  When (c.ConstructedArea=0) Then N'اعیان خالی'
					  When LEN(TRIM(c.MobileNo))!=11 Then N'شماره موبایل نامعتبر'
					  When LEN(TRIM(c.PhoneNo))!=8 Then N' تلفن ثابت نامعتبر'
					  When LEN(TRIM(c.Address))<10 Then N'آدرس کوتاه تر از 10 کاراکتر '
					  When (c.UsageId NOT IN (1,3) AND c.ContractCapacity=0) Then N'غیرمسکونی فاقد ظرفیت'
					  When (LEN(c.SewageInstallDate)!=0 AND c.MainSiphonTitle='0') Then N'دارای تاریخ نصب فاضلاب بدون سیفون'
					  When (c.UsageId=0) Then N'کد کاربری صفر '
					  Else N'هیچ مشکلی ندارد'
					End as Description
				From [CustomerWarehouse].dbo.Clients c
				Where
					(LEN(TRIM(c.NationalId))!=10 OR
					 LEN(TRIM(c.PostalCode))!=10 OR
					 ((LEN(TRIM(c.FirstName))=0 OR  c.FirstName IS NULL) AND
					  (LEN(TRIM(c.SureName))=0 OR  c.SureName IS NULL)) OR
					 (c.CommercialCount=0 AND c.DomesticCount=0 AND c.OtherCount=0) OR
					 (c.CommercialArea=0 AND c.DomesticArea=0 AND c.FieldArea=0 )OR
					 ( c.ConstructedArea=0) OR 
					 (LEN(TRIM(c.MobileNo))!=11) OR
					 (LEN(TRIM(c.PhoneNo))!=8 ) OR
					 (LEN(TRIM(c.Address))<10 ) OR
					 (c.UsageId NOT IN (1,3) AND c.ContractCapacity=0) OR
					 (LEN(c.SewageInstallDate)!=0 AND c.MainSiphonTitle='0') OR
					 (c.UsageId=0) 
					)AND
					(@fromReadingNumber IS NULL OR
					@toReadingNumber IS NULL OR
					c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
					c.ZoneId IN @zoneIds AND
					c.ToDayJalali IS NULL ";
		}
	}
}
