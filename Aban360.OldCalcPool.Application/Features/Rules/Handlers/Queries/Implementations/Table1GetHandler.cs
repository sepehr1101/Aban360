using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Implementations
{
    internal sealed class Table1GetHandler : ITable1GetHandler
    {
        private readonly ITable1GetService _table1GetService;
        public Table1GetHandler(ITable1GetService table1GetService)
        {
            _table1GetService = table1GetService;
            _table1GetService.NotNull(nameof(table1GetService));
        }

        public async Task<IEnumerable<Table1GetDto>> Handle(int id, CancellationToken cancellationToken)
        {
            IEnumerable<Table1GetDto> result = await _table1GetService.Get(id);
            return result;
        }
    }
}
