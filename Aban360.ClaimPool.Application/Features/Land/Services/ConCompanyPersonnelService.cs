using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text.Json;

namespace Aban360.ClaimPool.Application.Features.Land.Services;
public interface IConCompanyPersonnelService
{
    Task InsertAsync(ConCompanyPersonnelInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task UpdateAsync(ConCompanyPersonnelUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task RemoveAsync(ConCompanyPersonnelRemoveInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task<IEnumerable<ConCompanyPersonnelDetailOutputDto>> GetAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<NumericDictionary>> GetDictionaryAsync(CancellationToken cancellationToken);
}

internal sealed class ConCompanyPersonnelService : AbstractBaseConnection, IConCompanyPersonnelService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IConCompanyQueryService _conCompanyQueryService;
    private readonly IValidator<ConCompanyPersonnelInsertInputDto> _validator;
    private readonly IValidator<ConCompanyPersonnelUpdateInputDto> _updateValidator;
    public ConCompanyPersonnelService(IHttpContextAccessor contextAccessor, IConCompanyQueryService conCompanyQueryService, IValidator<ConCompanyPersonnelInsertInputDto> validator, IConfiguration configuration, IValidator<ConCompanyPersonnelUpdateInputDto> updateAsyncValidator) : base(configuration)
    {
        _contextAccessor = contextAccessor;
        _contextAccessor.NotNull(nameof(contextAccessor));
        _conCompanyQueryService = conCompanyQueryService;
        _conCompanyQueryService.NotNull(nameof(conCompanyQueryService));
        _validator = validator;
        _validator.NotNull(nameof(validator));
        _updateValidator = updateAsyncValidator;
        _updateValidator.NotNull(nameof(updateAsyncValidator));
    }

    public async Task InsertAsync(ConCompanyPersonnelInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await InsertValidate(inputDto, cancellationToken);
        ConCompanyGetDto conCompanyInfo = await _conCompanyQueryService.GetValid(inputDto.CompanyId);
        string ConCompanyPersonnelInsertJson = InsertGetPersonnelInsertJson(inputDto, appUser);
        string opLogText = string.Format(OpLogLiterals.ConCompanyPersonnelInsertOpLog, inputDto.FullName, inputDto.NationalCode);
        Console.WriteLine(ConCompanyPersonnelInsertJson);
        await InsertExecSql(ConCompanyPersonnelInsertJson, appUser, inputDto.CompanyId, opLogText);
    }

    private async Task InsertExecSql(string conCompanyPersonnelInsertJson, IAppUser appUser, int companyId, string opLogText)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                ConCompanyCommandService conCompanyCommandService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await conCompanyCommandService.InsertPersonnel(companyId, conCompanyPersonnelInsertJson);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private async Task InsertValidate(ConCompanyPersonnelInsertInputDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }
    }

    private string InsertGetPersonnelInsertJson(ConCompanyPersonnelInsertInputDto input, IAppUser appUser)
    {
        ConCompanyPersonnelInsertDto insertDto = new()
        {
            FullName = input.FullName,
            NationalCode = input.NationalCode,
            MobileNumber = input.MobileNumber,
            PersonnelCode = input.PersonnelCode,
            HomeAddress = input.HomeAddress,
            HomePhoneNumber = input.HomePhoneNumber,
            EducationGrade = input.EducationGrade,
            EducationField = input.EducationField,
            BirtDateJalali = input.BirtDateJalali,
            InsertedBy = appUser.UserId,
            RemovedBy = null,
            RemovedDateTime = null
        };
        return JsonSerializer.Serialize(insertDto);
    }

    public async Task UpdateAsync(ConCompanyPersonnelUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await UpdateValidate(inputDto, cancellationToken);
        ConCompanyGetDto conCompanyInfo = await _conCompanyQueryService.GetValid(inputDto.CompanyId);
        int personnelIndex = await _conCompanyQueryService.GetPersonnelIndex(inputDto.CompanyId, inputDto.Id);
        string ConCompanyPersonnelUpdateJson = UpdateGetPersonnelUpdateJson(inputDto, appUser);
        string opLogText = string.Format(OpLogLiterals.ConCompanyPersonnelUpdateOpLog, inputDto.FullName, inputDto.NationalCode);
        await UpdateExecSql(ConCompanyPersonnelUpdateJson, appUser, inputDto.CompanyId, personnelIndex, opLogText);
    }

    private async Task UpdateExecSql(string conCompanyPersonnelUpdateJson, IAppUser appUser, int companyId, int personnelIndex, string opLogText)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                ConCompanyCommandService conCompanyCommandService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await conCompanyCommandService.UpdatePersonnel(companyId, conCompanyPersonnelUpdateJson, personnelIndex);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private async Task UpdateValidate(ConCompanyPersonnelUpdateInputDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }
    }

