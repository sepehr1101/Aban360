using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Queries.Contracts;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;
using Aban360.TaxPool.Persistence.Features.MaaherTSP.Contracts;

namespace Aban360.TaxPool.Application.Features.MaaherSTP.Services;
public interface IMaaherInvoiceQueriesService
{
    Task<IEnumerable<MaliatMaaherWrapperGetDto>> MaliatMaaherWrapperGetAllAsync(CancellationToken cancellationToken);
    Task<ReportOutput<MaliatMaaherWrapperGetDto, MaliatMaaherDetailGetDto>> MaliatMaaherDetailGetByWrapperIdAsync(SearchByIdInput inputDto, CancellationToken cancellationToken);
}

internal sealed class MaaherInvoiceQueriesService : IMaaherInvoiceQueriesService
{
    private readonly IMaliatMaaherWrapperService _maaherWrapperService;
    const string _title = "مالیات";
    private readonly IMaliatMaaherDetailService _maaherDetailService;
    public MaaherInvoiceQueriesService(IMaliatMaaherWrapperService maaherWrapperService, IMaliatMaaherDetailService maaherDetailService)
    {
        _maaherWrapperService = maaherWrapperService;
        _maaherWrapperService.NotNull(nameof(maaherWrapperService));
        _maaherDetailService = maaherDetailService;
        _maaherDetailService.NotNull(nameof(maaherDetailService));
    }

    public async Task<IEnumerable<MaliatMaaherWrapperGetDto>> MaliatMaaherWrapperGetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<MaliatMaaherWrapperGetDto> result = await _maaherWrapperService.Get();
        return result;
    }

    public async Task<ReportOutput<MaliatMaaherWrapperGetDto, MaliatMaaherDetailGetDto>> MaliatMaaherDetailGetByWrapperIdAsync(SearchByIdInput inputDto, CancellationToken cancellationToken)
    {
        IEnumerable<MaliatMaaherDetailGetDto> maaherDetails = await _maaherDetailService.Get(inputDto.Id);
        MaliatMaaherWrapperGetDto maaherWrapper = await _maaherWrapperService.Get(inputDto.Id);
        ReportOutput<MaliatMaaherWrapperGetDto, MaliatMaaherDetailGetDto> result = new(_title, maaherWrapper, maaherDetails);
        return result;
    }
}
