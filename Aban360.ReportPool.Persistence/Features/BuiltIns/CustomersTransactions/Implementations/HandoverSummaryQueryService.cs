using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class HandoverSummaryQueryService : AbstractBaseConnection, IHandoverSummaryQueryService
    {
        public HandoverSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<HandoverHeaderOutputDto, HandoverSummaryDataOutputDto>> Get(HandoverInputDto input)
        {
            string handoverSummaryQuery = GetHandoverSummaryQuery();
            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,

                zoneIds = input.ZoneIds,
            };
            IEnumerable<HandoverSummaryDataOutputDto> data = await _sqlReportConnection.QueryAsync<HandoverSummaryDataOutputDto>(handoverSummaryQuery, @params);
            if (!data.Any())
            {
                throw new BaseException(ExceptionLiterals.NotFoundAnyData);
            }
            HandoverHeaderOutputDto header = new HandoverHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                RecordCount = data.Count(),
                ReprotDate = DateTime.Now.ToShortPersianDateString()
            };

            var result = new ReportOutput<HandoverHeaderOutputDto, HandoverSummaryDataOutputDto>(ReportLiterals.HandoverSummary, header, data);
            return result;
        }

        private string GetHandoverSummaryQuery()
        {
            return @"Select
                    	c.BranchType AS UseStateTitle,
                    	Count(c.BranchType) AS Count
                    From [CustomerWarehouse].dbo.Clients c
                    Where 
                    	c.ToDayJalali IS NULL AND
                    	(@fromReadingNumber IS NULL OR
                    	 @toReadingNumber IS NULL OR
                    	 c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                    	 c.ZoneId IN (131211,131213)
                    Group By 
                    	c.BranchType";
        }
    }
}
