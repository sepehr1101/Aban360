using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class WaterDiscountByDiscountTypeDetailHandler : IWaterDiscountByDiscountTypeDetailHandler
    {
        private readonly IWaterDiscountQueryService _waterDiscountDetailQueryService;
        private readonly IValidator<WaterDiscountByTypeDetailInputDto> _validator;
        private string _title = ReportLiterals.WaterDiscountDetail;
        public WaterDiscountByDiscountTypeDetailHandler(
            IWaterDiscountQueryService waterDiscountDetailQueryService,
            IValidator<WaterDiscountByTypeDetailInputDto> validator)
        {
            _waterDiscountDetailQueryService = waterDiscountDetailQueryService;
            _waterDiscountDetailQueryService.NotNull(nameof(waterDiscountDetailQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WaterDiscountDetailHeaderOutputDto, WaterDiscountDetailDataOutputDto>> Handle(WaterDiscountByTypeDetailInputDto input, CancellationToken cancellationToken)
        {
            await InputValidate(input, cancellationToken);

            IEnumerable<WaterDiscountDetailDataOutputDto> data = await _waterDiscountDetailQueryService.GetDetail(input);
            WaterDiscountDetailHeaderOutputDto header = new()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                RecordCount = data?.Count() ?? 0,
                Title = _title,
                CustomerCount = data?.GroupBy(w => w.BillId)?.Select(w => w.First())?.Count() ?? 0,
                BillCount = data?.Count() ?? 0,

                AbBaha = data?.Sum(d => d.AbBaha) ?? 0,
                FazelabBaha = data?.Sum(d => d.FazelabBaha) ?? 0,
                AbonmanAb = data?.Sum(d => d.AbonmanAb) ?? 0,
                AbonmanFazelab = data?.Sum(d => d.AbonmanFazelab) ?? 0,
                Maliat = data?.Sum(d => d.Maliat) ?? 0,
                Tabsare2 = data?.Sum(d => d.Tabsare2) ?? 0,
                Tabsare2_3 = data?.Sum(d => d.Tabsare2_3) ?? 0,
                Jarime = data?.Sum(d => d.Jarime) ?? 0,
                Abresani = data?.Sum(d => d.Abresani) ?? 0,
                JavaniJamiat = data?.Sum(d => d.JavaniJamiat) ?? 0,
                FaslGarm = data?.Sum(d => d.FaslGarm) ?? 0,
                ZaribTadil = data?.Sum(d => d.ZaribTadil) ?? 0,
                Tabsare3Ab = data?.Sum(d => d.Tabsare3Ab) ?? 0,
                Tabsare3Fazelab = data?.Sum(d => d.Tabsare3Fazelab) ?? 0,
                TabsareAbonmanFazelab = data?.Sum(d => d.TabsareAbonmanFazelab) ?? 0,
                GhanonBoodje = data?.Sum(d => d.GhanonBoodje) ?? 0,
                JavazemKahande = data?.Sum(d => d.JavazemKahande) ?? 0,
                AvarezSanati = data?.Sum(d => d.AvarezSanati) ?? 0,
            };
            return new ReportOutput<WaterDiscountDetailHeaderOutputDto, WaterDiscountDetailDataOutputDto>(_title, header, data);
        }
        public async Task InputValidate(WaterDiscountByTypeDetailInputDto input, CancellationToken cancellationToken)
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
