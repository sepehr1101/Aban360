using Aban360.Common.Extensions;
using Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Queries.Contracts;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto;
using Aban360.TaxPool.Infrastructure.Features.MaaherTSP.Contracts;
using Aban360.TaxPool.Persistence.Features.MaaherTSP.Query.Contracts;

namespace Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Queries.Implementations
{
    internal sealed class SentInvoiceHandler : ISentInvoiceHandler
    {
        private readonly IMaaherService _maaherService;
        private readonly IMaaherQueryService _maaherQueryService;
        public SentInvoiceHandler(
            IMaaherService maaherService,
            IMaaherQueryService maaherQueryService)
        {
            _maaherService = maaherService;
            _maaherService.NotNull(nameof(maaherService));

            _maaherQueryService = maaherQueryService;
            _maaherQueryService.NotNull(nameof(maaherQueryService));
        }

        public async Task<SentInvoiceRecieveDto> Handle(ICollection<MaaherTSPInvoiceDto> inputDto, CancellationToken cancellationToken)
        {
            SentInvoiceRecieveDto invoiceResult = await _maaherService.SendInvoice(inputDto);
            MaaherErrorsDto errors = await _maaherQueryService.GetErrors(invoiceResult.ResultCode);

            invoiceResult.StatusCode = errors.HttpStatus;
            invoiceResult.Description = errors.Description;
            return invoiceResult;
        }
    }
}
