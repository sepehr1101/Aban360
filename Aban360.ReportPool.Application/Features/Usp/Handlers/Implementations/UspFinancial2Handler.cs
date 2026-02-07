using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Usp.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.Usp.Input;
using Aban360.ReportPool.Domain.Features.Usp.Output;
using Aban360.ReportPool.Persistence.Features.Usp.Contracts;

namespace Aban360.ReportPool.Application.Features.Usp.Handlers.Implementations
{
    internal sealed class UspFinancial2Handler : IUspFinancial2Handler
    {
        private readonly IUspFinancial2QueryService _queryService;
        public UspFinancial2Handler(IUspFinancial2QueryService queryService)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(_queryService));
        }
        public async Task<IEnumerable<UspFinancial2Output>> Handle(UspFinancial2Input input, CancellationToken cancellationToken)
        {
            //TODO: validate
            return await _queryService.Get(input);
        }
    }
}