var builder = WebApplication.CreateBuilder(args);

//Services (container)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers(); // Controllerlardaki Route yap�s�n� anlamas� i�in

app.Run();
