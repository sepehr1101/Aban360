using System.Reflection;
using Aban360.LocationPool.Application.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Services;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.GatewayAdhoc.Extentions;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Extensions;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Aban360.LocationPool.Persistence.Migrations;
using Aban360.UserPool.Persistence.Contexts.Implementation;
using AutoMapper;
using FluentMigrator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

Assembly module = typeof(Country).Assembly;
Check(module.GetName().Name == "Aban360.LocationPool", "Unexpected assembly name.");
foreach (Type type in new[] { typeof(Country), typeof(ILocationLookupService), typeof(ILocationTreeAdHoc),
             typeof(LocationPoolContext), typeof(DbInitialDesign), typeof(Aban360.LocationPool.Infrastructure.Class1) })
{
    Check(type.Assembly == module, $"Type wasn't consolidated: {type.FullName}");
}
Check(!module.GetReferencedAssemblies().Any(reference => reference.Name?.StartsWith("Aban360.LocationPool.", StringComparison.Ordinal) == true),
    "The consolidated module still references a legacy assembly.");

ServiceCollection services = new();
services.AddLogging();
services.AddDbContext<LocationPoolContext>(options =>
    options.UseSqlServer("Server=localhost;Database=LocationPoolTest;Integrated Security=true;TrustServerCertificate=true"));
services.AddLocationPoolPersistenceInjections();
services.AddLocationPoolApplicationInjections();
services.AddLocationPoolGatewayInjections();

foreach (Type contract in new[] { typeof(IUnitOfWork), typeof(ICountryCommandService), typeof(ICountryQueryService),
             typeof(ILocationLookupService), typeof(ILocationTreeAdHoc) })
{
    ServiceDescriptor descriptor = services.Single(service => service.ServiceType == contract);
    Check(descriptor.Lifetime == ServiceLifetime.Scoped, $"Lifetime changed: {contract.FullName}");
}

using ServiceProvider provider = services.BuildServiceProvider();
using IServiceScope scope = provider.CreateScope();
IServiceProvider scoped = scope.ServiceProvider;
Check(scoped.GetRequiredService<ILocationTreeAdHoc>().GetType().Assembly == module, "Gateway resolution failed.");
Check(scoped.GetRequiredService<ILocationLookupService>().GetType().Assembly == module, "Application resolution failed.");
Check(ReferenceEquals(scoped.GetRequiredService<ICountryCommandService>(), scoped.GetRequiredService<ICountryQueryService>()),
    "Command and query contracts must share their scoped repository.");
using IServiceScope otherScope = provider.CreateScope();
Check(!ReferenceEquals(scoped.GetRequiredService<ICountryQueryService>(), otherScope.ServiceProvider.GetRequiredService<ICountryQueryService>()),
    "Repositories must be isolated across scopes.");

IMapper mapper = scoped.GetRequiredService<IMapper>();
CountryGetDto mapped = mapper.Map<CountryGetDto>(new Country { Id = 7, Title = "ایران" });
Check(mapped.Id == 7 && mapped.Title == "ایران", "Location AutoMapper profile was not registered.");
LocationPoolContext context = scoped.GetRequiredService<LocationPoolContext>();
Check(context.Model.GetDefaultSchema() == "LocationPool" && context.Model.FindEntityType(typeof(Country)) != null,
    "Location EF Core metadata changed.");
Check(typeof(DbInitialDesign).GetCustomAttribute<MigrationAttribute>()?.Version == 14031110, "Location migration identity changed.");
Check(File.Exists(Path.Combine(AppContext.BaseDirectory, "DbSeeder", "DataScript", "mainHierarchy.sql")),
    "The seeder SQL file must be copied to its existing runtime path.");
Console.WriteLine("PASS: LocationPool assembly, scoped DI, repository sharing, mapping, EF model, migration and seed output.");

static void Check(bool condition, string message)
{
    if (!condition)
        throw new InvalidOperationException(message);
}
