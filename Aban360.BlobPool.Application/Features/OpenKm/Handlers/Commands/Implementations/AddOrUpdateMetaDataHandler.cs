using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts;
using Aban360.Common.Extensions;
using System.Reflection;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Implementations
{
    internal sealed class AddOrUpdateMetaDataHandler : IAddOrUpdateMetaDataHandler
    {
        private IOpenKmQueryService _openKmQueryService;
        public AddOrUpdateMetaDataHandler(IOpenKmQueryService openKmQueryService)
        {
            _openKmQueryService = openKmQueryService;
            _openKmQueryService.NotNull(nameof(openKmQueryService));
        }

        public async Task Handle(AddOrUpdateMetaDataDto inputDto, string uuid, CancellationToken cancellationToken)
        {
            string valuesOfBody = " ";
            Type type = inputDto.GetType();
            PropertyInfo[] propertyInfo = type.GetProperties();
            foreach (var property in propertyInfo)
            {
                string propName = property.Name;
                object? propValue = property.GetValue(inputDto);
                if (propValue != null)
                {
                    string valueXml = $@"<simplePropertyGroup>
                                           <name>okp:moshtarakin.{propName}</name>
                                           <value>{propValue}</value>
                                        </simplePropertyGroup>";
                    valuesOfBody += valueXml;
                }
            }
            string body = @$"<?xml version=""1.0"" encoding=""UTF-8""?>
                                <simplePropertiesGroup>
                                    {valuesOfBody}
                                </simplePropertiesGroup>";

            await _openKmQueryService.AddOrUpdateMetadata(body, uuid);
        }
    }
    public record AddOrUpdateMetaDataDto
    {
        public int? section { get; set; }
        public int? city { get; set; }
        public int? village { get; set; }
    }
}
