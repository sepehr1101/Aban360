using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Implementations
{
    internal sealed class ConCompanyPersonnelRemoveHandler : AbstractBaseConnection, IConCompanyPersonnelRemoveHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConCompanyQueryService _conCompanyQueryService;
        public ConCompanyPersonnelRemoveHandler(
            IHttpContextAccessor contextAccessor,
            IConCompanyQueryService conCompanyQueryService,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _conCompanyQueryService = conCompanyQueryService;
            _conCompanyQueryService.NotNull(nameof(conCompanyQueryService));
        }
        public async Task Handle(ConCompanyPersonnelRemoveInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            ConCompanyPersonnelPersonalGetDto conCompanyIPersonnelnfo = await _conCompanyQueryService.GetPersonnelById(inputDto.CompanyId, inputDto.Id);
            int personnelIndex = await _conCompanyQueryService.GetPersonnelIndex(inputDto.CompanyId, inputDto.Id);
            ConCompanyPersonnelRemoveDto ConCompanyPersonnelRemoveDto = new(inputDto.CompanyId, personnelIndex, appUser.UserId);
            string opLogText = string.Format(OpLogLiterals.ConCompanyPersonnelRemoveOpLog, conCompanyIPersonnelnfo.FullName, conCompanyIPersonnelnfo.NationalCode);

            await ExecSql(ConCompanyPersonnelRemoveDto, appUser, opLogText);
        }
        private async Task ExecSql(ConCompanyPersonnelRemoveDto conCompanyPersonnelRemoveDto, IAppUser appUser, string opLogText)
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
    }
}
