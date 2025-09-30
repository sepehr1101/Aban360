using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using System.Runtime.InteropServices;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts
{
    public interface IHandoverHandler
    {
        Task<IEnumerable<HandoverQueryDto>> Handle(CancellationToken cancellationToken);
    }
}
