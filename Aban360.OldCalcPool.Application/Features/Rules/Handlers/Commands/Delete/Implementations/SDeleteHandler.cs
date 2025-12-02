using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Delete.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Contracts;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Delete.Implementations
{
    internal sealed class SDeleteHandler : ISDeleteHandler
    {
        private readonly ISCommandService _sCommandService;

        public SDeleteHandler(ISCommandService sCommandService)
        {
            _sCommandService = sCommandService;
            _sCommandService.NotNull(nameof(sCommandService));
        }
        public async Task Handle(int id, CancellationToken cancellationToken)
        {
            await _sCommandService.Delete(id);
        }
    }
}
