using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.CalculationPool.Persistence.Features.ServiceLink.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;
using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Implementations;
using Aban360.CalculationPool.Persistence.Features.ServiceLink.Qeuries.Contracts;
using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Contracts;
using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Implementations;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Literals;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Services
{
    public interface IOtherExpensesItemsService
    {
        Task OtherExpensesItemsInsertAsync(OtherExpensesItemsInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
        Task OtherExpensesItemsRemoveAsync(int id, IAppUser appUser, CancellationToken cancellationToken);
        Task<IEnumerable<OtherExpensesItemsDataDto>> OtherExpensesItemsGetAllAsync(CancellationToken cancellationToken);
        Task<OtherExpensesItemsDataDto> OtherExpensesItemsGetByIdAsync(int id, CancellationToken cancellationToken);
        Task<OtherExpensesItemsDataDto> OtherExpensesItemsGetByItemIdAsync(string billId, int itemId, CancellationToken cancellationToken);
    }

    internal sealed class OtherExpensesItemsService : AbstractBaseConnection, IOtherExpensesItemsService
    {
        private readonly IT100QueryService _t100QueryService;
        private readonly IT51QueryService _zoneQueryService;
        private readonly IT41QueryService _usageQueryService;
        private readonly IValidator<OtherExpensesItemsInsertInputDto> _validator;
        private readonly IOtherExpensesItemsQueryService _otherExpensesItemsQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        public OtherExpensesItemsService(IT100QueryService t100QueryService, IT51QueryService zoneQueryService, IT41QueryService usageQueryService, IValidator<OtherExpensesItemsInsertInputDto> validator, IConfiguration configuration, IOtherExpensesItemsQueryService otherExpensesItemsQueryService, ICommonMemberQueryService commonMemberQueryService) : base(configuration)
        {
            _t100QueryService = t100QueryService;
            _t100QueryService.NotNull(nameof(t100QueryService));
            _zoneQueryService = zoneQueryService;
            _zoneQueryService.NotNull(nameof(zoneQueryService));
            _usageQueryService = usageQueryService;
            _usageQueryService.NotNull(nameof(usageQueryService));
            _validator = validator;
            _validator.NotNull(nameof(validator));
            _otherExpensesItemsQueryService = otherExpensesItemsQueryService;
            _otherExpensesItemsQueryService.NotNull(nameof(otherExpensesItemsQueryService));
            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));
        }

        public async Task OtherExpensesItemsInsertAsync(OtherExpensesItemsInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await OtherExpensesItemsInsertValidate(inputDto, cancellationToken);
            OtherExpensesItemsInsertDto insertDto = await OtherExpensesItemsInsertGetInsertDto(inputDto, appUser);
            await OtherExpensesItemsInsertExecSql(insertDto);
        }

        private async Task OtherExpensesItemsInsertExecSql(OtherExpensesItemsInsertDto otherExpensesItmesInsertDto)
        {
            using (IDbConnection connection = _sqlConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    OtherExpensesItemsCommandService otherExpensesItmesCommandService = new(connection, transaction);
                    await otherExpensesItmesCommandService.Insert(otherExpensesItmesInsertDto);
                    transaction.Commit();
                }
            }
        }

        private async Task OtherExpensesItemsInsertValidate(OtherExpensesItemsInsertInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BaseException(message);
            }
        }

        private async Task<OtherExpensesItemsInsertDto> OtherExpensesItemsInsertGetInsertDto(OtherExpensesItemsInsertInputDto inputDto, IAppUser appUser)
        {
            return new OtherExpensesItemsInsertDto()
            {
                ZoneId = inputDto.ZoneId,
                ZoneTitle = (await _zoneQueryService.Get(inputDto.ZoneId, true)).Title,
                UsageId = inputDto.UsageId,
                UsageTitle = (await _usageQueryService.Get(inputDto.UsageId, true)).Title,
                ServiceId = inputDto.ServiceId,
                ServiceTitle = (await _t100QueryService.Get(inputDto.ServiceId, true)).Title,
                Amount = inputDto.Amount,
                InsertBy = appUser.UserId,
                InsertDateTime = DateTime.Now,
            };
        }

        public async Task OtherExpensesItemsRemoveAsync(int id, IAppUser appUser, CancellationToken cancellationToken)
        {
            OtherExpensesItemsDataDto itemInfo = await _otherExpensesItemsQueryService.Get(id);
            OtherExpensesItemsRemoveDto RemoveDto = OtherExpensesItemsRemoveGetRemoveDto(itemInfo, appUser);
            await OtherExpensesItemsRemoveExceSql(RemoveDto);
        }

        private async Task OtherExpensesItemsRemoveExceSql(OtherExpensesItemsRemoveDto otherExpensesItmesRemoveDto)
        {
            using (IDbConnection connection = _sqlConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    OtherExpensesItemsCommandService otherExpensesItmesCommandService = new(connection, transaction);
                    await otherExpensesItmesCommandService.Remove(otherExpensesItmesRemoveDto);
                    transaction.Commit();
                }
            }
        }

        private OtherExpensesItemsRemoveDto OtherExpensesItemsRemoveGetRemoveDto(OtherExpensesItemsDataDto itemInfo, IAppUser appUser)
        {
            return new OtherExpensesItemsRemoveDto()
            {
                Id = itemInfo.Id,
                RemoveBy = appUser.UserId,
                RemoveDateTime = DateTime.Now,
            };
        }

        public async Task<IEnumerable<OtherExpensesItemsDataDto>> OtherExpensesItemsGetAllAsync(CancellationToken cancellationToken)
        {
            IEnumerable<OtherExpensesItemsDataDto> data = await _otherExpensesItemsQueryService.Get();
            return data;
        }

        public async Task<OtherExpensesItemsDataDto> OtherExpensesItemsGetByIdAsync(int id, CancellationToken cancellationToken)
        {
            OtherExpensesItemsDataDto data = await _otherExpensesItemsQueryService.Get(id);
            return data;
        }

        public async Task<OtherExpensesItemsDataDto> OtherExpensesItemsGetByItemIdAsync(string billId, int itemId, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdCustomerNumber = await _commonMemberQueryService.Get(billId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdCustomerNumber);
            OtherExpensesItemsDataDto? data = await _otherExpensesItemsQueryService.Get(new OtherExpensesItemsGetDto(memberInfo.ZoneId, memberInfo.UsageId, itemId), false);
            if (data is null)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.NotFoundAmount);
            }

            //OtherExpensesItemsDataDto data = new()
            //{
            //    Id = 1,
            //    ServiceId = itemId,
            //    ServiceTitle = itemId.ToString(),
            //    Amount = 10000,
            //    InsertDateTime = DateTime.Now,
            //    RemoveDateTime = null,
            //};
            return data;
        }
    }
}
