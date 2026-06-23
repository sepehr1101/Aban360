using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts
{
    public interface IJudicalNoticeCommandHandler
    {
        Task<FlatReportOutput<JudicalNoticeCommandHeaderOutputDto, JudicalNoticeCommandDataOutputDto>> Handle(JudicalNoticeCommandInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }

}
