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
            string zoneQuery = hasZone ? "AND c1.ZoneId IN @zoneIds" : string.Empty;

            return @$"Select Distinct
                	c1.DeletionStateId,c1.DeletionStateTitle,
                	c1.CustomerNumber,
                	c1.ReadingNumber,
                	c1.BillId,
                	c2.RegisterDayJalali AS ChangeDateJalali,
                	c1.DeletionStateTitle AS FromDeletionStateTitle,
                	c2.DeletionStateTitle AS ToDeletionStateTitle,
                    c1.UsageTitle,
                    c1.ZoneTitle,
                    c1.ZoneId,
                	TRIM(c1.FirstName) AS FirstName,
                	TRIM(c1.SureName) AS Surname,
                	TRIM(c1.FatherName) AS FatherName,
                	TRIM(c1.FirstName)+' '+TRIM(c1.SureName) AS FullName,
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
                	c1.EmptyCount AS EmptyUnit,
				    DATEDIFF(DAY,[CustomerWarehouse].dbo.PersianToMiladi(c1.RegisterDayJalali),[CustomerWarehouse].dbo.PersianToMiladi(c2.RegisterDayJalali)) as Distance
                From [CustomerWarehouse].dbo.Clients c1
                Join [CustomerWarehouse].dbo.Clients c2
                	on c1.BillId=c2.BillId
                Where
                	c1.DeletionStateId IN @fromDeletionStateIds AND
                	c2.DeletionStateId IN @toDeletionStateIds AND
                	c2.RegisterDayJalali>c1.RegisterDayJalali AND
                	(@fromDate IS NULL OR
                	@toDate IS NULL OR
                	c2.RegisterDayJalali BETWEEN @fromDate AND @toDate) AND
                	(@fromReadingNumber IS NULL OR
                	@toReadingNumber IS NULL OR
                	c1.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                    c1.DeletionStateId != c2.DeletionStateId
                	{zoneQuery} ";
        }
    }
}
