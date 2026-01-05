using APIArenaAuto.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// CONTROLLERS
builder.Services.AddControllers();

// BANCO DE DADOS
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS (liberar front-end)
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

// CORS
app.UseCors("Livre");

// HTTPS
app.UseHttpsRedirection();

// MAPEAR CONTROLLERS
app.MapControllers();

app.Run();
