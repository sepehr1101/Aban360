using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class ReadingChecklistQueryService : AbstractBaseConnection, IReadingChecklistQueryService
    {
        public ReadingChecklistQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<ReadingChecklistHeaderOutputDto, ReadingChecklistDataOutputDto>> Get(ReadingChecklistInputDto input)
        {
            string ReadingChecklistQueryString = GetReadingChecklistQuery();
            IEnumerable<ReadingChecklistDataOutputDto> data = await _sqlReportConnection.QueryAsync<ReadingChecklistDataOutputDto>(ReadingChecklistQueryString);//todo:Params
            ReadingChecklistHeaderOutputDto header = new()
            {
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = data.Count()
            };

            ReportOutput<ReadingChecklistHeaderOutputDto, ReadingChecklistDataOutputDto> result = new(ReportLiterals.ReadingChecklist, header, data);
            return result;
        }
        private string GetReadingChecklistQuery()
        {
            return @"";
        }
    }
}
