using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
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
using System.Text.Json;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class ConCompanyPersonnelInsertHandler : AbstractBaseConnection, IConCompanyPersonnelInsertHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConCompanyQueryService _conCompanyQueryService;
        private readonly IValidator<ConCompanyPersonnelInsertInputDto> _validator;
        public ConCompanyPersonnelInsertHandler(
            IHttpContextAccessor contextAccessor,
            IConCompanyQueryService conCompanyQueryService,
            IValidator<ConCompanyPersonnelInsertInputDto> validator,
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
        public async Task Handle(ConCompanyPersonnelInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validate(inputDto, cancellationToken);
            ConCompanyGetDto conCompanyInfo = await _conCompanyQueryService.GetValid(inputDto.CompanyId);
            string ConCompanyPersonnelInsertJson = GetPersonnelInsertJson(inputDto, appUser);
            string opLogText = string.Format(OpLogLiterals.ConCompanyPersonnelInsertOpLog, inputDto.FullName, inputDto.NationalCode);
            Console.WriteLine(ConCompanyPersonnelInsertJson);

            await ExecSql(ConCompanyPersonnelInsertJson, appUser, inputDto.CompanyId, opLogText);
        }
        private async Task ExecSql(string conCompanyPersonnelInsertJson, IAppUser appUser, int companyId, string opLogText)
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
        private async Task Validate(ConCompanyPersonnelInsertInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BaseException(message);
            }
        }
        private string GetPersonnelInsertJson(ConCompanyPersonnelInsertInputDto input, IAppUser appUser)
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
    }
}
