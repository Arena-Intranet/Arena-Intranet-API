using APIArenaAuto.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// ============================
// CONTROLLERS
// ============================
builder.Services.AddControllers();

// ============================
// SWAGGER (Configuração de Serviço)
// ============================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "APIArenaAuto", Version = "v1" });
});

// ============================
// BANCO DE DADOS
// ============================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
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
// SWAGGER (Configuração de Pipeline)
// ============================
// Importante: Habilitar o Swagger antes de outros middlewares em desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIArenaAuto v1");
        c.RoutePrefix = "swagger"; // Acessível em: http://localhost:PORTA/swagger
    });
}

// Arquivos estáticos para fotos
var caminhoFotos = @"C:\Users\Administrator\Downloads\FotosUsuarios";
if (!Directory.Exists(caminhoFotos)) Directory.CreateDirectory(caminhoFotos);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(caminhoFotos),
    RequestPath = "/fotos"
});

app.UseCors("Livre");
app.UseHttpsRedirection();
app.UseAuthorization(); 
app.MapControllers();

app.Run();