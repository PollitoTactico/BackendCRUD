using BackendCRUD.ApiService.Controllers;
using BackendCRUD.ApiService.Data;
using BackendCRUD.ApiService.Services;
using Microsoft.EntityFrameworkCore;
using BackendCRUD.ApiService.Services.Interfaces;
using BackendCRUD.ApiService.Services.Implementations;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<IPdfTextExtractor, PdfTextExtractor>();
builder.Services.AddScoped<IPdfService, PdfServiceProxy>();
builder.Services.AddScoped<IKeywordProcessor, KeywordProcessor>();

// Singleton para el servicio ComparationService
builder.Services.AddSingleton<IComparationService, ComparationService>();  // Registro único

builder.Services.AddScoped<ISynonymService, SynonymService>();
builder.Services.AddSingleton<IPdfService, PdfService>();
builder.Services.AddScoped<PdfService>();
builder.Services.AddSingleton<ICVService, CVService>();

// Configuración del DbContext con la cadena de conexión
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICVService>(provider =>
    new CVServiceProxy(
        provider.GetRequiredService<ICVService>(),  // Inyecta el servicio real
        provider.GetRequiredService<ILogger<CVServiceProxy>>()  // Inyecta el logger
    )
);

// Configuración de logging
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

// Configuración de CORS para permitir solicitudes desde cualquier origen
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseExceptionHandler();
app.UseRouting();
app.UseCors("AllowAll");
app.MapControllers();
app.Run();
