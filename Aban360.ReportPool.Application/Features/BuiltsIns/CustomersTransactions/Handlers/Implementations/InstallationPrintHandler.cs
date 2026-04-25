using Aban360.Common.BaseEntities;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class InstallationPrintHandler : IInstallationPrintHandler
    {
        private readonly IInstallationPrintQueryService _queryService;
        private readonly ICommonMemberQueryService _memberQueryService;
        public InstallationPrintHandler(
            IInstallationPrintQueryService queryService,
            ICommonMemberQueryService memberQueryService)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));

            _memberQueryService = memberQueryService;
            _memberQueryService.NotNull(nameof(memberQueryService));
        }

        public async Task<FlatReportOutput<InstallationPrintHeaderOutputDto, InstallationPrintDataOutputDto>> Handle(InstallationPrintInputDto inputDto, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _memberQueryService.Get(inputDto.BillId);
            FlatReportOutput<InstallationPrintHeaderOutputDto, InstallationPrintDataOutputDto> result = await _queryService.Get(zoneIdAndCustomerNumber);
            result.ReportData.Base64Image = inputDto.Base64Image;

            return result;
        }
    }
}
