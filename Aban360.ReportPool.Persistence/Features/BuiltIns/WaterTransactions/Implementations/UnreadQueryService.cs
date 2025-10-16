using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class UnreadQueryService : UnreadBase, IUnreadQueryService
    {
        public UnreadQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<UnreadHeaderOutputDto, UnreadDataOutputDto>> GetInfo(UnreadInputDto input)
        {
            string query = GetDetailQuery(input.ZoneIds.HasValue());

            IEnumerable<UnreadDataOutputDto> unreadData = await _sqlReportConnection.QueryAsync<UnreadDataOutputDto>(query, input, null, 180);
            UnreadHeaderOutputDto unreadHeader = new UnreadHeaderOutputDto()
            { 
                FromReadingNumber=input.FromReadingNumber,
                ToReadingNumber= input.ToReadingNumber,
                FromPeriodCount= input.FromPeriodCount,
                ToPeriodCount= input.ToPeriodCount,
                ReportDateJalali=DateTime.Now.ToShortPersianDateString(),
                RecordCount= (unreadData is not null && unreadData.Any()) ? unreadData.Count() : 0,
                CustomerCount = (unreadData is not null && unreadData.Any()) ? unreadData.Count() : 0,
                Title= ReportLiterals.UnreadDetail,
            };

            var result = new ReportOutput<UnreadHeaderOutputDto, UnreadDataOutputDto>(ReportLiterals.UnreadDetail, unreadHeader, unreadData);
            return result;
        }
    }
}
