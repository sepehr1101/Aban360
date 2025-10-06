using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
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
    internal sealed class BranchTypeChangeHistoryQueryService : ChangeHistoryBase, IBranchTypeChangeHistoryQueryService
    {
        public BranchTypeChangeHistoryQueryService(IConfiguration configuration)
            : base(configuration)
        { }
        public async Task<ReportOutput<BranchTypeChangeHistoryHeaderOutputDto, ChangeHistoryDataOutputDto>> GetInfo(BranchTypeChangeHistoryInputDto input)
        {
            string query = GetDetailQuery(input.ZoneIds?.Any() == true, GroupingFields.UsageStateId, GroupingFields.BranchType);
            //string query = GetBranchTypeChangeHistoryQuery(input.ZoneIds?.Any() == true);

            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,

                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,

                zoneIds = input.ZoneIds,

                fromFieldIds = input.FromUseStateIds,
                toFieldIds = input.ToUseStateIds,

                //fromUseStateIds = input.FromUseStateIds,
                //toUseStateIds = input.ToUseStateIds,
            };

            IEnumerable<ChangeHistoryDataOutputDto> BranchTypeChangeHistoryData = await _sqlReportConnection.QueryAsync<ChangeHistoryDataOutputDto>(query, @params);
            BranchTypeChangeHistoryHeaderOutputDto BranchTypeChangeHistoryHeader = new BranchTypeChangeHistoryHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,

                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,

                CustomerCount = BranchTypeChangeHistoryData is not null && BranchTypeChangeHistoryData.Any() ? BranchTypeChangeHistoryData.Count() : 0,
                RecordCount = BranchTypeChangeHistoryData is not null && BranchTypeChangeHistoryData.Any() ? BranchTypeChangeHistoryData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),

                SumDomesticCount = BranchTypeChangeHistoryData is not null && BranchTypeChangeHistoryData.Any() ? BranchTypeChangeHistoryData.Sum(x => x.DomesticUnit) : 0,
                SumCommercialCount = BranchTypeChangeHistoryData is not null && BranchTypeChangeHistoryData.Any() ? BranchTypeChangeHistoryData.Sum(x => x.CommercialUnit) : 0,
                SumOtherCount = BranchTypeChangeHistoryData is not null && BranchTypeChangeHistoryData.Any() ? BranchTypeChangeHistoryData.Sum(x => x.OtherUnit) : 0,
                TotalUnit = BranchTypeChangeHistoryData is not null && BranchTypeChangeHistoryData.Any() ? BranchTypeChangeHistoryData.Sum(x => x.TotalUnit) : 0,
            };


            var result = new ReportOutput<BranchTypeChangeHistoryHeaderOutputDto, ChangeHistoryDataOutputDto>(ReportLiterals.BranchTypeChangeHistory, BranchTypeChangeHistoryHeader, BranchTypeChangeHistoryData);

            return result;
        }
        private string GetBranchTypeChangeHistoryQuery(bool hasZone)
        {
            string zoneQuery = hasZone ? "AND c.ZoneId IN @zoneIds" : string.Empty;

            return $@"use CustomerWarehouse
                    ;With FirstBillGroup as (
                    Select 
                    	c.UsageStateId,
                    	c.BillId,
                    	c.BranchType,
                    	MAX(RegisterDayJalali)as RegisterDayJalali
                    From [CustomerWarehouse].dbo.Clients c
                    Group By c.BillId,c.BranchType,UsageStateId
                    ),
                    SecondBillGroup as (
                    Select 
                    	c.UsageStateId,
                    	c.BillId,
                    	c.BranchType,
                    	MAX(RegisterDayJalali)as RegisterDayJalali
                    From [CustomerWarehouse].dbo.Clients c
                    Group By c.BillId,c.BranchType,UsageStateId
                    )
                    Select 
                        c.CustomerNumber,
                        c.ReadingNumber,
                        c.BillId,
                        ss.RegisterDayJalali AS ChangeDateJalali,
                        ff.BranchType AS FromUseStateTitle,
                        ss.BranchType AS ToUseStateTitle,
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
					    DATEDIFF(DAY,[CustomerWarehouse].dbo.PersianToMiladi(ff.RegisterDayJalali),[CustomerWarehouse].dbo.PersianToMiladi(ss.RegisterDayJalali)) as Distance
                    From CustomerWarehouse.dbo.Clients c 
                    Join FirstBillGroup ff 
                    	On c.BillId=ff.BillId
                    Join SecondBillGroup ss
                    	On c.BillId=ss.BillId AND ff.UsageStateId<> ss.UsageStateId
                    Where
                    	c.ToDayJalali IS NULL AND
                    	ff.UsageStateId IN @fromUseStateIds AND
                        ss.UsageStateId IN @toUseStateIds AND
                        ss.RegisterDayJalali>ff.RegisterDayJalali AND
                        (@fromDate IS NULL OR
                        @toDate IS NULL OR
                        ss.RegisterDayJalali BETWEEN @fromDate AND @toDate) AND
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
                        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber)
                    	{zoneQuery}  ";
        }
    }
}
