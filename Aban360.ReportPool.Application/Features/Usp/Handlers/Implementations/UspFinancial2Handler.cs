using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Usp.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.Usp.Input;
using Aban360.ReportPool.Domain.Features.Usp.Output;
using Aban360.ReportPool.Persistence.Features.Usp.Contracts;

namespace Aban360.ReportPool.Application.Features.Usp.Handlers.Implementations
{
    internal sealed class UspFinancial2Handler : IUspFinancial2Handler
    {
        private readonly IUspFinancial2QueryService _queryService;
        public UspFinancial2Handler(IUspFinancial2QueryService queryService)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(_queryService));
        }
        public async Task<ReportOutput<UspFinancialHeader, UspFinancial2Output>> Handle(UspFinancial2Input input, CancellationToken cancellationToken)
        {
            //TODO: validate
            input.VillageOrCityType = input.ZoneId > 140000 ? 2 : 1;
            IEnumerable<UspFinancial2Output> output= await _queryService.Get(input);
            UspFinancialHeader header = new()
            {
                Abon_ab = output.Sum(x => x.Abon_ab),
                Abon_fas = output.Sum(x => x.Abon_fas),
                Ab_baha = output.Sum(x => x.Ab_baha),
                Ahad_ghabs = output.Sum(x => x.Ahad_ghabs),
                Ahad_ghabs_Faz = output.Sum(x => x.Ahad_ghabs_Faz),
                Avarez = output.Sum(x => x.Avarez),
                baha_fas = output.Sum(x => x.baha_fas),
                Bodjeh = output.Sum(x => x.Bodjeh),
                Jam_Ab_baha = output.Sum(x => x.Jam_Ab_baha),
                Jam_Fas = output.Sum(x => x.Jam_Fas),
                Jam_kol = output.Sum(x => x.Jam_kol),
                Javani = output.Sum(x => x.Javani),
                Maliat = output.Sum(x => x.Maliat),
                Tabsare2 = output.Sum(x => x.Tabsare2),
                Tabsare3_ab = output.Sum(x => x.Tabsare3_ab),
                Tabsare_abon = output.Sum(x => x.Tabsare_abon),
                Tabsare_Fa = output.Sum(x => x.Tabsare_Fa),
                Tedad_ensh_Faz = output.Sum(x => x.Tedad_ensh_Faz),
                Tedad_ghabs = output.Sum(x => x.Tedad_ghabs),
                Tedad_ghabs_faz = output.Sum(x => x.Tedad_ghabs_faz),
                Tedad_Moshtrak = output.Sum(x => x.Tedad_Moshtrak),
                Tedad_vahd_Moshtrak = output.Sum(x => x.Tedad_vahd_Moshtrak),
                Tedad_vahed_Faz = output.Sum(x => x.Tedad_vahed_Faz),
                waste_Faz = output.Sum(x => x.waste_Faz),
                Zaribfasl = output.Sum(x => x.Zaribfasl),
                Sp=input.Sp
            };
            ReportOutput<UspFinancialHeader, UspFinancial2Output> reportOutput = new( ReportLiterals.UspFinancial2, header, output);
            return reportOutput;
        }
    }
}