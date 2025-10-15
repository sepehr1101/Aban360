using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class ReadingListDetailQueryService : ReadingListBase, IReadingListDetailQueryService
    {
        public ReadingListDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<ReadingListHeaderOutputDto, ReadingListDetailDataOutputDto>> GetInfo(ReadingListInputDto input)
        {
            string query = GetDetailQuery();
           
            var @params = new
            {
            };
            IEnumerable<ReadingListDetailDataOutputDto> modifiedBillsData = await _sqlReportConnection.QueryAsync<ReadingListDetailDataOutputDto>(query, @params);
            ReadingListHeaderOutputDto modifiedBillsHeader = new ReadingListHeaderOutputDto()
            {
                ReportDateJalali = DateTime.Now.ToShortDateString(),
                RecordCount = modifiedBillsData.Count(),
                CustomerCount = modifiedBillsData.Count(),
                Title= ReportLiterals.ReadingListDetail,
            };

            var result = new ReportOutput<ReadingListHeaderOutputDto, ReadingListDetailDataOutputDto>(ReportLiterals.ReadingListDetail, modifiedBillsHeader, modifiedBillsData);
            return result;
        }

        private string GetReadingListQuery()
        {
            return @"";
        }
    }
}
