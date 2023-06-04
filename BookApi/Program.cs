using AspNetCoreRateLimit;
using BookApi.Extensions;
using BookApi.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using Presentation.ActionFilters;
using Repositories.EFCore;
using Services;
using Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
// Add services to the container.

builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true; // i�erik pazarl���na a�mak i�in true yapt�k Accept */* default u bu yani false
    config.ReturnHttpNotAcceptable = true; // istenilen i�erik tipi desteklenmedi��inde hatay� 406 olarak g�nderir
    config.CacheProfiles.Add("5mins",new CacheProfile { Duration = 300 });
})
.AddCustomCsvOutputFormatter()
.AddXmlDataContractSerializerFormatters() // xml format�nda ��k�� verebilmesi i�in
.AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly); // Presentation katman�n� kullan Controllerlar i�in
    //AddNewtonsoftJson();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true; // model state invalid oldu�u zaman badrequest d�ns�n istiyoruz.
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EXTENSIONS
//DB context registration IOC = inversion of control (kontrol �n tersine �evrilmesi)  // IOC e DbContext kayd�n� yapt�k
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureActionFilters();
builder.Services.ConfigureDataShaper();
builder.Services.ConfigureCors();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddCustomMediaTypes();
builder.Services.AddScoped<IBookLinks, BookLinks>();
builder.Services.ConfigureVersionning();
builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureHttpCacheHeaders();
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();

var app = builder.Build();

// Logger ile middleware yap�land�rmas�
var logger = app.Services.GetRequiredService<ILoggerService>();
app.ConfigureExceptionHandler(logger);
//------------------------------------------

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseIpRateLimiting();
app.UseCors("CorsPolicy");

app.UseResponseCaching();
app.UseHttpCacheHeaders();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
