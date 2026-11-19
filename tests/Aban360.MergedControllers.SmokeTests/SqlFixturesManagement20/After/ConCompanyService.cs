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
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace SqlAfterManagement20.Aban360.ClaimPool.Application.Features.Land.Services;
public interface IConCompanyService
{
    Task InsertAsync(ConCompanyInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task UpdateAsync(ConCompanyUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task RemoveAsync(int id, IAppUser appUser, CancellationToken cancellationToken);
    Task<IEnumerable<ConCompanyGetDto>> GetAsync(CancellationToken cancellationToken);
    Task<ConCompanyGetDto> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<NumericDictionary>> GetDictionaryAsync(CancellationToken cancellationToken);
    Task<IEnumerable<NumericDictionary>> GetByBillIdDictionaryAsync(string billId, CancellationToken cancellationToken);
}

internal sealed class ConCompanyService : global::ManagementRecordingConnectionBase, IConCompanyService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IValidator<ConCompanyInsertInputDto> _validator;
    private readonly IConCompanyQueryService _conCompanyQueryService;
    private readonly IValidator<ConCompanyUpdateDto> _updateValidator;
    private readonly ICommonMemberQueryService _memberQueryService;
    public ConCompanyService(IHttpContextAccessor contextAccessor, IValidator<ConCompanyInsertInputDto> validator, IConfiguration configuration, IConCompanyQueryService conCompanyQueryService, IValidator<ConCompanyUpdateDto> updateAsyncValidator, ICommonMemberQueryService memberQueryService) : base(configuration)
    {
        _contextAccessor = contextAccessor;
        _contextAccessor.NotNull(nameof(contextAccessor));
        _validator = validator;
        _validator.NotNull(nameof(validator));
        _conCompanyQueryService = conCompanyQueryService;
        _conCompanyQueryService.NotNull(nameof(conCompanyQueryService));
        _updateValidator = updateAsyncValidator;
        _updateValidator.NotNull(nameof(updateAsyncValidator));
        _memberQueryService = memberQueryService;
        _memberQueryService.NotNull(nameof(memberQueryService));
    }

    public async Task InsertAsync(ConCompanyInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await InsertValidate(inputDto, cancellationToken);
        ConCompanyInsertDto conCompanyInsertDto = InsertGetInsertDto(inputDto, appUser);
        string opLogText = string.Format(OpLogLiterals.ConCompanyInsertOpLog, inputDto.CompanyName, inputDto.RepresentativeName);
        await InsertExecSql(conCompanyInsertDto, appUser, opLogText);
    }

    private async Task InsertExecSql(ConCompanyInsertDto conCompanyInsertDto, IAppUser appUser, string opLogText)
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
                await conCompanyCommandService.Insert(conCompanyInsertDto);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private async Task InsertValidate(ConCompanyInsertInputDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }
    }

    private ConCompanyInsertDto InsertGetInsertDto(ConCompanyInsertInputDto input, IAppUser appUser)
    {
        return new ConCompanyInsertDto()
        {
            ZoneId = input.ZoneId,
            CompanyName = input.CompanyName,
            RepresentativeName = input.RepresentativeName,
            CompanyNationalCode = input.CompanyNationalCode,
            RepresentativeNationalCode = input.RepresentativeNationalCode,
            RepresentativeFatherName = input.RepresentativeFatherName,
            CompanyMobileNumber = input.CompanyMobileNumber,
            RepresentativeMobileNumber = input.RepresentativeMobileNumber,
            CompanyAddress = input.CompanyAddress,
            RepresentativeAddress = input.RepresentativeAddress,
            CompanyPostalCode = input.CompanyPostalCode,
            RepresentativePostalCode = input.RepresentativePostalCode,
            RepresentativeBirthDateJalali = input.RepresentativeBirthDateJalali,
            RepresentativeBirthPlace = input.RepresentativeBirthPlace,
            RepresentativeCertificateNumber = input.RepresentativeCertificateNumber,
            AdministratorName = input.AdministratorName,
            AdministratorMobileNumber = input.AdministratorMobileNumber,
            ContractNumber = input.ContractNumber,
            ContractSubject = input.ContractSubject,
            ContractDataJalali = input.ContractDataJalali,
            ContractDuration = input.ContractDuration,
            ConCompanyPersonnel = "[]",
            InsertedBy = appUser.UserId,
        };
    }

    public async Task UpdateAsync(ConCompanyUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await UpdateValidate(inputDto, cancellationToken);
        string opLogText = string.Format(OpLogLiterals.ConCompanyUpdateOpLog, inputDto.CompanyName, inputDto.RepresentativeName);
        await UpdateExecSql(inputDto, appUser, opLogText);
    }

    private async Task UpdateExecSql(ConCompanyUpdateDto conCompanyUpdateDto, IAppUser appUser, string opLogText)
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
                await conCompanyCommandService.Update(conCompanyUpdateDto);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private async Task UpdateValidate(ConCompanyUpdateDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }
    }

    public async Task RemoveAsync(int id, IAppUser appUser, CancellationToken cancellationToken)
    {
        ConCompanyGetDto conCompanyInfo = await _conCompanyQueryService.GetValid(id);
        ConCompanyRemoveDto conCompanyRemoveDto = new(id, appUser.UserId);
        string opLogText = string.Format(OpLogLiterals.ConCompanyInsertOpLog, conCompanyInfo.CompanyName, conCompanyInfo.RepresentativeName);
        await RemoveExecSql(conCompanyRemoveDto, appUser, opLogText);
    }

    private async Task RemoveExecSql(ConCompanyRemoveDto conCompanyRemoveDto, IAppUser appUser, string opLogText)
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
                await conCompanyCommandService.Remove(conCompanyRemoveDto);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    public async Task<IEnumerable<ConCompanyGetDto>> GetAsync(CancellationToken cancellationToken)
    {
        IEnumerable<ConCompanyGetDto> result = await _conCompanyQueryService.Get();
        return result;
    }

    public async Task<ConCompanyGetDto> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        ConCompanyGetDto result = await _conCompanyQueryService.GetValid(id);
        return result;
    }

    public async Task<IEnumerable<NumericDictionary>> GetDictionaryAsync(CancellationToken cancellationToken)
    {
        IEnumerable<ConCompanyGetDto> conCompanyInfo = await _conCompanyQueryService.Get();
        IEnumerable<NumericDictionary> dictionary = conCompanyInfo.Select(c => new NumericDictionary(c.Id, c.CompanyName));
        return dictionary;
    }

    public async Task<IEnumerable<NumericDictionary>> GetByBillIdDictionaryAsync(string billId, CancellationToken cancellationToken)
    {
        ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _memberQueryService.Get(billId);
        IEnumerable<ConCompanyGetDto> conCompanyInfo = await _conCompanyQueryService.GetValidByZoneId(zoneIdAndCustomerNumber.ZoneId);
        IEnumerable<NumericDictionary> dictionary = conCompanyInfo.Select(c => new NumericDictionary(c.Id, c.CompanyName));
        return dictionary;
    }
}
