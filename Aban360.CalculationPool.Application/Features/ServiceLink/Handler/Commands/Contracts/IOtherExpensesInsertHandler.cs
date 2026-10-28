using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts
{
    public interface IOtherExpensesInsertHandler
    {
        Task<ReportOutput<OtherExpensesHeaderOutputDto, OtherExpensesDataOutputDto>> Handle(OtherExpensesInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
        Task<MemberInfoGetDto> ValidateAndGetMemberInfo(OtherExpensesInsertInputDto inputDto, IAppUser appUser);
        Task<(IEnumerable<KartInsertDto>, IEnumerable<RequestBillDetailsInsertDto>)> GetKartAndRequestBillDetailDto(OtherExpensesInsertInputDto inputDto, MemberInfoGetDto memberInfo);
        Task ExecSql(IDbConnection connection, IDbTransaction transaction, IEnumerable<KartInsertDto> kartsInsertDto, IEnumerable<RequestBillDetailsInsertDto> requestBillDetailsInsertDto, OtherExpensesInsertInputDto inputDto, MemberInfoGetDto memeberInfo, IAppUser appUser, string? opLogText);
    }
}
