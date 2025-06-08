using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class WithoutBillQueryService : AbstractBaseConnection, IWithoutBillQueryService
    {
        public WithoutBillQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WithoutBillHeaderOutputDto, WithoutBillDataOutputDto>> GetInfo(WithoutBillInputDto input)
        {
            string withoutBill = GetWithoutBillQuery();
            IEnumerable<WithoutBillDataOutputDto> withoutBillData = await _sqlConnection.QueryAsync<WithoutBillDataOutputDto>(withoutBill);//todo: Parameters
            WithoutBillHeaderOutputDto withoutBillHeader = new WithoutBillHeaderOutputDto()
            { };

            var result = new ReportOutput<WithoutBillHeaderOutputDto, WithoutBillDataOutputDto>(ReportLiterals.WithoutBill, withoutBillHeader, withoutBillData);
            return result;
        }

        private string GetWithoutBillQuery()
        {
            return @" ";
        }
    }
}
