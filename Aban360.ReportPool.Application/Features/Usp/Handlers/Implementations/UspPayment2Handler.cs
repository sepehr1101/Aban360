using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Usp.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.Usp.Input;
using Aban360.ReportPool.Domain.Features.Usp.Output;
using Aban360.ReportPool.Persistence.Features.Usp.Implementations;

namespace Aban360.ReportPool.Application.Features.Usp.Handlers.Implementations
{
    internal sealed class UspPayment2Handler : IUspPayment2Handler
    {
        private readonly IUspPayment2QueryService _queryService;
        public UspPayment2Handler(IUspPayment2QueryService queryService)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(_queryService));
        }
        public async Task<ReportOutput<UspPayment2Header, UspPayment2Output>> Handle(UspPayment2Input input, CancellationToken cancellationToken)
        {
            //TODO: validate            
            IEnumerable<UspPayment2Output> output = await _queryService.Get(input);
            UspPayment2Header header = new()
            {
                pard1 = output.Sum(i => i.pard1),
                pard2 = output.Sum(i => i.pard2),
                pard3 = output.Sum(i => i.pard3),
                pard4 = output.Sum(i => i.pard4),
                ted1 = output.Sum(i => i.ted1),
                ted2 = output.Sum(i => i.ted2),
                ted3 = output.Sum(i => i.ted3),
                ted4 = output.Sum(i => i.ted4),
                tedad = output.Sum(i => i.tedad)
            };
            ReportOutput<UspPayment2Header, UspPayment2Output> reportOutput = new(ReportLiterals.UspPayment2, header, output);
            return reportOutput;
        }
    }
}