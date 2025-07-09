using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class ServiceLinkCalculationDetailsQueryService : AbstractBaseConnection, IServiceLinkCalculationDetailsQueryService
    {
        public ServiceLinkCalculationDetailsQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<ServiceLinkCalculationDetailsHeaderOutputDto, ServiceLinkCalculationDetailsDataOutputDto>> GetInfo(ServiceLinkCalculationDetailsInputDto input)
        {
            string calculationDetailsDataInfoQuery = GetCalculationDetailsDataQuery();
            IEnumerable<ServiceLinkCalculationDetailsDataOutputDto> calculationDetailsData = await _sqlReportConnection.QueryAsync<ServiceLinkCalculationDetailsDataOutputDto>(calculationDetailsDataInfoQuery, new { Id = input.Input });
            ServiceLinkCalculationDetailsHeaderOutputDto calculationDetailsHeader = new ServiceLinkCalculationDetailsHeaderOutputDto()
			{

			};
			var result = new ReportOutput<ServiceLinkCalculationDetailsHeaderOutputDto, ServiceLinkCalculationDetailsDataOutputDto>(ReportLiterals.CalculationDetails, calculationDetailsHeader, calculationDetailsData );
            return result;
        }

        private string GetCalculationDetailsDataQuery()
        {
            return @"";
        }
    }
}
