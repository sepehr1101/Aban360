using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class NonPermanentBranchQueryService : NonPermanentBranchBase, INonPermanentBranchQueryService
    {
        public NonPermanentBranchQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }
        public async Task<ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchDataOutputDto>> GetInfo(NonPermanentBranchInputDto input)
        {
            string query = GetDetailsQuery();

            IEnumerable<NonPermanentBranchDataOutputDto> nonPremanentBranchData = await _sqlReportConnection.QueryAsync<NonPermanentBranchDataOutputDto>(query, input);
            NonPermanentBranchHeaderOutputDto nonPremanentBranchHeader = new NonPermanentBranchHeaderOutputDto()
            {
                FromReadingNumber= input.FromReadingNumber,
                ToReadingNumber= input.ToReadingNumber,
                RecordCount = (nonPremanentBranchData is not null && nonPremanentBranchData.Any()) ? nonPremanentBranchData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title= ReportLiterals.NonPermanentBranchDetail,

                CustomerCount = (nonPremanentBranchData is not null && nonPremanentBranchData.Any()) ? nonPremanentBranchData.Count() : 0,
                SumCommercialUnit = nonPremanentBranchData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = nonPremanentBranchData.Sum(i => i.DomesticUnit),
                SumOtherUnit = nonPremanentBranchData.Sum(i => i.OtherUnit),
                TotalUnit = nonPremanentBranchData.Sum(i => i.TotalUnit)
            };

            var result = new ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchDataOutputDto>(ReportLiterals.NonPermanentBranchDetail, nonPremanentBranchHeader, nonPremanentBranchData);

            return result;
        }
    }
}
