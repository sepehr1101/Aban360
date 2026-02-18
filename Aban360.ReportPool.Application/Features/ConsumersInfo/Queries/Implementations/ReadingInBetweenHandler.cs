using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations
{
    internal sealed class ReadingInBetweenHandler : IReadingInBetweenHandler
    {
        private readonly IReadingInBetweenService _queryService;
        public ReadingInBetweenHandler(IReadingInBetweenService readingInBetweenService)
        {
            _queryService = readingInBetweenService;
            _queryService.NotNull(nameof(readingInBetweenService));
        }

        public async Task<IEnumerable<ReadingInBetweenOutput>> Handle(ReadingInBetweenInput input, CancellationToken cancellationToken)
        {
            return await _queryService.Get(input);
        }
    }
}
