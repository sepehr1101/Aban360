using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class MeterReplacementLifeDetailQueryService : AbstractBaseConnection, IMeterReplacementLifeDetailQueryService
    {
        public MeterReplacementLifeDetailQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<MeterReplacementLifeHeaderOutputDto, MeterReplacementLifeDataOutputDto>> Get(MeterReplacementLifeInputDto input)
        {
            string reportTitle = ReportLiterals.MeterReplacementLifeDetail;
            string query = GetQuery();

            IEnumerable<MeterReplacementLifeDataOutputDto> data = await _sqlReportConnection.QueryAsync<MeterReplacementLifeDataOutputDto>(query, input);
            MeterReplacementLifeHeaderOutputDto header = new()
            {
                FromChangeDateJalali = input.FromChangeDateJalali,
                ToChangeDateJalali = input.ToChangeDateJalali,

                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = data.Count(),
                CustomerCount = data.Count(),
                Title = reportTitle,
            };
            ReportOutput<MeterReplacementLifeHeaderOutputDto, MeterReplacementLifeDataOutputDto> result = new(reportTitle, header, data);
            return result;
        }

        private string GetQuery()
        {
            return $@"";
        }
    }
}
