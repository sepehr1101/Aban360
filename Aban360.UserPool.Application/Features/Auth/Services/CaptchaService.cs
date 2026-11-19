using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Exceptions;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Auth.Services;

public interface ICaptchaService
{
    Task CreateAsync(CaptchaCreateDto captchaCreateDto, CancellationToken cancellationToken);
    Task UpdateAsync(CaptchaUpdateDto capthcaUpdateDto, CancellationToken cancellationToken);
    Task<CaptchaActiveDto> GetSingleAsync(CancellationToken cancellationToken);
    Task<CaptchaQueryDto> GetSingleAsync(int id, CancellationToken cancellationToken);
    Task<ICollection<CaptchaQueryDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<ICollection<CaptchaLanguageDto>> GetLanguagesAsync(CancellationToken cancellationToken);
    Task<ICollection<CaptchaDisplayModeDto>> GetDisplayModesAsync(CancellationToken cancellationToken);
    Task<ICollection<NumericDictionary>> GetDictionaryAsync(CancellationToken cancellationToken);
}

internal sealed class CaptchaService : ICaptchaService
{
    private readonly IMapper _mapper;
    private readonly ICaptchaQueryService _captchaQueryService;
    private readonly ICaptchaCommandService _captchaCommandService;
    private readonly ICaptchaLanguageQueryService _captchaLanguageQueryService;
    private readonly ICaptchaDisplayModeQueryService _captchaDisplayModeQueryService;
    private readonly IValidator<CaptchaCreateDto> _createValidator;
    private readonly IValidator<CaptchaUpdateDto> _updateValidator;
    public CaptchaService(
        IMapper mapper,
        ICaptchaQueryService captchaQueryService,
        ICaptchaCommandService captchaCommandService,
        ICaptchaLanguageQueryService captchaLanguageQueryService,
        ICaptchaDisplayModeQueryService captchaDisplayModeQueryService,
        IValidator<CaptchaCreateDto> captchaValidator,
        IValidator<CaptchaUpdateDto> updateCaptchaValidator)
    {
        _mapper = mapper;
        _mapper.NotNull(nameof(mapper));
        _captchaQueryService = captchaQueryService;
        _captchaQueryService.NotNull(nameof(captchaCommandService));
        _captchaCommandService = captchaCommandService;
        _captchaCommandService.NotNull(nameof(captchaCommandService));
        _captchaLanguageQueryService = captchaLanguageQueryService;
        _captchaLanguageQueryService.NotNull(nameof(captchaLanguageQueryService));
        _captchaDisplayModeQueryService = captchaDisplayModeQueryService;
        _captchaDisplayModeQueryService.NotNull(nameof(captchaDisplayModeQueryService));
        _createValidator = captchaValidator;
        _createValidator.NotNull(nameof(captchaValidator));
        _updateValidator = updateCaptchaValidator;
        _updateValidator.NotNull(nameof(updateCaptchaValidator));
        _captchaQueryService.NotNull(nameof(captchaQueryService));
        _mapper.NotNull();
        _captchaQueryService.NotNull();
        _mapper.NotNull(nameof(_mapper));
    }

    public async Task CreateAsync(CaptchaCreateDto captchaCreateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(captchaCreateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        await CreateValidate(captchaCreateDto.CaptchaLanguageId, captchaCreateDto.CaptchaDisplayModeId);
        Captcha captcha = _mapper.Map<Captcha>(captchaCreateDto);
        await _captchaCommandService.Create(captcha);
        if (captcha.IsSelected)
            await _captchaCommandService.SetIsSelected();
    }

    private async Task CreateValidate(short languegaId, short displayModeId)
    {
        CaptchaLanguage language = await _captchaLanguageQueryService.Get(languegaId);
        if (language == null)
            throw new InvalidIdException();
        CaptchaDisplayMode displayMode = await _captchaDisplayModeQueryService.Get(displayModeId);
        if (displayMode == null)
            throw new InvalidIdException();
    }

    public async Task UpdateAsync(CaptchaUpdateDto capthcaUpdateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(capthcaUpdateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        } //

        Captcha captcha = _mapper.Map<Captcha>(capthcaUpdateDto);
        _captchaCommandService.Update(captcha);
        if (captcha.IsSelected)
            await _captchaCommandService.SetIsSelected(captcha.Id);
    }

    public async Task<CaptchaActiveDto> GetSingleAsync(CancellationToken cancellationToken)
    {
        Captcha captcha = await _captchaQueryService.Get();
        return _mapper.Map<CaptchaActiveDto>(captcha);
    }

    public async Task<CaptchaQueryDto> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        Captcha captcha = await _captchaQueryService.Get(id);
        if (captcha == null)
        {
            throw new InvalidIdException();
        }

        return _mapper.Map<CaptchaQueryDto>(captcha);
    }

    public async Task<ICollection<CaptchaQueryDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<Captcha> captchas = await _captchaQueryService.GetAll();
        return _mapper.Map<ICollection<CaptchaQueryDto>>(captchas);
    }

    public async Task<ICollection<CaptchaLanguageDto>> GetLanguagesAsync(CancellationToken cancellationToken)
    {
        ICollection<CaptchaLanguage> captchaLanguages = await _captchaLanguageQueryService.Get();
        ICollection<CaptchaLanguageDto> captchaLanguageDtos = _mapper.Map<ICollection<CaptchaLanguageDto>>(captchaLanguages);
        return captchaLanguageDtos;
    }

    public async Task<ICollection<CaptchaDisplayModeDto>> GetDisplayModesAsync(CancellationToken cancellationToken)
    {
        ICollection<CaptchaDisplayMode> captchaDisplayModes = await _captchaDisplayModeQueryService.Get();
        ICollection<CaptchaDisplayModeDto> captchaDisplayModeDtos = _mapper.Map<ICollection<CaptchaDisplayModeDto>>(captchaDisplayModes);
        return captchaDisplayModeDtos;
    }

    public async Task<ICollection<NumericDictionary>> GetDictionaryAsync(CancellationToken cancellationToken)
    {
        ICollection<NumericDictionary> dictionary = await _captchaQueryService.GetDictionary();
        return dictionary;
    }
}
