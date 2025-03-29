using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Implementations
{
    internal sealed class RequestUserCreateHandler : IRequestUserCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestUserCommandService _requestUserCommandService;
        public RequestUserCreateHandler(
            IMapper mapper,
            IRequestUserCommandService requestUserCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestUserCommandService = requestUserCommandService;
            _requestUserCommandService.NotNull(nameof(_requestUserCommandService));
        }

        public async Task Handle(RequestUserCommandDto createDto, CancellationToken cancellationToken)
        {
            var requestUser = _mapper.Map<RequestUser>(createDto);

            requestUser.RequestFlat.RequestEstate= requestUser.RequestEstate;
            requestUser.RequestWaterMeter.RequestEstate = requestUser.RequestEstate;
            requestUser.RequestSiphon.RequestWaterMeter = requestUser.RequestWaterMeter;
            requestUser.RequestIndividual.RequestWaterMeter = requestUser.RequestWaterMeter;
            requestUser.RequestIndividualEstate.RequestEstate = requestUser.RequestEstate;
            requestUser.RequestIndividualEstate.RequestIndividual = requestUser.RequestIndividual;
            requestUser.RequestIndividualTag.RequestIndividual = requestUser.RequestIndividual;
            requestUser.RequestWaterMeterSiphon.RequestWaterMeter = requestUser.RequestWaterMeter;
            requestUser.RequestWaterMeterSiphon.RequestSiphon = requestUser.RequestSiphon;
            requestUser.RequestWaterMeterTag.RequestWaterMeter = requestUser.RequestWaterMeter;

            requestUser.RequestEstate.InsertLogInfo = "sample";
            requestUser.RequestEstate.Hash = "sample";

            requestUser.RequestIndividual.Hash = "sample";

            requestUser.RequestSiphon.ValidFrom=DateTime.Now;
            requestUser.RequestSiphon.InsertLogInfo = "sample";
            requestUser.RequestSiphon.Hash = "sample";
            
            requestUser.RequestIndividual.ValidFrom=DateTime.Now;
            requestUser.RequestIndividual.InsertLogInfo = "sample";
            requestUser.RequestIndividual.Hash = "sample";
            
            requestUser.RequestWaterMeter.ValidFrom=DateTime.Now;
            requestUser.RequestWaterMeter.InsertLogInfo = "sample";
            requestUser.RequestWaterMeter.Hash = "sample";

            await _requestUserCommandService.Add(requestUser.RequestWaterMeter);
        }
        //public async Task Handle(RequestUserCommandDto createDto, CancellationToken cancellationToken)
        //{

        //}
    }
}
