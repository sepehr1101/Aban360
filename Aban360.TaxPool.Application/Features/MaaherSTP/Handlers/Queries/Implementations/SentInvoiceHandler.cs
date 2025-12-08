using Aban360.Common.Extensions;
using Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Queries.Contracts;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto;
using Aban360.TaxPool.Infrastructure.Features.MaaherTSP.Contracts;
using Aban360.TaxPool.Persistence.Features.MaaherTSP.Contracts;

namespace Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Queries.Implementations
{
    internal sealed class SentInvoiceHandler : ISentInvoiceHandler
    {
        private readonly IMaaherService _maaherService;
        private readonly IMaaherErrorsQueryService _maaherQueryService;
        public SentInvoiceHandler(
            IMaaherService maaherService,
            IMaaherErrorsQueryService maaherQueryService)
        {
            _maaherService = maaherService;
            _maaherService.NotNull(nameof(maaherService));

            _maaherQueryService = maaherQueryService;
            _maaherQueryService.NotNull(nameof(maaherQueryService));
        }

        public async Task<IEnumerable<SentInvoiceRecieveDto>> Handle(ICollection<MaaherTSPInvoiceDto> inputDto, CancellationToken cancellationToken)
        {
            ICollection<SentInvoiceRecieveDto> invoiceResult = await _maaherService.SendInvoice(inputDto);

            foreach (var result in invoiceResult)
            {
                MaaherErrorsDto errors = await _maaherQueryService.GetErrors(result.ResultCode ?? 0);

                result.StatusCode = errors.HttpStatus;
                result.Description = errors.Description;
            }
           
            return invoiceResult.ToList();
        }
    }
}
