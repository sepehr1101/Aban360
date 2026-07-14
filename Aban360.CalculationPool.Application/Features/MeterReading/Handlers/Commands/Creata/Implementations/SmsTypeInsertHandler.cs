using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Implementations
{
    internal sealed class SmsTypeInsertHandler : ISmsTypeInsertHandler
    {
        private readonly ISmsTypeService _service;
        public SmsTypeInsertHandler(ISmsTypeService service)
        {
            _service = service;
            _service.NotNull(nameof(service));
        }

        public async Task Handle(string title, CancellationToken cancellationToken)
        {
            await _service.Insert(title);
        }
    }
}
