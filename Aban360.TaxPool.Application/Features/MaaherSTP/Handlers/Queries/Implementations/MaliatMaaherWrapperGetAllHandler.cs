using Aban360.Common.Extensions;
using Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Queries.Contracts;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;
using Aban360.TaxPool.Persistence.Features.MaaherTSP.Contracts;

namespace Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Queries.Implementations
{
    internal sealed class MaliatMaaherWrapperGetAllHandler : IMaliatMaaherWrapperGetAllHandler
    {
        private readonly IMaliatMaaherWrapperService _maaherWrapperService;
        public MaliatMaaherWrapperGetAllHandler(IMaliatMaaherWrapperService maaherWrapperService)
        {
            _maaherWrapperService = maaherWrapperService;
            _maaherWrapperService.NotNull(nameof(maaherWrapperService));
        }

        public async Task<IEnumerable<MaliatMaaherWrapperGetDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<MaliatMaaherWrapperGetDto> result = await _maaherWrapperService.Get();
            return result;
        }
    }
}
