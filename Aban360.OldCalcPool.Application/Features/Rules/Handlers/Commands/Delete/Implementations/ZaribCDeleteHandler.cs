using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Delete.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Contracts;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Delete.Implementations
{
    internal sealed class ZaribCDeleteHandler : IZaribCDeleteHandler
    {
        private readonly IZaribCCommandService _zaribCCommandService;

        public ZaribCDeleteHandler(IZaribCCommandService zaribCCommandService)
        {
            _zaribCCommandService = zaribCCommandService;
            _zaribCCommandService.NotNull(nameof(zaribCCommandService));
        }
        public async Task Handle(int id, CancellationToken cancellationToken)
        {
            await _zaribCCommandService.Delete(id);
        }
    }
}
