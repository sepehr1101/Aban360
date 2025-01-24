using Aban360.Api.ExceptionHandlers;
using Aban360.Api.Extensions;
using Aban360.UserPool.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
//DI
builder.Services.AddDI();
builder.Services.AddAuth(configuration);
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerService();
builder.Services.AddCaptcha();
builder.Services.AddCustomDbContext(configuration);
builder.Services.UpdateAndSeedUserPoolDb();
builder.AddHangfire();


builder.Services.AddCustomCors();
builder.Services.AddCustomOptions(configuration);

//Exceptions
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

//serilog
builder.Services.AddSerilog(configuration);

//builder.Services.AddUserPoolExtensions();
//builder.Services.AddCustomJwtBearer(configuration);
//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//builder.Services.AddSwaggerGenWithConventionalRoutes();
//builder.Services.AddCaptcha();
//builder.Services.AddCustomDbContext(configuration);

//builder.Services.UpdateAndSeedUserPoolDb();
//builder.Services.AddCustomCors();
//builder.Services.AddCustomAntiforgery();
//builder.Services.AddCustomOptions(configuration);

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

app.AddHangfireDashboard();

app.MapControllers();
app.Run();

public partial class Program { }

