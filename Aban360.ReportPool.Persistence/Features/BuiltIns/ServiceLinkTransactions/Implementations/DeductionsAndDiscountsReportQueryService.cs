using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class DeductionsAndDiscountsReportQueryService : AbstractBaseConnection, IDeductionsAndDiscountsReportQueryService
    {
        public DeductionsAndDiscountsReportQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<DeductionsAndDiscountsReportHeaderOutputDto, DeductionsAndDiscountsReportDataOutputDto>> GetInfo(DeductionsAndDiscountsReportInputDto input)
        {
            string deductionsAndDiscountsReportQueryString = GetDeductionsAndDiscountsReportDataQuery();
            IEnumerable<DeductionsAndDiscountsReportDataOutputDto> deductionsAndDiscountsReportData = await _sqlConnection.QueryAsync<DeductionsAndDiscountsReportDataOutputDto>(deductionsAndDiscountsReportQueryString);//todo: parameters
            DeductionsAndDiscountsReportHeaderOutputDto deductionsAndDiscountsReportHeader = new DeductionsAndDiscountsReportHeaderOutputDto()
            { };

            var result = new ReportOutput<DeductionsAndDiscountsReportHeaderOutputDto, DeductionsAndDiscountsReportDataOutputDto>(ReportLiterals.DeductionsAndDiscountsReport, deductionsAndDiscountsReportHeader, deductionsAndDiscountsReportData);

            return result;
        }

        private string GetDeductionsAndDiscountsReportDataQuery()
        {
            return " ";
        }

    }
}
