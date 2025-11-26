using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;

namespace Aban360.Api.Cronjobs
{
    public class MeterLifeJob
    {
        private readonly IMeterLifeInsertHandler _handler;

        public MeterLifeJob(IMeterLifeInsertHandler handler)
        {
            _handler = handler;
        }

        public async Task RunAsync()
        {           
            await _handler.Handle(CancellationToken.None);
        }
    }
}
