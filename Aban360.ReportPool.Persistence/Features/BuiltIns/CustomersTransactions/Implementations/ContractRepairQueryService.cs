using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class ContractRepairQueryService : AbstractBaseConnection, IContractRepairQueryService
    {
        public ContractRepairQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<ReportOutput<ContractRepairHeaderOutputDto, ContractRepairDataOutputDto>> GetInfo(ContractRepairInputDto input)
        {
            string title = ReportLiterals.ContractRepair;
            string query = GetQuery(input.IsWater);

            IEnumerable<ContractRepairDataOutputDto> data = await _sqlReportConnection.QueryAsync<ContractRepairDataOutputDto>(query, input);
            ContractRepairHeaderOutputDto header = new ContractRepairHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,

                CustomerCount = data is not null && data.Any() ? data.Count() : 0,
                RecordCount = data is not null && data.Any() ? data.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = title,
            };


            var result = new ReportOutput<ContractRepairHeaderOutputDto, ContractRepairDataOutputDto>(title, header, data);

            return result;
        }
        private string GetQuery(bool isWater)
        {
            string requestBillDetailItems = isWater ? "(311,106,101,103,109,331,361,601)" : "(312,201,203,209,332,362,602)";

            return $@";WITH History AS
                    (
                        SELECT *
                        FROM
                        (
                            SELECT
                                Id,
                                BillId,
                                CommercialCount AS CommerciaUnit,
                                DomesticCount AS DomesticUnit,
                                OtherCount AS OtherUnit,
                                UsageTitle,
                                ContractCapacity AS ContractualCapacity,
                                RegisterDayJalali,
                                LAG(CommercialCount) OVER(PARTITION BY BillId ORDER BY RegisterDayJalali, localId) AS PreviousCommercialUnit,
                                LAG(DomesticCount) OVER(PARTITION BY BillId ORDER BY RegisterDayJalali, localId) AS PreviousDomesticUnit,
                                LAG(OtherCount) OVER(PARTITION BY BillId ORDER BY RegisterDayJalali, localId) AS PreviousOtherUnit,
                                LAG(UsageTitle) OVER(PARTITION By BillId ORDER BY RegisterDayJalali, localId) PreviousUsageTitle,
                                LAG(ContractCapacity) OVER(PARTITION By BillId ORDER BY RegisterDayJalali, localId) PreviousContractualCapacity,
                                LAG(RegisterDayJalali) OVER(PARTITION By BillId ORDER BY RegisterDayJalali, localId) PreviousRegisterDayJalali
                            FROM CustomerWarehouse.dbo.Clients
                            WHERE 
                                ZoneId IN @ZoneIds AND
                                UsageId IN @UsageIds
                        ) AS ClientsInfo
                        WHERE
                            RegisterDayJalali BETWEEN @FromDateJalali AND @ToDateJalali
                            AND PreviousContractualCapacity IS NOT NULL
                            AND (
                                   PreviousCommercialUnit <> CommerciaUnit
                                OR PreviousDomesticUnit <> DomesticUnit
                                OR PreviousOtherUnit <> OtherUnit
                                OR PreviousUsageTitle <> UsageTitle
                                OR PreviousContractualCapacity <> ContractualCapacity
                            )
                    ),
                    RequestBillInfo as
                    (
                    	Select h.BillId,
                    	        h.RegisterDayJalali,
                    	        h.PreviousRegisterDayJalali,
                    	        h.UsageTitle,
                    	        h.CommerciaUnit,
                    	        h.DomesticUnit,
                    	        h.OtherUnit,
                    	        h.ContractualCapacity,
                    	        h.PreviousUsageTitle,
                    	        h.PreviousCommercialUnit,
                    	        h.PreviousDomesticUnit,
                    	        h.PreviousOtherUnit,
                    	        h.PreviousContractualCapacity,
                    			SUM(amount) Amount
                    	From CustomerWarehouse.dbo.RequestBillDetails r
                    	LEFT Join History h	
                    		ON r.BillId collate Arabic_CI_AS=h.BillId 
                    	        AND r.RegisterDate = h.RegisterDayJalali
                    	Where 
                    		r.ItemId IN {requestBillDetailItems} AND
                    		r.RegisterDate BETWEEN @FromDateJalali AND @ToDateJalali AND
                    		r.ZoneId IN @ZoneIds and
                    		h.PreviousCommercialUnit is not null
                    	GROUP BY
                    	        h.BillId,
                    	        h.RegisterDayJalali,
                    	        h.PreviousRegisterDayJalali,
                    	        h.UsageTitle,
                    	        h.CommerciaUnit,
                    	        h.DomesticUnit,
                    	        h.OtherUnit,
                    	        h.ContractualCapacity,
                    	        h.PreviousUsageTitle,
                    	        h.PreviousCommercialUnit,
                    	        h.PreviousDomesticUnit,
                    	        h.PreviousOtherUnit,
                    	        h.PreviousContractualCapacity
                    )
                    SELECT
                        t46.C0 AS RegionId,
                        t46.C2 AS RegionTitle,
                        c.ZoneId,
                        c.ZoneTitle,
                        c.CustomerNumber,
                        c.ReadingNumber,
                        c.BillId,
                        c.FirstName,
                        c.SureName AS Surname,
                        c.FirstName + ' ' + c.SureName AS FullName,
                    
                        r.RegisterDayJalali,
                        r.PreviousRegisterDayJalali,
                    
                        r.UsageTitle,
                        r.CommerciaUnit,
                        r.DomesticUnit,
                        r.OtherUnit,
                        r.ContractualCapacity,
                    
                        r.PreviousUsageTitle,
                        r.PreviousCommercialUnit,
                        r.PreviousDomesticUnit,
                        r.PreviousOtherUnit,
                        r.PreviousContractualCapacity,
                    
                        r.Amount
                    FROM RequestBillInfo r
                    JOIN CustomerWarehouse.dbo.Clients c
                        ON c.BillId = r.BillId
                    JOIN Db70.dbo.T51 t51
                        ON t51.C0 = c.ZoneId
                    JOIN Db70.dbo.T46 t46
                        ON t46.C0 = t51.C1
                    Where c.ToDayJalali IS NULL
                    ORDER BY
                        c.ZoneTitle,
                        c.BillId,
                        r.RegisterDayJalali;";
        }
    }
}
