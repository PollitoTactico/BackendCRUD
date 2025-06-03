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
builder.Services.AddScoped<IKeywordProcessor, KeywordProcessor>();
builder.Services.AddScoped<IComparationService, ComparationService>();
builder.Services.AddScoped<ISynonymService, SynonymService>();
builder.Services.AddSingleton<IPdfService, PdfService>();
builder.Services.AddSingleton<ICVService, CVService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IComparationService, ComparationService>();

builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.WithOrigins("https://front-core-df-pollitotacticos-projects.vercel.app", "http://localhost:3000")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

var app = builder.Build();

app.UseExceptionHandler();
app.UseRouting();
app.UseCors("AllowAll");
app.MapControllers();
app.Run();