using APIArenaAuto.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;

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
// BANCO DE DADOS (POSTGRESQL)
// ============================

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
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

// ============================
// BUILD
// ============================
var app = builder.Build();

// ============================
// CRIA TABELAS AUTOMATICAMENTE (SE AINDA NÃO EXISTIREM)
// ============================
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        Console.WriteLine("Tentando conectar ao PostgreSQL...");
        db.Database.EnsureCreated();
        Console.WriteLine("Conexão e tabelas verificadas com sucesso!");
    }
    catch (Exception ex)
    {
        Console.WriteLine("=========================================");
        Console.WriteLine("ERRO AO CONECTAR NO BANCO:");
        Console.WriteLine(ex.Message);
        if (ex.InnerException != null)
            Console.WriteLine("DETALHE: " + ex.InnerException.Message);
        Console.WriteLine("=========================================");
        // O app não vai mais fechar sozinho aqui, permitindo ver o erro.
    }
}

// ============================
// PASTA DE FOTOS (RENDER)
// ============================
var caminhoFotos = Path.Combine(Directory.GetCurrentDirectory(), "FotosUsuarios");
if (!Directory.Exists(caminhoFotos))
{
    Directory.CreateDirectory(caminhoFotos);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(caminhoFotos),
    RequestPath = "/fotos"
});

// ============================
// SWAGGER
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
// app.UseHttpsRedirection(); // ❌ NÃO usar HTTPS no Render
app.UseAuthorization();
app.MapControllers();

// ============================
// PORTA DINÂMICA (RENDER)
// ============================
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");
