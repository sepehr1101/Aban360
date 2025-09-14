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
    internal sealed class UsageChangeHistoryQueryService : AbstractBaseConnection, IUsageChangeHistoryQueryService
    {
        public UsageChangeHistoryQueryService(IConfiguration configuration)
            : base(configuration)
        { }
        public async Task<ReportOutput<UsageChangeHistoryHeaderOutputDto, UsageChangeHistoryDataOutputDto>> GetInfo(UsageChangeHistoryInputDto input)
        {
            string UsageChangeHistoryQuery = GetUsageChangeHistoryQuery(input.ZoneIds.Any());
            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,

                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,

                zoneIds = input.ZoneIds,

                fromUsageIds = input.FromUsageIds,
                toUsageIds = input.ToUsageIds,
            };

            IEnumerable<UsageChangeHistoryDataOutputDto> usageChangeHistoryData = await _sqlReportConnection.QueryAsync<UsageChangeHistoryDataOutputDto>(UsageChangeHistoryQuery, @params);
            UsageChangeHistoryHeaderOutputDto usageChangeHistoryHeader = new UsageChangeHistoryHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,

                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,

                RecordCount = usageChangeHistoryData is not null && usageChangeHistoryData.Any() ? usageChangeHistoryData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),

                SumDomesticCount = usageChangeHistoryData is not null && usageChangeHistoryData.Any() ? usageChangeHistoryData.Sum(x => x.DomesticUnit) : 0,
                SumCommercialCount = usageChangeHistoryData is not null && usageChangeHistoryData.Any() ? usageChangeHistoryData.Sum(x => x.CommercialUnit) : 0,
                SumOtherCount = usageChangeHistoryData is not null && usageChangeHistoryData.Any() ? usageChangeHistoryData.Sum(x => x.OtherUnit) : 0,
                TotalUnit = usageChangeHistoryData is not null && usageChangeHistoryData.Any() ? usageChangeHistoryData.Sum(x => x.TotalUnit) : 0,
            };


            var result = new ReportOutput<UsageChangeHistoryHeaderOutputDto, UsageChangeHistoryDataOutputDto>(ReportLiterals.UsageChangeHistory, usageChangeHistoryHeader, usageChangeHistoryData);

            return result;
        }
        private string GetUsageChangeHistoryQuery(bool hasZone)
        {
            string zoneQuery = hasZone ? "AND c1.ZoneId IN @zoneIds" : string.Empty;

            return @$"use CustomerWarehouse
                    Select 
                    	c1.CustomerNumber,
                    	c1.ReadingNumber,
                    	c1.BillId,
                    	c2.RegisterDayJalali AS ChangeDateJalali,
                    	c1.UsageTitle AS FromUsageTitle,
                    	c2.UsageTitle AS ToUsageTitle,
                        c1.ZoneTitle,
                        c1.ZoneId,
                    	TRIM(c1.FirstName) AS FirstName,
                    	TRIM(c1.SureName) AS Surname,
                    	TRIM(c1.FirstName)+' '+TRIM(c1.SureName) AS FullName,
	                    TRIM(c1.FatherName) AS FatherName,
                    	TRIM(c1.NationalId) AS NationalCode,
                    	TRIM(c1.PhoneNo) AS PhoneNumber,
                    	TRIM(c1.Address) AS Address,
	                    TRIM(c1.PostalCode) AS PostalCode,
                    	c1.ContractCapacity AS ContractualCapacity,
                    	c1.CommercialCount,
                    	c1.DomesticCount,
                    	c1.OtherCount,
                    	(c1.CommercialCount+c1.DomesticCount+c1.OtherCount) AS TotalCount,
                    	c1.WaterDiameterTitle AS MeterDiameterTitle,
                    	c1.MainSiphonTitle AS SiphonDiameterTitle,
	                    c1.BranchType AS UseStateTitle,
	                    c1.EmptyCount AS EmptyUnit
                    From [CustomerWarehouse].dbo.Clients c1
                    Join [CustomerWarehouse].dbo.Clients c2
                    	on c1.BillId=c2.BillId
                    Where
                    	c1.UsageId IN @fromUsageIds AND
                    	c2.UsageId IN @toUsageIds AND
                    	c2.RegisterDayJalali>c1.RegisterDayJalali AND
                    	(@fromDate IS NULL OR
                    	@toDate IS NULL OR
                    	c2.RegisterDayJalali BETWEEN @fromDate AND @toDate) AND
                    	(@fromReadingNumber IS NULL OR
                    	@toReadingNumber IS NULL OR
                    	c1.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                        c1.UsageId!=c2.UsageId
                    	{zoneQuery} ";
        }
    }
}
