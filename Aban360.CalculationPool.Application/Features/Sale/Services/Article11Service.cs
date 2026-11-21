using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;
using FluentValidation;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Implementations;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.Common.Exceptions;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Sale.Commands.Contracts;
using Aban360.Common.ApplicationUser;

namespace Aban360.CalculationPool.Application.Features.Sale.Services;
public interface IArticle11Service
{
    Task<IEnumerable<Article11OutputDto>> Article11GetAllAsync(CancellationToken cancellationToken);
    Task<Article11OutputDto> Article11GetAsync(SearchById input, CancellationToken cancellationToken);
    Task Article11CreateAsync(Article11CreateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task Article11UpdateAsync(Article11UpdateDto inputDto, IAppUser appuser, CancellationToken cancellationToken);
    Task Article11DeleteAsync(SearchById inputDto, IAppUser appUser, CancellationToken cancellationToken);
}

internal sealed class Article11Service : IArticle11Service
{
    private readonly IArticle11QueryService _queryService;
    private readonly IValidator<SearchById> _validator;
    private readonly IArticle11CommandService _commandService;
    private readonly IValidator<Article11CreateDto> _article11CreateValidator;
    private readonly IValidator<Article11UpdateDto> _article11UpdateValidator;
    public Article11Service(IArticle11QueryService queryService, IValidator<SearchById> validator, IArticle11CommandService commandService, IValidator<Article11CreateDto> article11CreateValidator, IValidator<Article11UpdateDto> article11UpdateValidator)
    {
        _queryService = queryService;
        _queryService.NotNull(nameof(queryService));
        _validator = validator;
        _validator.NotNull(nameof(validator));
        _commandService = commandService;
        _commandService.NotNull(nameof(commandService));
        _article11CreateValidator = article11CreateValidator;
        _article11CreateValidator.NotNull(nameof(article11CreateValidator));
        _article11UpdateValidator = article11UpdateValidator;
        _article11UpdateValidator.NotNull(nameof(article11UpdateValidator));
    }

    public async Task<IEnumerable<Article11OutputDto>> Article11GetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<Article11OutputDto> result = await _queryService.Get(DateTime.Now.ToShortPersianDateString());
        return result;
    }

    public async Task<Article11OutputDto> Article11GetAsync(SearchById input, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(input, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        Article11OutputDto result = await _queryService.Get(input.Id, DateTime.Now.ToShortPersianDateString());
        return result;
    }

    public async Task Article11CreateAsync(Article11CreateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        var validationResult = await _article11CreateValidator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        Article11InputDto article11Input = Article11CreateGetArticle11(inputDto, appUser.UserId);
        await _commandService.Create(article11Input);
    }

    private Article11InputDto Article11CreateGetArticle11(Article11CreateDto input, Guid userId)
    {
        return new Article11InputDto()
        {
            DomesticWaterAmount = input.DomesticWaterAmount,
            DomesticSewageAmount = input.DomesticSewageAmount,
            NonDomesticWaterAmount = input.NonDomesticWaterAmount,
            NonDomesticSewageAmount = input.NonDomesticSewageAmount,
            BlockCode = input.BlockCode,
            ZoneId = input.ZoneId,
            FromDateJalali = input.FromDateJalali,
            ToDateJalali = input.ToDateJalali,
            RegisterDateTime = DateTime.Now,
            RegisterByUserId = userId
        };
    }

    public async Task Article11UpdateAsync(Article11UpdateDto inputDto, IAppUser appuser, CancellationToken cancellationToken)
    {
        var validationResult = await _article11UpdateValidator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        Article11InputDto article11Create = Article11UpdateGetArticle11(inputDto, appuser.UserId);
        DeleteDto article11Delete = new(inputDto.Id, DateTime.Now, appuser.UserId);
        await _commandService.Update(article11Create, article11Delete);
    }

    private Article11InputDto Article11UpdateGetArticle11(Article11UpdateDto input, Guid userId)
    {
        return new Article11InputDto()
        {
            DomesticWaterAmount = input.DomesticWaterAmount,
            DomesticSewageAmount = input.DomesticSewageAmount,
            NonDomesticWaterAmount = input.NonDomesticWaterAmount,
            NonDomesticSewageAmount = input.NonDomesticSewageAmount,
            BlockCode = input.BlockCode,
            ZoneId = input.ZoneId,
            FromDateJalali = input.FromDateJalali,
            ToDateJalali = input.ToDateJalali,
            RegisterDateTime = DateTime.Now,
            RegisterByUserId = userId
        };
    }

    public async Task Article11DeleteAsync(SearchById inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var deleteDto = new DeleteDto(inputDto.Id, DateTime.Now, appUser.UserId);
        await _commandService.Delete(deleteDto);
    }
}
