using Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;

namespace Aban360.Api.Controllers.V1.ClaimPool.Tracking.Queries
{
    [Route("v1/tracking")]
    public class TrackingDetailGetController : BaseController
    {
        private readonly IRequestIsRegisteredDetailHandler _requestIsRegisteredHandler;
        private readonly IExamineTimeSetDetailHandler _examineTimeSetDetailHandler;
        private readonly ISetExaminationResultDetailHandler _setExaminationResultDetailHandler;
        private readonly ITrackNumberAndDescriptionDetailHandler _trackNumberAndDescriptionDetailHandler;
        private readonly ICalculationConfirmedDetailHandler _calculationConfirmedDetailHandler;
        private readonly ICustomerNumberSpecifiedDetailHandler _customerNumberSpecifiedDetailHandler;
        private readonly IAmountConfirmedDetailHandler _amountConfirmedDetailHandler;
        public TrackingDetailGetController(
            IRequestIsRegisteredDetailHandler requestIsRegisteredHandler,
            IExamineTimeSetDetailHandler examineTimeSetDetailHandler,
            ISetExaminationResultDetailHandler setExaminationResultDetailHandler,
            ITrackNumberAndDescriptionDetailHandler trackNumberAndDescriptionDetailHandler,
            ICalculationConfirmedDetailHandler calculationConfirmedDetailHandler,
            ICustomerNumberSpecifiedDetailHandler customerNumberSpecifiedDetailHandler,
            IAmountConfirmedDetailHandler amountConfirmedDetailHandler)
        {
            _requestIsRegisteredHandler = requestIsRegisteredHandler;
            _requestIsRegisteredHandler.NotNull(nameof(requestIsRegisteredHandler));

            _examineTimeSetDetailHandler = examineTimeSetDetailHandler;
            _examineTimeSetDetailHandler.NotNull(nameof(examineTimeSetDetailHandler));

            _setExaminationResultDetailHandler = setExaminationResultDetailHandler;
            _setExaminationResultDetailHandler.NotNull(nameof(setExaminationResultDetailHandler));

            _trackNumberAndDescriptionDetailHandler = trackNumberAndDescriptionDetailHandler;
            _trackNumberAndDescriptionDetailHandler.NotNull(nameof(trackNumberAndDescriptionDetailHandler));

            _calculationConfirmedDetailHandler = calculationConfirmedDetailHandler;
            _calculationConfirmedDetailHandler.NotNull(nameof(calculationConfirmedDetailHandler));

            _customerNumberSpecifiedDetailHandler = customerNumberSpecifiedDetailHandler;
            _customerNumberSpecifiedDetailHandler.NotNull(nameof(customerNumberSpecifiedDetailHandler));

            _amountConfirmedDetailHandler = amountConfirmedDetailHandler;
            _amountConfirmedDetailHandler.NotNull(nameof(amountConfirmedDetailHandler));
        }

        [HttpPost]
        [Route("display-detail")]
        public async Task<IActionResult> Detail([FromBody] TrackingDetailInputDto input, CancellationToken cancellationToken)
        {
            TrackingDetailGetDto TrackDetailInput = GetTrackDetail(input);
            switch (input.StateId)
            {
                case 0://ثبت درخواست
                    {
                        RequestIsRegisterdOutputDto result = await _requestIsRegisteredHandler.Handle(TrackDetailInput, cancellationToken);
                        return Ok(result);
                    }
                case 10://تعیین روز بازدید
                    {
                        ExamineTimeSetOutputDto result = await _examineTimeSetDetailHandler.Handle(TrackDetailInput, cancellationToken);
                        return Ok(result);
                    }
                case 110://نتیجه ثبت شده
                    {
                        SetExaminationResultOutputDto result = await _setExaminationResultDetailHandler.Handle(TrackDetailInput, cancellationToken);
                        return Ok(result);
                    }
                case  65 or 90000 or 90003:// برگشت به محاسبه,آرشیو شده ,حذف درخواست
                    {
                        TrackNumberAndDescriptionOutputDto result = await _trackNumberAndDescriptionDetailHandler.Handle(TrackDetailInput, cancellationToken);
                        return Ok(result);
                    }
                case 60 or 90002://تایید محاسبه , تایید محاسبه دارای ردیف
                    {
                        CalculationConfirmedOutputDto result = await _calculationConfirmedDetailHandler.Handle(TrackDetailInput, cancellationToken);
                        return Ok(result);
                    }
                case 70://اختصاص ردیف
                    {
                        CustomerNumberSpecifiedOutputDto result = await _customerNumberSpecifiedDetailHandler.Handle(TrackDetailInput, cancellationToken);
                        return Ok(result);
                    }
                case 75://تایید مبلغ
                    {
                        AmountConfirmedOutputDto result = await _amountConfirmedDetailHandler.Handle(TrackDetailInput, cancellationToken);
                        return Ok(result);
                    }
                default: throw new InvalidTrackNumberException(ExceptionLiterals.InvalidStateId);
            }
        }
        private TrackingDetailGetDto GetTrackDetail(TrackingDetailInputDto input)
        {
            return new TrackingDetailGetDto(input.ZoneId, input.TrackId, input.TrackNumber);
        }
    }
}
