using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations
{
    internal sealed class RequestBillDetailQueryService : AbstractBaseConnection, IRequestBillDetailQueryService
    {
        public RequestBillDetailQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<RequestBillDetailGetDto> Get(int id, string billId)
        {
            string query = GetByIdQuery();
            RequestBillDetailGetDto? data = await _sqlReportConnection.QueryFirstOrDefaultAsync<RequestBillDetailGetDto>(query, new { id, billId });
            if (data is null)
            {
                throw new InvalidTrackingException(ExceptionLiterals.NotFoundRequestBillDetail);
            }
            return data;
        }
        private string GetByIdQuery()
        {
            return $@"Select 
                    	Id,
                    	ZoneId,
                    	CustomerNumber,
                    	BillId,
                    	RegisterDate RegisterDateJalali,
                    	Amount,
                    	FinalAmount,
                    	OffAmount DiscountAmount,
                    	OffTitle DiscountTypeTitle,
                    	ItemId,
                    	ItemTitle,
                    	TypeId TypeTitle,
                    	TypeCode TypeCode
                    From CustomerWarehouse.dbo.RequestBillDetails
                    Where 
                    	Id=@Id AND 
                    	BillId=@BillId";
        }
    }
}
