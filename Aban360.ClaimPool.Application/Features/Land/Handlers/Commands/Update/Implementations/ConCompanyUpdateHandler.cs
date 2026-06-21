using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class ConCompanyUpdateHandler : AbstractBaseConnection, IConCompanyUpdateHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConCompanyQueryService _conCompanyQueryService;
        private readonly IValidator<ConCompanyUpdateDto> _validator;
        public ConCompanyUpdateHandler(
            IHttpContextAccessor contextAccessor,
            IConCompanyQueryService conCompanyQueryService,
            IValidator<ConCompanyUpdateDto> validator,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _conCompanyQueryService = conCompanyQueryService;
            _conCompanyQueryService.NotNull(nameof(conCompanyQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }
        public async Task Handle(ConCompanyUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validate(inputDto, cancellationToken);
            string opLogText = string.Format(OpLogLiterals.ConCompanyUpdateOpLog, inputDto.CompanyName, inputDto.RepresentativeName);

            await ExecSql(inputDto, appUser, opLogText);
        }
        private async Task ExecSql(ConCompanyUpdateDto conCompanyUpdateDto, IAppUser appUser, string opLogText)
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
        private async Task Validate(ConCompanyUpdateDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BaseException(message);
            }
        }
        //private ConCompanyUpdateDto GetUpdateDto(ConCompanyUpdateDto input, IAppUser appUser)
        //{
        //    return new ConCompanyUpdateDto()
        //    {
        //        CompanyName = input.CompanyName,
        //        RepresentativeName = input.RepresentativeName,
        //        CompanyNationalCode = input.CompanyNationalCode,
        //        RepresentativeNationalCode = input.RepresentativeNationalCode,
        //        RepresentativeFatherName = input.RepresentativeFatherName,
        //        CompanyMobileNumber = input.CompanyMobileNumber,
        //        RepresentativeMobileNumber = input.RepresentativeMobileNumber,
        //        CompanyAddress = input.CompanyAddress,
        //        RepresentativeAddress = input.RepresentativeAddress,
        //        CompanyPostalCode = input.CompanyPostalCode,
        //        RepresentativePostalCode = input.RepresentativePostalCode,
        //        RepresentativeBirthDateJalali = input.RepresentativeBirthDateJalali,
        //        RepresentativeBirthPlace = input.RepresentativeBirthPlace,
        //        RepresentativeCertificateNumber = input.RepresentativeCertificateNumber,
        //        AdministratorName = input.AdministratorName,
        //        AdministratorMobileNumber = input.AdministratorMobileNumber,
        //        ContractNumber = input.ContractNumber,
        //        ContractSubject = input.ContractSubject,
        //        ContractDataJalali = input.ContractDataJalali,
        //        ContractDuration = input.ContractDuration,
        //    };
        //}
    }
}
