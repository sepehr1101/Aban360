using Aban360.Common.ApplicationUser;
using Aban360.ReportPool.Domain.Features.Tagging;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts
{
    public interface IBillIdTagInsertExcelFileHandler
    {
        Task Handle(BillIdTagInsertByExcelFileInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
