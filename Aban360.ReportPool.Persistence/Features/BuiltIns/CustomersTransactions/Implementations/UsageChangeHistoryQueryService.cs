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

            return $@"use CustomerWarehouse
                    ;With FirstBillGroup as (
                    Select 
                    	c.UsageId,
                    	c.BillId,
                    	c.UsageTitle,
                    	MAX(RegisterDayJalali)as RegisterDayJalali
                    From [CustomerWarehouse].dbo.Clients c
                    Group By c.BillId,c.UsageTitle,UsageId
                    ),
                    SecondBillGroup as (
                    Select 
                    	c.UsageId,
                    	c.BillId,
                    	c.UsageTitle,
                    	MAX(RegisterDayJalali)as RegisterDayJalali
                    From [CustomerWarehouse].dbo.Clients c
                    Group By c.BillId,c.UsageTitle,UsageId
                    )
                    Select 
                        c.CustomerNumber,
                        c.ReadingNumber,
                        c.BillId,
                        ss.RegisterDayJalali AS ChangeDateJalali,
                        ff.UsageTitle AS FromUsageTitle,
                        ss.UsageTitle AS ToUsageTitle,
                        c.ZoneTitle,
                        c.ZoneId,
                        TRIM(c.FirstName) AS FirstName,
                        TRIM(c.SureName) AS Surname,
                        TRIM(c.FirstName)+' '+TRIM(c.SureName) AS FullName,
                        TRIM(c.FatherName) AS FatherName,
                        TRIM(c.NationalId) AS NationalCode,
                        TRIM(c.PhoneNo) AS PhoneNumber,
                        TRIM(c.Address) AS Address,
                        TRIM(c.PostalCode) AS PostalCode,
                        c.ContractCapacity AS ContractualCapacity,
                        c.CommercialCount,
                        c.DomesticCount,
                        c.OtherCount,
                        (c.CommercialCount+c.DomesticCount+c.OtherCount) AS TotalCount,
                        c.WaterDiameterTitle AS MeterDiameterTitle,
                        c.MainSiphonTitle AS SiphonDiameterTitle,
                        c.BranchType AS UseStateTitle,
                        c.EmptyCount AS EmptyUnit,
						DATEDIFF(DAY,[CustomerWarehouse].dbo.PersianToMiladi(c1.RegisterDayJalali),[CustomerWarehouse].dbo.PersianToMiladi(c2.RegisterDayJalali)) as Distance
                    From CustomerWarehouse.dbo.Clients c 
                    Join FirstBillGroup ff 
                    	On c.BillId=ff.BillId
                    Join SecondBillGroup ss
                    	On c.BillId=ss.BillId AND ff.UsageId<> ss.UsageId
                    Where
                    	c.ToDayJalali IS NULL AND
                    	ff.UsageId IN @fromUsageIds AND
                        ss.UsageId IN @toUsageIds AND
                        ss.RegisterDayJalali>ff.RegisterDayJalali AND
                        (@fromDate IS NULL OR
                        @toDate IS NULL OR
                        ss.RegisterDayJalali BETWEEN @fromDate AND @toDate) AND
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
                        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) 
                    	{zoneQuery} ";
        }
    }
}
