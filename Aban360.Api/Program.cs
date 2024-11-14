using Aban360.Api.Extensions;
using Aban360.UserPool.Persistence.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen.ConventionalRouting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//DI
builder.Services.AddUserPoolExtensions();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGenWithConventionalRoutes();
builder.Services.AddCaptcha();
builder.Services.UpdateAndSeedUserPoolDb();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthorization();

//app.MapControllers();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

