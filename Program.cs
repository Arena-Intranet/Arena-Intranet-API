using APIArenaAuto.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// ============================
// CONTROLLERS
// ============================
builder.Services.AddControllers();

// ============================
// SWAGGER
// ============================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "APIArenaAuto",
        Version = "v1"
    });
});

// ============================
// BANCO DE DADOS (FAKE - IN MEMORY)
// ============================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ArenaFakeDb")
);

// ============================
// CORS
// ============================
builder.Services.AddCors(options =>
{
    options.AddPolicy("Livre", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ============================
// SWAGGER (EM PRODUÇÃO TAMBÉM)
// ============================
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIArenaAuto v1");
    c.RoutePrefix = "swagger";
});

// ============================
// PIPELINE
// ============================
app.UseCors("Livre");

// ❌ NÃO usar HTTPS no Render
// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

// ============================
// PORTA DINÂMICA (RENDER)
// ============================
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");
