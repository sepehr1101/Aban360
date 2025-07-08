using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Implementations
{
    internal sealed class CustomerInfoUpdateHandler : ICustomerInfoUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICustomerInfoCommandService _customerInfoCommandService;
        public CustomerInfoUpdateHandler(
            IMapper mapper,
            ICustomerInfoCommandService customerInfoCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _customerInfoCommandService = customerInfoCommandService;
            _customerInfoCommandService.NotNull(nameof(customerInfoCommandService));
        }

        public async Task Handle(CustomerInfoLevel1UpdateDto updateDto, CancellationToken cancellationToken)
        {
            CustomerInfoUpdateDto customerUpdate = _mapper.Map<CustomerInfoUpdateDto>(updateDto);
            await _customerInfoCommandService.Update(customerUpdate);
        }

        public async Task Handle(CustomerInfoLevel2UpdateDto updateDto, CancellationToken cancellationToken)
        {
            CustomerInfoUpdateDto customerUpdate = _mapper.Map<CustomerInfoUpdateDto>(updateDto);
            await _customerInfoCommandService.Update(customerUpdate);
        }
    }
}
