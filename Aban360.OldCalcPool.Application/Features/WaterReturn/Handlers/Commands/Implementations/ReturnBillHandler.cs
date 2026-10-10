using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Constants;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Implementations
{
    internal sealed class ReturnBillHandler : IReturnBillHandler
    {
        private readonly IReturnBillBaseHandler _returnBillBaseHandler;
        private readonly IBillReturnCauseQueryService _billReturnCauseQueryService;
        private readonly IReturnBillPartialHandler _returnBillPartialHandler;
        private readonly IReturnBillFullHandler _returnBillFullHandler;
        public ReturnBillHandler(
            IReturnBillBaseHandler returnBillBaseHandler,
            IBillReturnCauseQueryService billReturnCauseQueryService,
            IReturnBillPartialHandler returnBillPartialHandler,
            IReturnBillFullHandler returnBillFullHandler)
        {
            _returnBillBaseHandler = returnBillBaseHandler;
            _returnBillBaseHandler.NotNull(nameof(returnBillBaseHandler));

            _billReturnCauseQueryService = billReturnCauseQueryService;
            _billReturnCauseQueryService.NotNull(nameof(billReturnCauseQueryService));

            _returnBillPartialHandler = returnBillPartialHandler;
            _returnBillPartialHandler.NotNull(nameof(returnBillPartialHandler));


            _returnBillFullHandler = returnBillFullHandler;
            _returnBillFullHandler.NotNull(nameof(returnBillFullHandler));
        }

        public async Task<FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto>> Handle(ReturnBillInputDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> validBillReturnCauseList = await _billReturnCauseQueryService.Get(isLastMeterValid: true);
            if (validBillReturnCauseList.Select(r => r.Id).Contains(input.ReturnCauseId))
            {
                //Full
                ReturnBillFullInputDto fullDto = GetFullDto(input);
                FullValidate(input);
                return await _returnBillFullHandler.Handle(fullDto, appUser, cancellationToken);
            }
            else
            {
                //Partial
                ReturnBillPartialInputDto PartialDto = GetPartialDto(input);
                return await _returnBillPartialHandler.Handle(PartialDto, appUser, cancellationToken);
            }
        }
        private ReturnBillFullInputDto GetFullDto(ReturnBillInputDto inputDto)
        {
            return new ReturnBillFullInputDto()
            {
                BillId = inputDto.BillId,
                ReturnCauseId = inputDto.ReturnCauseId,
                MinutesNumber = inputDto.MinutesNumber,
                FromDateJalali = inputDto.FromDateJalali,
                ToDateJalali = inputDto.ToDateJalali,
                IsConfirm = inputDto.IsConfirm,
                Description = inputDto.Description,
            };
        }
        private ReturnBillPartialInputDto GetPartialDto(ReturnBillInputDto inputDto)
        {
            return new ReturnBillPartialInputDto()
            {
                BillId = inputDto.BillId,
                ReturnCauseId = inputDto.ReturnCauseId,
                CalculationType = inputDto.CalculationType,
                UserInput = inputDto.UserInput,
                MinutesNumber = inputDto.MinutesNumber,
                FromDateJalali = inputDto.FromDateJalali,
                ToDateJalali = inputDto.ToDateJalali,
                IsConfirm = inputDto.IsConfirm,
                Description = inputDto.Description,
            };
        }
        private void FullValidate(ReturnBillInputDto inputDto)
        {
            if (inputDto.CalculationType == ReturnedBillCalculationTypeEnum.UserInput)
            {
                throw new ReturnedBillException(ExceptionLiterals.InvalidFullReturnByUserInput);
            }
        }
    }
}
