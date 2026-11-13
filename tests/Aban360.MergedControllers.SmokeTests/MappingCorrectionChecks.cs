using System.Reflection;
using Aban360.BlobPool.Application.Features.Taxonomy.Services;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.ClaimPool.Application.Features.Draft.Services;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

static class MappingCorrectionChecks
{
    public static async Task<int> Run(IMapper mapper)
    {
        int checks = 0, queries = 0;
        void Check(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); checks++; }
        var previous = new MapperConfiguration(configuration =>
        {
            configuration.AddProfile<BeforeMappingCorrections.Aban360.ClaimPool.Application.Features.Draft.Mappings.RequestTrackingMapper>();
            configuration.AddProfile<BeforeMappingCorrections.Aban360.BlobPool.Application.Features.Taxonomy.Mappings.DocumentCategoryMapper>();
            configuration.AddProfile<BeforeMappingCorrections.Aban360.BlobPool.Application.Features.Taxonomy.Mappings.DocumentTypeMapper>();
        }).CreateMapper();

        object Build(Type contract, object queryResult, long expectedId = 7)
        {
            var implementation = contract.Assembly.GetTypes().Single(candidate => candidate.IsClass && contract.IsAssignableFrom(candidate));
            var constructor = implementation.GetConstructors().Single();
            return constructor.Invoke(constructor.GetParameters().Select(parameter =>
            {
                if (parameter.ParameterType == typeof(IMapper)) return mapper;
                var proxy = (MappingCorrectionProxy)DispatchProxy.Create(parameter.ParameterType, typeof(MappingCorrectionProxy));
                proxy.Callback = (method, arguments) =>
                {
                    if (parameter.ParameterType.FullName!.StartsWith("FluentValidation.IValidator"))
                        return Task.FromResult(new ValidationResult());
                    Check(parameter.ParameterType.Name.EndsWith("QueryService") && method.Name == "Get"
                        && Convert.ToInt64(arguments![0]) == expectedId, "Update must query its target ID without issuing persistence commands");
                    queries++;
                    return queryResult;
                };
                return (object)proxy;
            }).ToArray());
        }

        var owner = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var tracking = new RequestTracking { Id = 7, WaterMeterId = 1, Status = 1, UserId = owner, WaterMeter = new RequestWaterMeter() };
        var navigation = tracking.WaterMeter;
        var trackingInput = new RequestTrackingUpdateDto { Id = 7, WaterMeterId = 42, Status = 3 };
        bool missingMap = false;
        try { previous.Map(trackingInput, tracking); }
        catch (AutoMapperMappingException) { missingMap = true; }
        Check(missingMap, "Frozen mapper must reproduce the previous missing update map");
        await ((IRequestTrackingService)Build(typeof(IRequestTrackingService), Task.FromResult(tracking))).UpdateAsync(trackingInput, CancellationToken.None);
        Check(tracking.Id == 7 && tracking.WaterMeterId == 42 && tracking.Status == 3, "Tracking update must map changed values into the queried entity");
        Check(tracking.UserId == owner && ReferenceEquals(tracking.WaterMeter, navigation), "Tracking update must preserve owner and navigation fields absent from the DTO");
        Check(queries == 1, "Tracking update keeps its existing query/staging boundary");

