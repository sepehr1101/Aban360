using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Implementations
{
    internal sealed class Table1GetAllHandler : ITable1GetAllHandler
    {
        private readonly ITable1GetAllService _table1GetAllService;
        public Table1GetAllHandler(ITable1GetAllService table1GetAllService)
        {
            _table1GetAllService = table1GetAllService;
            _table1GetAllService.NotNull(nameof(table1GetAllService));
        }

        public async Task<IEnumerable<Table1GetDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<Table1GetDto> result = await _table1GetAllService.Get();
            return result;
        }
    }
}
