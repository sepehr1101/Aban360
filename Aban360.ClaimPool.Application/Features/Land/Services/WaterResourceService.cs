using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Services;

public interface IWaterResourceService
{
    Task<WaterResourceGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<WaterResourceGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(WaterResourceUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(WaterResourceDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(WaterResourceCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class WaterResourceService : IWaterResourceService
{
    private readonly IMapper _mapper;
    private readonly IWaterResourceQueryService _waterResourceQueryService;
    private readonly IHeadquartersAddhoc _headquartersAddhoc;
    private readonly IValidator<WaterResourceUpdateDto> _updateValidator;
    private readonly IWaterResourceCommandService _waterResourceCommandService;
    private readonly IValidator<WaterResourceCreateDto> _createValidator;

    public WaterResourceService(IMapper mapper, IWaterResourceQueryService waterResourceQueryService, IHeadquartersAddhoc headquartersAddhoc, IValidator<WaterResourceUpdateDto> updateValidator, IWaterResourceCommandService waterResourceCommandService, IValidator<WaterResourceCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        waterResourceQueryService.NotNull(nameof(waterResourceQueryService));
        _waterResourceQueryService = waterResourceQueryService;
        headquartersAddhoc.NotNull(nameof(headquartersAddhoc));
        _headquartersAddhoc = headquartersAddhoc;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        waterResourceCommandService.NotNull(nameof(waterResourceCommandService));
        _waterResourceCommandService = waterResourceCommandService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task<WaterResourceGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        WaterResource waterResource = await _waterResourceQueryService.Get(id);
        return _mapper.Map<WaterResourceGetDto>(waterResource);
    }

    public async Task<ICollection<WaterResourceGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<WaterResource> waterResource = await _waterResourceQueryService.Get();
        return _mapper.Map<ICollection<WaterResourceGetDto>>(waterResource);
    }

    public async Task UpdateAsync(WaterResourceUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        WaterResource waterResource = await _waterResourceQueryService.Get(updateDto.Id);
     
        string headquartersTitle = await _headquartersAddhoc.Handle(updateDto.HeadquartersId, cancellationToken);
        waterResource.HeadquartersTitle = headquartersTitle;
        
        _mapper.Map(updateDto, waterResource);
    }

    public async Task DeleteAsync(WaterResourceDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        WaterResource waterResource = await _waterResourceQueryService.Get(deleteDto.Id);
        await _waterResourceCommandService.Remove(waterResource);
    }

    public async Task CreateAsync(WaterResourceCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        WaterResource waterResource = _mapper.Map<WaterResource>(createDto);
        if (waterResource == null)
        {
            throw new InvalidDataException();
        }
        string headquartersTitle = await _headquartersAddhoc.Handle(createDto.HeadquartersId, cancellationToken);
        waterResource.HeadquartersTitle = headquartersTitle;
        await _waterResourceCommandService.Add(waterResource);
    }

}
