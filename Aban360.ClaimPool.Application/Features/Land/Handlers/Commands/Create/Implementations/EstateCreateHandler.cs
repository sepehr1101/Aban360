using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class EstateCreateHandler : IEstateCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IEstateCommandService _commandService;
        private readonly IValidator<EstateCreateDto> _estateValidator;
        public EstateCreateHandler(
            IMapper mapper,
            IEstateCommandService commandService,
            IValidator<EstateCreateDto> estateValidator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(_commandService));

            _estateValidator = estateValidator;
            _estateValidator.NotNull(nameof(estateValidator));
        }

        public async Task Handle(EstateCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult=await _estateValidator.ValidateAsync(createDto,cancellationToken);
            if (!validationResult.IsValid)
            {
                var message=string.Join(",",validationResult.Errors.Select(x=>x.ErrorMessage));
                throw new BaseException(message);
            }
            Estate estate = _mapper.Map<Estate>(createDto);
            estate.ValidFrom = DateTime.Now;
            estate.InsertLogInfo = "loginfo";
            estate.Hash = "hash";

            await _commandService.Add(estate);
        }
    }
}
