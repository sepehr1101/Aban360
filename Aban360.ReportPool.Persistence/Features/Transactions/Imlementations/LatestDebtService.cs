using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.Transactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.Transactions.Imlementations
{
    internal sealed class LatestDebtService : AbstractBaseConnection, ILatestDebtService
    {
        public LatestDebtService(IConfiguration configuration) : base(configuration)
        {

        }
        public async Task<LatestDebtDto> GetLatestDebt(string billId)
        {
            DebtDto? ServiceLinkDebt = await GetServiceLinkDebt(billId);
            DebtDto? WaterBillDebt = await GetWaterBillDebt(billId);
            return new LatestDebtDto(billId, WaterBillDebt?.Debt, ServiceLinkDebt?.Debt);
        }
        private async Task<DebtDto?> GetWaterBillDebt(string billId)
        {
            DebtDto? result = await _sqlConnection.QueryFirstOrDefaultAsync<DebtDto?>(GetWaterBillDebtQuery(), new { BillId = billId });
            return result;
            string GetWaterBillDebtQuery()
            {
                string query = @"SELECT SUM(Amount) AS Debt FROM 
                                (SELECt SumItems Amount from [172.18.12.60].CustomerWarehouse.dbo.Bills where TRIM(billId)=@BillId
                                Union
                                SELECT -1*Amount Amount from [172.18.12.60].CustomerWarehouse.dbo.Payments where TRIM(billId)=@BillId) X";
                return query;
            }
        }
        private async Task<DebtDto>? GetServiceLinkDebt(string billId)
        {
            DebtDto? result = await _sqlConnection.QueryFirstOrDefaultAsync<DebtDto?>(GetServiceLinkDebtQuery(), new { BillId = billId });
            return result;
            string GetServiceLinkDebtQuery()
            {
                string query = @"SELECT BedehiAll AS Debt
                                 FROM [172.18.12.60].[CustomerWarehouse].[dbo].[VosoolEnsheabAlert] 
                                 WHERE BillId=@BillId";
                return query;
            }
        }
    }
}
