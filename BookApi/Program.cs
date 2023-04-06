using BookApi.Extensions;
using BookApi.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using Repositories.EFCore;
using Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
// Add services to the container.

builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true; // içerik pazarlýðýna açmak için true yaptýk Accept */* default u bu yani false
    config.ReturnHttpNotAcceptable = true; // istenilen içerik tipi desteklenmediððinde hatayý 406 olarak gönderir
})
.AddCustomCsvOutputFormatter()
.AddXmlDataContractSerializerFormatters() // xml formatýnda çýkýþ verebilmesi için
.AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly). // Presentation katmanýný kullan Controllerlar için
    AddNewtonsoftJson();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true; // model state invalid olduðu zaman badrequest dönsün istiyoruz.
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EXTENSIONS
//DB context registration IOC = inversion of control (kontrol ün tersine çevrilmesi)  // IOC e DbContext kaydýný yaptýk
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureLoggerService();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Logger ile middleware yapýlandýrmasý
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

app.UseAuthorization();

app.MapControllers();

app.Run();