    private string UpdateGetPersonnelUpdateJson(ConCompanyPersonnelUpdateInputDto input, IAppUser appUser)
    {
        ConCompanyPersonnelUpdateDto UpdateDto = new()
        {
            Id = input.Id,
            FullName = input.FullName,
            NationalCode = input.NationalCode,
            MobileNumber = input.MobileNumber,
            PersonnelCode = input.PersonnelCode,
            HomeAddress = input.HomeAddress,
            HomePhoneNumber = input.HomePhoneNumber,
            EducationGrade = input.EducationGrade,
            EducationField = input.EducationField,
            BirtDateJalali = input.BirtDateJalali,
            InsertedBy = appUser.UserId,
            RemovedBy = null,
            RemovedDateTime = null,
        };
        return JsonSerializer.Serialize(UpdateDto);
    }

    public async Task RemoveAsync(ConCompanyPersonnelRemoveInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        ConCompanyPersonnelPersonalGetDto conCompanyIPersonnelnfo = await _conCompanyQueryService.GetPersonnelById(inputDto.CompanyId, inputDto.Id);
        int personnelIndex = await _conCompanyQueryService.GetPersonnelIndex(inputDto.CompanyId, inputDto.Id);
        ConCompanyPersonnelRemoveDto ConCompanyPersonnelRemoveDto = new(inputDto.CompanyId, personnelIndex, appUser.UserId);
        string opLogText = string.Format(OpLogLiterals.ConCompanyPersonnelRemoveOpLog, conCompanyIPersonnelnfo.FullName, conCompanyIPersonnelnfo.NationalCode);
        await RemoveExecSql(ConCompanyPersonnelRemoveDto, appUser, opLogText);
    }

    private async Task RemoveExecSql(ConCompanyPersonnelRemoveDto conCompanyPersonnelRemoveDto, IAppUser appUser, string opLogText)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                ConCompanyCommandService conCompanyCommandService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await conCompanyCommandService.RemovePersonnel(conCompanyPersonnelRemoveDto);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    public async Task<IEnumerable<ConCompanyPersonnelDetailOutputDto>> GetAsync(int id, CancellationToken cancellationToken)
    {
        ConCompanyPersonnelGetDto result = await _conCompanyQueryService.GetPersonnel(id);
        if (!string.IsNullOrWhiteSpace(result.ConCompanyPersonnel))
        {
            IEnumerable<ConCompanyPersonnelDetailOutputDto> personnelInfo = JsonSerializer.Deserialize<IEnumerable<ConCompanyPersonnelDetailOutputDto>>(result.ConCompanyPersonnel);
            return personnelInfo.Where(p => p.RemovedBy is null);
        }

        throw new InvalidTrackingException(ExceptionLiterals.InvalidConCompanyPersonnelDeserialize);
    }

    public async Task<IEnumerable<NumericDictionary>> GetDictionaryAsync(CancellationToken cancellationToken)
    {
        IEnumerable<ConCompanyPersonnelGetDto> conCompanyPersonnelInfo = await _conCompanyQueryService.GetPersonnel();
        ICollection<NumericDictionary> resultDictionary = new List<NumericDictionary>();
        conCompanyPersonnelInfo.ForEach(c =>
        {
            var personnels = JsonSerializer.Deserialize<IEnumerable<ConCompanyPersonnelDetailOutputDto>>(c.ConCompanyPersonnel);
            IEnumerable<NumericDictionary> dictionary = personnels?.Select(p => new NumericDictionary(c.Id, $@"{p.FullName}-{c.CompanyName}")) ?? Enumerable.Empty<NumericDictionary>();
            resultDictionary = resultDictionary.Concat(dictionary.ToList()).ToList();
        });
        return resultDictionary;
    }
}
