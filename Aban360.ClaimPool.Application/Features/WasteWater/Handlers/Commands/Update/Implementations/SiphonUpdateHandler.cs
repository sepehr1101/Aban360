using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Implementations
{
    internal sealed class SiphonUpdateHandler : ISiphonUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ISiphonQueryService _queryService;
        private readonly IValidator<SiphonUpdateDto> _siphonValidator;
        public SiphonUpdateHandler(
            IMapper mapper,
            ISiphonQueryService queryService,
            IValidator<SiphonUpdateDto> siphonValidator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));

            _siphonValidator = siphonValidator;
            _siphonValidator.NotNull(nameof(siphonValidator));
        }

        public async Task Handle(SiphonUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _siphonValidator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message=string.Join(", ", validationResult.Errors.Select(x=>x.ErrorMessage));
                throw new BaseException(message);
            }

            Siphon siphon = await _queryService.Get(updateDto.Id);
            siphon.ValidFrom = DateTime.Now;
            siphon.InsertLogInfo = "SampleLogInfo";
            siphon.Hash = "SmapleHash";

            _mapper.Map(updateDto, siphon);
        }
    }


}
