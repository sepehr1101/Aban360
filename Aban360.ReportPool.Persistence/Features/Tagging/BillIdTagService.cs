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
        Task<IEnumerable<int>> GetIdsByBillId(string billId);
        Task<bool> HasBillIdTags(string billId, int tagId);
        Task<bool> HasBillId(string billId);
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
                INSERT INTO [CustomerWarehouse].dbo.BillIdTags (BillId, TagId, TagTitle, ExpireDateJalali, CreateDateTime)
                SELECT @BillId, @TagId, t.Title, @ExpireDateJalali, GETUTCDATE()  
                FROM [CustomerWarehouse].dbo.Tags t
                WHERE t.Id = @TagId;
                SELECT CAST(SCOPE_IDENTITY() as bigint);";

            return await _sqlReportConnection.ExecuteScalarAsync<long>(sql, dto);
        }

        public async Task<IEnumerable<BillIdTagDto>> GetByBillId(string billId)
        {
            var sql = @"
                SELECT Id, BillId, ExpireDateJalali, TagId, TagTitle, CreateDateTime, DeleteDateTime
                FROM [CustomerWarehouse].dbo.BillIdTags
                WHERE 
                    BillId = @BillId AND 
                    DeleteDateTime IS NULL   AND
	                [CustomerWarehouse].dbo.PersianToMiladi(ExpireDateJalali)>GETDATE() ";

            return await _sqlReportConnection.QueryAsync<BillIdTagDto>(sql, new { BillId = billId });
        }

        public async Task<IEnumerable<int>> GetIdsByBillId(string billId)
        {
            var sql = @"
                SELECT TagId
                FROM  [CustomerWarehouse].dbo.BillIdTags
                WHERE 
                    BillId = @BillId AND 
                    DeleteDateTime IS NULL  AND
	                [CustomerWarehouse].dbo.PersianToMiladi(ExpireDateJalali)>GETDATE() 
                GROUP BY TagId";

            IEnumerable<int> tagIds = await _sqlReportConnection.QueryAsync<int>(sql, new { BillId = billId });
            if (tagIds is null)
            {
                new List<int>();
            }
            return tagIds;
        }

        public async Task<bool> HasBillIdTags(string billId, int tagId)
        {
            var sql = @"Select 1
                        From [CustomerWarehouse].dbo.BillIdTags b
                        Where 
                        	b.BillId=@billId AND
                        	b.TagId=@tagId";
            int hasRecord = await _sqlConnection.QueryFirstOrDefaultAsync<int>(sql, new { billId, tagId });
            return hasRecord == 0 ? false : true;
        }

        public async Task<bool> HasBillId(string billId)
        {
            var sql = @"Select 1
                       From [CustomerWarehouse].dbo.Clients c
                       Where 
                       	c.BillId=@billId";
            int hasRecord = await _sqlReportConnection.QueryFirstOrDefaultAsync<int>(sql, new { billId });
            return hasRecord == 0 ? false : true;
        }

        public async Task<bool> Delete(long id)
        {
            var sql = "DELETE FROM [CustomerWarehouse].dbo.BillIdTags WHERE Id = @Id";
            var rows = await _sqlReportConnection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