        var bytes = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10, 17, 33 };
        var expected = Convert.ToBase64String(bytes);
        IFormFile File() => new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Icon", "icon.png")
            { Headers = new HeaderDictionary(), ContentType = "image/png" };
        var categoryInput = new DocumentCategoryUpdateDto { Id = 7, Title = "Changed category", Css = "new-css", Icon = File() };
        var typeInput = new DocumentTypeUpdateDto { Id = 7, Title = "Changed type", DocumentCategoryId = 9, Css = "type-css", Icon = File() };
        var oldCategory = new DocumentCategory { Icon = expected };
        var oldType = new DocumentType { Icon = expected };
        previous.Map(categoryInput, oldCategory); previous.Map(typeInput, oldType);
        Check(oldCategory.Icon == categoryInput.Icon!.ToString() && oldCategory.Icon != expected, "Frozen category map must reproduce base64 overwrite");
        Check(oldType.Icon == typeInput.Icon.ToString() && oldType.Icon != expected, "Frozen type map must reproduce base64 overwrite");

        var category = new DocumentCategory { Id = 7, Title = "Old", Css = "old-css", Icon = expected };
        var type = new DocumentType { Id = 7, Title = "Old", Css = "old-css", DocumentCategoryId = 1, Icon = expected };
        mapper.Map(categoryInput, category); mapper.Map(typeInput, type);
        Check(category.Icon == expected && type.Icon == expected, "DTO maps must leave byte conversion to the application service");
        category.Icon = "old-icon"; type.Icon = "old-icon";
        await ((IDocumentCategoryService)Build(typeof(IDocumentCategoryService), Task.FromResult(category))).UpdateAsync(categoryInput, CancellationToken.None);
        await ((IDocumentTypeService)Build(typeof(IDocumentTypeService), Task.FromResult(type))).UpdateAsync(typeInput, CancellationToken.None);
        Check(category.Icon == expected && Convert.FromBase64String(category.Icon).SequenceEqual(bytes), "Category service must retain exact uploaded bytes as base64");
        Check(type.Icon == expected && Convert.FromBase64String(type.Icon).SequenceEqual(bytes), "Type service must retain exact uploaded bytes as base64");
        Check(category.Title == categoryInput.Title && category.Css == categoryInput.Css, "Ignoring category Icon must still map other editable fields");
        Check(type.Title == typeInput.Title && type.Css == typeInput.Css && type.DocumentCategoryId == 9, "Ignoring type Icon must still map title, CSS and category");
        Check(queries == 3, "All three updates query once and leave committing to the caller");
        var subscriptionId = Enum.GetValues<Aban360.ClaimPool.Domain.Constants.SubscriptionTypeEnum>()[0];
        var subscription = new Aban360.ClaimPool.Domain.Features.Metering.Entities.SubscriptionType { Id = subscriptionId, Title = "Old title" };
        var subscriptionInput = new Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands.SubscriptionTypeUpdateDto { Id = subscriptionId, Title = "Changed title" };
        var original = (Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts.ISubscriptionTypeUpdateHandler)
            Build(typeof(Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts.ISubscriptionTypeUpdateHandler), Task.FromResult(subscription), Convert.ToInt64(subscriptionId));
        bool reverseMapFailed = false;
        try { await original.Handle(subscriptionInput, CancellationToken.None); }
        catch (AutoMapperMappingException) { reverseMapFailed = true; }
        Check(reverseMapFailed && subscription.Title == "Old title", "Frozen SubscriptionType handler must reproduce the reversed mapping failure");
        var corrected = (Aban360.ClaimPool.Application.Features.Metering.Services.ISubscriptionTypeService)
            Build(typeof(Aban360.ClaimPool.Application.Features.Metering.Services.ISubscriptionTypeService), Task.FromResult(subscription), Convert.ToInt64(subscriptionId));
        await corrected.UpdateAsync(subscriptionInput, CancellationToken.None);
        Check(subscription.Id.Equals(subscriptionId) && subscription.Title == "Changed title", "SubscriptionType update must change the queried entity");
        Check(subscriptionInput.Id.Equals(subscriptionId) && subscriptionInput.Title == "Changed title", "SubscriptionType update must not overwrite the caller DTO");
        Check(queries == 5, "Mapping corrections must keep existing query/staging boundaries");
        Console.WriteLine($"PASS: {checks} mapping correction checks; four old defects reproduced and corrected with production profiles/services.");
        return checks;
    }
}

public class MappingCorrectionProxy : DispatchProxy
{
    public Func<MethodInfo, object?[]?, object?> Callback { get; set; } = null!;
    protected override object? Invoke(MethodInfo? method, object?[]? arguments) => Callback(method!, arguments);
}
