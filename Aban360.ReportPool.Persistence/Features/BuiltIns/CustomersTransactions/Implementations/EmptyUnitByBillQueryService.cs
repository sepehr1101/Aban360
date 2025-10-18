using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
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
    internal sealed class EmptyUnitByBillQueryService : EmptyUnitByBillBase, IEmptyUnitByBillQueryService
    {
        public EmptyUnitByBillQueryService(IConfiguration configuration)
            : base(configuration)
        {
		}
        public async Task<ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto>> GetInfo(EmptyUnitByBillInputDto input)
        {
            string query = GetDetailQuery(input.ZoneIds.HasValue(), input.UsageSellIds.HasValue());
           
            IEnumerable<EmptyUnitDataOutputDto> emptyUnitData = await _sqlReportConnection.QueryAsync<EmptyUnitDataOutputDto>(query, input);
            EmptyUnitHeaderOutputDto emptyUnitHeader = new EmptyUnitHeaderOutputDto()
            {
                FromEmptyUnit = input.FromEmptyUnit,
                ToEmptyUnit = input.ToEmptyUnit,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                CustomerCount = emptyUnitData is not null && emptyUnitData.Any() ? emptyUnitData.Count() : 0,
                RecordCount = emptyUnitData is not null && emptyUnitData.Any() ? emptyUnitData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
				Title= ReportLiterals.EmptyUnitByBillDetail,

                SumDomesticUnit = (emptyUnitData is not null && emptyUnitData.Any()) ? emptyUnitData.Sum(x => x.DomesticUnit) : 0,
                SumCommercialUnit = emptyUnitData is not null && emptyUnitData.Any() ? emptyUnitData.Sum(x => x.CommercialUnit) : 0,
                SumOtherUnit = emptyUnitData is not null && emptyUnitData.Any() ? emptyUnitData.Sum(x => x.OtherUnit) : 0,
                TotalUnit = emptyUnitData is not null && emptyUnitData.Any() ? emptyUnitData.Sum(x => x.TotalUnit) : 0,
                SumEmptyUnit = (emptyUnitData is not null && emptyUnitData.Any()) ? emptyUnitData.Sum(x => x.EmptyUnit) : 0,

            };

            var result = new ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto>(ReportLiterals.EmptyUnitByBillDetail, emptyUnitHeader, emptyUnitData);
            return result;
        }
    }
}
