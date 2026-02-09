using Aban360.BrdigeApi.ExceptionHandlers;
using Aban360.BrdigeApi.Extensions;
using Aban360.BrdigeApi.Filters;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
//DI
builder.Services.AddDI();
builder.Services.AddAuth(configuration);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new TruncateDoubleConverter());
        options.JsonSerializerOptions.Converters.Add(new TruncateFloatConverter());
        options.JsonSerializerOptions.Converters.Add(new TruncateNullableDoubleConverter());
    });

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerService();
builder.Services.AddCustomDbContext(configuration);

builder.AddHangfire();
builder.Services.AddMvc();
builder.Services.AddRazorPages();


builder.Services.AddCustomCors();
builder.Services.AddCustomOptions(configuration);

//ExceptionsInvalidOperationException: Unable to resolve service for type 'Aban360.Common.Db.QueryServices.ICommonZoneService' while attempting to activate 'Aban360.ReportPool.Application.Features.Dashboard.Handlers.Implementations.GetReportByTileScriptContentHandler'.

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

//serilog
builder.Services.AddSerilog(configuration);

builder.Services.AddCustomHttpClients(configuration);

var app = builder.Build();
app.UseExceptionHandler("/error");
app.AddSwaggerApp();

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();

app.UseRouting();
app.UseCustomCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseSerilogInterface();

app.MapControllers();
app.Run();

public partial class Program { }

