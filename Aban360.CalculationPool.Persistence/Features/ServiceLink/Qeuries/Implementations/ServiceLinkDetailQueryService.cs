using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.CalculationPool.Persistence.Features.ServiceLink.Qeuries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.ServiceLink.Qeuries.Implementations
{
    internal sealed class ServiceLinkDetailQueryService : AbstractBaseConnection, IServiceLinkDetailQueryService
    {
        public ServiceLinkDetailQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<ServiceLinkInquiryOutputDto>> Get(ServiceLinkInquiryInputDto input)
        {
            string query = GetDetailQuery();
            IEnumerable<ServiceLinkInquiryOutputDto> result = await _sqlReportConnection.QueryAsync<ServiceLinkInquiryOutputDto>(query, input);
            return result;
        }

        private string GetDetailQuery()
        {
            return @"Select 
                    	ItemTitle,
                    	Amount,
                    	OffAmount,
                    	FinalAmount,
                    	OffTitle,
                    	TypeId TypeTitle,
                    	RegisterDate RegisterDateJalali
                    From CustomerWarehouse.dbo.RequestBillDetails
                    Where 
                    	TrackNumber=@PaymentId AND
                    	BillId=@BillId";
        }
    }
}
