using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.Tagging.CustomerWarehouse.Application.DTOs;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.Tagging
{
    public interface IBillIdTagService
    {
        Task<long> Create(CreateBillIdTagDto dto);
        Task<bool> Delete(long id);
        Task<IEnumerable<BillIdTagDto>> GetByBillId(string billId);
    }

    internal sealed class BillIdTagService : AbstractBaseConnection, IBillIdTagService
    {
        public BillIdTagService(IConfiguration configuration)
            : base(configuration)
        {
        }

        // CREATE
        public async Task<long> Create(CreateBillIdTagDto dto)
        {
            var sql = @"
                INSERT INTO BillIdTags (BillId, TagId, TagTitle, CreateDateTime)
                VALUES (@BillId, @TagId, @TagTitle, GETUTCDATE());
                SELECT CAST(SCOPE_IDENTITY() as bigint);";

            return await _sqlReportConnection.ExecuteScalarAsync<long>(sql, dto);
        }

        public async Task<IEnumerable<BillIdTagDto>> GetByBillId(string billId)
        {
            var sql = @"
                SELECT Id, BillId, TagId, TagTitle, CreateDateTime, DeleteDateTime
                FROM BillIdTags
                WHERE BillId = @BillId AND DeleteDateTime IS NULL";

            return await _sqlReportConnection.QueryAsync<BillIdTagDto>(sql, new { BillId = billId });
        }

        public async Task<bool> Delete(long id)
        {
            var sql = "DELETE FROM BillIdTags WHERE Id = @Id";
            var rows = await _sqlReportConnection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
