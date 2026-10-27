using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NetTopologySuite.Index.HPRtree;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Implementations
{
    public interface IReturnBillManualHandler
    {
        Task Handle(ReturnBillManualInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
    internal sealed class ReturnBillManualHandler : AbstractBaseConnection, IReturnBillManualHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICommonMemberQueryService _memberQueryService;
        private readonly ICommonZoneService _zoneService;
        private readonly IVariabService _variabService;
        private readonly IReturnBillBaseHandler _returnBillBaseHandler;
        private readonly IBillReturnCauseQueryService _billReturnCauseQueryService;
        private readonly IValidator<ReturnBillManualInputDto> _validator;
        public ReturnBillManualHandler(
            IHttpContextAccessor contextAccessor,
            ICommonMemberQueryService memberQueryService,
            ICommonZoneService zoneService,
            IVariabService variabService,
            IReturnBillBaseHandler returnBillBaseHandler,
            IBillReturnCauseQueryService billReturnCauseQueryService,
            IValidator<ReturnBillManualInputDto> validator,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _memberQueryService = memberQueryService;
            _memberQueryService.NotNull(nameof(memberQueryService));

            _zoneService = zoneService;
            _zoneService.NotNull(nameof(zoneService));

            _variabService = variabService;
            _variabService.NotNull(nameof(variabService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

            _billReturnCauseQueryService = billReturnCauseQueryService;
            _billReturnCauseQueryService.NotNull(nameof(billReturnCauseQueryService));

            _returnBillBaseHandler = returnBillBaseHandler;
            _returnBillBaseHandler.NotNull(nameof(returnBillBaseHandler));
        }


        public async Task Handle(ReturnBillManualInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            CustomerInfoOutputDto customerInfo = await _returnBillBaseHandler.RealDateValidate(appUser, inputDto.BillId, inputDto.FromDateJalali, inputDto.ToDateJalali);

            IEnumerable<NumericDictionary> returnPartialCauseList = await _billReturnCauseQueryService.Get(false);
            if (!returnPartialCauseList.Where(r => r.Id == inputDto.ReturnCauseId).Any())
            {
                throw new ReturnedBillException(ExceptionLiterals.InvalidReturnCauseId);
            }

            IEnumerable<BedBesCreateDto> bedBesInfo = await _returnBillBaseHandler.GetBedBesList(customerInfo, inputDto.FromDateJalali, inputDto.ToDateJalali);
            BedBesCreateDto bedBesResult = _returnBillBaseHandler.GetBedbes(bedBesInfo, customerInfo);
            long inputAmounts = inputDto.Item1 + inputDto.Item2 + inputDto.Item3 + inputDto.Item4 + inputDto.Item5 + inputDto.Item6 + inputDto.Item10 + inputDto.Item11 + inputDto.Item13 + inputDto.Item14 + inputDto.Item16;
            if (inputAmounts > bedBesResult.Baha)
            {
                throw new ReturnedBillException(ExceptionLiterals.InvalidInputAmountGTBedBes);
            }


        }
        private async Task InputValidate(ReturnBillManualInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }


    }
}