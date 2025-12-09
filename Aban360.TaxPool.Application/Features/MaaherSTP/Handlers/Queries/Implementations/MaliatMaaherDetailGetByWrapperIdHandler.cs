using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Queries.Contracts;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;
using Aban360.TaxPool.Persistence.Features.MaaherTSP.Contracts;

namespace Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Queries.Implementations
{
    internal sealed class MaliatMaaherDetailGetByWrapperIdHandler : IMaliatMaaherDetailGetByWrapperIdHandler
    {
        const string _title = "مالیات";
        private readonly IMaliatMaaherDetailService _maaherDetailService;
        private readonly IMaliatMaaherWrapperService _maaherWrapperService;
        public MaliatMaaherDetailGetByWrapperIdHandler(
            IMaliatMaaherDetailService maaherDetailService,
            IMaliatMaaherWrapperService maaherWrapperService)
        {
            _maaherDetailService = maaherDetailService;
            _maaherDetailService.NotNull(nameof(maaherDetailService));

            _maaherWrapperService = maaherWrapperService;
            _maaherWrapperService.NotNull(nameof(maaherWrapperService));
        }

        public async Task<ReportOutput<MaliatMaaherWrapperGetDto, MaliatMaaherDetailGetDto>> Handle(SearchInput inputDto, CancellationToken cancellationToken)
        {
            int wrapperId = int.Parse(inputDto.Input);
            IEnumerable<MaliatMaaherDetailGetDto> maaherDetails = await _maaherDetailService.Get(wrapperId);
            MaliatMaaherWrapperGetDto maaherWrapper = await _maaherWrapperService.Get(wrapperId);

            ReportOutput<MaliatMaaherWrapperGetDto, MaliatMaaherDetailGetDto> result = new(_title, maaherWrapper, maaherDetails);
            return result;
        }
    }
}
