using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.GatewayAdhoc.Features.ConsumersInfo.Contracts;
using AutoMapper;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Implementations
{
    internal sealed class _CalculationCreateHandler : I_CalculationCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ITariffCommandService _tariffCommandService;
        private readonly IConsumerSummaryAddhoc _consumerSummaryAddhoc;

        private static readonly Dictionary<string, PropertyInfo> _propertyCache = new();
        private static readonly Type _dtoType = typeof(ConsumerSummaryDto);

        public _CalculationCreateHandler(
            IMapper mapper,
            ITariffCommandService tariffCommandService,
            IConsumerSummaryAddhoc consumerSummaryAddhoc)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _tariffCommandService = tariffCommandService;
            _tariffCommandService.NotNull(nameof(tariffCommandService));

            _consumerSummaryAddhoc = consumerSummaryAddhoc;
            _consumerSummaryAddhoc.NotNull(nameof(consumerSummaryAddhoc));
        }

        public async Task Handle(string billId, string counterNumber, CancellationToken cancellationToken)
        {
            billId = "100189916315";
            counterNumber = "5000";
            string W_FromDate = "1402/06/15";
            string W_ToDate = "1404/01/24";

            ICollection<waterDate> dateRanges = new List<waterDate>()
            {
                new waterDate {FromDate="1402/01/01",ToDate= "1402/06/30" ,Formula="'CustomerNumber'+'HouseholdNumber'"},
                new waterDate {FromDate="1402/07/01",ToDate= "1402/12/29" ,Formula="'CustomerNumber'*'HouseholdNumber'"},
                new waterDate {FromDate="1403/01/01",ToDate= "1403/12/29" ,Formula="'CustomerNumber'/'HouseholdNumber'"},
                new waterDate {FromDate="1404/01/01",ToDate= "1404/06/30" ,Formula="'CustomerNumber'-'HouseholdNumber'"},
            };
            var dateSegment = dateRanges
          .OrderBy(d => d.FromDate)
          .Where(d =>
              string.Compare(d.FromDate, W_ToDate) <= 0 &&
              string.Compare(d.ToDate, W_FromDate) >= 0)
          .Select(d => new waterDate
          {
              FromDate = string.Compare(d.FromDate, W_FromDate) < 0 ? W_FromDate : d.FromDate,
              ToDate = string.Compare(d.ToDate, W_ToDate) > 0 ? W_ToDate : d.ToDate,
              Formula = d.Formula
          });

            var consumerSummary = await _consumerSummaryAddhoc.Handle(billId, cancellationToken);
            dateSegment.ForEach(d =>
            {
                d.Formula =CalcFormula(d.Formula, consumerSummary);
            });

        }
        private string CalcFormula(string formula, ConsumerSummaryDto consumerSummary)
        {
            //var variableNames = Regex.Matches(formula, @"'(.*?)'");

            //var y = Regex.Replace(formula, @"\{([^}]+)\}", match =>
            //{
            //    string propertyName = match.Groups[1].Value;
            //    var propertyInfo = typeof(ConsumerSummaryDto).GetProperty(propertyName);

            //    if (propertyInfo != null)
            //    {
            //        object value = propertyInfo.GetValue(consumerSummary);
            //        return value?.ToString() ?? string.Empty;
            //    }

            //    return match.Value; // اگر پراپرتی پیدا نشد، همان متن اصلی باقی بماند
            //});
            //return y;
            return Regex.Replace(formula, @"'([^']+)'", match =>
            {
                string propertyName = match.Groups[1].Value.Trim();

                // بررسی وجود پراپرتی در کلاس
                if (TryGetPropertyValue(consumerSummary, propertyName, out object value))
                {
                    return value?.ToString() ?? string.Empty;
                }

                // اگر پراپرتی وجود نداشت، همان متن را بدون کوتیشن برگردان
                return propertyName;
            });

        }
        private bool TryGetPropertyValue(ConsumerSummaryDto consumer, string propertyName, out object value)
        {
            value = null;

            if (!_propertyCache.TryGetValue(propertyName, out PropertyInfo propertyInfo))
            {
                propertyInfo = _dtoType.GetProperty(propertyName,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                if (propertyInfo == null) return false;

                _propertyCache[propertyName] = propertyInfo;
            }

            value = propertyInfo.GetValue(consumer);
            return true;
        }
    }

    public class waterDate
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Formula { get; set; }
    }

    public class Dto
    {
        public int CustomerNumber { get; set; }
        public string BillId { get; set; } = default!;
        public string? ReadingNumber { get; set; }
        public string InstallationDate { get; set; } = default!;
        public short ContractualCapacity { get; set; }
        public short HouseholdNumber { get; set; }
        public short UnitDomesticWater { get; set; }
        public short UnitCommercialWater { get; set; }
        public short UnitOtherWater { get; set; }
        public short UnitDomesticSewage { get; set; }
        public short UnitCommercialSewage { get; set; }
        public short UnitOtherSewage { get; set; }
        public short EmptyUnit { get; set; }
        public short ConstructionTypeId { get; set; }
        public short UsageConsumtionId { get; set; }
        public short UsageSellId { get; set; }
        public string? SiphonInstallationDate { get; set; }
        public int HeadquarterId { get; set; }
        public int ProvinceId { get; set; }
        public int RegionId { get; set; }
        public int ZoneId { get; set; }
        public int MunicipalityId { get; set; }
        public int PreviousWaterMeterNumber { get; set; }
        public string? PreviousWaterMeterDateJalali { get; set; }
        public Dictionary<short, string>? WaterMeterTags { get; set; }
        public Dictionary<short, string>? IndividualTags { get; set; }
        public Dictionary<short, string>? Discounts { get; set; }
    }
}

