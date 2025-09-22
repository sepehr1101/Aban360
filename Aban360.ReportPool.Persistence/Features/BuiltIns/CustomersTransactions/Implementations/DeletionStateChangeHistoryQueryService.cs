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
    internal sealed class DeletionStateChangeHistoryQueryService : AbstractBaseConnection, IDeletionStateChangeHistoryQueryService
    {
        public DeletionStateChangeHistoryQueryService(IConfiguration configuration)
            : base(configuration)
        { }
        public async Task<ReportOutput<DeletionStateChangeHistoryHeaderOutputDto, DeletionStateChangeHistoryDataOutputDto>> GetInfo(DeletionStateChangeHistoryInputDto input)
        {
            string DeletionStateChangeHistoryQuery = GetDeletionStateChangeHistoryQuery(input.ZoneIds.Any());
            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,

                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,

                zoneIds = input.ZoneIds,

                fromDeletionStateIds = input.FromDeletionStateIds,
                toDeletionStateIds = input.ToDeletionStateIds,
            };

            IEnumerable<DeletionStateChangeHistoryDataOutputDto> deletionStateChangeHistoryData = await _sqlReportConnection.QueryAsync<DeletionStateChangeHistoryDataOutputDto>(DeletionStateChangeHistoryQuery, @params);
            DeletionStateChangeHistoryHeaderOutputDto deletionStateChangeHistoryHeader = new DeletionStateChangeHistoryHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,

                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,

                RecordCount = deletionStateChangeHistoryData is not null && deletionStateChangeHistoryData.Any() ? deletionStateChangeHistoryData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),

                SumDomesticCount = deletionStateChangeHistoryData is not null && deletionStateChangeHistoryData.Any() ? deletionStateChangeHistoryData.Sum(x => x.DomesticUnit) : 0,
                SumCommercialCount = deletionStateChangeHistoryData is not null && deletionStateChangeHistoryData.Any() ? deletionStateChangeHistoryData.Sum(x => x.CommercialUnit) : 0,
                SumOtherCount = deletionStateChangeHistoryData is not null && deletionStateChangeHistoryData.Any() ? deletionStateChangeHistoryData.Sum(x => x.OtherUnit) : 0,
                TotalUnit = deletionStateChangeHistoryData is not null && deletionStateChangeHistoryData.Any() ? deletionStateChangeHistoryData.Sum(x => x.TotalUnit) : 0,
            };


            var result = new ReportOutput<DeletionStateChangeHistoryHeaderOutputDto, DeletionStateChangeHistoryDataOutputDto>(ReportLiterals.DeletionStateChangeHistory, deletionStateChangeHistoryHeader, deletionStateChangeHistoryData);

            return result;
        }
        private string GetDeletionStateChangeHistoryQuery(bool hasZone)
        {
            string zoneQuery = hasZone ? "AND c.ZoneId IN @zoneIds" : string.Empty;

            return $@"use CustomerWarehouse
                    ;With FirstBillGroup as (
                    Select 
                    	c.DeletionStateId,
                    	c.BillId,
                    	c.DeletionStateTitle,
                    	MAX(RegisterDayJalali)as RegisterDayJalali
                    From [CustomerWarehouse].dbo.Clients c
                    Group By c.BillId,c.DeletionStateId,DeletionStateTitle
                    ),
                    SecondBillGroup as (
                    Select 
                    	c.DeletionStateId,
                    	c.BillId,
                    	c.DeletionStateTitle,
                    	MAX(RegisterDayJalali)as RegisterDayJalali
                    From [CustomerWarehouse].dbo.Clients c
                    Group By c.BillId,c.DeletionStateId,DeletionStateTitle
                    )
                    Select 
                        c.CustomerNumber,
                        c.ReadingNumber,
                        c.BillId,
                        ss.RegisterDayJalali AS ChangeDateJalali,
                        ff.DeletionStateTitle AS FromDeletionStateTitle,
                        ss.DeletionStateTitle AS ToDeletionStateTitle,
                        c.ZoneTitle,
                        c.ZoneId,
						c.UsageTitle,
                        TRIM(c.FirstName) AS FirstName,
                        TRIM(c.SureName) AS Surname,
                        TRIM(c.FirstName)+' '+TRIM(c.SureName) AS FullName,
                        TRIM(c.FatherName) AS FatherName,
                        TRIM(c.NationalId) AS NationalCode,
                        TRIM(c.PhoneNo) AS PhoneNumber,
                        TRIM(c.Address) AS Address,
                        TRIM(c.PostalCode) AS PostalCode,
                        c.ContractCapacity AS ContractualCapacity,
                        c.CommercialCount as CommercialUnit,
                        c.DomesticCount as DomesticUnit,
                        c.OtherCount as OtherUnit,
                        (c.CommercialCount+c.DomesticCount+c.OtherCount) AS TotalUnit,
                        c.WaterDiameterTitle AS MeterDiameterTitle,
                        c.MainSiphonTitle AS SiphonDiameterTitle,
                        c.BranchType AS UseStateTitle,
                        c.EmptyCount AS EmptyUnit,
					    DATEDIFF(DAY,[CustomerWarehouse].dbo.PersianToMiladi(ff.RegisterDayJalali),[CustomerWarehouse].dbo.PersianToMiladi(ss.RegisterDayJalali)) as Distance
                    From CustomerWarehouse.dbo.Clients c 
                    Join FirstBillGroup ff 
                    	On c.BillId=ff.BillId
                    Join SecondBillGroup ss
                    	On c.BillId=ss.BillId AND ff.DeletionStateId<> ss.DeletionStateId
                    Where
                    	c.ToDayJalali IS NULL AND
                    	ff.DeletionStateId IN @fromDeletionStateIds AND
                        ss.DeletionStateId IN @toDeletionStateIds AND
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
