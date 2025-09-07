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
    internal sealed class BranchTypeChangeHistoryQueryService : AbstractBaseConnection, IBranchTypeChangeHistoryQueryService
    {
        public BranchTypeChangeHistoryQueryService(IConfiguration configuration)
            : base(configuration)
        { }
        public async Task<ReportOutput<BranchTypeChangeHistoryHeaderOutputDto, BranchTypeChangeHistoryDataOutputDto>> GetInfo(BranchTypeChangeHistoryInputDto input)
        {
            string BranchTypeChangeHistoryQuery = GetBranchTypeChangeHistoryQuery(input.ZoneIds.Any());
            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,

                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,

                zoneIds = input.ZoneIds,

                fromUseStateIds = input.FromUseStateIds,
                toUseStateIds = input.ToUseStateIds,
            };

            IEnumerable<BranchTypeChangeHistoryDataOutputDto> BranchTypeChangeHistoryData = await _sqlReportConnection.QueryAsync<BranchTypeChangeHistoryDataOutputDto>(BranchTypeChangeHistoryQuery, @params);
            BranchTypeChangeHistoryHeaderOutputDto BranchTypeChangeHistoryHeader = new BranchTypeChangeHistoryHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,

                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,

                RecordCount = BranchTypeChangeHistoryData is not null && BranchTypeChangeHistoryData.Any() ? BranchTypeChangeHistoryData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),

                SumDomesticCount = BranchTypeChangeHistoryData is not null && BranchTypeChangeHistoryData.Any() ? BranchTypeChangeHistoryData.Sum(x => x.DomesticUnit) : 0,
                SumCommercialCount = BranchTypeChangeHistoryData is not null && BranchTypeChangeHistoryData.Any() ? BranchTypeChangeHistoryData.Sum(x => x.CommercialUnit) : 0,
                SumOtherCount = BranchTypeChangeHistoryData is not null && BranchTypeChangeHistoryData.Any() ? BranchTypeChangeHistoryData.Sum(x => x.OtherUnit) : 0,
                TotalUnit = BranchTypeChangeHistoryData is not null && BranchTypeChangeHistoryData.Any() ? BranchTypeChangeHistoryData.Sum(x => x.TotalUnit) : 0,
            };


            var result = new ReportOutput<BranchTypeChangeHistoryHeaderOutputDto, BranchTypeChangeHistoryDataOutputDto>(ReportLiterals.BranchTypeChangeHistory, BranchTypeChangeHistoryHeader, BranchTypeChangeHistoryData);

            return result;
        }
        private string GetBranchTypeChangeHistoryQuery(bool hasZone)
        {
            string zoneQuery = hasZone ? "AND c1.ZoneId IN @zoneIds" : string.Empty;

            return @$"Select 
                    	c1.CustomerNumber,
                    	c1.ReadingNumber,
                    	c1.BillId,
                    	c2.RegisterDayJalali AS ChangeDateJalali,
                    	c1.BranchType AS FromUseStateTitle,
                    	c2.BranchType AS ToUseStateTitle,
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
                    	c1.CommercialCount AS CommercialUnit,
                    	c1.DomesticCount AS DomesticUnit,
                    	c1.OtherCount AS OtherUnit,
                    	(c1.CommercialCount+c1.DomesticCount+c1.OtherCount) AS TotalUnit,
                    	c1.WaterDiameterTitle AS MeterDiameterTitle,
                    	c1.MainSiphonTitle AS SiphonDiameterTitle,
	                    c1.EmptyCount AS EmptyUnit
                    From [CustomerWarehouse].dbo.Clients c1
                    Join [CustomerWarehouse].dbo.Clients c2
                    	on c1.BillId=c2.BillId
                    Where
                    	c1.UsageStateId IN @fromUseStateIds AND
                    	c2.UsageStateId IN @toUseStateIds AND
                    	c2.RegisterDayJalali>c1.RegisterDayJalali AND
                    	(@fromDate IS NULL OR
                    	@toDate IS NULL OR
                    	c2.RegisterDayJalali BETWEEN @fromDate AND @toDate) AND
                    	(@fromReadingNumber IS NULL OR
                    	@toReadingNumber IS NULL OR
                    	c1.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber)
                    	{zoneQuery} ";
        }
    }
}
