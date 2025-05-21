using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Implementations
{
    internal sealed class RequestIndividualCreateHandler : IRequestIndividualCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestIndividualEstateCommandService _requestIndividualEstateCommandService;
        private readonly IRequestIndividualCommandService _requestIndividualCommandService;
        private readonly IValidator<IndividualRequestCreateDto> _validator;

        public RequestIndividualCreateHandler(
            IMapper mapper,
            IRequestIndividualEstateCommandService requestIndividualEstateCommandService,
            IRequestIndividualCommandService requestIndividualCommandService,
            IValidator<IndividualRequestCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestIndividualEstateCommandService = requestIndividualEstateCommandService;
            _requestIndividualEstateCommandService.NotNull(nameof(_requestIndividualEstateCommandService));

            _requestIndividualCommandService = requestIndividualCommandService;
            _requestIndividualCommandService.NotNull(nameof(_requestIndividualCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(IAppUser currentUser, IndividualRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var requestIndividual = _mapper.Map<RequestIndividual>(createDto);
            requestIndividual.Hash = "-";
            requestIndividual.InsertLogInfo = "-";
            requestIndividual.ValidFrom = DateTime.Now;
            requestIndividual.UserId=currentUser.UserId;

            RequestIndividualEstate requestIndividualEstate = new RequestIndividualEstate()
            {
                RequestIndividual = requestIndividual,
                EstateId = createDto.EstateId,
                IndividualEstateRelationTypeId = createDto.IndividualEstateRelationTypeId,
            };


            createDto.TagIds.ForEach(tags =>
            {
                RequestIndividualTag requestIndividualTag = new RequestIndividualTag()
                {
                    RequestIndividual = requestIndividual,
                    IndividualTagDefinitionId = tags,
                    Hash = "-",
                    InsertLogInfo = "-",
                    ValidFrom = DateTime.Now,
                };
                requestIndividual.IndividualTags.Add(requestIndividualTag);
            });


            await _requestIndividualCommandService.Add(requestIndividual);
            await _requestIndividualEstateCommandService.Add(requestIndividualEstate);
        }
    }
}
