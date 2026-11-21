using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Services;
public interface ICustomerInfoService
{
    Task UpdateAsync(CustomerInfoLevel1UpdateDto updateDto, CancellationToken cancellationToken);
    Task UpdateAsync(CustomerInfoLevel2UpdateDto updateDto, CancellationToken cancellationToken);
    Task<CustomerInfoGetDto> GetByBillIdAsync(SearchInput input, CancellationToken cancellationToken);
}

internal sealed class CustomerInfoService : ICustomerInfoService
{
    private readonly IMapper _mapper;
    private readonly ICustomerInfoCommandService _customerInfoCommandService;
    private readonly ICustomerInfoQueryService _customerInfoQueryService;
    public CustomerInfoService(IMapper mapper, ICustomerInfoCommandService customerInfoCommandService, ICustomerInfoQueryService customerInfoQueryService)
    {
        _mapper = mapper;
        _mapper.NotNull(nameof(mapper));
        _customerInfoCommandService = customerInfoCommandService;
        _customerInfoCommandService.NotNull(nameof(customerInfoCommandService));
        _customerInfoQueryService = customerInfoQueryService;
        _customerInfoQueryService.NotNull(nameof(customerInfoQueryService));
    }

    public async Task UpdateAsync(CustomerInfoLevel1UpdateDto updateDto, CancellationToken cancellationToken)
    {
        CustomerInfoUpdateDto customerUpdate = _mapper.Map<CustomerInfoUpdateDto>(updateDto);
        await _customerInfoCommandService.Update(customerUpdate);
    }

    public async Task UpdateAsync(CustomerInfoLevel2UpdateDto updateDto, CancellationToken cancellationToken)
    {
        CustomerInfoUpdateDto customerUpdate = _mapper.Map<CustomerInfoUpdateDto>(updateDto);
        await _customerInfoCommandService.Update(customerUpdate);
    }

    public async Task<CustomerInfoGetDto> GetByBillIdAsync(SearchInput input, CancellationToken cancellationToken)
    {
        CustomerInfoGetDto customeInfo = await _customerInfoQueryService.Get(input.Input);
        return customeInfo;
    }
}
