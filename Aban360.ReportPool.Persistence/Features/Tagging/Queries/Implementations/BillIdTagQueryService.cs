using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Domain.Features.Tagging.CustomerWarehouse.Application.DTOs;
using Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.Tagging.Queries.Implementations
{
    internal sealed class BillIdTagQueryService : AbstractBaseConnection, IBillIdTagQueryService
    {
        public BillIdTagQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<BillIdTagDto>> GetByBillId(string billId)
        {
            var sql = @"
                SELECT 
                    Id, 
                    BillId,
                    ExpireDateJalali, 	
                    IIF(ExpireDateJalali IS NULL OR [CustomerWarehouse].dbo.PersianToMiladi(ExpireDateJalali)>GETDATE()  ,1,0) IsValid,
                    TagId,
                    TagTitle, 
                    CreateDateTime,
                    DeleteDateTime
                FROM [CustomerWarehouse].dbo.BillIdTags
                WHERE 
                    BillId = @BillId AND 
                    DeleteDateTime IS NULL";

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
                    (ExpireDateJalali IS NULL OR LEN(ExpireDateJalali)=0 OR [CustomerWarehouse].dbo.PersianToMiladi(ExpireDateJalali)>GETDATE() )
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
        public async Task<IEnumerable<BillIdTagDto>> GetByTagIds(IEnumerable<int> tagIds)
        {
            var sql = @"
                SELECT 
                    Id, 
                    BillId,
                    ExpireDateJalali, 	
                    IIF(  ExpireDateJalali IS NULL OR LEN(ExpireDateJalali)=0 OR [CustomerWarehouse].dbo.PersianToMiladi(ExpireDateJalali)>GETDATE() ,1,0) IsValid,
                    TagId,
                    TagTitle, 
                    CreateDateTime,
                    DeleteDateTime
                FROM [CustomerWarehouse].dbo.BillIdTags
                WHERE 
                    DeleteDateTime IS NULL AND
                    TagId IN @TagIds";

            return await _sqlReportConnection.QueryAsync<BillIdTagDto>(sql, new { tagIds });
        }

    }
}
