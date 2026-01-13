using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Implementations
{
    internal sealed class ReturnBillFullHandler : IReturnBillFullHandler
    {
        private readonly IReturnBillBaseHandler _returnBillBaseHandler;
        public ReturnBillFullHandler(IReturnBillBaseHandler returnBillBaseHandler)
        {
            _returnBillBaseHandler = returnBillBaseHandler;
            _returnBillBaseHandler.NotNull(nameof(returnBillBaseHandler));
        }

        public async Task<ReturnBillOutputDto> Handle(ReturnBillFullInputDto input, CancellationToken cancellationToken)
        {
            CustomerInfoOutputDto customerInfo = await Validation(input, cancellationToken);

            int jalaseNumber = await _returnBillBaseHandler.GetJalaliNumber(customerInfo.ZoneId, customerInfo.Radif);
            IEnumerable<BedBesCreateDto> bedBesInfo = await _returnBillBaseHandler.GetBedBesList(customerInfo, input.FromDateJalali, input.ToDateJalali);
            BedBesCreateDto bedBesResult = _returnBillBaseHandler.GetBedbes(bedBesInfo, customerInfo);

            AutoBackCreateDto bedBes = _returnBillBaseHandler.GetBedBes(bedBesResult, bedBesInfo.Count(), jalaseNumber, input.ReturnCauseId);
            AutoBackCreateDto newCalculation = _returnBillBaseHandler.GetFullNewCalculation(bedBesResult, input.ReturnCauseId, bedBesInfo.Count(), jalaseNumber);

            return await _returnBillBaseHandler.GetReturn(bedBes, newCalculation, bedBes, bedBesInfo.Count(), input.IsConfirm);

        }
        private async Task<CustomerInfoOutputDto> Validation(ReturnBillFullInputDto input, CancellationToken cancellationToken)
        {
            await _returnBillBaseHandler.FullValidation(input, cancellationToken);
            CustomerInfoOutputDto customerInfo = await _returnBillBaseHandler.Validation(input.BillId, input.FromDateJalali, input.ToDateJalali);
            return customerInfo;
        }
    }
}